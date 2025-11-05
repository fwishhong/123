using UnityEngine;
using System;

namespace MindLink
{
    /// <summary>
    /// 属性管理器 - 负责角色属性的计算、升级和管理
    /// </summary>
    public class AttributeManager : MonoBehaviour
    {
        #region Singleton
        private static AttributeManager _instance;
        public static AttributeManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("AttributeManager");
                    _instance = go.AddComponent<AttributeManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 属性经验值增加事件 (AttributeType, Amount)
        /// </summary>
        public static event Action<AttributeType, int> OnAttributeExpGain;

        /// <summary>
        /// 属性升级事件 (AttributeType, NewLevel)
        /// </summary>
        public static event Action<AttributeType, int> OnAttributeLevelUp;

        /// <summary>
        /// 属性变化事件 (用于UI刷新)
        /// </summary>
        public static event Action OnAttributeChanged;
        #endregion

        #region Properties
        private GameState _gameState;

        /// <summary>
        /// 知识属性
        /// </summary>
        public AttributeData Knowledge => _gameState?.Knowledge;

        /// <summary>
        /// 勇气属性
        /// </summary>
        public AttributeData Courage => _gameState?.Courage;

        /// <summary>
        /// 魅力属性
        /// </summary>
        public AttributeData Charm => _gameState?.Charm;

        /// <summary>
        /// 战斗力属性
        /// </summary>
        public AttributeData Combat => _gameState?.Combat;
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
        /// 初始化属性管理器
        /// </summary>
        public void Initialize(GameState gameState)
        {
            _gameState = gameState;
            Debug.Log("[AttributeManager] Initialized.");
        }
        #endregion

        #region Attribute Operations
        /// <summary>
        /// 添加属性经验值
        /// </summary>
        /// <param name="type">属性类型</param>
        /// <param name="amount">经验值数量</param>
        public void AddAttributeExp(AttributeType type, int amount)
        {
            if (_gameState == null)
            {
                Debug.LogError("[AttributeManager] GameState is null!");
                return;
            }

            AttributeData attr = _gameState.GetAttribute(type);
            if (attr == null)
            {
                Debug.LogError($"[AttributeManager] Attribute {type} not found!");
                return;
            }

            int previousLevel = attr.Level;

            // 添加经验值（会自动处理升级）
            bool leveledUp = attr.AddExp(amount);

            Debug.Log($"[AttributeManager] {type} gained {amount} exp. " +
                      $"Level: {attr.Level}, Exp: {attr.Exp}/{attr.ExpToNext}");

            // 触发事件
            OnAttributeExpGain?.Invoke(type, amount);

            if (leveledUp)
            {
                Debug.Log($"[AttributeManager] {type} leveled up! {previousLevel} → {attr.Level}");
                OnAttributeLevelUp?.Invoke(type, attr.Level);

                // 显示升级通知
                UIManager.Instance?.ShowNotification(
                    $"{GetAttributeName(type)} 升级到 Lv {attr.Level}!",
                    NotificationType.Success
                );
            }

            OnAttributeChanged?.Invoke();
        }

        /// <summary>
        /// 获取属性数据
        /// </summary>
        public AttributeData GetAttribute(AttributeType type)
        {
            return _gameState?.GetAttribute(type);
        }

        /// <summary>
        /// 获取属性等级
        /// </summary>
        public int GetAttributeLevel(AttributeType type)
        {
            return GetAttribute(type)?.Level ?? 1;
        }

        /// <summary>
        /// 获取属性经验值
        /// </summary>
        public int GetAttributeExp(AttributeType type)
        {
            return GetAttribute(type)?.Exp ?? 0;
        }

        /// <summary>
        /// 检查属性是否达到指定等级
        /// </summary>
        public bool CheckAttributeLevel(AttributeType type, int requiredLevel)
        {
            return GetAttributeLevel(type) >= requiredLevel;
        }

        /// <summary>
        /// 获取属性升级进度百分比
        /// </summary>
        public float GetAttributeProgress(AttributeType type)
        {
            return GetAttribute(type)?.GetProgressPercentage() ?? 0f;
        }

