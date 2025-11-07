using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using MindLink.Data;

namespace MindLink.UI
{
    /// <summary>
    /// 角色信息UI - 显示属性、关系和角色详情
    /// </summary>
    public class CharacterInfoUI : MonoBehaviour
    {
        [Header("属性面板")]
        [Tooltip("属性容器")]
        public Transform attributeContainer;

        [Tooltip("知识文本")]
        public TextMeshProUGUI knowledgeText;
        public Slider knowledgeSlider;

        [Tooltip("战斗文本")]
        public TextMeshProUGUI combatText;
        public Slider combatSlider;

        [Tooltip("魅力文本")]
        public TextMeshProUGUI charismaText;
        public Slider charismaSlider;

        [Tooltip("勇气文本")]
        public TextMeshProUGUI courageText;
        public Slider courageSlider;

        [Header("关系面板")]
        [Tooltip("关系列表容器")]
        public Transform relationshipListContainer;

        [Tooltip("关系项预制体")]
        public GameObject relationshipItemPrefab;

        [Header("角色详情面板")]
        [Tooltip("详情面板")]
        public GameObject detailPanel;

        [Tooltip("角色立绘")]
        public Image characterPortrait;

        [Tooltip("角色名称")]
        public TextMeshProUGUI characterNameText;

        [Tooltip("角色描述")]
        public TextMeshProUGUI characterDescriptionText;

        [Tooltip("关系等级")]
        public TextMeshProUGUI relationshipLevelText;

        [Tooltip("关系进度条")]
        public Slider relationshipProgressSlider;

        [Tooltip("解锁内容文本")]
        public TextMeshProUGUI unlockContentText;

        [Header("设置")]
        [Tooltip("是否自动刷新")]
        public bool autoRefresh = true;

        // 状态
        private List<string> _knownCharacters = new List<string>();
        private string _selectedCharacterId;

