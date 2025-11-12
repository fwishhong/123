/**
 * SkillManager - 技能系统管理器
 */

export class SkillManager {
    constructor() {
        this.unlockedSkills = new Set(['skill_normal_attack']); // 默认解锁普通攻击
        this.equippedSkills = ['skill_normal_attack']; // 最多装备4个技能
        this.maxEquipped = 4;
        this.playerSP = 100; // 技能点数
        this.maxSP = 100;
    }

    /**
     * 初始化
     */
    init(skills) {
        this.skills = skills || [];
        console.log('[SkillManager] Initialized with', this.skills.length, 'skills');
    }

    /**
     * 重置
     */
    reset() {
        this.unlockedSkills = new Set(['skill_normal_attack']);
        this.equippedSkills = ['skill_normal_attack'];
        this.playerSP = 100;
    }

    /**
     * 解锁技能
     */
    unlockSkill(skillId) {
        this.unlockedSkills.add(skillId);
        console.log('[Skill] Unlocked:', skillId);
        return true;
    }

    /**
     * 装备技能
     */
    equipSkill(skillId) {
        if (!this.unlockedSkills.has(skillId)) {
            return { success: false, message: '技能未解锁' };
        }

        if (this.equippedSkills.includes(skillId)) {
            return { success: false, message: '技能已装备' };
        }

        if (this.equippedSkills.length >= this.maxEquipped) {
            return { success: false, message: '技能栏已满' };
        }

        this.equippedSkills.push(skillId);
        return { success: true };
    }

    /**
     * 卸下技能
     */
    unequipSkill(skillId) {
        const index = this.equippedSkills.indexOf(skillId);
        if (index === -1) {
            return { success: false, message: '技能未装备' };
        }

        // 不能卸下普通攻击
        if (skillId === 'skill_normal_attack') {
            return { success: false, message: '不能卸下基础技能' };
        }

        this.equippedSkills.splice(index, 1);
        return { success: true };
    }

    /**
     * 使用技能
     */
    useSkill(skillId, target) {
        const skill = this.getSkill(skillId);
        if (!skill) {
            return { success: false, message: '技能不存在' };
        }

        if (!this.equippedSkills.includes(skillId)) {
            return { success: false, message: '技能未装备' };
        }

        if (this.playerSP < skill.cost) {
            return { success: false, message: 'SP不足' };
        }

        // 消耗SP
        this.playerSP -= skill.cost;

        // 计算技能效果
        const result = {
            success: true,
            skill: skill,
            spUsed: skill.cost,
            spRemaining: this.playerSP
        };

        // 根据技能类型计算效果
        if (skill.type === 'physical' || skill.type === 'mental') {
            // 攻击技能
            result.damage = Math.floor(target.baseAttack * skill.power * (0.9 + Math.random() * 0.2));
        } else if (skill.type === 'support') {
            // 辅助技能
            if (skill.healing) {
                result.healing = Math.floor(target.maxHP * skill.healing);
            }
            if (skill.effect) {
                result.effect = skill.effect;
            }
        } else if (skill.type === 'ultimate') {
            // 终极技能
            result.damage = Math.floor(target.baseAttack * skill.power * (0.9 + Math.random() * 0.2));
            result.effect = 'ultimate';
        }

        return result;
    }

    /**
     * 恢复SP
     */
    restoreSP(amount) {
        this.playerSP = Math.min(this.maxSP, this.playerSP + amount);
        return this.playerSP;
    }

    /**
     * 战斗结束后恢复
     */
    resetSP() {
        this.playerSP = this.maxSP;
    }

    /**
     * 检查技能是否解锁
     */
    checkUnlocks(combatLevel) {
        let newUnlocks = [];

        for (const skill of this.skills) {
            if (!this.unlockedSkills.has(skill.id) && skill.unlockLevel && combatLevel >= skill.unlockLevel) {
                this.unlockSkill(skill.id);
                newUnlocks.push(skill);
            }
        }

        return newUnlocks;
    }

    /**
     * 获取技能
     */
    getSkill(skillId) {
        return this.skills.find(s => s.id === skillId);
    }

    /**
     * 获取已装备的技能
     */
    getEquippedSkills() {
        return this.equippedSkills.map(id => this.getSkill(id)).filter(s => s);
    }

    /**
     * 获取所有已解锁的技能
     */
    getUnlockedSkills() {
        return this.skills.filter(s => this.unlockedSkills.has(s.id));
    }

    /**
     * 获取当前SP
     */
    getCurrentSP() {
        return this.playerSP;
    }

    /**
     * 获取存档数据
     */
    getSaveData() {
        return {
            unlockedSkills: Array.from(this.unlockedSkills),
            equippedSkills: this.equippedSkills,
            playerSP: this.playerSP,
            maxSP: this.maxSP
        };
    }

    /**
     * 加载数据
     */
    load(data) {
        if (data) {
            this.unlockedSkills = new Set(data.unlockedSkills || ['skill_normal_attack']);
            this.equippedSkills = data.equippedSkills || ['skill_normal_attack'];
            this.playerSP = data.playerSP || 100;
            this.maxSP = data.maxSP || 100;
        }
    }
}
