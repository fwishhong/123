using System;
using System.Collections.Generic;
using UnityEngine;

namespace MindLink
{
    /// <summary>
    /// 游戏状态数据 - 包含所有需要保存的游戏信息
    /// </summary>
    [Serializable]
    public class GameState
    {
        #region Time Data
        /// <summary>
        /// 当前天数 (1-100)
        /// </summary>
        public int CurrentDay = 1;

        /// <summary>
        /// 当前时间段
        /// </summary>
        public Timeslot CurrentTimeslot = Timeslot.Morning;

        /// <summary>
        /// 最大天数
        /// </summary>
        public int MaxDay = 100;
        #endregion

        #region Attributes Data
        /// <summary>
        /// 知识属性
        /// </summary>
        public AttributeData Knowledge = new AttributeData();

        /// <summary>
        /// 勇气属性
        /// </summary>
        public AttributeData Courage = new AttributeData();

        /// <summary>
        /// 魅力属性
        /// </summary>
        public AttributeData Charm = new AttributeData();

        /// <summary>
        /// 战斗力属性
        /// </summary>
        public AttributeData Combat = new AttributeData();
        #endregion

        #region Relationships Data
        /// <summary>
        /// 角色关系字典 (CharacterID -> RelationshipData)
        /// </summary>
        public Dictionary<string, RelationshipData> Relationships = new Dictionary<string, RelationshipData>();
        #endregion

        #region Mission Data
        /// <summary>
        /// 当前活跃的任务列表
        /// </summary>
        public List<MissionProgress> ActiveMissions = new List<MissionProgress>();

        /// <summary>
        /// 已完成的任务ID列表
        /// </summary>
        public List<string> CompletedMissions = new List<string>();
        #endregion

        #region Event Data
        /// <summary>
        /// 已完成的事件ID列表
        /// </summary>
        public List<string> CompletedEvents = new List<string>();

        /// <summary>
        /// 今天完成的活动列表
        /// </summary>
        public List<string> TodayActivities = new List<string>();

        /// <summary>
        /// 事件冷却字典 (EventID -> 剩余天数)
        /// </summary>
        public Dictionary<string, int> EventCooldowns = new Dictionary<string, int>();
        #endregion

        #region Flags Data
        /// <summary>
        /// 游戏标记字典 (用于剧情分支判断)
        /// </summary>
        public Dictionary<string, bool> Flags = new Dictionary<string, bool>();

        /// <summary>
        /// 数值型标记字典 (用于计数等)
        /// </summary>
        public Dictionary<string, int> NumericFlags = new Dictionary<string, int>();
        #endregion

        #region Inventory Data
        /// <summary>
        /// 道具背包 (ItemID -> 数量)
        /// </summary>
        public Dictionary<string, int> Inventory = new Dictionary<string, int>();

        /// <summary>
        /// 收集的记忆碎片ID列表
        /// </summary>
        public List<string> CollectedMemories = new List<string>();
        #endregion

        #region Save Metadata
        /// <summary>
        /// 存档时间
        /// </summary>
        public string SaveTime;

        /// <summary>
        /// 存档版本
        /// </summary>
        public string Version = "1.0";

        /// <summary>
        /// 游戏总时长（秒）
        /// </summary>
        public float TotalPlayTime = 0f;
        #endregion

        #region Constructors
        /// <summary>
        /// 默认构造函数 - 初始化新游戏
        /// </summary>
        public GameState()
        {
            InitializeNewGame();
        }

