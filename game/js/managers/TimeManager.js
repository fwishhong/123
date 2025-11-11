/**
 * TimeManager - 时间管理器
 * 管理游戏的时间系统：100天，每天3个时间段
 */

export class TimeManager {
    constructor() {
        this.currentDay = 1;
        this.currentPeriod = 0; // 0: 早晨, 1: 下午, 2: 夜晚
        this.periodNames = ['早晨', '下午', '夜晚'];
    }

    /**
     * 初始化
     */
    init() {
        this.reset();
        console.log('[TimeManager] Initialized');
    }

    /**
     * 重置时间
     */
    reset() {
        this.currentDay = 1;
        this.currentPeriod = 0;
    }

    /**
     * 推进时间
     * @returns {Object} 包含是否改变日期等信息
     */
    advance() {
        this.currentPeriod++;

        let dayChanged = false;

        if (this.currentPeriod >= 3) {
            this.currentPeriod = 0;
            this.currentDay++;
            dayChanged = true;
        }

        return {
            currentDay: this.currentDay,
            currentPeriod: this.currentPeriod,
            periodName: this.getPeriodName(),
            dayChanged: dayChanged
        };
    }

    /**
     * 获取当前时间段名称
     */
    getPeriodName() {
        return this.periodNames[this.currentPeriod];
    }

    /**
     * 获取当前时间信息
     */
    getCurrentTime() {
        return {
            day: this.currentDay,
            period: this.currentPeriod,
            periodName: this.getPeriodName()
        };
    }

    /**
     * 获取总时间段数（用于进度计算）
     */
    getTotalPeriods() {
        return (this.currentDay - 1) * 3 + this.currentPeriod;
    }

    /**
     * 计算距离某个日期的剩余时间
     */
    getDaysUntil(targetDay) {
        return Math.max(0, targetDay - this.currentDay);
    }

    /**
     * 检查是否是周末
     */
    isWeekend() {
        const dayOfWeek = this.currentDay % 7;
        return dayOfWeek === 0 || dayOfWeek === 6;
    }

    /**
     * 获取存档数据
     */
    getSaveData() {
        return {
            currentDay: this.currentDay,
            currentPeriod: this.currentPeriod
        };
    }

    /**
     * 加载数据
     */
    load(data) {
        if (data) {
            this.currentDay = data.currentDay || 1;
            this.currentPeriod = data.currentPeriod || 0;
        }
    }
}
