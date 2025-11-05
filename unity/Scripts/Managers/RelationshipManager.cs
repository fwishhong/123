using UnityEngine;
using System;
using System.Collections.Generic;

namespace MindLink
{
    /// <summary>
    /// 关系管理器 - 负责角色关系（好感度）的管理
    /// </summary>
    public class RelationshipManager : MonoBehaviour
    {
        #region Singleton
        private static RelationshipManager _instance;
        public static RelationshipManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("RelationshipManager");
                    _instance = go.AddComponent<RelationshipManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 关系经验值增加事件 (CharacterID, Amount)
        /// </summary>
        public static event Action<string, int> OnRelationshipExpGain;

        /// <summary>
        /// 关系等级提升事件 (CharacterID, NewLevel)
        /// </summary>
        public static event Action<string, int> OnRelationshipLevelUp;

        /// <summary>
        /// 关系变化事件
        /// </summary>
        public static event Action OnRelationshipChanged;
        #endregion

        #region Properties
        private GameState _gameState;

        /// <summary>
        /// 所有角色关系数据
        /// </summary>
        public Dictionary<string, RelationshipData> Relationships => _gameState?.Relationships;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            transform.SetParent(GameManager.Instance.transform);
        }
        #endregion

        #region Initialization
        /// <summary>
        /// 初始化关系管理器
        /// </summary>
        public void Initialize(GameState gameState)
        {
            _gameState = gameState;
            Debug.Log("[RelationshipManager] Initialized.");
        }
        #endregion

        #region Relationship Operations
        /// <summary>
        /// 添加关系经验值
        /// </summary>
        /// <param name="characterId">角色ID</param>
        /// <param name="amount">经验值数量</param>
        public void AddRelationshipExp(string characterId, int amount)
        {
            if (_gameState == null)
            {
                Debug.LogError("[RelationshipManager] GameState is null!");
                return;
            }

            RelationshipData relationship = _gameState.GetRelationship(characterId);
            if (relationship == null)
            {
                Debug.LogWarning($"[RelationshipManager] Character {characterId} not found! Creating new relationship.");
                relationship = new RelationshipData("未知", 0);
                _gameState.Relationships.Add(characterId, relationship);
            }

            int previousLevel = relationship.Level;

            // 添加经验值
            bool leveledUp = relationship.AddExp(amount);

            Debug.Log($"[RelationshipManager] {relationship.Name} gained {amount} exp. " +
                      $"Level: {relationship.Level}, Exp: {relationship.Exp}/{relationship.ExpToNext}");

            // 触发事件
            OnRelationshipExpGain?.Invoke(characterId, amount);

            if (leveledUp)
            {
                Debug.Log($"[RelationshipManager] {relationship.Name} relationship leveled up! " +
                          $"{previousLevel} → {relationship.Level}");
                OnRelationshipLevelUp?.Invoke(characterId, relationship.Level);

                // 显示升级通知
                UIManager.Instance?.ShowNotification(
                    $"与 {relationship.Name} 的关系提升到 Lv {relationship.Level}!",
                    NotificationType.Success
                );

                // 检查是否解锁特殊剧情
                CheckRelationshipUnlocks(characterId, relationship.Level);
            }

            OnRelationshipChanged?.Invoke();
        }

        /// <summary>
        /// 获取角色关系数据
        /// </summary>
        public RelationshipData GetRelationship(string characterId)
        {
            return _gameState?.GetRelationship(characterId);
        }

        /// <summary>
        /// 获取角色关系等级
        /// </summary>
        public int GetRelationshipLevel(string characterId)
        {
            return GetRelationship(characterId)?.Level ?? 0;
        }

        /// <summary>
        /// 检查关系是否达到指定等级
        /// </summary>
        public bool CheckRelationshipLevel(string characterId, int requiredLevel)
        {
            return GetRelationshipLevel(characterId) >= requiredLevel;
        }

        /// <summary>
        /// 获取关系升级进度百分比
        /// </summary>
        public float GetRelationshipProgress(string characterId)
        {
            return GetRelationship(characterId)?.GetProgressPercentage() ?? 0f;
        }

        /// <summary>
        /// 获取所有角色的关系总等级
        /// </summary>
        public int GetTotalRelationshipLevel()
        {
            if (_gameState == null || _gameState.Relationships == null) return 0;

            int total = 0;
            foreach (var relationship in _gameState.Relationships.Values)
            {
                total += relationship.Level;
            }
            return total;
        }

        /// <summary>
        /// 获取平均关系等级
        /// </summary>
        public float GetAverageRelationshipLevel()
        {
            if (_gameState == null || _gameState.Relationships == null || _gameState.Relationships.Count == 0)
                return 0f;

            return (float)GetTotalRelationshipLevel() / _gameState.Relationships.Count;
        }
        #endregion

