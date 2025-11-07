using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace MindLink.UI
{
    /// <summary>
    /// 任务追踪UI - 显示当前活跃的任务和进度
    /// </summary>
    public class MissionTrackerUI : MonoBehaviour
    {
        [Header("UI引用")]
        [Tooltip("任务列表容器")]
        public Transform missionListContainer;

        [Tooltip("任务项预制体")]
        public GameObject missionItemPrefab;

        [Tooltip("任务详情面板")]
        public GameObject detailPanel;

        [Tooltip("详情文本组件")]
        public TextMeshProUGUI detailNameText;
        public TextMeshProUGUI detailDescriptionText;
        public TextMeshProUGUI detailObjectivesText;
        public TextMeshProUGUI detailDeadlineText;
        public TextMeshProUGUI detailRewardsText;

        [Header("设置")]
        [Tooltip("是否自动刷新")]
        public bool autoRefresh = true;

        [Tooltip("是否显示已完成的任务")]
        public bool showCompletedMissions = false;

        [Tooltip("最大显示任务数量")]
        public int maxDisplayMissions = 5;

        // 状态
        private List<MissionProgress> _currentMissions = new List<MissionProgress>();
        private MissionProgress _selectedMission;

        #region Unity Lifecycle
        private void Start()
        {
            InitializeUI();
            SubscribeToEvents();
            RefreshMissionList();
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
            MissionManager.OnMissionStarted += OnMissionStarted;
            MissionManager.OnMissionCompleted += OnMissionCompleted;
            MissionManager.OnMissionFailed += OnMissionFailed;
            MissionManager.OnObjectiveUpdated += OnObjectiveUpdated;
            MissionManager.OnMissionDeadlineWarning += OnMissionDeadlineWarning;
        }

        private void UnsubscribeFromEvents()
        {
            if (MissionManager.Instance != null)
            {
                MissionManager.OnMissionStarted -= OnMissionStarted;
                MissionManager.OnMissionCompleted -= OnMissionCompleted;
                MissionManager.OnMissionFailed -= OnMissionFailed;
                MissionManager.OnObjectiveUpdated -= OnObjectiveUpdated;
                MissionManager.OnMissionDeadlineWarning -= OnMissionDeadlineWarning;
            }
        }

        private void OnMissionStarted(string missionId)
        {
            if (autoRefresh)
                RefreshMissionList();
        }

        private void OnMissionCompleted(string missionId, bool success)
        {
            if (autoRefresh)
                RefreshMissionList();
        }

        private void OnMissionFailed(string missionId)
        {
            if (autoRefresh)
                RefreshMissionList();
        }

        private void OnObjectiveUpdated(string missionId, string objectiveId, int currentValue)
        {
            if (autoRefresh)
                RefreshMissionList();
        }

        private void OnMissionDeadlineWarning(string missionId, int daysLeft)
        {
            // 可以添加警告视觉效果
            Debug.LogWarning($"[MissionTrackerUI] Mission {missionId} deadline warning: {daysLeft} days left");
        }
        #endregion

        #region Mission List Management
        /// <summary>
        /// 刷新任务列表
        /// </summary>
        public void RefreshMissionList()
        {
            if (MissionManager.Instance == null || missionListContainer == null)
                return;

            // 清空现有列表
            foreach (Transform child in missionListContainer)
            {
                Destroy(child.gameObject);
            }

            // 获取活跃任务
            GameState gameState = GameManager.Instance.CurrentGameState;
            _currentMissions = gameState.ActiveMissions;

            // 如果显示已完成的任务
            if (showCompletedMissions)
            {
                // TODO: 添加已完成任务的显示
            }

            // 限制显示数量
            var missionsToDisplay = _currentMissions.Take(maxDisplayMissions).ToList();

            // 创建任务项
            foreach (var mission in missionsToDisplay)
            {
                CreateMissionItem(mission);
            }

            // 如果没有任务
            if (_currentMissions.Count == 0)
            {
                CreateNoMissionsMessage();
            }
        }

        private void CreateMissionItem(MissionProgress mission)
        {
            if (missionItemPrefab == null) return;

            GameObject itemObj = Instantiate(missionItemPrefab, missionListContainer);

            // 设置任务名称
            TextMeshProUGUI nameText = itemObj.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            if (nameText != null)
            {
                string prefix = mission.IsMainMission ? "【主线】" : "【支线】";
                nameText.text = prefix + mission.Name;
            }

            // 设置进度
            TextMeshProUGUI progressText = itemObj.transform.Find("ProgressText")?.GetComponent<TextMeshProUGUI>();
            if (progressText != null)
            {
                int completedObjectives = mission.Objectives.Count(o => o.Value.CurrentValue >= o.Value.TargetValue);
                int totalObjectives = mission.Objectives.Count;
                progressText.text = $"{completedObjectives}/{totalObjectives}";
            }

            // 设置截止日期
            TextMeshProUGUI deadlineText = itemObj.transform.Find("DeadlineText")?.GetComponent<TextMeshProUGUI>();
            if (deadlineText != null)
            {
                int currentDay = GameManager.Instance.CurrentGameState.CurrentDay;
                int daysLeft = mission.Deadline - currentDay;

                if (daysLeft <= 0)
                {
                    deadlineText.text = "已超时！";
                    deadlineText.color = Color.red;
                }
                else if (daysLeft <= 2)
                {
                    deadlineText.text = $"剩余{daysLeft}天";
                    deadlineText.color = Color.yellow;
                }
                else
                {
                    deadlineText.text = $"剩余{daysLeft}天";
                    deadlineText.color = Color.white;
                }
            }

            // 设置进度条
            Slider progressSlider = itemObj.GetComponentInChildren<Slider>();
            if (progressSlider != null)
            {
                int completedObjectives = mission.Objectives.Count(o => o.Value.CurrentValue >= o.Value.TargetValue);
                int totalObjectives = mission.Objectives.Count;
                progressSlider.value = totalObjectives > 0 ? (float)completedObjectives / totalObjectives : 0f;
            }

            // 设置点击事件
            Button button = itemObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnMissionSelected(mission));
            }
        }

        private void CreateNoMissionsMessage()
        {
            if (missionItemPrefab == null) return;

            GameObject messageObj = Instantiate(missionItemPrefab, missionListContainer);
            Button button = messageObj.GetComponent<Button>();
            if (button != null)
                button.interactable = false;

            TextMeshProUGUI text = messageObj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = "当前没有活跃的任务";
        }
        #endregion

        #region Mission Selection
        private void OnMissionSelected(MissionProgress mission)
        {
            _selectedMission = mission;

            // 显示详情面板
            if (detailPanel != null)
            {
                detailPanel.SetActive(true);
                UpdateDetailPanel(mission);
            }
        }

        private void UpdateDetailPanel(MissionProgress mission)
        {
            // 更新名称
            if (detailNameText != null)
            {
                string prefix = mission.IsMainMission ? "【主线任务】" : "【支线任务】";
                detailNameText.text = prefix + mission.Name;
            }

            // 更新描述
            if (detailDescriptionText != null)
                detailDescriptionText.text = mission.Description;

            // 更新目标
            if (detailObjectivesText != null)
                detailObjectivesText.text = GetObjectivesString(mission);

            // 更新截止日期
            if (detailDeadlineText != null)
            {
                int currentDay = GameManager.Instance.CurrentGameState.CurrentDay;
                int daysLeft = mission.Deadline - currentDay;
                string deadlineColor = daysLeft <= 2 ? "<color=red>" : "";
                string deadlineEndColor = daysLeft <= 2 ? "</color>" : "";
                detailDeadlineText.text = $"截止日期: Day {mission.Deadline} {deadlineColor}(剩余{daysLeft}天){deadlineEndColor}";
            }

            // 更新奖励（需要从MissionData加载）
            if (detailRewardsText != null)
            {
                var missionData = Resources.Load<MindLink.Data.MissionData>($"Missions/{mission.MissionId}");
                if (missionData != null)
                {
                    detailRewardsText.text = GetRewardsString(missionData);
                }
                else
                {
                    detailRewardsText.text = "未知奖励";
                }
            }
        }

        private string GetObjectivesString(MissionProgress mission)
        {
            List<string> objectives = new List<string>();

            foreach (var obj in mission.Objectives)
            {
                string status = obj.Value.CurrentValue >= obj.Value.TargetValue ? "✓" : "○";
                string optional = obj.Value.IsOptional ? " (可选)" : "";
                objectives.Add($"{status} {obj.Value.Description}: {obj.Value.CurrentValue}/{obj.Value.TargetValue}{optional}");
            }

            return string.Join("\n", objectives);
        }

        private string GetRewardsString(MindLink.Data.MissionData missionData)
        {
            List<string> rewards = new List<string>();

            // 属性奖励
            foreach (var reward in missionData.attributeRewards)
            {
                rewards.Add($"{GetAttributeString(reward.attributeType)} +{reward.amount}");
            }

            // 关系奖励
            foreach (var reward in missionData.relationshipRewards)
            {
                rewards.Add($"{reward.characterId}好感度 +{reward.amount}");
            }

            // 道具奖励
            foreach (var reward in missionData.itemRewards)
            {
                rewards.Add($"{reward.itemId} x{reward.amount}");
            }

            return rewards.Count > 0 ? string.Join("\n", rewards) : "无特殊奖励";
        }
        #endregion

        #region Helper Methods
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
            RefreshMissionList();
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            _selectedMission = null;
            if (detailPanel != null)
                detailPanel.SetActive(false);
        }

        /// <summary>
        /// 隐藏详情面板
        /// </summary>
        public void HideDetailPanel()
        {
            _selectedMission = null;
            if (detailPanel != null)
                detailPanel.SetActive(false);
        }

        /// <summary>
        /// 获取当前活跃任务数量
        /// </summary>
        public int GetActiveMissionCount()
        {
            return _currentMissions.Count;
        }
        #endregion
    }
}
