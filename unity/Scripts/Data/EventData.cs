using UnityEngine;
using System;
using System.Collections.Generic;

namespace MindLink.Data
{
    /// <summary>
    /// 事件数据 - ScriptableObject
    /// 用于配置所有游戏事件
    /// </summary>
    [CreateAssetMenu(fileName = "New Event", menuName = "MindLink/Event Data")]
    public class EventData : ScriptableObject
    {
        [Header("基本信息")]
        [Tooltip("事件唯一ID")]
        public string eventId;

        [Tooltip("事件名称")]
        public string eventName;

        [TextArea(2, 4)]
        [Tooltip("事件描述")]
        public string description;

        [Tooltip("事件图标")]
        public Sprite icon;

        [Tooltip("事件类型")]
        public EventType eventType;

        [Header("触发条件")]
        [Tooltip("触发的天数范围")]
        public DayRange dayRange;

        [Tooltip("可触发的时间段")]
        public List<Timeslot> availableTimeslots = new List<Timeslot>();

        [Tooltip("仅工作日触发")]
        public bool weekdayOnly = false;

        [Tooltip("仅周末触发")]
        public bool weekendOnly = false;

        [Tooltip("需要的属性等级")]
        public List<AttributeRequirement> attributeRequirements = new List<AttributeRequirement>();

        [Tooltip("需要的关系等级")]
        public List<RelationshipRequirement> relationshipRequirements = new List<RelationshipRequirement>();

        [Tooltip("需要的标记")]
        public List<string> requiredFlags = new List<string>();

        [Tooltip("禁止的标记（有这些标记就不触发）")]
        public List<string> blockedFlags = new List<string>();

        [Tooltip("随机触发概率 (0-1)")]
        [Range(0f, 1f)]
        public float randomChance = 1f;

        [Header("事件效果")]
        [Tooltip("属性变化")]
        public List<AttributeChange> attributeChanges = new List<AttributeChange>();

        [Tooltip("关系变化")]
        public List<RelationshipChange> relationshipChanges = new List<RelationshipChange>();

        [Tooltip("道具奖励")]
        public List<ItemReward> itemRewards = new List<ItemReward>();

        [Tooltip("设置的标记")]
        public List<FlagChange> flagChanges = new List<FlagChange>();

        [Tooltip("触发的对话")]
        public DialogueData triggerDialogue;

        [Header("重复性")]
        [Tooltip("是否可重复触发")]
        public bool repeatable = true;

        [Tooltip("冷却天数（0=无冷却）")]
        public int cooldownDays = 0;

        [Tooltip("每天最多触发次数")]
        public int maxPerDay = 1;

        [Header("任务相关")]
        [Tooltip("是否影响任务进度")]
        public bool affectsMission = false;

        [Tooltip("更新的任务目标")]
        public List<MissionObjectiveUpdate> missionUpdates = new List<MissionObjectiveUpdate>();

        [Header("特殊设置")]
        [Tooltip("是否为自动触发事件（剧情事件）")]
        public bool autoTrigger = false;

        [Tooltip("是否消耗时间段")]
        public bool consumeTimeslot = true;

        [Tooltip("事件优先级（用于自动触发时的排序）")]
        public int priority = 0;

