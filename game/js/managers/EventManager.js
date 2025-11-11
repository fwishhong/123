/**
 * EventManager - 事件管理器
 * 管理游戏事件和活动
 */

export class EventManager {
    constructor() {
        this.activities = [];
        this.events = [];
        this.missions = [];
        this.completedEvents = new Set();
        this.completedMissions = new Set();
        this.activeFlags = new Set();
    }

    /**
     * 初始化
     */
    init(activities, events, missions) {
        this.activities = activities || [];
        this.events = events || [];
        this.missions = missions || [];

        console.log('[EventManager] Initialized');
        console.log(`- Activities: ${this.activities.length}`);
        console.log(`- Events: ${this.events.length}`);
        console.log(`- Missions: ${this.missions.length}`);
    }

    /**
     * 重置
     */
    reset() {
        this.completedEvents.clear();
        this.completedMissions.clear();
        this.activeFlags.clear();
    }

    /**
     * 获取当前可用活动列表
     */
    getAvailableActivities(currentDay, currentPeriod, attributes, relationships) {
        return this.activities.filter(activity => {
            return this.checkEventConditions(activity, currentDay, currentPeriod, attributes, relationships);
        });
    }

    /**
     * 获取活动
     */
    getActivity(activityId) {
        return this.activities.find(a => a.id === activityId);
    }

    /**
     * 检查事件触发条件
     */
    checkEventConditions(event, currentDay, currentPeriod, attributes, relationships) {
        const cond = event.conditions;
        if (!cond) return true;

        // 检查日期范围
        if (cond.date_range) {
            if (currentDay < cond.date_range[0] || currentDay > cond.date_range[1]) {
                return false;
            }
        }

        // 检查时间段
        if (cond.time_period !== undefined && cond.time_period !== currentPeriod) {
            return false;
        }

        // 检查属性要求
        if (cond.required_attributes) {
            for (const [attr, required] of Object.entries(cond.required_attributes)) {
                if (attributes[attr] < required) {
                    return false;
                }
            }
        }

        // 检查好感度要求
        if (cond.required_relationship) {
            const charId = cond.required_relationship.character_id;
            const required = cond.required_relationship.level;
            if (relationships[charId] < required) {
                return false;
            }
        }

        // 检查必需标志
        if (cond.required_flags) {
            for (const flag of cond.required_flags) {
                if (!this.activeFlags.has(flag)) {
                    return false;
                }
            }
        }

        // 检查互斥标志
        if (cond.forbidden_flags) {
            for (const flag of cond.forbidden_flags) {
                if (this.activeFlags.has(flag)) {
                    return false;
                }
            }
        }

        // 检查是否已完成
        if (event.once && this.completedEvents.has(event.id)) {
            return false;
        }

        // 检查随机概率
        if (cond.random_chance && Math.random() > cond.random_chance) {
            return false;
        }

        return true;
    }

    /**
     * 触发自动事件
     */
    triggerAutoEvents() {
        // 这里可以检查是否有自动触发的事件
        return null;
    }

    /**
     * 完成事件
     */
    completeEvent(eventId) {
        this.completedEvents.add(eventId);
    }

    /**
     * 完成任务
     */
    completeMission(missionId) {
        this.completedMissions.add(missionId);
    }

    /**
     * 设置标志
     */
    setFlag(flag) {
        this.activeFlags.add(flag);
    }

    /**
     * 移除标志
     */
    removeFlag(flag) {
        this.activeFlags.delete(flag);
    }

    /**
     * 检查标志
     */
    hasFlag(flag) {
        return this.activeFlags.has(flag);
    }

    /**
     * 检查截止日期
     */
    checkDeadlines(currentDay) {
        const failedMissions = [];

        for (const mission of this.missions) {
            if (mission.deadline &&
                currentDay > mission.deadline &&
                !this.completedMissions.has(mission.id)) {
                failedMissions.push(mission);
            }
        }

        return failedMissions;
    }

    /**
     * 获取已完成的任务
     */
    getCompletedMissions() {
        return Array.from(this.completedMissions);
    }

    /**
     * 获取活跃任务
     */
    getActiveMissions() {
        return this.missions.filter(m =>
            !this.completedMissions.has(m.id) &&
            (!m.deadline || m.deadline >= 0)
        );
    }

    /**
     * 获取存档数据
     */
    getSaveData() {
        return {
            completedEvents: Array.from(this.completedEvents),
            completedMissions: Array.from(this.completedMissions),
            activeFlags: Array.from(this.activeFlags)
        };
    }

    /**
     * 加载数据
     */
    load(data) {
        if (data) {
            this.completedEvents = new Set(data.completedEvents || []);
            this.completedMissions = new Set(data.completedMissions || []);
            this.activeFlags = new Set(data.activeFlags || []);
        }
    }
}