        /// <summary>
        /// 获取所有属性的总等级
        /// </summary>
        public int GetTotalAttributeLevel()
        {
            if (_gameState == null) return 4; // 默认每个属性1级

            return Knowledge.Level + Courage.Level + Charm.Level + Combat.Level;
        }

        /// <summary>
        /// 获取所有属性的平均等级
        /// </summary>
        public float GetAverageAttributeLevel()
        {
            return GetTotalAttributeLevel() / 4f;
        }
        #endregion

        #region Attribute Requirements
        /// <summary>
        /// 检查是否满足属性要求
        /// </summary>
        /// <param name="requirements">属性要求字典</param>
        /// <returns>是否全部满足</returns>
        public bool CheckAttributeRequirements(System.Collections.Generic.Dictionary<AttributeType, int> requirements)
        {
            if (requirements == null || requirements.Count == 0) return true;

            foreach (var kvp in requirements)
            {
                if (!CheckAttributeLevel(kvp.Key, kvp.Value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取未满足的属性要求
        /// </summary>
        public System.Collections.Generic.List<string> GetUnmetRequirements(
            System.Collections.Generic.Dictionary<AttributeType, int> requirements)
        {
            var unmet = new System.Collections.Generic.List<string>();

            if (requirements == null) return unmet;

            foreach (var kvp in requirements)
            {
                int currentLevel = GetAttributeLevel(kvp.Key);
                if (currentLevel < kvp.Value)
                {
                    unmet.Add($"{GetAttributeName(kvp.Key)} Lv{kvp.Value} (当前: Lv{currentLevel})");
                }
            }

            return unmet;
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// 获取属性中文名称
        /// </summary>
        public string GetAttributeName(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.Knowledge: return "知识";
                case AttributeType.Courage: return "勇气";
                case AttributeType.Charm: return "魅力";
                case AttributeType.Combat: return "战斗力";
                default: return "未知";
            }
        }

        /// <summary>
        /// 获取属性图标
        /// </summary>
        public string GetAttributeIcon(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.Knowledge: return "📚";
                case AttributeType.Courage: return "⚔️";
                case AttributeType.Charm: return "✨";
                case AttributeType.Combat: return "⚡";
                default: return "❓";
            }
        }

        /// <summary>
        /// 获取属性颜色（用于UI）
        /// </summary>
        public Color GetAttributeColor(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.Knowledge: return new Color(0.04f, 0.52f, 0.89f); // 蓝色
                case AttributeType.Courage: return new Color(0.84f, 0.19f, 0.19f); // 红色
                case AttributeType.Charm: return new Color(0.99f, 0.47f, 0.66f); // 粉色
                case AttributeType.Combat: return new Color(0.88f, 0.44f, 0.33f); // 橙色
                default: return Color.gray;
            }
        }
        #endregion

        #region Debug
        /// <summary>
        /// 调试：直接设置属性等级
        /// </summary>
        public void DebugSetAttributeLevel(AttributeType type, int level)
        {
            if (_gameState == null) return;

            AttributeData attr = _gameState.GetAttribute(type);
            if (attr == null) return;

            attr.Level = Mathf.Clamp(level, 1, AttributeData.MaxLevel);
            attr.Exp = 0;

            Debug.Log($"[AttributeManager] DEBUG: Set {type} to Level {attr.Level}");
            OnAttributeChanged?.Invoke();
        }

        /// <summary>
        /// 获取属性调试信息
        /// </summary>
        public string GetDebugInfo()
        {
            if (_gameState == null) return "GameState is null";

            return $"Knowledge: Lv{Knowledge.Level} ({Knowledge.Exp}/{Knowledge.ExpToNext})\n" +
                   $"Courage: Lv{Courage.Level} ({Courage.Exp}/{Courage.ExpToNext})\n" +
                   $"Charm: Lv{Charm.Level} ({Charm.Exp}/{Charm.ExpToNext})\n" +
                   $"Combat: Lv{Combat.Level} ({Combat.Exp}/{Combat.ExpToNext})\n" +
                   $"Total Level: {GetTotalAttributeLevel()}";
        }
        #endregion
    }
}
