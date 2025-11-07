using UnityEngine;
using System;
using System.Collections.Generic;

namespace MindLink.Data
{
    /// <summary>
    /// 角色数据 - ScriptableObject
    /// 用于配置可交互角色的所有信息
    /// </summary>
    [CreateAssetMenu(fileName = "New Character", menuName = "MindLink/Character Data")]
    public class CharacterData : ScriptableObject
    {
        [Header("基本信息")]
        [Tooltip("角色唯一ID")]
        public string characterId;

        [Tooltip("角色名称")]
        public string characterName;

        [Tooltip("角色全名（如果有）")]
        public string fullName;

        [Tooltip("年龄")]
        public int age;

        [Tooltip("性别")]
        public CharacterGender gender = CharacterGender.Other;

        [TextArea(2, 4)]
        [Tooltip("角色简介")]
        public string description;

        [Header("外观")]
        [Tooltip("角色立绘（各种表情）")]
        public CharacterPortraits portraits;

        [Tooltip("角色图标（用于UI）")]
        public Sprite icon;

        [Tooltip("角色主题颜色")]
        public Color themeColor = Color.white;

        [Header("初始状态")]
        [Tooltip("初始关系等级")]
        [Range(1, 10)]
        public int initialRelationshipLevel = 1;

        [Tooltip("初始关系经验值")]
        public int initialRelationshipExp = 0;

        [Tooltip("是否在游戏开始时已解锁")]
        public bool unlockedByDefault = false;

        [Tooltip("解锁此角色需要的标记")]
        public string unlockFlag;

        [Header("关系等级解锁内容")]
        [Tooltip("等级2解锁的活动")]
        public List<string> level2UnlockedEvents = new List<string>();

        [Tooltip("等级4解锁的支线任务")]
        public string level4SideQuest;

        [Tooltip("等级6解锁的对话")]
        public List<string> level6UnlockedDialogues = new List<string>();

        [Tooltip("等级8解锁的特殊事件")]
        public string level8SpecialEvent;

        [Tooltip("等级10解锁的结局")]
        public string level10EndingId;

        [Header("角色特性")]
        [Tooltip("角色标签（用于分类）")]
        public List<CharacterTag> tags = new List<CharacterTag>();

        [Tooltip("喜好（加成好感度的活动类型）")]
        public List<EventType> preferredActivities = new List<EventType>();

        [Tooltip("讨厌（减少好感度的活动类型）")]
        public List<EventType> dislikedActivities = new List<EventType>();

        [Tooltip("生日（Day）")]
        public int birthday = -1;

        [Header("对话和语音")]
        [Tooltip("默认问候对话")]
        public DialogueData defaultGreeting;

        [Tooltip("关系等级提升对话")]
        public List<DialogueData> levelUpDialogues = new List<DialogueData>();

        [Tooltip("语音音色（用于语音合成）")]
        public VoiceType voiceType = VoiceType.Normal;

        [Tooltip("语音音高调整")]
        [Range(0.5f, 2f)]
        public float voicePitch = 1f;

        [Header("战斗相关")]
        [Tooltip("是否可作为战斗伙伴")]
        public bool canBattlePartner = false;

        [Tooltip("战斗伙伴解锁等级")]
        public int battlePartnerUnlockLevel = 5;

        [Tooltip("角色战斗能力")]
        public CharacterCombatStats combatStats;

        [Header("故事相关")]
        [Tooltip("是否为主线剧情角色")]
        public bool isMainStoryCharacter = false;

        [Tooltip("相关的主线任务")]
        public List<string> relatedMissions = new List<string>();

        [Tooltip("角色专属事件")]
        public List<EventData> exclusiveEvents = new List<EventData>();

        #region Helper Methods
        /// <summary>
        /// 获取指定等级解锁的内容
        /// </summary>
        public CharacterUnlockInfo GetUnlockInfo(int level)
        {
            var info = new CharacterUnlockInfo();
            info.level = level;

            switch (level)
            {
                case 2:
                    info.unlockedEvents = level2UnlockedEvents;
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(level4SideQuest))
                        info.unlockedMission = level4SideQuest;
                    break;
                case 6:
                    info.unlockedDialogues = level6UnlockedDialogues;
                    break;
                case 8:
                    if (!string.IsNullOrEmpty(level8SpecialEvent))
                        info.specialEvent = level8SpecialEvent;
                    break;
                case 10:
                    if (!string.IsNullOrEmpty(level10EndingId))
                        info.endingUnlocked = level10EndingId;
                    break;
            }

            return info;
        }

        /// <summary>
        /// 获取指定表情的立绘
        /// </summary>
        public Sprite GetPortrait(CharacterEmotion emotion)
        {
            if (portraits == null) return null;
            return portraits.GetSprite(emotion);
        }

        /// <summary>
        /// 获取关系等级提升对话
        /// </summary>
        public DialogueData GetLevelUpDialogue(int level)
        {
            if (levelUpDialogues == null || levelUpDialogues.Count == 0) return null;

            // 尝试获取对应等级的对话（索引 = level - 2，因为1级不需要对话）
            int index = level - 2;
            if (index >= 0 && index < levelUpDialogues.Count)
                return levelUpDialogues[index];

            return null;
        }

