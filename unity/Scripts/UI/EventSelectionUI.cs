using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using MindLink.Data;

namespace MindLink.UI
{
    /// <summary>
    /// 事件选择UI - 显示可用事件列表并处理玩家选择
    /// </summary>
    public class EventSelectionUI : MonoBehaviour
    {
        [Header("UI引用")]
        [Tooltip("事件列表容器")]
        public Transform eventListContainer;

        [Tooltip("事件按钮预制体")]
        public GameObject eventButtonPrefab;

        [Tooltip("详情面板")]
        public GameObject detailPanel;

        [Tooltip("详情文本组件")]
        public TextMeshProUGUI detailNameText;
        public TextMeshProUGUI detailDescriptionText;
        public TextMeshProUGUI detailEffectsText;
        public TextMeshProUGUI detailRequirementsText;
        public Image detailIconImage;

        [Tooltip("分类过滤按钮")]
        public Transform filterContainer;

        [Tooltip("确认按钮")]
        public Button confirmButton;

        [Tooltip("取消按钮")]
        public Button cancelButton;

        [Header("设置")]
        [Tooltip("是否自动刷新")]
        public bool autoRefresh = true;

        [Tooltip("是否显示不可用的事件（灰色显示）")]
        public bool showUnavailableEvents = false;

        // 状态
        private List<EventData> _currentEvents = new List<EventData>();
        private EventData _selectedEvent;
        private EventType _currentFilter = EventType.All;
        private Dictionary<EventType, Button> _filterButtons = new Dictionary<EventType, Button>();

        #region Unity Lifecycle
        private void Start()
        {
            InitializeUI();
            SubscribeToEvents();
            RefreshEventList();
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

            // 设置按钮监听
            if (confirmButton != null)
                confirmButton.onClick.AddListener(OnConfirmClicked);

            if (cancelButton != null)
                cancelButton.onClick.AddListener(OnCancelClicked);

            // 创建分类过滤按钮
            CreateFilterButtons();
        }

        private void CreateFilterButtons()
        {
            if (filterContainer == null) return;

            // 清空现有按钮
            foreach (Transform child in filterContainer)
            {
                Destroy(child.gameObject);
            }

            _filterButtons.Clear();

            // 创建"全部"按钮
            CreateFilterButton(EventType.All, "全部");

            // 创建各类型按钮
            CreateFilterButton(EventType.Story, "剧情");
            CreateFilterButton(EventType.Study, "学习");
            CreateFilterButton(EventType.Training, "训练");
            CreateFilterButton(EventType.Social, "社交");
            CreateFilterButton(EventType.PartTime, "打工");
            CreateFilterButton(EventType.Exploration, "探索");
            CreateFilterButton(EventType.Rest, "休息");
        }

        private void CreateFilterButton(EventType type, string label)
        {
            if (eventButtonPrefab == null) return;

            GameObject buttonObj = Instantiate(eventButtonPrefab, filterContainer);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            if (text != null)
                text.text = label;

            if (button != null)
            {
                button.onClick.AddListener(() => SetFilter(type));
                _filterButtons[type] = button;
            }

            // 设置初始选中状态
            UpdateFilterButtonState(type, type == _currentFilter);
        }

        private void UpdateFilterButtonState(EventType type, bool selected)
        {
            if (!_filterButtons.ContainsKey(type)) return;

            Button button = _filterButtons[type];
            ColorBlock colors = button.colors;
            colors.normalColor = selected ? new Color(0.2f, 0.6f, 1f) : Color.white;
            button.colors = colors;
        }
        #endregion

        #region Event Subscription
        private void SubscribeToEvents()
        {
            EventManager.OnAvailableEventsChanged += OnAvailableEventsChanged;
            EventManager.OnEventCompleted += OnEventCompleted;
            TimeManager.OnTimeAdvance += OnTimeAdvance;
        }

        private void UnsubscribeFromEvents()
        {
            if (EventManager.Instance != null)
            {
                EventManager.OnAvailableEventsChanged -= OnAvailableEventsChanged;
                EventManager.OnEventCompleted -= OnEventCompleted;
            }

            if (TimeManager.Instance != null)
            {
                TimeManager.OnTimeAdvance -= OnTimeAdvance;
            }
        }

        private void OnAvailableEventsChanged(List<EventData> events)
        {
            if (autoRefresh)
                RefreshEventList();
        }

        private void OnEventCompleted(string eventId)
        {
            // 清除选择
            _selectedEvent = null;
            if (detailPanel != null)
                detailPanel.SetActive(false);

            // 刷新列表
            if (autoRefresh)
                RefreshEventList();
        }

        private void OnTimeAdvance(int day, Timeslot timeslot)
        {
            if (autoRefresh)
                RefreshEventList();
        }
        #endregion

        #region Event List Management
        /// <summary>
        /// 刷新事件列表
        /// </summary>
        public void RefreshEventList()
        {
            if (EventManager.Instance == null || eventListContainer == null)
                return;

            // 清空现有列表
            foreach (Transform child in eventListContainer)
            {
                Destroy(child.gameObject);
            }

            // 获取当前时间段的可用事件
            Timeslot currentTimeslot = GameManager.Instance.CurrentGameState.CurrentTimeslot;
            _currentEvents = EventManager.Instance.GetAvailableEventsForTimeslot(currentTimeslot);

            // 应用过滤
            if (_currentFilter != EventType.All)
            {
                _currentEvents = _currentEvents.Where(e => e.eventType == _currentFilter).ToList();
            }

            // 按优先级排序
            _currentEvents = _currentEvents.OrderByDescending(e => e.priority).ToList();

            // 创建事件按钮
            foreach (var eventData in _currentEvents)
            {
                CreateEventButton(eventData);
            }

            // 如果没有可用事件
            if (_currentEvents.Count == 0)
            {
                CreateNoEventsMessage();
            }
        }