        #region Validation
        /// <summary>
        /// 检查事件是否可以触发
        /// </summary>
        public bool CanTrigger(GameState gameState)
        {
            // 检查天数范围
            if (!dayRange.IsInRange(gameState.CurrentDay))
                return false;

            // 检查时间段
            if (availableTimeslots.Count > 0 && !availableTimeslots.Contains(gameState.CurrentTimeslot))
                return false;

            // 检查工作日/周末
            int dayOfWeek = (gameState.CurrentDay - 1) % 7;
            if (weekdayOnly && dayOfWeek >= 5) return false;
            if (weekendOnly && dayOfWeek < 5) return false;

            // 检查是否已完成（不可重复事件）
            if (!repeatable && gameState.CompletedEvents.Contains(eventId))
                return false;

            // 检查冷却
            if (gameState.EventCooldowns.ContainsKey(eventId) && gameState.EventCooldowns[eventId] > 0)
                return false;

            // 检查属性要求
            foreach (var req in attributeRequirements)
            {
                var attr = gameState.GetAttribute(req.attributeType);
                if (attr == null || attr.Level < req.requiredLevel)
                    return false;
            }

            // 检查关系要求
            foreach (var req in relationshipRequirements)
            {
                var rel = gameState.GetRelationship(req.characterId);
                if (rel == null || rel.Level < req.requiredLevel)
                    return false;
            }

            // 检查必需标记
            foreach (var flag in requiredFlags)
            {
                if (!gameState.GetFlag(flag))
                    return false;
            }

            // 检查禁止标记
            foreach (var flag in blockedFlags)
            {
                if (gameState.GetFlag(flag))
                    return false;
            }

            // 检查随机概率
            if (randomChance < 1f && UnityEngine.Random.value > randomChance)
                return false;

            return true;
        }

        /// <summary>
        /// 执行事件效果
        /// </summary>
        public void Execute(GameState gameState)
        {
            // 应用属性变化
            foreach (var change in attributeChanges)
            {
                AttributeManager.Instance.AddAttributeExp(change.attributeType, change.amount);
            }

            // 应用关系变化
            foreach (var change in relationshipChanges)
            {
                RelationshipManager.Instance.AddRelationshipExp(change.characterId, change.amount);
            }

            // 给予道具
            foreach (var item in itemRewards)
            {
                gameState.AddItem(item.itemId, item.amount);
            }

            // 设置标记
            foreach (var flag in flagChanges)
            {
                gameState.SetFlag(flag.flagName, flag.value);
            }

            // 更新任务进度
            if (affectsMission)
            {
                foreach (var update in missionUpdates)
                {
                    MissionManager.Instance.UpdateMissionObjective(
                        update.missionId,
                        update.objectiveId,
                        update.amount
                    );
                }
            }

            // 记录完成
            if (!repeatable)
            {
                gameState.CompletedEvents.Add(eventId);
            }

            // 设置冷却
            if (cooldownDays > 0)
            {
                gameState.EventCooldowns[eventId] = cooldownDays;
            }

            // 记录今日活动
            gameState.TodayActivities.Add(eventName);
        }
        #endregion
    }

    #region Supporting Data Structures
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum EventType
    {
        Story,          // 剧情事件
        Daily,          // 日常活动
        Social,         // 社交活动
        Study,          // 学习活动
        Training,       // 训练活动
        Investigation,  // 调查活动
        Mission,        // 任务相关
        Special         // 特殊事件
    }

    /// <summary>
    /// 天数范围
    /// </summary>
    [Serializable]
    public struct DayRange
    {
        public int minDay;
        public int maxDay;

        public bool IsInRange(int day)
        {
            return day >= minDay && day <= maxDay;
        }
    }

    /// <summary>
    /// 属性要求
    /// </summary>
    [Serializable]
    public struct AttributeRequirement
    {
        public AttributeType attributeType;
        public int requiredLevel;
    }

    /// <summary>
    /// 关系要求
    /// </summary>
    [Serializable]
    public struct RelationshipRequirement
    {
        public string characterId;
        public int requiredLevel;
    }

    /// <summary>
    /// 属性变化
    /// </summary>
    [Serializable]
    public struct AttributeChange
    {
        public AttributeType attributeType;
        public int amount;
    }

    /// <summary>
    /// 关系变化
    /// </summary>
    [Serializable]
    public struct RelationshipChange
    {
        public string characterId;
        public int amount;
    }

    /// <summary>
    /// 道具奖励
    /// </summary>
    [Serializable]
    public struct ItemReward
    {
        public string itemId;
        public int amount;
    }

    /// <summary>
    /// 标记变化
    /// </summary>
    [Serializable]
    public struct FlagChange
    {
        public string flagName;
        public bool value;
    }

    /// <summary>
    /// 任务目标更新
    /// </summary>
    [Serializable]
    public struct MissionObjectiveUpdate
    {
        public string missionId;
        public string objectiveId;
        public int amount;
    }
    #endregion
}