        /// <summary>
        /// 初始化新游戏
        /// </summary>
        private void InitializeNewGame()
        {
            // 初始化属性
            Knowledge = new AttributeData();
            Courage = new AttributeData();
            Charm = new AttributeData();
            Combat = new AttributeData();

            // 初始化关系（初始角色）
            Relationships = new Dictionary<string, RelationshipData>
            {
                { "akira", new RelationshipData("晓", 1) },
                { "rei", new RelationshipData("零", 1) },
                { "mizuki", new RelationshipData("美月", 1) }
            };

            // 初始化标记
            Flags = new Dictionary<string, bool>();
            NumericFlags = new Dictionary<string, int>();

            // 设置初始标记
            SetFlag("tutorial_complete", false);
            SetFlag("ability_awakened", false);
            SetFlag("mission_started", false);

            SaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region Flag Methods
        /// <summary>
        /// 设置布尔标记
        /// </summary>
        public void SetFlag(string flagName, bool value)
        {
            if (Flags.ContainsKey(flagName))
            {
                Flags[flagName] = value;
            }
            else
            {
                Flags.Add(flagName, value);
            }
        }

        /// <summary>
        /// 获取布尔标记
        /// </summary>
        public bool GetFlag(string flagName, bool defaultValue = false)
        {
            if (Flags.ContainsKey(flagName))
            {
                return Flags[flagName];
            }
            return defaultValue;
        }

        /// <summary>
        /// 设置数值标记
        /// </summary>
        public void SetNumericFlag(string flagName, int value)
        {
            if (NumericFlags.ContainsKey(flagName))
            {
                NumericFlags[flagName] = value;
            }
            else
            {
                NumericFlags.Add(flagName, value);
            }
        }

        /// <summary>
        /// 获取数值标记
        /// </summary>
        public int GetNumericFlag(string flagName, int defaultValue = 0)
        {
            if (NumericFlags.ContainsKey(flagName))
            {
                return NumericFlags[flagName];
            }
            return defaultValue;
        }

        /// <summary>
        /// 增加数值标记
        /// </summary>
        public void IncrementNumericFlag(string flagName, int amount = 1)
        {
            int current = GetNumericFlag(flagName);
            SetNumericFlag(flagName, current + amount);
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// 检查是否游戏结束
        /// </summary>
        public bool IsGameOver()
        {
            return CurrentDay > MaxDay || GetFlag("mission_complete");
        }

        /// <summary>
        /// 获取属性数据
        /// </summary>
        public AttributeData GetAttribute(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.Knowledge: return Knowledge;
                case AttributeType.Courage: return Courage;
                case AttributeType.Charm: return Charm;
                case AttributeType.Combat: return Combat;
                default: return null;
            }
        }

        /// <summary>
        /// 获取角色关系数据
        /// </summary>
        public RelationshipData GetRelationship(string characterId)
        {
            if (Relationships.ContainsKey(characterId))
            {
                return Relationships[characterId];
            }
            return null;
        }

        /// <summary>
        /// 添加道具
        /// </summary>
        public void AddItem(string itemId, int amount = 1)
        {
            if (Inventory.ContainsKey(itemId))
            {
                Inventory[itemId] += amount;
            }
            else
            {
                Inventory.Add(itemId, amount);
            }
        }

        /// <summary>
        /// 移除道具
        /// </summary>
        public bool RemoveItem(string itemId, int amount = 1)
        {
            if (!Inventory.ContainsKey(itemId)) return false;

            if (Inventory[itemId] < amount) return false;

            Inventory[itemId] -= amount;
            if (Inventory[itemId] <= 0)
            {
                Inventory.Remove(itemId);
            }

            return true;
        }

        /// <summary>
        /// 检查是否拥有道具
        /// </summary>
        public bool HasItem(string itemId, int amount = 1)
        {
            if (!Inventory.ContainsKey(itemId)) return false;
            return Inventory[itemId] >= amount;
        }
        #endregion
    }

    /// <summary>
    /// 时间段枚举
    /// </summary>
    public enum Timeslot
    {
        Morning,   // 早晨
        Afternoon, // 下午
        Night      // 夜晚
    }

    /// <summary>
    /// 属性类型枚举
    /// </summary>
    public enum AttributeType
    {
        Knowledge, // 知识
        Courage,   // 勇气
        Charm,     // 魅力
        Combat     // 战斗力
    }

    /// <summary>
    /// 属性数据
    /// </summary>
    [Serializable]
    public class AttributeData
    {
        /// <summary>
        /// 等级 (1-10)
        /// </summary>
        public int Level = 1;

        /// <summary>
        /// 当前经验值
        /// </summary>
        public int Exp = 0;

        /// <summary>
        /// 升级所需经验值
        /// </summary>
        public int ExpToNext = 100;

        /// <summary>
        /// 最大等级
        /// </summary>
        public const int MaxLevel = 10;

