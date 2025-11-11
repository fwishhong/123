/**
 * UIManager - UI管理器
 * 处理所有UI更新和用户交互
 */

export class UIManager {
    constructor(managers) {
        this.managers = managers;
        this.currentView = 'activity-view';
        this.notification = null;
    }

    /**
     * 初始化UI
     */
    init() {
        this.bindEvents();
        this.notification = document.getElementById('notification');
        console.log('[UIManager] Initialized');
    }

    /**
     * 绑定事件
     */
    bindEvents() {
        // 开始游戏按钮
        document.getElementById('start-game')?.addEventListener('click', () => {
            this.hideScreen('loading-screen');
            this.showScreen('main-screen');
            this.managers.game.startNewGame();
        });

        // 加载游戏按钮
        document.getElementById('load-game')?.addEventListener('click', () => {
            const saveData = this.loadSaveData();
            if (saveData) {
                this.hideScreen('loading-screen');
                this.showScreen('main-screen');
                this.managers.game.loadGame(saveData);
            } else {
                this.showNotification('没有存档');
            }
        });

        // 菜单按钮
        document.getElementById('menu-button')?.addEventListener('click', () => {
            this.toggleMenu();
        });

        // 菜单选项
        document.getElementById('save-game-btn')?.addEventListener('click', () => {
            this.managers.game.saveGame();
            this.toggleMenu();
        });

        document.getElementById('close-menu-btn')?.addEventListener('click', () => {
            this.toggleMenu();
        });

        // 底部导航
        document.querySelectorAll('.nav-button').forEach(btn => {
            btn.addEventListener('click', () => {
                const viewId = btn.dataset.view;
                this.switchView(viewId);

                // 更新按钮状态
                document.querySelectorAll('.nav-button').forEach(b => b.classList.remove('active'));
                btn.classList.add('active');
            });
        });

        // 战斗按钮
        document.querySelectorAll('.action-button').forEach(btn => {
            btn.addEventListener('click', () => {
                const action = btn.dataset.action;
                this.handleBattleAction(action);
            });
        });
    }

    /**
     * 显示/隐藏屏幕
     */
    showScreen(screenId) {
        document.getElementById(screenId)?.classList.add('active');
    }

    hideScreen(screenId) {
        document.getElementById(screenId)?.classList.remove('active');
    }

    /**
     * 切换视图
     */
    switchView(viewId) {
        // 隐藏所有视图
        document.querySelectorAll('.view').forEach(view => {
            view.classList.remove('active');
        });

        // 显示目标视图
        document.getElementById(viewId)?.classList.add('active');
        this.currentView = viewId;

        // 更新视图内容
        if (viewId === 'activity-view') {
            this.updateActivityList();
        } else if (viewId === 'relationship-view') {
            this.updateRelationshipList();
        } else if (viewId === 'calendar-view') {
            this.updateCalendar();
        }
    }

    /**
     * 更新所有显示
     */
    updateAllDisplays() {
        this.updateTimeDisplay();
        this.updateAttributesDisplay();
        this.updateActivityList();
    }

    /**
     * 更新时间显示
     */
    updateTimeDisplay() {
        const time = this.managers.time.getCurrentTime();
        document.getElementById('current-day').textContent = time.day;
        document.getElementById('time-period').textContent = time.periodName;
    }

    /**
     * 更新属性显示
     */
    updateAttributesDisplay() {
        const attrs = this.managers.attributes.getAll();
        document.getElementById('knowledge-value').textContent = attrs.knowledge;
        document.getElementById('courage-value').textContent = attrs.courage;
        document.getElementById('charm-value').textContent = attrs.charm;
        document.getElementById('combat-value').textContent = attrs.combat;
    }

    /**
     * 更新活动列表
     */
    updateActivityList() {
        const container = document.getElementById('activity-list');
        if (!container) return;

        const time = this.managers.time.getCurrentTime();
        const attrs = this.managers.attributes.getAll();
        const rels = this.managers.relationships.getAll();

        const activities = this.managers.events.getAvailableActivities(
            time.day,
            time.period,
            attrs,
            rels
        );

        container.innerHTML = '';

        if (activities.length === 0) {
            container.innerHTML = '<p style="color: var(--text-secondary); text-align: center; padding: 40px;">当前没有可用活动</p>';
            return;
        }

        for (const activity of activities) {
            const card = this.createActivityCard(activity);
            container.appendChild(card);
        }
    }

