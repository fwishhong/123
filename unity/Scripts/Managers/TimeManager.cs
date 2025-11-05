using UnityEngine;
using System;

namespace MindLink
{
    /// <summary>
    /// 时间管理器 - 负责游戏内时间的推进和管理
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        #region Singleton
        private static TimeManager _instance;
        public static TimeManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("TimeManager");
                    _instance = go.AddComponent<TimeManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 时间推进事件 (Day, Timeslot)
        /// </summary>
        public static event Action<int, Timeslot> OnTimeAdvance;

        /// <summary>
        /// 新的一天开始事件 (Day)
        /// </summary>
        public static event Action<int> OnDayStart;

        /// <summary>
        /// 一天结束事件 (Day)
        /// </summary>
        public static event Action<int> OnDayEnd;

        /// <summary>
        /// 时间段变化事件 (Timeslot)
        /// </summary>
        public static event Action<Timeslot> OnTimeslotChange;
        #endregion

        #region Properties
        private GameState _gameState;

        /// <summary>
        /// 当前天数
        /// </summary>
        public int CurrentDay => _gameState?.CurrentDay ?? 1;

        /// <summary>
        /// 当前时间段
        /// </summary>
        public Timeslot CurrentTimeslot => _gameState?.CurrentTimeslot ?? Timeslot.Morning;

        /// <summary>
        /// 最大天数
        /// </summary>
        public int MaxDay => _gameState?.MaxDay ?? 100;

        /// <summary>
        /// 剩余天数
        /// </summary>
        public int DaysRemaining => MaxDay - CurrentDay;

        /// <summary>
        /// 今天是否为周末
        /// </summary>
        public bool IsWeekend => GetDayOfWeek() >= 5; // 5=Saturday, 6=Sunday

        /// <summary>
        /// 今天是星期几 (0=Monday, 6=Sunday)
        /// </summary>
        public int DayOfWeek => GetDayOfWeek();
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
        /// 初始化时间管理器
        /// </summary>
        public void Initialize(GameState gameState)
        {
            _gameState = gameState;
            Debug.Log($"[TimeManager] Initialized. Current Day: {CurrentDay}, Timeslot: {CurrentTimeslot}");
        }
        #endregion

        #region Time Advancement
        /// <summary>
        /// 推进时间到下一个时间段
        /// </summary>
        public void AdvanceTime()
        {
            if (_gameState == null)
            {
                Debug.LogError("[TimeManager] GameState is null!");
                return;
            }

            Timeslot previousTimeslot = _gameState.CurrentTimeslot;

            // 根据当前时间段决定下一步
            switch (_gameState.CurrentTimeslot)
            {
                case Timeslot.Morning:
                    _gameState.CurrentTimeslot = Timeslot.Afternoon;
                    break;

                case Timeslot.Afternoon:
                    _gameState.CurrentTimeslot = Timeslot.Night;
                    break;

                case Timeslot.Night:
                    // 夜晚结束，进入下一天
                    AdvanceDay();
                    return;
            }

            Debug.Log($"[TimeManager] Time advanced: Day {CurrentDay} - {previousTimeslot} → {_gameState.CurrentTimeslot}");

            // 触发事件
            OnTimeAdvance?.Invoke(CurrentDay, _gameState.CurrentTimeslot);
            OnTimeslotChange?.Invoke(_gameState.CurrentTimeslot);

            // 减少事件冷却
            UpdateEventCooldowns();

            // 检查时间段触发的事件
            EventManager.Instance?.CheckTimeslotEvents();
        }

        /// <summary>
        /// 进入下一天
        /// </summary>
        private void AdvanceDay()
        {
            if (_gameState == null) return;

            int previousDay = _gameState.CurrentDay;

            // 触发一天结束事件
            OnDayEnd?.Invoke(previousDay);

            // 进入新的一天
            _gameState.CurrentDay++;
            _gameState.CurrentTimeslot = Timeslot.Morning;
            _gameState.TodayActivities.Clear();

            Debug.Log($"[TimeManager] New day started: Day {_gameState.CurrentDay}");

            // 触发新一天开始事件
            OnDayStart?.Invoke(_gameState.CurrentDay);
            OnTimeAdvance?.Invoke(_gameState.CurrentDay, _gameState.CurrentTimeslot);
            OnTimeslotChange?.Invoke(_gameState.CurrentTimeslot);

            // 检查是否游戏结束
            if (_gameState.IsGameOver())
            {
                GameManager.Instance.GameOver();
            }

            // 触发新一天的事件
            EventManager.Instance?.TriggerDayStart();
        }

