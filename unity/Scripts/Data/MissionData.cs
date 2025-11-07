using UnityEngine;
using System;
using System.Collections.Generic;

namespace MindLink.Data
{
    /// <summary>
    /// 任务数据 - ScriptableObject
    /// 用于配置游戏中的所有任务/关卡
    /// </summary>
    [CreateAssetMenu(fileName = "New Mission", menuName = "MindLink/Mission Data")]
    public class MissionData : ScriptableObject
    {
        [Header("基本信息")]
        [Tooltip("任务唯一ID")]
        public string missionId;

        [Tooltip("任务名称")]
        public string missionName;

        [TextArea(3, 6)]
        [Tooltip("任务描述")]
        public string description;

        [Tooltip("任务类型")]
        public MissionType missionType = MissionType.Main;

        [Tooltip("任务图标")]
        public Sprite icon;

        [Header("触发条件")]
        [Tooltip("触发任务的天数范围")]
        public DayRange triggerDayRange = new DayRange { minDay = 1, maxDay = 100 };

        [Tooltip("需要的标记")]
        public List<string> requiredFlags = new List<string>();

        [Tooltip("需要的属性等级")]
        public List<AttributeRequirement> attributeRequirements = new List<AttributeRequirement>();

        [Tooltip("需要的关系等级")]
        public List<RelationshipRequirement> relationshipRequirements = new List<RelationshipRequirement>();

        [Tooltip("需要完成的前置任务")]
        public List<string> prerequisiteMissions = new List<string>();

        [Tooltip("自动触发（满足条件时自动开始）")]
        public bool autoTrigger = false;

        [Header("时间限制")]
        [Tooltip("是否有截止日期")]
        public bool hasDeadline = true;

        [Tooltip("持续天数（从接受任务开始算）")]
        public int durationDays = 10;

        [Tooltip("失败后是否Game Over")]
        public bool gameOverOnFailure = false;

        [Header("任务目标")]
        [Tooltip("任务目标列表")]
        public List<MissionObjectiveData> objectives = new List<MissionObjectiveData>();

        [Tooltip("是否需要完成所有目标")]
        public bool requireAllObjectives = true;

        [Header("奖励")]
        [Tooltip("属性经验奖励")]
        public List<AttributeChange> attributeRewards = new List<AttributeChange>();

        [Tooltip("关系经验奖励")]
        public List<RelationshipChange> relationshipRewards = new List<RelationshipChange>();

        [Tooltip("道具奖励")]
        public List<ItemReward> itemRewards = new List<ItemReward>();

        [Tooltip("设置的标记")]
        public List<FlagChange> flagRewards = new List<FlagChange>();

        [Tooltip("解锁的内容")]
        public List<string> unlockContent = new List<string>();

        [Header("对话")]
        [Tooltip("接受任务时的对话")]
        public DialogueData acceptDialogue;

        [Tooltip("任务进行中的对话")]
        public DialogueData progressDialogue;

        [Tooltip("完成任务时的对话")]
        public DialogueData completeDialogue;

        [Tooltip("任务失败时的对话")]
        public DialogueData failDialogue;

        [Header("关联角色")]
        [Tooltip("任务发布者")]
        public string questGiver;

        [Tooltip("相关角色")]
        public List<string> relatedCharacters = new List<string>();

        [Header("特殊设置")]
        [Tooltip("任务优先级（用于排序显示）")]
        [Range(0, 100)]
        public int priority = 50;

        [Tooltip("是否可以放弃")]
        public bool canAbandon = true;

        [Tooltip("是否显示在任务列表")]
        public bool showInQuestLog = true;

        [Tooltip("完成后是否隐藏")]
        public bool hideWhenComplete = false;

        [Tooltip("任务章节/阶段")]
        public int chapter = 1;

        #region Helper Methods
        /// <summary>
        /// 检查是否满足接受任务的条件
        /// </summary>
        public bool CanAccept(GameState gameState)
        {
            // 检查天数范围
            if (!triggerDayRange.IsInRange(gameState.CurrentDay))
                return false;

            // 检查标记
            foreach (var flag in requiredFlags)
            {
                if (!gameState.GetFlag(flag))
                    return false;
            }

            // 检查属性
            foreach (var req in attributeRequirements)
            {
                var attr = gameState.GetAttribute(req.attributeType);
                if (attr == null || attr.Level < req.requiredLevel)
                    return false;
            }

            // 检查关系
            foreach (var req in relationshipRequirements)
            {
                var rel = gameState.GetRelationship(req.characterId);
                if (rel == null || rel.Level < req.requiredLevel)
                    return false;
            }

            // 检查前置任务
            foreach (var prereq in prerequisiteMissions)
            {
                if (!gameState.CompletedMissions.Contains(prereq))
                    return false;
            }

            // 检查是否已经接受或完成
            if (gameState.CompletedMissions.Contains(missionId))
                return false;

            if (gameState.ActiveMissions.Exists(m => m.MissionId == missionId))
                return false;

            return true;
        }

        /// <summary>
        /// 创建任务进度对象
        /// </summary>
        public MissionProgress CreateMissionProgress(int startDay)
        {
            var progress = new MissionProgress
            {
                MissionId = missionId,
                Name = missionName,
                Description = description,
                StartDay = startDay,
                Deadline = hasDeadline ? startDay + durationDays : 999,
                IsMainMission = (missionType == MissionType.Main),
                Objectives = new Dictionary<string, ObjectiveProgress>()
            };

            // 初始化所有目标
            foreach (var obj in objectives)
            {
                progress.Objectives[obj.objectiveId] = new ObjectiveProgress
                {
                    Description = obj.description,
                    TargetValue = obj.targetValue,
                    CurrentValue = obj.initialValue,
                    IsOptional = obj.isOptional
                };
            }

            return progress;
        }

