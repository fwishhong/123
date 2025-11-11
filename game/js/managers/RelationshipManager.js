/**
 * RelationshipManager - 好感度管理器
 * 管理与8个角色的关系
 */

export class RelationshipManager {
    constructor() {
        this.relationships = {};
        this.maxLevel = 10;
        this.decayRate = 0.1; // 每天衰减率
    }

    /**
     * 初始化
     */
    init(characters) {
        this.reset();

        // 为每个角色初始化好感度
        for (const char of characters) {
            this.relationships[char.id] = {
                level: 0,
                exp: 0,
                lastInteraction: 0 // 最后互动的天数
            };
        }

        console.log('[RelationshipManager] Initialized');
    }

    /**
     * 重置所有关系
     */
    reset() {
        this.relationships = {};
    }

    /**
     * 修改好感度
     * @param {string} charId - 角色ID
     * @param {number} expGain - 获得的经验值
     * @param {number} currentDay - 当前天数
     */
    modify(charId, expGain, currentDay = 0) {
        if (!this.relationships[charId]) {
            console.error('Invalid character ID:', charId);
            return { leveledUp: false };
        }

        const rel = this.relationships[charId];
        const oldLevel = rel.level;

        rel.exp += expGain;
        rel.lastInteraction = currentDay;

        // 升级阈值：每级需要10点经验
        const expPerLevel = 10;

        while (rel.level < this.maxLevel && rel.exp >= expPerLevel) {
            rel.exp -= expPerLevel;
            rel.level++;
        }

        if (rel.level >= this.maxLevel) {
            rel.level = this.maxLevel;
            rel.exp = 0;
        }

        const leveledUp = rel.level > oldLevel;

        return {
            charId: charId,
            oldLevel: oldLevel,
            newLevel: rel.level,
            leveledUp: leveledUp,
            expGain: expGain
        };
    }

    /**
     * 获取好感度等级
     */
    getLevel(charId) {
        return this.relationships[charId]?.level || 0;
    }

    /**
     * 获取所有好感度
     */
    getAll() {
        const result = {};
        for (const [charId, rel] of Object.entries(this.relationships)) {
            result[charId] = rel.level;
        }
        return result;
    }

    /**
     * 获取好感度详情
     */
    getDetails(charId) {
        const rel = this.relationships[charId];
        if (!rel) return null;

        const expPerLevel = 10;

        return {
            level: rel.level,
            exp: rel.exp,
            expRequired: rel.level < this.maxLevel ? expPerLevel : 0,
            progress: rel.level < this.maxLevel ? rel.exp / expPerLevel : 1,
            lastInteraction: rel.lastInteraction
        };
    }

    /**
     * 每日衰减（长时间不互动会降低）
     */
    dailyDecay(currentDay) {
        for (const [charId, rel] of Object.entries(this.relationships)) {
            const daysSinceInteraction = currentDay - rel.lastInteraction;

            // 超过7天未互动，开始衰减
            if (daysSinceInteraction > 7 && rel.level > 0) {
                rel.exp -= this.decayRate;

                // 如果经验为负，降级
                if (rel.exp < 0) {
                    rel.level = Math.max(0, rel.level - 1);
                    rel.exp = rel.level > 0 ? 5 : 0; // 降级后保留一些经验
                }
            }
        }
    }

    /**
     * 获取总好感度等级
     */
    getTotalLevel() {
        return Object.values(this.relationships).reduce((sum, rel) => sum + rel.level, 0);
    }

    /**
     * 获取高好感度角色列表
     */
    getHighRelationshipCharacters(minLevel = 8) {
        return Object.entries(this.relationships)
            .filter(([_, rel]) => rel.level >= minLevel)
            .map(([charId, _]) => charId);
    }

    /**
     * 检查是否可以触发角色事件
     */
    canTriggerEvent(charId, requiredLevel) {
        return this.getLevel(charId) >= requiredLevel;
    }

    /**
     * 获取存档数据
     */
    getSaveData() {
        return {
            relationships: JSON.parse(JSON.stringify(this.relationships))
        };
    }

    /**
     * 加载数据
     */
    load(data) {
        if (data && data.relationships) {
            this.relationships = JSON.parse(JSON.stringify(data.relationships));
        }
    }
}
