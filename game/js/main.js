/**
 * MindLink - 认知空间
 * 主入口文件
 */

import { GameManager } from './managers/GameManager.js';
import { TimeManager } from './managers/TimeManager.js';
import { AttributeManager } from './managers/AttributeManager.js';
import { RelationshipManager } from './managers/RelationshipManager.js';
import { EventManager } from './managers/EventManager.js';
import { BattleManager } from './managers/BattleManager.js';
import { UIManager } from './ui/UIManager.js';
import { activities, characters, missions, enemies, events } from './data/gameData.js';

/**
 * 游戏应用类
 */
class MindLinkGame {
    constructor() {
        this.managers = {
            game: null,
            time: null,
            attributes: null,
            relationships: null,
            events: null,
            battle: null,
            ui: null
        };

        this.initialized = false;
    }

    /**
     * 初始化游戏
     */
    async init() {
        console.log('='.repeat(50));
        console.log('MindLink - 认知空间');
        console.log('初始化中...');
        console.log('='.repeat(50));

        try {
            // 创建所有管理器
            this.managers.time = new TimeManager();
            this.managers.attributes = new AttributeManager();
            this.managers.relationships = new RelationshipManager();
            this.managers.events = new EventManager();
            this.managers.battle = new BattleManager();
            this.managers.game = new GameManager();
            this.managers.ui = new UIManager(this.managers);

            // 初始化管理器
            this.managers.time.init();
            this.managers.attributes.init();
            this.managers.relationships.init(characters);
            this.managers.events.init(activities, events, missions);
            this.managers.battle.init();
            this.managers.game.init(this.managers);
            this.managers.ui.init();

            this.initialized = true;

            console.log('='.repeat(50));
            console.log('初始化完成！');
            console.log('='.repeat(50));

            // 显示加载进度
            this.simulateLoading();

        } catch (error) {
            console.error('初始化失败:', error);
            alert('游戏初始化失败，请刷新页面重试');
        }
    }

    /**
     * 模拟加载进度
     */
    simulateLoading() {
        const progressBar = document.querySelector('.loading-progress');
        if (!progressBar) return;

        let progress = 0;
        const interval = setInterval(() => {
            progress += Math.random() * 15;
            if (progress >= 100) {
                progress = 100;
                clearInterval(interval);

                // 显示开始按钮
                setTimeout(() => {
                    document.getElementById('start-game').style.display = 'inline-block';
                    document.getElementById('load-game').style.display = 'inline-block';
                }, 300);
            }
            progressBar.style.width = `${progress}%`;
        }, 150);
    }

    /**
     * 启动游戏
     */
    start() {
        if (!this.initialized) {
            console.error('游戏未初始化');
            return;
        }

        console.log('游戏开始！');
    }
}

/**
 * 全局调试函数
 */
window.debugGame = {
    getManagers: () => window.game.managers,

    setDay: (day) => {
        window.game.managers.time.currentDay = day;
        window.game.managers.ui.updateTimeDisplay();
        console.log(`Day set to ${day}`);
    },

    setAttribute: (attr, level) => {
        window.game.managers.attributes.attributes[attr].level = level;
        window.game.managers.ui.updateAttributesDisplay();
        console.log(`${attr} set to level ${level}`);
    },

    setRelationship: (charId, level) => {
        if (window.game.managers.relationships.relationships[charId]) {
            window.game.managers.relationships.relationships[charId].level = level;
            console.log(`Relationship with ${charId} set to ${level}`);
        }
    },

    startBattle: (enemyId) => {
        const enemy = enemies.find(e => e.id === enemyId);
        if (enemy) {
            const combat = window.game.managers.attributes.getLevel('combat');
            window.game.managers.battle.startBattle(enemy, combat);
            window.game.managers.ui.switchView('battle-view');
            window.game.managers.ui.updateBattleDisplay();
            console.log(`Battle started with ${enemy.name}`);
        }
    },

    save: () => {
        window.game.managers.game.saveGame();
    },

    load: () => {
        const saveData = JSON.parse(localStorage.getItem('mindlink_save'));
        window.game.managers.game.loadGame(saveData);
    },

    clearSave: () => {
        localStorage.removeItem('mindlink_save');
        console.log('Save data cleared');
    },

    showStats: () => {
        const time = window.game.managers.time.getCurrentTime();
        const attrs = window.game.managers.attributes.getAll();
        const rels = window.game.managers.relationships.getAll();

        console.log('=== 游戏状态 ===');
        console.log(`Day ${time.day} - ${time.periodName}`);
        console.log('属性:', attrs);
        console.log('好感度:', rels);
        console.log('=============');
    }
};

/**
 * 当DOM加载完成后启动游戏
 */
document.addEventListener('DOMContentLoaded', async () => {
    console.log('DOM加载完成，启动游戏...');

    // 创建游戏实例
    window.game = new MindLinkGame();

    // 初始化游戏
    await window.game.init();

    console.log('\n调试命令已注入到 window.debugGame');
    console.log('可用命令:');
    console.log('- debugGame.showStats() - 显示当前状态');
    console.log('- debugGame.setDay(n) - 设置天数');
    console.log('- debugGame.setAttribute("knowledge", 5) - 设置属性');
    console.log('- debugGame.startBattle("enemy_shadow") - 开始战斗');
    console.log('- debugGame.save() - 保存游戏');
    console.log('- debugGame.load() - 加载游戏');
    console.log('');
});

// 导出供调试使用
export { MindLinkGame };
