using UnityEngine;
using System;
using System.Collections.Generic;

namespace MindLink.Data
{
    /// <summary>
    /// 对话数据 - ScriptableObject
    /// 用于配置对话树和选择分支
    /// </summary>
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "MindLink/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        [Header("基本信息")]
        [Tooltip("对话唯一ID")]
        public string dialogueId;

        [Tooltip("对话标题/名称")]
        public string dialogueName;

        [Tooltip("对话节点列表")]
        public List<DialogueNode> nodes = new List<DialogueNode>();

        [Tooltip("起始节点索引")]
        public int startNodeIndex = 0;

        [Header("设置")]
        [Tooltip("是否可以跳过")]
        public bool canSkip = false;

        [Tooltip("打字机效果速度（字符/秒）")]
        [Range(10f, 100f)]
        public float textSpeed = 30f;

        [Tooltip("是否自动保存对话历史")]
        public bool saveToHistory = true;

        #region Helper Methods
        /// <summary>
        /// 获取起始节点
        /// </summary>
        public DialogueNode GetStartNode()
        {
            if (nodes.Count == 0) return null;
            if (startNodeIndex < 0 || startNodeIndex >= nodes.Count) return nodes[0];
            return nodes[startNodeIndex];
        }

        /// <summary>
        /// 根据ID获取节点
        /// </summary>
        public DialogueNode GetNodeById(string nodeId)
        {
            return nodes.Find(n => n.nodeId == nodeId);
        }