    /**
     * 创建活动卡片
     */
    createActivityCard(activity) {
        const card = document.createElement('div');
        card.className = 'activity-card';

        // 检查是否锁定
        const attrs = this.managers.attributes.getAll();
        const isLocked = !this.managers.game.checkActivityRequirements(activity);
        if (isLocked) {
            card.classList.add('locked');
        }

        card.innerHTML = `
            <div class="activity-title">${activity.name}</div>
            <div class="activity-description">${activity.description}</div>
            <div class="activity-effects">
                ${this.renderEffects(activity.effects)}
            </div>
        `;

        if (!isLocked) {
            card.addEventListener('click', () => {
                this.managers.game.doActivity(activity.id);
                this.updateAllDisplays();
            });
        }

        return card;
    }

    /**
     * 渲染效果标签
     */
    renderEffects(effects) {
        if (!effects) return '';

        let html = '';

        if (effects.attributes) {
            for (const [attr, value] of Object.entries(effects.attributes)) {
                const name = this.managers.attributes.getAttributeName(attr);
                html += `<span class="effect-tag">+${value} ${name}</span>`;
            }
        }

        return html;
    }

    /**
     * 更新关系列表
     */
    updateRelationshipList() {
        const container = document.getElementById('character-list');
        if (!container) return;

        // 这里需要角色数据
        // 暂时显示占位符
        container.innerHTML = '<p style="color: var(--text-secondary); text-align: center; padding: 40px;">角色关系（待实现）</p>';
    }

    /**
     * 更新日历
     */
    updateCalendar() {
        const container = document.getElementById('calendar-grid');
        if (!container) return;

        const currentDay = this.managers.time.getCurrentTime().day;

        container.innerHTML = '';

        // 显示当前周围的天数
        for (let i = 1; i <= 35; i++) {
            const day = Math.floor((currentDay - 1) / 35) * 35 + i;
            if (day > 100) break;

            const dayEl = document.createElement('div');
            dayEl.className = 'calendar-day';

            if (day === currentDay) {
                dayEl.classList.add('current');
            } else if (day < currentDay) {
                dayEl.classList.add('past');
            }

            dayEl.innerHTML = `
                <div class="day-num">${day}</div>
                <div class="day-event"></div>
            `;

            container.appendChild(dayEl);
        }
    }

    /**
     * 处理战斗行动
     */
    handleBattleAction(action) {
        const result = this.managers.battle.playerAction(action);
        if (!result) return;

        this.updateBattleDisplay();

        if (result.battleEnd) {
            setTimeout(() => {
                if (result.victory) {
                    this.showNotification('战斗胜利！');
                } else {
                    this.showNotification('战斗失败...');
                }
                this.switchView('activity-view');
            }, 1500);
        }
    }

    /**
     * 更新战斗显示
     */
    updateBattleDisplay() {
        const state = this.managers.battle.getBattleState();

        if (!state.inBattle) return;

        // 更新敌人HP
        const enemyHPPercent = (state.enemy.currentHP / state.enemy.maxHP) * 100;
        document.getElementById('enemy-hp-fill').style.width = `${enemyHPPercent}%`;
        document.getElementById('enemy-hp-text').textContent =
            `${Math.max(0, state.enemy.currentHP)}/${state.enemy.maxHP}`;

        // 更新玩家HP
        const playerHPPercent = (state.player.currentHP / state.player.maxHP) * 100;
        document.getElementById('player-hp-fill').style.width = `${playerHPPercent}%`;
        document.getElementById('player-hp-text').textContent =
            `${Math.max(0, state.player.currentHP)}/${state.player.maxHP}`;

        // 更新战斗日志
        const logEl = document.getElementById('battle-log');
        if (logEl) {
            logEl.innerHTML = state.log.map(msg => `<p>${msg}</p>`).join('');
            logEl.scrollTop = logEl.scrollHeight;
        }
    }

    /**
     * 切换菜单
     */
    toggleMenu() {
        const menu = document.getElementById('menu-overlay');
        menu?.classList.toggle('active');
    }

    /**
     * 显示通知
     */
    showNotification(message, duration = 2000) {
        if (!this.notification) return;

        this.notification.textContent = message;
        this.notification.classList.add('show');

        setTimeout(() => {
            this.notification.classList.remove('show');
        }, duration);
    }

    /**
     * 加载存档数据
     */
    loadSaveData() {
        try {
            const data = localStorage.getItem('mindlink_save');
            return data ? JSON.parse(data) : null;
        } catch (error) {
            console.error('Failed to load save data:', error);
            return null;
        }
    }

    /**
     * 显示结局
     */
    showEnding(ending) {
        alert(`\n${ending.name}\n\n${ending.description}\n\n游戏结束！`);
    }
}
