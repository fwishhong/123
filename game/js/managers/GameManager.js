/**
 * GameManager - 游戏主管理器
 * 负责协调所有其他管理器，控制游戏流程
 */

export class GameManager {
    constructor() {
        this.initialized = false;
        this.gameState = null;
        this.managers = {};
    }

    /**
     * 初始化游戏
     */
    async init(managers) {
        if (this.initialized) return;

        this.managers = managers;
        this.gameState = {
            currentDay: 1,
            currentTimePeriod: 0, // 0: 早晨, 1: 下午, 2: 夜晚
            currentView: 'activity-view',
            inEvent: false,
            inBattle: false,
            gameOver: false,
            ending: null
        };

        this.initialized = true;
        console.log('[GameManager] Initialized');
    }

    /**
     * 开始新游戏
     */
    startNewGame() {
        console.log('[GameManager] Starting new game');

        // 重置所有管理器
        this.managers.time.reset();
        this.managers.attributes.reset();
        this.managers.relationships.reset();
        this.managers.events.reset();

        // 重置游戏状态
        this.gameState = {
            currentDay: 1,
            currentTimePeriod: 0,
            currentView: 'activity-view',
            inEvent: false,
            inBattle: false,
            gameOver: false,
            ending: null
        };

        // 更新UI
        this.managers.ui.updateAllDisplays();
        this.managers.ui.showNotification('游戏开始！Day 1 - 早晨');

        // 触发开始事件
        this.managers.events.triggerAutoEvents();
    }

    /**
     * 加载游戏
     */
    loadGame(saveData) {
        console.log('[GameManager] Loading game');

        if (!saveData) {
            this.managers.ui.showNotification('没有找到存档');
            return false;
        }

        try {
            // 加载各个管理器的数据
            this.managers.time.load(saveData.time);
            this.managers.attributes.load(saveData.attributes);
            this.managers.relationships.load(saveData.relationships);
            this.managers.events.load(saveData.events);

            // 加载游戏状态
            this.gameState = saveData.gameState;

            // 更新UI
            this.managers.ui.updateAllDisplays();
            this.managers.ui.showNotification('游戏加载成功');

            return true;
        } catch (error) {
            console.error('Load game error:', error);
            this.managers.ui.showNotification('加载失败');
            return false;
        }
    }

    /**
     * 保存游戏
     */
    saveGame() {
        console.log('[GameManager] Saving game');

        const saveData = {
            version: '1.0',
            timestamp: Date.now(),
            gameState: this.gameState,
            time: this.managers.time.getSaveData(),
            attributes: this.managers.attributes.getSaveData(),
            relationships: this.managers.relationships.getSaveData(),
            events: this.managers.events.getSaveData()
        };

        try {
            localStorage.setItem('mindlink_save', JSON.stringify(saveData));
            this.managers.ui.showNotification('游戏保存成功');
            return true;
        } catch (error) {
            console.error('Save game error:', error);
            this.managers.ui.showNotification('保存失败');
            return false;
        }
    }

    /**
     * 执行活动
     */
    doActivity(activityId) {
        console.log('[GameManager] Doing activity:', activityId);

        const activity = this.managers.events.getActivity(activityId);
        if (!activity) {
            console.error('Activity not found:', activityId);
            return;
        }

        // 检查是否满足条件
        if (!this.checkActivityRequirements(activity)) {
            this.managers.ui.showNotification('不满足活动条件');
            return;
        }

        // 执行活动效果
        this.applyActivityEffects(activity);

        // 推进时间
        this.advanceTime();

        // 检查是否触发事件
        this.managers.events.triggerAutoEvents();
    }

    /**
     * 检查活动要求
     */
    checkActivityRequirements(activity) {
        if (!activity.requirements) return true;

        const attrs = this.managers.attributes.getAll();

        // 检查属性要求
        if (activity.requirements.attributes) {
            for (const [attr, required] of Object.entries(activity.requirements.attributes)) {
                if (attrs[attr] < required) {
                    return false;
                }
            }
        }

        return true;
    }

    /**
     * 应用活动效果
     */
    applyActivityEffects(activity) {
        if (!activity.effects) return;

        // 属性变化
        if (activity.effects.attributes) {
            for (const [attr, value] of Object.entries(activity.effects.attributes)) {
                this.managers.attributes.modify(attr, value);
            }
        }

        // 好感度变化
        if (activity.effects.relationships) {
            for (const [charId, value] of Object.entries(activity.effects.relationships)) {
                this.managers.relationships.modify(charId, value);
            }
        }

        this.managers.ui.showNotification(`完成活动: ${activity.name}`);
    }

    /**
     * 推进时间
     */
    advanceTime() {
        const result = this.managers.time.advance();

        if (result.dayChanged) {
            this.onDayChanged(result.currentDay);
        }

        this.managers.ui.updateTimeDisplay();
    }

    /**
     * 日期改变时
     */
    onDayChanged(newDay) {
        console.log('[GameManager] Day changed to:', newDay);

        // 检查截止日期
        this.managers.events.checkDeadlines(newDay);

        // 好感度衰减
        this.managers.relationships.dailyDecay();

        // 检查游戏结束条件
        if (newDay > 100) {
            this.triggerEnding();
        }
    }

    /**
     * 触发结局
     */
    triggerEnding() {
        console.log('[GameManager] Triggering ending');
        this.gameState.gameOver = true;

        // 根据各种条件计算结局
        const ending = this.calculateEnding();
        this.gameState.ending = ending;

        this.managers.ui.showEnding(ending);
    }

    /**
     * 计算结局
     */
    calculateEnding() {
        const completedMissions = this.managers.events.getCompletedMissions();
        const totalRelationships = this.managers.relationships.getTotalLevel();
        const totalAttributes = this.managers.attributes.getTotal();

        // 简化的结局判定逻辑
        if (completedMissions.length === 6 && totalRelationships >= 50) {
            return {
                id: 'true_ending',
                name: '真结局 - 新世界',
                description: '你成功拯救了所有人，揭露了真相，开启了新的未来...'
            };
        } else if (completedMissions.length >= 4) {
            return {
                id: 'good_ending',
                name: '好结局 - 希望之光',
                description: '虽然有遗憾，但你守护了重要的人们...'
            };
        } else if (completedMissions.length >= 2) {
            return {
                id: 'normal_ending',
                name: '普通结局 - 继续前行',
                description: '事件告一段落，但阴影依然存在...'
            };
        } else {
            return {
                id: 'bad_ending',
                name: '坏结局 - 崩溃',
                description: '认知崩溃事件失控，世界陷入混乱...'
            };
        }
    }

    /**
     * 获取当前游戏状态
     */
    getState() {
        return this.gameState;
    }
}
