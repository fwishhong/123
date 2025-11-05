using UnityEngine;
using System;
using System.Collections.Generic;

namespace MindLink
{
    /// <summary>
    /// 任务管理器 - 负责任务的创建、追踪和完成判定
    /// </summary>
    public class MissionManager : MonoBehaviour
    {
        #region Singleton
        private static MissionManager _instance;
        public static MissionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("MissionManager");
                    _instance = go.AddComponent<MissionManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 任务开始事件 (MissionID)
        /// </summary>
        public static event Action<string> OnMissionStart;

        /// <summary>
        /// 任务进度更新事件 (MissionID, ObjectiveID, Progress)
        /// </summary>
        public static event Action<string, string, int> OnMissionProgressUpdate;

        /// <summary>
        /// 任务完成事件 (MissionID, Success)
        /// </summary>
        public static event Action<string, bool> OnMissionComplete;

        /// <summary>
        /// 任务失败事件 (MissionID)
        /// </summary>
        public static event Action<string> OnMissionFailed;

        /// <summary>
        /// 任务截止日提醒事件 (MissionID, DaysLeft)
        /// </summary>
        public static event Action<string, int> OnMissionDeadlineWarning;
        #endregion

        #region Properties
        private GameState _gameState;

        /// <summary>
        /// 当前活跃的任务列表
        /// </summary>
        public List<MissionProgress> ActiveMissions => _gameState?.ActiveMissions;

        /// <summary>
        /// 已完成的任务ID列表
        /// </summary>
        public List<string> CompletedMissions => _gameState?.CompletedMissions;
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

        private void Start()
        {
            // 订阅时间事件来检查截止日期
            TimeManager.OnDayStart += CheckMissionDeadlines;
        }

        private void OnDestroy()
        {
            TimeManager.OnDayStart -= CheckMissionDeadlines;
        }
        #endregion

        #region Initialization
        /// <summary>
        /// 初始化任务管理器
        /// </summary>
        public void Initialize(GameState gameState)
        {
            _gameState = gameState;
            Debug.Log("[MissionManager] Initialized.");
        }
        #endregion

        #region Mission Operations
        /// <summary>
        /// 开始新任务
        /// </summary>
        /// <param name="missionId">任务ID</param>
        /// <param name="name">任务名称</param>
        /// <param name="deadline">截止天数</param>
        /// <param name="objectives">任务目标字典</param>
        public void StartMission(string missionId, string name, int deadline,
                                Dictionary<string, ObjectiveProgress> objectives = null)
        {
            if (_gameState == null) return;

            // 检查是否已经在进行中
            if (GetActiveMission(missionId) != null)
            {
                Debug.LogWarning($"[MissionManager] Mission {missionId} is already active!");
                return;
            }

            // 检查是否已完成
            if (_gameState.CompletedMissions.Contains(missionId))
            {
                Debug.LogWarning($"[MissionManager] Mission {missionId} is already completed!");
                return;
            }

            var mission = new MissionProgress
            {
                MissionId = missionId,
                Name = name,
                StartDay = TimeManager.Instance.CurrentDay,
                Deadline = deadline,
                Objectives = objectives ?? new Dictionary<string, ObjectiveProgress>()
            };

            _gameState.ActiveMissions.Add(mission);

            Debug.Log($"[MissionManager] Mission started: {name} (Deadline: Day {deadline})");
            OnMissionStart?.Invoke(missionId);

            // 显示通知
            UIManager.Instance?.ShowNotification(
                $"新任务：{name}",
                NotificationType.Info
            );
        }

        /// <summary>
        /// 更新任务目标进度
        /// </summary>
        public void UpdateMissionObjective(string missionId, string objectiveId, int amount = 1)
        {
            var mission = GetActiveMission(missionId);
            if (mission == null)
            {
                Debug.LogWarning($"[MissionManager] Mission {missionId} not found!");
                return;
            }

            if (!mission.Objectives.ContainsKey(objectiveId))
            {
                Debug.LogWarning($"[MissionManager] Objective {objectiveId} not found in mission {missionId}!");
                return;
            }

            var objective = mission.Objectives[objectiveId];
            objective.AddProgress(amount);

            Debug.Log($"[MissionManager] Objective updated: {objective.Description} " +
                      $"({objective.Current}/{objective.Required})");

            OnMissionProgressUpdate?.Invoke(missionId, objectiveId, objective.Current);

            // 检查任务是否完成
            if (mission.CheckAllObjectivesComplete())
            {
                CompleteMission(missionId, true);
            }
        }

        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="missionId">任务ID</param>
        /// <param name="success">是否成功完成</param>
        public void CompleteMission(string missionId, bool success)
        {
            var mission = GetActiveMission(missionId);
            if (mission == null) return;

            mission.IsCompleted = success;
            mission.IsFailed = !success;

            // 从活跃列表移除
            _gameState.ActiveMissions.Remove(mission);

            if (success)
            {
                // 添加到已完成列表
                _gameState.CompletedMissions.Add(missionId);
                Debug.Log($"[MissionManager] Mission completed: {mission.Name}");

                UIManager.Instance?.ShowNotification(
                    $"任务完成：{mission.Name}",
                    NotificationType.Success
                );
            }
            else
            {
                Debug.Log($"[MissionManager] Mission failed: {mission.Name}");

                UIManager.Instance?.ShowNotification(
                    $"任务失败：{mission.Name}",
                    NotificationType.Error
                );
            }

            OnMissionComplete?.Invoke(missionId, success);
        }

