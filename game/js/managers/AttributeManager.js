/**
 * AttributeManager - 属性管理器
 * 管理玩家的4项属性：知识、勇气、魅力、战斗力
 */

export class AttributeManager {
    constructor() {
        this.attributes = {
            knowledge: { level: 1, exp: 0 },
            courage: { level: 1, exp: 0 },
            charm: { level: 1, exp: 0 },
            combat: { level: 1, exp: 0 }
        };

        // 每级所需经验（递增）
        this.expRequired = [0, 10, 25, 45, 70, 100, 135, 175, 220, 270, 325];
        this.maxLevel = 10;
    }

    /**
     * 初始化
     */
    init() {
        this.reset();
        console.log('[AttributeManager] Initialized');
    }

    /**
     * 重置属性
     */
    reset() {
        this.attributes = {
            knowledge: { level: 1, exp: 0 },
            courage: { level: 1, exp: 0 },
            charm: { level: 1, exp: 0 },
            combat: { level: 1, exp: 0 }
        };
    }

    /**
     * 修改属性（增加经验）
     * @param {string} attrName - 属性名称
     * @param {number} expGain - 获得的经验值
     * @returns {Object} 包含是否升级等信息
     */
    modify(attrName, expGain) {
        if (!this.attributes[attrName]) {
            console.error('Invalid attribute:', attrName);
            return { leveledUp: false };
        }

        const attr = this.attributes[attrName];
        const oldLevel = attr.level;

        attr.exp += expGain;

        // 检查升级
        while (attr.level < this.maxLevel && attr.exp >= this.expRequired[attr.level]) {
            attr.exp -= this.expRequired[attr.level];
            attr.level++;
        }

        // 如果达到最大等级，清空多余经验
        if (attr.level >= this.maxLevel) {
            attr.exp = 0;
        }

        const leveledUp = attr.level > oldLevel;

        return {
            attrName: attrName,
            oldLevel: oldLevel,
            newLevel: attr.level,
            leveledUp: leveledUp,
            expGain: expGain
        };
    }

    /**
     * 获取属性等级
     */
    getLevel(attrName) {
        return this.attributes[attrName]?.level || 0;
    }

    /**
     * 获取所有属性等级
     */
    getAll() {
        return {
            knowledge: this.attributes.knowledge.level,
            courage: this.attributes.courage.level,
            charm: this.attributes.charm.level,
            combat: this.attributes.combat.level
        };
    }

    /**
     * 获取属性详细信息
     */
    getDetails(attrName) {
        const attr = this.attributes[attrName];
        if (!attr) return null;

        return {
            level: attr.level,
            exp: attr.exp,
            expRequired: attr.level < this.maxLevel ? this.expRequired[attr.level] : 0,
            progress: attr.level < this.maxLevel ? attr.exp / this.expRequired[attr.level] : 1
        };
    }

    /**
     * 获取所有属性总和
     */
    getTotal() {
        return Object.values(this.attributes).reduce((sum, attr) => sum + attr.level, 0);
    }

    /**
     * 检查是否满足属性要求
     */
    checkRequirements(requirements) {
        for (const [attr, required] of Object.entries(requirements)) {
            if (this.getLevel(attr) < required) {
                return false;
            }
        }
        return true;
    }

    /**
     * 获取属性中文名
     */
    getAttributeName(attrName) {
        const names = {
            knowledge: '知识',
            courage: '勇气',
            charm: '魅力',
            combat: '战斗力'
        };
        return names[attrName] || attrName;
    }

    /**
     * 获取存档数据
     */
    getSaveData() {
        return {
            attributes: JSON.parse(JSON.stringify(this.attributes))
        };
    }

    /**
     * 加载数据
     */
    load(data) {
        if (data && data.attributes) {
            this.attributes = JSON.parse(JSON.stringify(data.attributes));
        }
    }
}