        #region Relationship Unlocks
        /// <summary>
        /// 检查关系等级解锁
        /// </summary>
        private void CheckRelationshipUnlocks(string characterId, int level)
        {
            // 不同等级解锁不同内容
            switch (level)
            {
                case 2:
                    Debug.Log($"[RelationshipManager] {characterId} Lv2: 解锁角色专属活动");
                    _gameState.SetFlag($"{characterId}_exclusive_unlocked", true);
                    break;

                case 4:
                    Debug.Log($"[RelationshipManager] {characterId} Lv4: 解锁角色支线任务");
                    _gameState.SetFlag($"{characterId}_quest_unlocked", true);
                    break;

                case 6:
                    Debug.Log($"[RelationshipManager] {characterId} Lv6: 角色可在大任务中提供帮助");
                    _gameState.SetFlag($"{characterId}_support_unlocked", true);
                    break;

                case 8:
                    Debug.Log($"[RelationshipManager] {characterId} Lv8: 解锁角色个人危机事件");
                    _gameState.SetFlag($"{characterId}_crisis_unlocked", true);
                    break;

                case 10:
                    Debug.Log($"[RelationshipManager] {characterId} Lv10: 解锁角色专属结局");
                    _gameState.SetFlag($"{characterId}_ending_unlocked", true);
                    break;
            }
        }
        #endregion

        #region Character Management
        /// <summary>
        /// 添加新角色关系
        /// </summary>
        public void AddCharacter(string characterId, string name, int initialLevel = 0)
        {
            if (_gameState == null) return;

            if (_gameState.Relationships.ContainsKey(characterId))
            {
                Debug.LogWarning($"[RelationshipManager] Character {characterId} already exists!");
                return;
            }

            _gameState.Relationships.Add(characterId, new RelationshipData(name, initialLevel));
            Debug.Log($"[RelationshipManager] Added new character: {name} (ID: {characterId})");

            OnRelationshipChanged?.Invoke();
        }

        /// <summary>
        /// 获取所有角色ID列表
        /// </summary>
        public List<string> GetAllCharacterIds()
        {
            if (_gameState == null || _gameState.Relationships == null)
                return new List<string>();

            return new List<string>(_gameState.Relationships.Keys);
        }

        /// <summary>
        /// 获取所有可见角色列表（根据进度）
        /// </summary>
        public List<string> GetAvailableCharacters()
        {
            var available = new List<string>();

            if (_gameState == null) return available;

            // 根据游戏进度解锁角色
            foreach (var kvp in _gameState.Relationships)
            {
                // 检查是否已遇到该角色
                if (_gameState.GetFlag($"{kvp.Key}_met", true))
                {
                    available.Add(kvp.Key);
                }
            }

            return available;
        }
        #endregion

        #region Relationship Requirements
        /// <summary>
        /// 检查是否满足关系要求
        /// </summary>
        public bool CheckRelationshipRequirements(Dictionary<string, int> requirements)
        {
            if (requirements == null || requirements.Count == 0) return true;

            foreach (var kvp in requirements)
            {
                if (!CheckRelationshipLevel(kvp.Key, kvp.Value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取未满足的关系要求
        /// </summary>
        public List<string> GetUnmetRelationshipRequirements(Dictionary<string, int> requirements)
        {
            var unmet = new List<string>();

            if (requirements == null) return unmet;

            foreach (var kvp in requirements)
            {
                int currentLevel = GetRelationshipLevel(kvp.Key);
                if (currentLevel < kvp.Value)
                {
                    var relationship = GetRelationship(kvp.Key);
                    string name = relationship?.Name ?? kvp.Key;
                    unmet.Add($"{name} Lv{kvp.Value} (当前: Lv{currentLevel})");
                }
            }

            return unmet;
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// 获取角色名称
        /// </summary>
        public string GetCharacterName(string characterId)
        {
            return GetRelationship(characterId)?.Name ?? "未知";
        }

        /// <summary>
        /// 根据关系等级获取称号
        /// </summary>
        public string GetRelationshipTitle(int level)
        {
            if (level >= 10) return "灵魂伴侣";
            if (level >= 8) return "挚友";
            if (level >= 6) return "好友";
            if (level >= 4) return "朋友";
            if (level >= 2) return "熟人";
            return "陌生人";
        }

        /// <summary>
        /// 获取关系描述
        /// </summary>
        public string GetRelationshipDescription(string characterId)
        {
            var relationship = GetRelationship(characterId);
            if (relationship == null) return "未知关系";

            string title = GetRelationshipTitle(relationship.Level);
            return $"{relationship.Name} - {title} (Lv{relationship.Level})";
        }
        #endregion

        #region Debug
        /// <summary>
        /// 调试：直接设置关系等级
        /// </summary>
        public void DebugSetRelationshipLevel(string characterId, int level)
        {
            if (_gameState == null) return;

            var relationship = GetRelationship(characterId);
            if (relationship == null) return;

            relationship.Level = Mathf.Clamp(level, 0, RelationshipData.MaxLevel);
            relationship.Exp = 0;

            Debug.Log($"[RelationshipManager] DEBUG: Set {relationship.Name} to Level {relationship.Level}");
            OnRelationshipChanged?.Invoke();
        }

        /// <summary>
        /// 获取关系调试信息
        /// </summary>
        public string GetDebugInfo()
        {
            if (_gameState == null || _gameState.Relationships == null)
                return "No relationships data";

            string info = "=== Relationships ===\n";
            foreach (var kvp in _gameState.Relationships)
            {
                var rel = kvp.Value;
                info += $"{rel.Name}: Lv{rel.Level} ({rel.Exp}/{rel.ExpToNext})\n";
            }
            info += $"Total Level: {GetTotalRelationshipLevel()}";
            return info;
        }
        #endregion
    }
}