        #region Unity Lifecycle
        private void Start()
        {
            InitializeUI();
            SubscribeToEvents();
            RefreshAll();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        #endregion

        #region Initialization
        private void InitializeUI()
        {
            // 隐藏详情面板
            if (detailPanel != null)
                detailPanel.SetActive(false);
        }
        #endregion

        #region Event Subscription
        private void SubscribeToEvents()
        {
            AttributeManager.OnAttributeLevelUp += OnAttributeLevelUp;
            AttributeManager.OnAttributeExpGained += OnAttributeExpGained;
            RelationshipManager.OnRelationshipLevelUp += OnRelationshipLevelUp;
            RelationshipManager.OnRelationshipExpGained += OnRelationshipExpGained;
        }

        private void UnsubscribeFromEvents()
        {
            if (AttributeManager.Instance != null)
            {
                AttributeManager.OnAttributeLevelUp -= OnAttributeLevelUp;
                AttributeManager.OnAttributeExpGained -= OnAttributeExpGained;
            }

            if (RelationshipManager.Instance != null)
            {
                RelationshipManager.OnRelationshipLevelUp -= OnRelationshipLevelUp;
                RelationshipManager.OnRelationshipExpGained -= OnRelationshipExpGained;
            }
        }

        private void OnAttributeLevelUp(AttributeType type, int newLevel)
        {
            if (autoRefresh)
                RefreshAttributes();
        }

        private void OnAttributeExpGained(AttributeType type, int amount, int currentExp)
        {
            if (autoRefresh)
                RefreshAttributes();
        }

        private void OnRelationshipLevelUp(string characterId, int newLevel)
        {
            if (autoRefresh)
            {
                RefreshRelationships();
                // 如果是当前选中的角色，刷新详情
                if (_selectedCharacterId == characterId)
                    RefreshCharacterDetail(_selectedCharacterId);
            }
        }

        private void OnRelationshipExpGained(string characterId, int amount, int currentExp)
        {
            if (autoRefresh)
            {
                RefreshRelationships();
                // 如果是当前选中的角色，刷新详情
                if (_selectedCharacterId == characterId)
                    RefreshCharacterDetail(_selectedCharacterId);
            }
        }
        #endregion

        #region Display Management
        /// <summary>
        /// 刷新所有显示
        /// </summary>
        public void RefreshAll()
        {
            RefreshAttributes();
            RefreshRelationships();
        }

        /// <summary>
        /// 刷新属性显示
        /// </summary>
        public void RefreshAttributes()
        {
            GameState gameState = GameManager.Instance.CurrentGameState;

            // 更新知识
            UpdateAttributeDisplay(
                gameState.Knowledge,
                knowledgeText,
                knowledgeSlider,
                "知识"
            );

            // 更新战斗
            UpdateAttributeDisplay(
                gameState.Combat,
                combatText,
                combatSlider,
                "战斗"
            );

            // 更新魅力
            UpdateAttributeDisplay(
                gameState.Charisma,
                charismaText,
                charismaSlider,
                "魅力"
            );

            // 更新勇气
            UpdateAttributeDisplay(
                gameState.Courage,
                courageText,
                courageSlider,
                "勇气"
            );
        }

        private void UpdateAttributeDisplay(AttributeData attr, TextMeshProUGUI text, Slider slider, string name)
        {
            if (text != null)
            {
                text.text = $"{name} Lv{attr.Level}";
            }

            if (slider != null)
            {
                int expToNext = AttributeManager.GetExpRequiredForLevel(attr.Level + 1);
                int currentLevelExp = AttributeManager.GetExpRequiredForLevel(attr.Level);
                int expInLevel = attr.Exp - currentLevelExp;
                int expNeeded = expToNext - currentLevelExp;

                slider.value = expNeeded > 0 ? (float)expInLevel / expNeeded : 1f;
            }
        }

        /// <summary>
        /// 刷新关系列表
        /// </summary>
        public void RefreshRelationships()
        {
            if (relationshipListContainer == null) return;

            // 清空现有列表
            foreach (Transform child in relationshipListContainer)
            {
                Destroy(child.gameObject);
            }

            GameState gameState = GameManager.Instance.CurrentGameState;
            _knownCharacters.Clear();

            // 获取所有已知角色
            foreach (var relationship in gameState.Relationships)
            {
                _knownCharacters.Add(relationship.Key);
                CreateRelationshipItem(relationship.Key, relationship.Value);
            }

            // 如果没有认识的角色
            if (_knownCharacters.Count == 0)
            {
                CreateNoCharactersMessage();
            }
        }

        private void CreateRelationshipItem(string characterId, RelationshipData relationship)
        {
            if (relationshipItemPrefab == null) return;

            GameObject itemObj = Instantiate(relationshipItemPrefab, relationshipListContainer);

            // 加载角色数据
            CharacterData characterData = Resources.Load<CharacterData>($"Characters/{characterId}");

            // 设置角色名称
            TextMeshProUGUI nameText = itemObj.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            if (nameText != null)
            {
                string displayName = characterData != null ? characterData.characterName : characterId;
                nameText.text = displayName;
            }

            // 设置等级
            TextMeshProUGUI levelText = itemObj.transform.Find("LevelText")?.GetComponent<TextMeshProUGUI>();
            if (levelText != null)
            {
                levelText.text = $"Lv{relationship.Level}";
            }

            // 设置图标
            Image icon = itemObj.transform.Find("Icon")?.GetComponent<Image>();
            if (icon != null && characterData != null && characterData.icon != null)
            {
                icon.sprite = characterData.icon;
            }

            // 设置进度条
            Slider progressSlider = itemObj.GetComponentInChildren<Slider>();
            if (progressSlider != null)
            {
                int expToNext = RelationshipManager.GetExpRequiredForLevel(relationship.Level + 1);
                int currentLevelExp = RelationshipManager.GetExpRequiredForLevel(relationship.Level);
                int expInLevel = relationship.Exp - currentLevelExp;
                int expNeeded = expToNext - currentLevelExp;

                progressSlider.value = expNeeded > 0 ? (float)expInLevel / expNeeded : 1f;
            }

            // 设置点击事件
            Button button = itemObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnCharacterSelected(characterId));
            }
        }

        private void CreateNoCharactersMessage()
        {
            if (relationshipItemPrefab == null) return;

            GameObject messageObj = Instantiate(relationshipItemPrefab, relationshipListContainer);
            Button button = messageObj.GetComponent<Button>();
            if (button != null)
                button.interactable = false;

            TextMeshProUGUI text = messageObj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = "还没有认识任何人";
        }
        #endregion

        #region Character Selection
        private void OnCharacterSelected(string characterId)
        {
            _selectedCharacterId = characterId;

            // 显示详情面板
            if (detailPanel != null)
            {
                detailPanel.SetActive(true);
                RefreshCharacterDetail(characterId);
            }
        }