        /// <summary>
        /// 添加经验值
        /// </summary>
        public bool AddExp(int amount)
        {
            Exp += amount;

            bool leveledUp = false;

            // 检查是否升级
            while (Exp >= ExpToNext && Level < MaxLevel)
            {
                Exp -= ExpToNext;
                Level++;
                ExpToNext = Mathf.FloorToInt(ExpToNext * 1.5f); // 经验需求递增
                leveledUp = true;
            }

            // 最高等级时清零经验
            if (Level >= MaxLevel)
            {
                Exp = 0;
                ExpToNext = 0;
            }

            return leveledUp;
        }

        /// <summary>
        /// 获取升级进度百分比
        /// </summary>
        public float GetProgressPercentage()
        {
            if (Level >= MaxLevel) return 1f;
            return (float)Exp / ExpToNext;
        }
    }

    /// <summary>
    /// 关系数据
    /// </summary>
    [Serializable]
    public class RelationshipData
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 好感度等级 (0-10)
        /// </summary>
        public int Level = 0;

        /// <summary>
        /// 当前经验值
        /// </summary>
        public int Exp = 0;

        /// <summary>
        /// 升级所需经验值
        /// </summary>
        public int ExpToNext = 100;

        /// <summary>
        /// 最大等级
        /// </summary>
        public const int MaxLevel = 10;

        public RelationshipData() { }

        public RelationshipData(string name, int initialLevel = 0)
        {
            Name = name;
            Level = initialLevel;
        }

        /// <summary>
        /// 添加经验值
        /// </summary>
        public bool AddExp(int amount)
        {
            Exp += amount;

            bool leveledUp = false;

            while (Exp >= ExpToNext && Level < MaxLevel)
            {
                Exp -= ExpToNext;
                Level++;
                leveledUp = true;
            }

            if (Level >= MaxLevel)
            {
                Exp = 0;
                ExpToNext = 0;
            }

            return leveledUp;
        }

        /// <summary>
        /// 获取升级进度百分比
        /// </summary>
        public float GetProgressPercentage()
        {
            if (Level >= MaxLevel) return 1f;
            return (float)Exp / ExpToNext;
        }
    }

    /// <summary>
    /// 任务进度
    /// </summary>
    [Serializable]
    public class MissionProgress
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string MissionId;

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 任务开始天数
        /// </summary>
        public int StartDay;

        /// <summary>
        /// 任务截止天数
        /// </summary>
        public int Deadline;

        /// <summary>
        /// 任务目标字典 (目标ID -> 当前进度/需求进度)
        /// </summary>
        public Dictionary<string, ObjectiveProgress> Objectives = new Dictionary<string, ObjectiveProgress>();

        /// <summary>
        /// 任务是否完成
        /// </summary>
        public bool IsCompleted = false;

        /// <summary>
        /// 任务是否失败
        /// </summary>
        public bool IsFailed = false;

        /// <summary>
        /// 检查任务是否全部达成
        /// </summary>
        public bool CheckAllObjectivesComplete()
        {
            foreach (var objective in Objectives.Values)
            {
                if (!objective.IsCompleted())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取任务完成度百分比
        /// </summary>
        public float GetCompletionPercentage()
        {
            if (Objectives.Count == 0) return 0f;

            float total = 0f;
            foreach (var objective in Objectives.Values)
            {
                total += objective.GetProgressPercentage();
            }

            return total / Objectives.Count;
        }
    }

    /// <summary>
    /// 目标进度
    /// </summary>
    [Serializable]
    public class ObjectiveProgress
    {
        /// <summary>
        /// 目标描述
        /// </summary>
        public string Description;

        /// <summary>
        /// 当前进度
        /// </summary>
        public int Current;

        /// <summary>
        /// 需求进度
        /// </summary>
        public int Required;

        public ObjectiveProgress() { }

        public ObjectiveProgress(string description, int required, int current = 0)
        {
            Description = description;
            Required = required;
            Current = current;
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsCompleted()
        {
            return Current >= Required;
        }

        /// <summary>
        /// 获取完成百分比
        /// </summary>
        public float GetProgressPercentage()
        {
            if (Required == 0) return 1f;
            return Mathf.Clamp01((float)Current / Required);
        }

        /// <summary>
        /// 增加进度
        /// </summary>
        public void AddProgress(int amount = 1)
        {
            Current = Mathf.Min(Current + amount, Required);
        }
    }
}
