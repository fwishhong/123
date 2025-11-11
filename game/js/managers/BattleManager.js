/**
 * BattleManager - 战斗管理器
 * 简化的回合制战斗系统
 */

export class BattleManager {
    constructor() {
        this.inBattle = false;
        this.currentEnemy = null;
        this.playerStats = null;
        this.battleLog = [];
    }

    /**
     * 初始化
     */
    init() {
        console.log('[BattleManager] Initialized');
    }

    /**
     * 开始战斗
     */
    startBattle(enemy, playerCombatLevel) {
        this.inBattle = true;
        this.currentEnemy = {
            ...enemy,
            currentHP: enemy.maxHP
        };

        // 根据战斗力计算玩家属性
        this.playerStats = {
            maxHP: 100 + playerCombatLevel * 20,
            currentHP: 100 + playerCombatLevel * 20,
            attack: 10 + playerCombatLevel * 5,
            defense: 5 + playerCombatLevel * 3
        };

        this.battleLog = [];
        this.addLog('战斗开始！');
        this.addLog(`${enemy.name} 出现了！`);

        return {
            enemy: this.currentEnemy,
            player: this.playerStats
        };
    }

    /**
     * 玩家行动
     */
    playerAction(action) {
        if (!this.inBattle) return null;

        const result = {
            playerAction: null,
            enemyAction: null,
            battleEnd: false,
            victory: false
        };

        // 玩家行动
        switch (action) {
            case 'attack':
                result.playerAction = this.playerAttack();
                break;
            case 'skill':
                result.playerAction = this.playerSkill();
                break;
            case 'defend':
                result.playerAction = this.playerDefend();
                break;
            case 'item':
                result.playerAction = this.useItem();
                break;
        }

        // 检查敌人是否死亡
        if (this.currentEnemy.currentHP <= 0) {
            this.addLog(`${this.currentEnemy.name} 被击败了！`);
            this.addLog('战斗胜利！');
            this.inBattle = false;
            result.battleEnd = true;
            result.victory = true;
            return result;
        }

        // 敌人回合
        result.enemyAction = this.enemyTurn();

        // 检查玩家是否死亡
        if (this.playerStats.currentHP <= 0) {
            this.addLog('你被击败了...');
            this.inBattle = false;
            result.battleEnd = true;
            result.victory = false;
            return result;
        }

        return result;
    }

    /**
     * 玩家攻击
     */
    playerAttack() {
        const damage = Math.floor(this.playerStats.attack * (0.8 + Math.random() * 0.4));
        this.currentEnemy.currentHP -= damage;
        this.addLog(`你造成了 ${damage} 点伤害！`);
        return { type: 'attack', damage };
    }

    /**
     * 玩家使用技能
     */
    playerSkill() {
        const damage = Math.floor(this.playerStats.attack * 1.5 * (0.8 + Math.random() * 0.4));
        this.currentEnemy.currentHP -= damage;
        this.addLog(`你使用了技能，造成 ${damage} 点伤害！`);
        return { type: 'skill', damage };
    }

    /**
     * 玩家防御
     */
    playerDefend() {
        this.addLog('你进入了防御姿态！');
        return { type: 'defend', defenseBonus: 0.5 };
    }

    /**
     * 使用道具
     */
    useItem() {
        const heal = 30;
        this.playerStats.currentHP = Math.min(
            this.playerStats.maxHP,
            this.playerStats.currentHP + heal
        );
        this.addLog(`你使用了恢复道具，恢复了 ${heal} HP！`);
        return { type: 'item', heal };
    }

    /**
     * 敌人回合
     */
    enemyTurn() {
        const damage = Math.floor(this.currentEnemy.attack * (0.8 + Math.random() * 0.4));
        this.playerStats.currentHP -= damage;
        this.addLog(`${this.currentEnemy.name} 攻击了你，造成 ${damage} 点伤害！`);
        return { type: 'attack', damage };
    }

    /**
     * 添加战斗日志
     */
    addLog(message) {
        this.battleLog.push(message);
        if (this.battleLog.length > 10) {
            this.battleLog.shift();
        }
    }

    /**
     * 获取战斗日志
     */
    getLog() {
        return this.battleLog;
    }

    /**
     * 获取当前战斗状态
     */
    getBattleState() {
        return {
            inBattle: this.inBattle,
            enemy: this.currentEnemy,
            player: this.playerStats,
            log: this.battleLog
        };
    }

    /**
     * 结束战斗
     */
    endBattle() {
        this.inBattle = false;
        this.currentEnemy = null;
        this.playerStats = null;
        this.battleLog = [];
    }
}