        /// <summary>
        /// 跳过当前时间段（直接进入下一个）
        /// </summary>
        public void SkipTimeslot()
        {
            Debug.Log("[TimeManager] Skipping current timeslot...");
            AdvanceTime();
        }

        /// <summary>
        /// 直接跳转到指定天数和时间段（调试用）
        /// </summary>
        public void JumpToTime(int day, Timeslot timeslot)
        {
            if (_gameState == null) return;

            Debug.Log($"[TimeManager] Jumping to Day {day} - {timeslot}");

            _gameState.CurrentDay = Mathf.Clamp(day, 1, MaxDay);
            _gameState.CurrentTimeslot = timeslot;

            OnTimeAdvance?.Invoke(_gameState.CurrentDay, _gameState.CurrentTimeslot);
            OnDayStart?.Invoke(_gameState.CurrentDay);
        }
        #endregion

        #region Cooldown Management
        /// <summary>
        /// 更新事件冷却（每次推进时间时调用）
        /// </summary>
        private void UpdateEventCooldowns()
        {
            if (_gameState == null || _gameState.EventCooldowns == null) return;

            // 只在进入新一天时减少冷却（即只在 Night→Morning 时）
            // 这个方法在 AdvanceTime 中每次都调用，但只在特定条件下减少
            // 实际的冷却减少在 AdvanceDay 中通过 DayCooldownManager 处理
        }

        /// <summary>
        /// 每天减少一次冷却（在 AdvanceDay 后调用）
        /// </summary>
        public void DecrementDailyCooldowns()
        {
            if (_gameState == null || _gameState.EventCooldowns == null) return;

            var keysToRemove = new System.Collections.Generic.List<string>();

            foreach (var kvp in _gameState.EventCooldowns)
            {
                _gameState.EventCooldowns[kvp.Key]--;

                if (_gameState.EventCooldowns[kvp.Key] <= 0)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            // 移除冷却结束的事件
            foreach (var key in keysToRemove)
            {
                _gameState.EventCooldowns.Remove(key);
                Debug.Log($"[TimeManager] Event cooldown ended: {key}");
            }
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// 获取当前是星期几 (0=Monday, 6=Sunday)
        /// </summary>
        private int GetDayOfWeek()
        {
            // 假设 Day 1 是星期一
            return (CurrentDay - 1) % 7;
        }

        /// <summary>
        /// 获取星期几的名称
        /// </summary>
        public string GetDayOfWeekName()
        {
            string[] dayNames = { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" };
            return dayNames[GetDayOfWeek()];
        }

        /// <summary>
        /// 获取时间段名称
        /// </summary>
        public string GetTimeslotName(Timeslot timeslot)
        {
            switch (timeslot)
            {
                case Timeslot.Morning: return "早晨";
                case Timeslot.Afternoon: return "下午";
                case Timeslot.Night: return "夜晚";
                default: return "未知";
            }
        }

        /// <summary>
        /// 获取当前时间段名称
        /// </summary>
        public string GetCurrentTimeslotName()
        {
            return GetTimeslotName(CurrentTimeslot);
        }

        /// <summary>
        /// 检查指定天数是否为特殊日期
        /// </summary>
        public bool IsSpecialDay(int day)
        {
            // 可以定义特殊日期（节假日等）
            // 例如：Day 50 是中秋节
            switch (day)
            {
                case 50: return true; // 中秋节示例
                case 100: return true; // 最后一天
                default: return false;
            }
        }

        /// <summary>
        /// 获取游戏进度百分比
        /// </summary>
        public float GetGameProgress()
        {
            return (float)CurrentDay / MaxDay;
        }

        /// <summary>
        /// 检查是否到达截止日期
        /// </summary>
        public bool HasReachedDeadline(int deadline)
        {
            return CurrentDay >= deadline;
        }

        /// <summary>
        /// 获取距离截止日期的剩余天数
        /// </summary>
        public int GetDaysUntilDeadline(int deadline)
        {
            return Mathf.Max(0, deadline - CurrentDay);
        }
        #endregion

        #region Debug
        /// <summary>
        /// 获取时间调试信息
        /// </summary>
        public string GetDebugInfo()
        {
            return $"Day {CurrentDay}/{MaxDay} - {GetCurrentTimeslotName()} ({GetDayOfWeekName()})";
        }
        #endregion
    }
}