        private void RefreshCharacterDetail(string characterId)
        {
            // 加载角色数据
            CharacterData characterData = Resources.Load<CharacterData>($"Characters/{characterId}");
            if (characterData == null)
            {
                Debug.LogWarning($"[CharacterInfoUI] Character data not found: {characterId}");
                return;
            }

            // 获取关系数据
            GameState gameState = GameManager.Instance.CurrentGameState;
            RelationshipData relationship = gameState.GetRelationship(characterId);

            // 更新立绘
            if (characterPortrait != null)
            {
                Sprite portrait = characterData.GetPortrait(CharacterEmotion.Normal);
                if (portrait != null)
                {
                    characterPortrait.sprite = portrait;
                    characterPortrait.enabled = true;
                }
                else
                {
                    characterPortrait.enabled = false;
                }
            }

            // 更新名称
            if (characterNameText != null)
            {
                characterNameText.text = characterData.characterName;
            }

            // 更新描述
            if (characterDescriptionText != null)
            {
                characterDescriptionText.text = characterData.description;
            }

            // 更新关系等级
            if (relationshipLevelText != null && relationship != null)
            {
                relationshipLevelText.text = $"关系等级: Lv{relationship.Level}";
            }

            // 更新关系进度条
            if (relationshipProgressSlider != null && relationship != null)
            {
                int expToNext = RelationshipManager.GetExpRequiredForLevel(relationship.Level + 1);
                int currentLevelExp = RelationshipManager.GetExpRequiredForLevel(relationship.Level);
                int expInLevel = relationship.Exp - currentLevelExp;
                int expNeeded = expToNext - currentLevelExp;

                relationshipProgressSlider.value = expNeeded > 0 ? (float)expInLevel / expNeeded : 1f;
            }

            // 更新解锁内容
            if (unlockContentText != null && relationship != null)
            {
                unlockContentText.text = GetUnlockContentString(characterData, relationship.Level);
            }
        }

        private string GetUnlockContentString(CharacterData characterData, int currentLevel)
        {
            List<string> unlocks = new List<string>();

            // 当前等级解锁
            var currentUnlock = characterData.GetUnlockInfo(currentLevel);
            if (currentUnlock.HasAnyUnlocks())
            {
                unlocks.Add($"=== Lv{currentLevel} 已解锁 ===");
                if (currentUnlock.unlockedEvents.Count > 0)
                    unlocks.Add($"• 活动: {string.Join(", ", currentUnlock.unlockedEvents)}");
                if (!string.IsNullOrEmpty(currentUnlock.unlockedMission))
                    unlocks.Add($"• 任务: {currentUnlock.unlockedMission}");
                if (!string.IsNullOrEmpty(currentUnlock.specialEvent))
                    unlocks.Add($"• 特殊事件: {currentUnlock.specialEvent}");
                if (!string.IsNullOrEmpty(currentUnlock.endingUnlocked))
                    unlocks.Add($"• 结局: {currentUnlock.endingUnlocked}");
            }

            // 下一等级预览
            if (currentLevel < 10)
            {
                var nextUnlock = characterData.GetUnlockInfo(currentLevel + 1);
                if (nextUnlock.HasAnyUnlocks())
                {
                    unlocks.Add($"\n=== Lv{currentLevel + 1} 可解锁 ===");
                    if (nextUnlock.unlockedEvents.Count > 0)
                        unlocks.Add($"• 活动: {string.Join(", ", nextUnlock.unlockedEvents)}");
                    if (!string.IsNullOrEmpty(nextUnlock.unlockedMission))
                        unlocks.Add($"• 任务: {nextUnlock.unlockedMission}");
                    if (!string.IsNullOrEmpty(nextUnlock.specialEvent))
                        unlocks.Add($"• 特殊事件: {nextUnlock.specialEvent}");
                    if (!string.IsNullOrEmpty(nextUnlock.endingUnlocked))
                        unlocks.Add($"• 结局: {nextUnlock.endingUnlocked}");
                }
            }

            return unlocks.Count > 0 ? string.Join("\n", unlocks) : "无特殊解锁内容";
        }
        #endregion

        #region Public API
        /// <summary>
        /// 显示UI
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            RefreshAll();
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            _selectedCharacterId = null;
            if (detailPanel != null)
                detailPanel.SetActive(false);
        }

        /// <summary>
        /// 隐藏详情面板
        /// </summary>
        public void HideDetailPanel()
        {
            _selectedCharacterId = null;
            if (detailPanel != null)
                detailPanel.SetActive(false);
        }
        #endregion
    }
}