        /// <summary>
        /// 检查活动类型是否受角色喜欢
        /// </summary>
        public float GetActivityAffinityMultiplier(EventType activityType)
        {
            if (preferredActivities.Contains(activityType))
                return 1.5f; // 喜欢的活动，好感度提升1.5倍

            if (dislikedActivities.Contains(activityType))
                return 0.5f; // 讨厌的活动，好感度减半

            return 1f; // 正常
        }

        /// <summary>
        /// 是否可以作为战斗伙伴
        /// </summary>
        public bool CanUseBattlePartner(int currentLevel)
        {
            return canBattlePartner && currentLevel >= battlePartnerUnlockLevel;
        }

        /// <summary>
        /// 验证数据完整性
        /// </summary>
        public bool Validate(out string error)
        {
            error = "";

            if (string.IsNullOrEmpty(characterId))
            {
                error = "Character ID is empty";
                return false;
            }

            if (string.IsNullOrEmpty(characterName))
            {
                error = "Character name is empty";
                return false;
            }

            if (initialRelationshipLevel < 1 || initialRelationshipLevel > 10)
            {
                error = "Initial relationship level must be between 1 and 10";
                return false;
            }

            return true;
        }
        #endregion
    }

    #region Supporting Data Structures
    /// <summary>
    /// 角色立绘集合
    /// </summary>
    [Serializable]
    public class CharacterPortraits
    {
        [Tooltip("普通表情")]
        public Sprite normal;

        [Tooltip("高兴表情")]
        public Sprite happy;

        [Tooltip("悲伤表情")]
        public Sprite sad;

        [Tooltip("生气表情")]
        public Sprite angry;

        [Tooltip("惊讶表情")]
        public Sprite surprised;

        [Tooltip("担心表情")]
        public Sprite worried;

        [Tooltip("思考表情")]
        public Sprite thinking;

        [Tooltip("尴尬表情")]
        public Sprite embarrassed;

        [Tooltip("严肃表情")]
        public Sprite serious;

        [Tooltip("微笑表情")]
        public Sprite smiling;

        /// <summary>
        /// 根据表情获取对应的立绘
        /// </summary>
        public Sprite GetSprite(CharacterEmotion emotion)
        {
            switch (emotion)
            {
                case CharacterEmotion.Normal: return normal;
                case CharacterEmotion.Happy: return happy;
                case CharacterEmotion.Sad: return sad;
                case CharacterEmotion.Angry: return angry;
                case CharacterEmotion.Surprised: return surprised;
                case CharacterEmotion.Worried: return worried;
                case CharacterEmotion.Thinking: return thinking;
                case CharacterEmotion.Embarrassed: return embarrassed;
                case CharacterEmotion.Serious: return serious;
                case CharacterEmotion.Smiling: return smiling;
                default: return normal;
            }
        }
    }

    /// <summary>
    /// 角色解锁信息
    /// </summary>
    [Serializable]
    public class CharacterUnlockInfo
    {
        public int level;
        public List<string> unlockedEvents = new List<string>();
        public string unlockedMission;
        public List<string> unlockedDialogues = new List<string>();
        public string specialEvent;
        public string endingUnlocked;

        public bool HasAnyUnlocks()
        {
            return unlockedEvents.Count > 0 ||
                   !string.IsNullOrEmpty(unlockedMission) ||
                   unlockedDialogues.Count > 0 ||
                   !string.IsNullOrEmpty(specialEvent) ||
                   !string.IsNullOrEmpty(endingUnlocked);
        }
    }

    /// <summary>
    /// 角色战斗属性
    /// </summary>
    [Serializable]
    public class CharacterCombatStats
    {
        [Tooltip("基础生命值")]
        public int baseHP = 100;

        [Tooltip("基础攻击力")]
        public int baseAttack = 10;

        [Tooltip("基础防御力")]
        public int baseDefense = 5;

        [Tooltip("基础速度")]
        public int baseSpeed = 10;

        [Tooltip("特殊技能")]
        public List<string> specialSkills = new List<string>();

        [Tooltip("战斗风格")]
        public CombatStyle combatStyle = CombatStyle.Balanced;
    }

    /// <summary>
    /// 战斗风格
    /// </summary>
    public enum CombatStyle
    {
        Aggressive,     // 激进型（高攻低防）
        Defensive,      // 防御型（高防低攻）
        Balanced,       // 均衡型
        Support,        // 辅助型（增益/治疗）
        Technical       // 技巧型（特殊效果）
    }

    /// <summary>
    /// 角色性别
    /// </summary>
    public enum CharacterGender
    {
        Male,
        Female,
        Other
    }

    /// <summary>
    /// 角色标签
    /// </summary>
    public enum CharacterTag
    {
        Student,        // 学生
        Teacher,        // 老师
        Ally,           // 同伴
        Enemy,          // 敌人
        Neutral,        // 中立
        Mentor,         // 导师
        Rival,          // 竞争对手
        Love,           // 恋爱对象
        Family,         // 家人
        Friend,         // 朋友
        Mysterious,     // 神秘
        Comic           // 搞笑
    }

    /// <summary>
    /// 语音类型
    /// </summary>
    public enum VoiceType
    {
        Normal,         // 普通
        Young,          // 年轻
        Mature,         // 成熟
        Deep,           // 低沉
        High,           // 高亢
        Gentle,         // 温柔
        Energetic       // 活力
    }
    #endregion
}