        /// <summary>
        /// 应用任务奖励
        /// </summary>
        public void ApplyRewards(GameState gameState)
        {
            // 应用属性奖励
            foreach (var reward in attributeRewards)
            {
                AttributeManager.Instance?.AddAttributeExp(reward.attributeType, reward.amount);
            }

            // 应用关系奖励
            foreach (var reward in relationshipRewards)
            {
                RelationshipManager.Instance?.AddRelationshipExp(reward.characterId, reward.amount);
            }

            // 给予道具
            foreach (var item in itemRewards)
            {
                gameState.AddItem(item.itemId, item.amount);
            }

            // 设置标记
            foreach (var flag in flagRewards)
            {
                gameState.SetFlag(flag.flagName, flag.value);
            }

            // 解锁内容
            foreach (var content in unlockContent)
            {
                gameState.SetFlag($"unlocked_{content}", true);
            }

            // 记录完成
            if (!gameState.CompletedMissions.Contains(missionId))
            {
                gameState.CompletedMissions.Add(missionId);
            }
        }

        /// <summary>
        /// 验证数据完整性
        /// </summary>
        public bool Validate(out string error)
        {
            error = "";

            if (string.IsNullOrEmpty(missionId))
            {
                error = "Mission ID is empty";
                return false;
            }

            if (string.IsNullOrEmpty(missionName))
            {
                error = "Mission name is empty";
                return false;
            }

            if (objectives.Count == 0)
            {
                error = "No mission objectives defined";
                return false;
            }

            // 检查目标ID唯一性
            HashSet<string> objectiveIds = new HashSet<string>();
            foreach (var obj in objectives)
            {
                if (string.IsNullOrEmpty(obj.objectiveId))
                {
                    error = "Objective has empty ID";
                    return false;
                }

                if (objectiveIds.Contains(obj.objectiveId))
                {
                    error = $"Duplicate objective ID: {obj.objectiveId}";
                    return false;
                }

                objectiveIds.Add(obj.objectiveId);
            }

            if (hasDeadline && durationDays <= 0)
            {
                error = "Duration days must be positive when deadline is enabled";
                return false;
            }

            return true;
        }
        #endregion
    }

    #region Supporting Data Structures
    /// <summary>
    /// 任务目标数据
    /// </summary>
    [Serializable]
    public class MissionObjectiveData
    {
        [Tooltip("目标唯一ID")]
        public string objectiveId;

        [Tooltip("目标描述")]
        public string description;

        [Tooltip("目标类型")]
        public ObjectiveType objectiveType = ObjectiveType.Custom;

        [Tooltip("目标值（需要达到的数量）")]
        public int targetValue = 1;

        [Tooltip("初始值")]
        public int initialValue = 0;

        [Tooltip("是否为可选目标")]
        public bool isOptional = false;

        [Tooltip("目标完成时触发的对话")]
        public DialogueData completionDialogue;

        [Tooltip("目标完成时触发的事件")]
        public string completionEventId;

        [Tooltip("目标在UI中的图标")]
        public Sprite icon;

        [Header("自动检测设置")]
        [Tooltip("自动检测类型（用于自动更新进度）")]
        public AutoTrackType autoTrack = AutoTrackType.None;

        [Tooltip("检测的属性类型（如果autoTrack = AttributeLevel）")]
        public AttributeType trackedAttribute;

        [Tooltip("检测的角色ID（如果autoTrack = RelationshipLevel）")]
        public string trackedCharacterId;

        [Tooltip("检测的标记名（如果autoTrack = FlagSet）")]
        public string trackedFlag;

        [Tooltip("检测的道具ID（如果autoTrack = ItemCollected）")]
        public string trackedItemId;
    }

    /// <summary>
    /// 任务类型
    /// </summary>
    public enum MissionType
    {
        Main,           // 主线任务
        Side,           // 支线任务
        Character,      // 角色任务
        Daily,          // 每日任务
        Tutorial,       // 教程任务
        Challenge,      // 挑战任务
        Hidden          // 隐藏任务
    }

    /// <summary>
    /// 目标类型
    /// </summary>
    public enum ObjectiveType
    {
        Custom,             // 自定义（手动更新）
        AttributeLevel,     // 属性达到等级
        RelationshipLevel,  // 关系达到等级
        CollectItems,       // 收集道具
        CompleteEvents,     // 完成事件
        WinBattles,         // 战斗胜利
        SolvePuzzles,       // 解谜
        TalkToCharacter,    // 与角色对话
        ReachDay,           // 到达指定天数
        SetFlag             // 设置特定标记
    }

    /// <summary>
    /// 自动追踪类型
    /// </summary>
    public enum AutoTrackType
    {
        None,               // 不自动追踪
        AttributeLevel,     // 自动检测属性等级
        RelationshipLevel,  // 自动检测关系等级
        ItemCollected,      // 自动检测道具数量
        FlagSet,            // 自动检测标记状态
        EventCompleted,     // 自动检测事件完成
        BattleWon,          // 自动检测战斗胜利
        DayReached          // 自动检测天数
    }
    #endregion
}