        private void CreateEventButton(EventData eventData)
        {
            if (eventButtonPrefab == null) return;

            GameObject buttonObj = Instantiate(eventButtonPrefab, eventListContainer);
            Button button = buttonObj.GetComponent<Button>();

            // 设置文本
            TextMeshProUGUI[] texts = buttonObj.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length > 0)
                texts[0].text = eventData.eventName;
            if (texts.Length > 1)
                texts[1].text = GetEventTypeString(eventData.eventType);

            // 设置图标
            Image icon = buttonObj.transform.Find("Icon")?.GetComponent<Image>();
            if (icon != null && eventData.icon != null)
                icon.sprite = eventData.icon;

            // 设置点击事件
            if (button != null)
            {
                button.onClick.AddListener(() => OnEventSelected(eventData));
            }
        }

        private void CreateNoEventsMessage()
        {
            if (eventButtonPrefab == null) return;

            GameObject messageObj = Instantiate(eventButtonPrefab, eventListContainer);
            Button button = messageObj.GetComponent<Button>();
            if (button != null)
                button.interactable = false;

            TextMeshProUGUI text = messageObj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = "当前时间段没有可用的活动";
        }
        #endregion

        #region Event Selection
        private void OnEventSelected(EventData eventData)
        {
            _selectedEvent = eventData;

            // 显示详情面板
            if (detailPanel != null)
            {
                detailPanel.SetActive(true);
                UpdateDetailPanel(eventData);
            }
        }

        private void UpdateDetailPanel(EventData eventData)
        {
            // 更新名称
            if (detailNameText != null)
                detailNameText.text = eventData.eventName;

            // 更新描述
            if (detailDescriptionText != null)
                detailDescriptionText.text = eventData.description;

            // 更新图标
            if (detailIconImage != null && eventData.icon != null)
                detailIconImage.sprite = eventData.icon;

            // 更新效果
            if (detailEffectsText != null)
                detailEffectsText.text = GetEffectsString(eventData);

            // 更新需求
            if (detailRequirementsText != null)
                detailRequirementsText.text = GetRequirementsString(eventData);
        }

        private string GetEffectsString(EventData eventData)
        {
            List<string> effects = new List<string>();

            // 属性变化
            foreach (var change in eventData.attributeChanges)
            {
                string sign = change.amount > 0 ? "+" : "";
                effects.Add($"{GetAttributeString(change.attributeType)} {sign}{change.amount}");
            }

            // 关系变化
            foreach (var change in eventData.relationshipChanges)
            {
                string sign = change.amount > 0 ? "+" : "";
                effects.Add($"{change.characterId}好感度 {sign}{change.amount}");
            }

            // 消耗时间
            if (eventData.consumeTimeslot)
            {
                effects.Add("⏱ 消耗1个时间段");
            }

            return effects.Count > 0 ? string.Join("\n", effects) : "无特殊效果";
        }

        private string GetRequirementsString(EventData eventData)
        {
            List<string> requirements = new List<string>();

            // 属性需求
            foreach (var req in eventData.attributeRequirements)
            {
                requirements.Add($"{GetAttributeString(req.attributeType)} Lv{req.requiredLevel}");
            }

            // 关系需求
            foreach (var req in eventData.relationshipRequirements)
            {
                requirements.Add($"{req.characterId} Lv{req.requiredLevel}");
            }

            return requirements.Count > 0 ? string.Join(", ", requirements) : "无特殊要求";
        }
        #endregion

        #region Button Callbacks
        private void OnConfirmClicked()
        {
            if (_selectedEvent == null)
            {
                Debug.LogWarning("[EventSelectionUI] No event selected!");
                return;
            }

            // 执行事件
            EventManager.Instance.ExecuteEvent(_selectedEvent);

            // 隐藏UI
            if (detailPanel != null)
                detailPanel.SetActive(false);

            _selectedEvent = null;
        }

        private void OnCancelClicked()
        {
            // 取消选择
            _selectedEvent = null;

            // 隐藏详情面板
            if (detailPanel != null)
                detailPanel.SetActive(false);
        }

        /// <summary>
        /// 设置过滤类型
        /// </summary>
        public void SetFilter(EventType type)
        {
            if (_currentFilter == type) return;

            // 更新过滤器状态
            UpdateFilterButtonState(_currentFilter, false);
            _currentFilter = type;
            UpdateFilterButtonState(_currentFilter, true);

            // 刷新列表
            RefreshEventList();
        }
        #endregion

        #region Helper Methods
        private string GetEventTypeString(EventType type)
        {
            switch (type)
            {
                case EventType.Story: return "📖剧情";
                case EventType.Study: return "📚学习";
                case EventType.Training: return "💪训练";
                case EventType.Social: return "👥社交";
                case EventType.PartTime: return "💼打工";
                case EventType.Exploration: return "🔍探索";
                case EventType.Rest: return "😴休息";
                case EventType.Battle: return "⚔战斗";
                case EventType.Puzzle: return "🧩解谜";
                default: return type.ToString();
            }
        }

        private string GetAttributeString(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.Knowledge: return "知识";
                case AttributeType.Combat: return "战斗";
                case AttributeType.Charisma: return "魅力";
                case AttributeType.Courage: return "勇气";
                default: return type.ToString();
            }
        }
        #endregion

        #region Public API
        /// <summary>
        /// 显示UI
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            RefreshEventList();
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            _selectedEvent = null;
            if (detailPanel != null)
                detailPanel.SetActive(false);
        }

        /// <summary>
        /// 获取当前显示的事件数量
        /// </summary>
        public int GetEventCount()
        {
            return _currentEvents.Count;
        }
        #endregion
    }
}