        /// <summary>
        /// 获取活跃任务
        /// </summary>
        public MissionProgress GetActiveMission(string missionId)
        {
            if (_gameState == null || _gameState.ActiveMissions == null) return null;

            return _gameState.ActiveMissions.Find(m => m.MissionId == missionId);
        }

        /// <summary>
        /// 检查任务是否已完成
        /// </summary>
        public bool IsMissionCompleted(string missionId)
        {
            return _gameState?.CompletedMissions.Contains(missionId) ?? false;
        }

        /// <summary>
        /// 检查任务是否活跃
        /// </summary>
        public bool IsMissionActive(string missionId)
        {
            return GetActiveMission(missionId) != null;
        }

        /// <summary>
        /// 获取任务完成度百分比
        /// </summary>
        public float GetMissionCompletion(string missionId)
        {
            var mission = GetActiveMission(missionId);
            return mission?.GetCompletionPercentage() ?? 0f;
        }
        #endregion

        #region Deadline Management
        /// <summary>
        /// 检查任务截止日期（每天调用）
        /// </summary>
        private void CheckMissionDeadlines(int currentDay)
        {
            if (_gameState == null || _gameState.ActiveMissions == null) return;

            var missionsToFail = new List<MissionProgress>();

            foreach (var mission in _gameState.ActiveMissions)
            {
                int daysLeft = mission.Deadline - currentDay;

                if (daysLeft <= 0)
                {
                    // 任务失败
                    missionsToFail.Add(mission);
                }
                else if (daysLeft <= 2)
                {
                    // 截止日提醒
                    Debug.Log($"[MissionManager] Mission deadline warning: {mission.Name} ({daysLeft} days left)");
                    OnMissionDeadlineWarning?.Invoke(mission.MissionId, daysLeft);

                    UIManager.Instance?.ShowNotification(
                        $"任务截止提醒：{mission.Name} 还有 {daysLeft} 天！",
                        NotificationType.Warning
                    );
                }
            }

            // 失败所有过期任务
            foreach (var mission in missionsToFail)
            {
                CompleteMission(mission.MissionId, false);
                OnMissionFailed?.Invoke(mission.MissionId);
            }
        }

        /// <summary>
        /// 获取任务剩余天数
        /// </summary>
        public int GetMissionDaysRemaining(string missionId)
        {
            var mission = GetActiveMission(missionId);
            if (mission == null) return 0;

            return Mathf.Max(0, mission.Deadline - TimeManager.Instance.CurrentDay);
        }
        #endregion

        #region Objective Management
        /// <summary>
        /// 添加任务目标
        /// </summary>
        public void AddMissionObjective(string missionId, string objectiveId,
                                       string description, int required, int current = 0)
        {
            var mission = GetActiveMission(missionId);
            if (mission == null) return;

            if (mission.Objectives.ContainsKey(objectiveId))
            {
                Debug.LogWarning($"[MissionManager] Objective {objectiveId} already exists!");
                return;
            }

            mission.Objectives.Add(objectiveId, new ObjectiveProgress(description, required, current));
            Debug.Log($"[MissionManager] Objective added: {description}");
        }

        /// <summary>
        /// 获取任务目标进度
        /// </summary>
        public ObjectiveProgress GetObjective(string missionId, string objectiveId)
        {
            var mission = GetActiveMission(missionId);
            if (mission == null || !mission.Objectives.ContainsKey(objectiveId))
                return null;

            return mission.Objectives[objectiveId];
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// 获取所有活跃任务数量
        /// </summary>
        public int GetActiveMissionCount()
        {
            return _gameState?.ActiveMissions?.Count ?? 0;
        }

        /// <summary>
        /// 获取已完成任务数量
        /// </summary>
        public int GetCompletedMissionCount()
        {
            return _gameState?.CompletedMissions?.Count ?? 0;
        }

        /// <summary>
        /// 获取最紧急的任务
        /// </summary>
        public MissionProgress GetMostUrgentMission()
        {
            if (_gameState == null || _gameState.ActiveMissions == null || _gameState.ActiveMissions.Count == 0)
                return null;

            MissionProgress mostUrgent = _gameState.ActiveMissions[0];
            int currentDay = TimeManager.Instance.CurrentDay;

            foreach (var mission in _gameState.ActiveMissions)
            {
                int daysLeft = mission.Deadline - currentDay;
                int mostUrgentDaysLeft = mostUrgent.Deadline - currentDay;

                if (daysLeft < mostUrgentDaysLeft)
                {
                    mostUrgent = mission;
                }
            }

            return mostUrgent;
        }
        #endregion

        #region Debug
        /// <summary>
        /// 获取任务调试信息
        /// </summary>
        public string GetDebugInfo()
        {
            if (_gameState == null) return "No mission data";

            string info = $"=== Missions ===\n";
            info += $"Active: {GetActiveMissionCount()}\n";
            info += $"Completed: {GetCompletedMissionCount()}\n\n";

            if (_gameState.ActiveMissions != null)
            {
                foreach (var mission in _gameState.ActiveMissions)
                {
                    int daysLeft = GetMissionDaysRemaining(mission.MissionId);
                    info += $"[{mission.Name}]\n";
                    info += $"  Deadline: Day {mission.Deadline} ({daysLeft} days left)\n";
                    info += $"  Progress: {mission.GetCompletionPercentage():P0}\n";
                    info += $"  Objectives:\n";

                    foreach (var obj in mission.Objectives.Values)
                    {
                        string status = obj.IsCompleted() ? "✓" : "✗";
                        info += $"    {status} {obj.Description} ({obj.Current}/{obj.Required})\n";
                    }
                }
            }

            return info;
        }
        #endregion
    }
}
