using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MindLink.UI
{
    /// <summary>
    /// 主游戏UI管理器 - 协调所有UI组件
    /// </summary>
    public class MainGameUI : MonoBehaviour
    {
        [Header("UI组件引用")]
        [Tooltip("事件选择UI")]
        public EventSelectionUI eventSelectionUI;

        [Tooltip("对话UI")]
        public DialogueUI dialogueUI;

        [Tooltip("任务追踪UI")]
        public MissionTrackerUI missionTrackerUI;

        [Tooltip("角色信息UI")]
        public CharacterInfoUI characterInfoUI;

        [Header("顶部信息栏")]
        [Tooltip("日期文本")]
        public TextMeshProUGUI dateText;

        [Tooltip("时间段文本")]
        public TextMeshProUGUI timeslotText;

        [Tooltip("金钱文本")]
        public TextMeshProUGUI moneyText;

        [Header("快捷按钮")]
        [Tooltip("菜单按钮")]
        public Button menuButton;

        [Tooltip("角色按钮")]
        public Button characterButton;

        [Tooltip("任务按钮")]
        public Button missionButton;

        [Tooltip("存档按钮")]
        public Button saveButton;

        [Tooltip("读档按钮")]
        public Button loadButton;

        [Header("通知系统")]
        [Tooltip("通知文本")]
        public TextMeshProUGUI notificationText;

        [Tooltip("通知面板")]
        public GameObject notificationPanel;

        [Tooltip("通知显示时间")]
        public float notificationDuration = 3f;

        // 状态
        private float _notificationTimer = 0f;

        #region Unity Lifecycle
        private void Start()
        {
            InitializeUI();
            SubscribeToEvents();
            RefreshAll();
        }

        private void Update()
        {
            // 通知计时器
            if (_notificationTimer > 0f)
            {
                _notificationTimer -= Time.deltaTime;
                if (_notificationTimer <= 0f)
                {
                    HideNotification();
                }
            }

            // 快捷键
            HandleShortcuts();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        #endregion

        #region Initialization
        private void InitializeUI()
        {
            // 隐藏通知面板
            if (notificationPanel != null)
                notificationPanel.SetActive(false);

            // 设置按钮监听
            if (menuButton != null)
                menuButton.onClick.AddListener(OnMenuButtonClicked);

            if (characterButton != null)
                characterButton.onClick.AddListener(OnCharacterButtonClicked);

            if (missionButton != null)
                missionButton.onClick.AddListener(OnMissionButtonClicked);

            if (saveButton != null)
                saveButton.onClick.AddListener(OnSaveButtonClicked);

            if (loadButton != null)
                loadButton.onClick.AddListener(OnLoadButtonClicked);

            // 默认隐藏一些UI
            if (eventSelectionUI != null)
                eventSelectionUI.Hide();

            if (characterInfoUI != null)
                characterInfoUI.Hide();

            if (missionTrackerUI != null && missionTrackerUI.gameObject != eventSelectionUI?.gameObject)
                missionTrackerUI.Hide();
        }
        #endregion

        #region Event Subscription
        private void SubscribeToEvents()
        {
            // 时间事件
            TimeManager.OnTimeAdvance += OnTimeAdvance;
            TimeManager.OnDayStart += OnDayStart;

            // 属性事件
            AttributeManager.OnAttributeLevelUp += OnAttributeLevelUp;

            // 关系事件
            RelationshipManager.OnRelationshipLevelUp += OnRelationshipLevelUp;

            // 任务事件
            MissionManager.OnMissionStarted += OnMissionStarted;
            MissionManager.OnMissionCompleted += OnMissionCompleted;

            // 事件事件
            EventManager.OnEventCompleted += OnEventCompleted;

            // 对话事件
            DialogueManager.OnDialogueStart += OnDialogueStart;
            DialogueManager.OnDialogueEnd += OnDialogueEnd;
        }

        private void UnsubscribeFromEvents()
        {
            if (TimeManager.Instance != null)
            {
                TimeManager.OnTimeAdvance -= OnTimeAdvance;
                TimeManager.OnDayStart -= OnDayStart;
            }

            if (AttributeManager.Instance != null)
            {
                AttributeManager.OnAttributeLevelUp -= OnAttributeLevelUp;
            }

            if (RelationshipManager.Instance != null)
            {
                RelationshipManager.OnRelationshipLevelUp -= OnRelationshipLevelUp;
            }

            if (MissionManager.Instance != null)
            {
                MissionManager.OnMissionStarted -= OnMissionStarted;
                MissionManager.OnMissionCompleted -= OnMissionCompleted;
            }

            if (EventManager.Instance != null)
            {
                EventManager.OnEventCompleted -= OnEventCompleted;
            }

            if (DialogueManager.Instance != null)
            {
                DialogueManager.OnDialogueStart -= OnDialogueStart;
                DialogueManager.OnDialogueEnd -= OnDialogueEnd;
            }
        }

        private void OnTimeAdvance(int day, Timeslot timeslot)
        {
            RefreshTopBar();
            ShowNotification($"Day {day} - {GetTimeslotString(timeslot)}");
        }

        private void OnDayStart(int day)
        {
            RefreshTopBar();
            ShowNotification($"新的一天！Day {day}");
        }

        private void OnAttributeLevelUp(AttributeType type, int newLevel)
        {
            ShowNotification($"{GetAttributeString(type)} 提升到 Lv{newLevel}！");
        }

        private void OnRelationshipLevelUp(string characterId, int newLevel)
        {
            ShowNotification($"与 {characterId} 的关系提升到 Lv{newLevel}！");
        }

        private void OnMissionStarted(string missionId)
        {
            ShowNotification($"新任务：{missionId}");
        }

        private void OnMissionCompleted(string missionId, bool success)
        {
            if (success)
                ShowNotification($"任务完成：{missionId}");
            else
                ShowNotification($"任务失败：{missionId}");
        }

        private void OnEventCompleted(string eventId)
        {
            RefreshTopBar();
        }

        private void OnDialogueStart(Data.DialogueData dialogueData)
        {
            // 对话开始时隐藏其他UI
            if (eventSelectionUI != null)
                eventSelectionUI.Hide();
        }

        private void OnDialogueEnd()
        {
            // 对话结束后显示事件选择UI
            if (eventSelectionUI != null)
                eventSelectionUI.Show();
        }
        #endregion

        #region UI Refresh
        /// <summary>
        /// 刷新所有UI
        /// </summary>
        public void RefreshAll()
        {
            RefreshTopBar();

            if (characterInfoUI != null && characterInfoUI.gameObject.activeSelf)
                characterInfoUI.RefreshAll();

            if (missionTrackerUI != null && missionTrackerUI.gameObject.activeSelf)
                missionTrackerUI.RefreshMissionList();

            if (eventSelectionUI != null && eventSelectionUI.gameObject.activeSelf)
                eventSelectionUI.RefreshEventList();
        }

        /// <summary>
        /// 刷新顶部信息栏
        /// </summary>
        public void RefreshTopBar()
        {
            GameState gameState = GameManager.Instance.CurrentGameState;

            // 更新日期
            if (dateText != null)
            {
                dateText.text = $"Day {gameState.CurrentDay}";
            }

            // 更新时间段
            if (timeslotText != null)
            {
                timeslotText.text = GetTimeslotString(gameState.CurrentTimeslot);
            }

            // 更新金钱
            if (moneyText != null)
            {
                moneyText.text = $"¥{gameState.Money}";
            }
        }
        #endregion

        #region Button Callbacks
        private void OnMenuButtonClicked()
        {
            Debug.Log("[MainGameUI] Menu button clicked");
            // TODO: 显示菜单
        }

        private void OnCharacterButtonClicked()
        {
            if (characterInfoUI != null)
            {
                if (characterInfoUI.gameObject.activeSelf)
                    characterInfoUI.Hide();
                else
                    characterInfoUI.Show();
            }
        }

        private void OnMissionButtonClicked()
        {
            if (missionTrackerUI != null)
            {
                if (missionTrackerUI.gameObject.activeSelf)
                    missionTrackerUI.Hide();
                else
                    missionTrackerUI.Show();
            }
        }

        private void OnSaveButtonClicked()
        {
            GameManager.Instance.QuickSave();
            ShowNotification("游戏已保存");
        }

        private void OnLoadButtonClicked()
        {
            GameManager.Instance.QuickLoad();
            ShowNotification("游戏已读取");
            RefreshAll();
        }
        #endregion

        #region Shortcuts
        private void HandleShortcuts()
        {
            // ESC - 菜单
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnMenuButtonClicked();
            }

            // C - 角色信息
            if (Input.GetKeyDown(KeyCode.C))
            {
                OnCharacterButtonClicked();
            }

            // M - 任务
            if (Input.GetKeyDown(KeyCode.M))
            {
                OnMissionButtonClicked();
            }

            // F5 - 快速保存
            if (Input.GetKeyDown(KeyCode.F5))
            {
                OnSaveButtonClicked();
            }

            // F9 - 快速读取
            if (Input.GetKeyDown(KeyCode.F9))
            {
                OnLoadButtonClicked();
            }
        }
        #endregion

        #region Notification System
        /// <summary>
        /// 显示通知
        /// </summary>
        public void ShowNotification(string message)
        {
            if (notificationPanel == null || notificationText == null)
                return;

            notificationText.text = message;
            notificationPanel.SetActive(true);
            _notificationTimer = notificationDuration;
        }

        /// <summary>
        /// 隐藏通知
        /// </summary>
        public void HideNotification()
        {
            if (notificationPanel != null)
                notificationPanel.SetActive(false);
        }
        #endregion

        #region Helper Methods
        private string GetTimeslotString(Timeslot timeslot)
        {
            switch (timeslot)
            {
                case Timeslot.Morning: return "上午";
                case Timeslot.Afternoon: return "下午";
                case Timeslot.Night: return "夜晚";
                default: return timeslot.ToString();
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
        /// 显示事件选择UI
        /// </summary>
        public void ShowEventSelection()
        {
            if (eventSelectionUI != null)
                eventSelectionUI.Show();
        }

        /// <summary>
        /// 隐藏事件选择UI
        /// </summary>
        public void HideEventSelection()
        {
            if (eventSelectionUI != null)
                eventSelectionUI.Hide();
        }
        #endregion
    }
}