        /// <summary>
        /// 验证对话数据完整性
        /// </summary>
        public bool Validate(out string error)
        {
            error = "";

            if (string.IsNullOrEmpty(dialogueId))
            {
                error = "Dialogue ID is empty";
                return false;
            }

            if (nodes.Count == 0)
            {
                error = "No dialogue nodes";
                return false;
            }

            // 检查节点ID唯一性
            HashSet<string> nodeIds = new HashSet<string>();
            foreach (var node in nodes)
            {
                if (string.IsNullOrEmpty(node.nodeId))
                {
                    error = "Node has empty ID";
                    return false;
                }

                if (nodeIds.Contains(node.nodeId))
                {
                    error = $"Duplicate node ID: {node.nodeId}";
                    return false;
                }

                nodeIds.Add(node.nodeId);
            }

            // 检查引用的节点是否存在
            foreach (var node in nodes)
            {
                if (!string.IsNullOrEmpty(node.nextNodeId))
                {
                    if (!nodeIds.Contains(node.nextNodeId))
                    {
                        error = $"Node {node.nodeId} references non-existent node: {node.nextNodeId}";
                        return false;
                    }
                }

                foreach (var choice in node.choices)
                {
                    if (!string.IsNullOrEmpty(choice.nextNodeId))
                    {
                        if (!nodeIds.Contains(choice.nextNodeId))
                        {
                            error = $"Choice in node {node.nodeId} references non-existent node: {choice.nextNodeId}";
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        #endregion
    }

    /// <summary>
    /// 对话节点 - 单个对话段落
    /// </summary>
    [Serializable]
    public class DialogueNode
    {
        [Header("节点信息")]
        [Tooltip("节点唯一ID")]
        public string nodeId;

        [Tooltip("节点备注（编辑器用）")]
        public string editorNote;

        [Header("角色和文本")]
        [Tooltip("说话角色ID（为空表示旁白）")]
        public string characterId;

        [Tooltip("角色名称显示（为空则使用角色ID）")]
        public string characterDisplayName;

        [Tooltip("角色表情/立绘变体")]
        public CharacterEmotion emotion = CharacterEmotion.Normal;

        [TextArea(3, 10)]
        [Tooltip("对话文本内容")]
        public string text;

        [Header("语音和音效")]
        [Tooltip("语音音频文件")]
        public AudioClip voiceClip;

        [Tooltip("对话音效（如脚步声、开门声等）")]
        public AudioClip sfxClip;

        [Header("流程控制")]
        [Tooltip("节点类型")]
        public DialogueNodeType nodeType = DialogueNodeType.Normal;

        [Tooltip("下一个节点ID（对于Normal类型）")]
        public string nextNodeId;

        [Tooltip("选择项（对于Choice类型）")]
        public List<DialogueChoice> choices = new List<DialogueChoice>();

        [Header("条件检查")]
        [Tooltip("显示此节点需要的标记")]
        public List<string> requiredFlags = new List<string>();

        [Tooltip("显示此节点需要的属性等级")]
        public List<AttributeRequirement> attributeRequirements = new List<AttributeRequirement>();

        [Tooltip("显示此节点需要的关系等级")]
        public List<RelationshipRequirement> relationshipRequirements = new List<RelationshipRequirement>();

        [Header("效果")]
        [Tooltip("显示此节点时触发的效果")]
        public DialogueEffects effects = new DialogueEffects();

        [Header("视觉效果")]
        [Tooltip("背景图片（为空使用默认）")]
        public Sprite backgroundImage;

        [Tooltip("相机效果")]
        public CameraEffect cameraEffect = CameraEffect.None;

        [Tooltip("屏幕特效")]
        public ScreenEffect screenEffect = ScreenEffect.None;

        /// <summary>
        /// 检查是否满足显示条件
        /// </summary>
        public bool CheckConditions(GameState gameState)
        {
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

            return true;
        }
    }

    /// <summary>
    /// 对话选择项
    /// </summary>
    [Serializable]
    public class DialogueChoice
    {
        [Tooltip("选择文本")]
        [TextArea(1, 3)]
        public string text;

        [Tooltip("选择后跳转的节点ID")]
        public string nextNodeId;

        [Header("条件")]
        [Tooltip("需要的属性等级")]
        public List<AttributeRequirement> attributeRequirements = new List<AttributeRequirement>();

        [Tooltip("需要的关系等级")]
        public List<RelationshipRequirement> relationshipRequirements = new List<RelationshipRequirement>();

        [Tooltip("需要的标记")]
        public List<string> requiredFlags = new List<string>();

        [Tooltip("条件不满足时的提示文本")]
        public string lockedHint = "条件不满足";

        [Header("效果")]
        [Tooltip("选择此项的效果")]
        public DialogueEffects effects = new DialogueEffects();

        [Tooltip("选择此项后是否结束对话")]
        public bool endsDialogue = false;

        /// <summary>
        /// 检查选择是否可用
        /// </summary>
        public bool IsAvailable(GameState gameState)
        {
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

            // 检查标记
            foreach (var flag in requiredFlags)
            {
                if (!gameState.GetFlag(flag))
                    return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 对话效果 - 对话触发的游戏状态变化
    /// </summary>
    [Serializable]
    public class DialogueEffects
    {
        [Header("属性和关系")]
        [Tooltip("属性变化")]
        public List<AttributeChange> attributeChanges = new List<AttributeChange>();

        [Tooltip("关系变化")]
        public List<RelationshipChange> relationshipChanges = new List<RelationshipChange>();

        [Header("道具和标记")]
        [Tooltip("获得的道具")]
        public List<ItemReward> itemRewards = new List<ItemReward>();

        [Tooltip("设置的标记")]
        public List<FlagChange> flagChanges = new List<FlagChange>();

        [Header("任务")]
        [Tooltip("开始的任务")]
        public string startMissionId;

        [Tooltip("更新的任务目标")]
        public List<MissionObjectiveUpdate> missionUpdates = new List<MissionObjectiveUpdate>();

        [Header("事件")]
        [Tooltip("触发的事件")]
        public string triggerEventId;

        [Tooltip("解锁的对话")]
        public List<string> unlockDialogueIds = new List<string>();

        /// <summary>
        /// 应用所有效果
        /// </summary>
        public void Apply(GameState gameState)
        {
            // 应用属性变化
            foreach (var change in attributeChanges)
            {
                AttributeManager.Instance?.AddAttributeExp(change.attributeType, change.amount);
            }

            // 应用关系变化
            foreach (var change in relationshipChanges)
            {
                RelationshipManager.Instance?.AddRelationshipExp(change.characterId, change.amount);
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

            // 开始任务
            if (!string.IsNullOrEmpty(startMissionId))
            {
                // 通过EventManager触发任务开始事件
                gameState.SetFlag($"mission_{startMissionId}_started", true);
            }

            // 更新任务进度
            foreach (var update in missionUpdates)
            {
                MissionManager.Instance?.UpdateMissionObjective(
                    update.missionId,
                    update.objectiveId,
                    update.amount
                );
            }

            // 解锁对话
            foreach (var dialogueId in unlockDialogueIds)
            {
                gameState.SetFlag($"dialogue_{dialogueId}_unlocked", true);
            }
        }
    }

    #region Enums
    /// <summary>
    /// 对话节点类型
    /// </summary>
    public enum DialogueNodeType
    {
        Normal,         // 普通对话，自动继续
        Choice,         // 选择分支
        End             // 对话结束
    }

    /// <summary>
    /// 角色表情
    /// </summary>
    public enum CharacterEmotion
    {
        Normal,         // 普通
        Happy,          // 高兴
        Sad,            // 悲伤
        Angry,          // 生气
        Surprised,      // 惊讶
        Worried,        // 担心
        Thinking,       // 思考
        Embarrassed,    // 尴尬
        Serious,        // 严肃
        Smiling         // 微笑
    }

    /// <summary>
    /// 相机效果
    /// </summary>
    public enum CameraEffect
    {
        None,           // 无
        Shake,          // 震动
        ZoomIn,         // 拉近
        ZoomOut,        // 拉远
        Flash           // 闪光
    }

    /// <summary>
    /// 屏幕特效
    /// </summary>
    public enum ScreenEffect
    {
        None,           // 无
        FadeIn,         // 淡入
        FadeOut,        // 淡出
        Blur,           // 模糊
        ColorFlash,     // 颜色闪光
        Shake           // 抖动
    }
    #endregion
}
