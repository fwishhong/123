using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using MindLink.Data;

namespace MindLink
{
    /// <summary>
    /// 事件管理器 - 负责游戏事件的触发和管理
    /// 使用ScriptableObject数据驱动的事件系统
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        #region Singleton
        private static EventManager _instance;
        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("EventManager");
                    _instance = go.AddComponent<EventManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 事件触发时（参数：eventId, eventData）
        /// </summary>
        public static event Action<string, EventData> OnEventTriggered;

        /// <summary>
        /// 事件完成时（参数：eventId）
        /// </summary>
        public static event Action<string> OnEventCompleted;

        /// <summary>
        /// 有新的可用事件时
        /// </summary>
        public static event Action<List<EventData>> OnAvailableEventsChanged;
        #endregion

        #region Properties
        private GameState _gameState;

        [Header("事件数据库")]
        [Tooltip("所有事件数据的列表")]
        public List<EventData> allEvents = new List<EventData>();

        [Header("调试设置")]
        [Tooltip("在控制台显示详细日志")]
        public bool verboseLogging = true;

        // 缓存：可用事件列表
        private List<EventData> _cachedAvailableEvents = new List<EventData>();
        private bool _needsRefresh = true;
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
            LoadAllEvents();
        }
        #endregion

        #region Initialization
        public void Initialize(GameState gameState)
        {
            _gameState = gameState;
            _needsRefresh = true;

            if (verboseLogging)
                Debug.Log($"[EventManager] Initialized with {allEvents.Count} events.");
        }

        /// <summary>
        /// 从Resources文件夹加载所有事件
        /// </summary>
        private void LoadAllEvents()
        {
            EventData[] loadedEvents = Resources.LoadAll<EventData>("Events");

            if (loadedEvents.Length > 0)
            {
                allEvents.AddRange(loadedEvents);
                Debug.Log($"[EventManager] Loaded {loadedEvents.Length} events from Resources/Events/");
            }
            else if (verboseLogging)
            {
                Debug.LogWarning("[EventManager] No events found in Resources/Events/. Create EventData assets there.");
            }
        }
        #endregion

        #region Event Triggering
        /// <summary>
        /// 触发新一天的事件
        /// </summary>
        public void TriggerDayStart()
        {
            if (_gameState == null) return;

            int day = TimeManager.Instance.CurrentDay;

            if (verboseLogging)
                Debug.Log($"[EventManager] Day {day} started - checking auto-trigger events...");

            _needsRefresh = true;

            // 获取所有自动触发的事件
            var autoEvents = GetAutoTriggerEvents();

            if (autoEvents.Count > 0)
            {
                // 按优先级排序
                autoEvents = autoEvents.OrderByDescending(e => e.priority).ToList();

                if (verboseLogging)
                    Debug.Log($"[EventManager] Found {autoEvents.Count} auto-trigger events.");

                // 触发优先级最高的事件
                ExecuteEvent(autoEvents[0]);
            }
        }

        /// <summary>
        /// 检查时间段事件
        /// </summary>
        public void CheckTimeslotEvents()
        {
            if (_gameState == null) return;

            _needsRefresh = true;

            if (verboseLogging)
            {
                var available = GetAvailableEvents();
                Debug.Log($"[EventManager] Timeslot changed - {available.Count} events available.");
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        public void ExecuteEvent(EventData eventData)
        {
            if (_gameState == null || eventData == null) return;

            if (verboseLogging)
                Debug.Log($"[EventManager] Executing event: {eventData.eventName} ({eventData.eventId})");

            // 检查条件
            if (!eventData.CanTrigger(_gameState))
            {
                Debug.LogWarning($"[EventManager] Event {eventData.eventId} conditions not met!");
                return;
            }

            // 触发事件
            OnEventTriggered?.Invoke(eventData.eventId, eventData);

            // 执行效果
            eventData.Execute(_gameState);

            // 触发对话
            if (eventData.triggerDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(eventData.triggerDialogue);
            }

            // 完成回调
            OnEventCompleted?.Invoke(eventData.eventId);

            _needsRefresh = true;

            if (verboseLogging)
                Debug.Log($"[EventManager] Event {eventData.eventId} completed.");
        }

        /// <summary>
        /// 通过ID执行事件
        /// </summary>
        public void ExecuteEvent(string eventId)
        {
            EventData eventData = GetEventById(eventId);

            if (eventData == null)
            {
                Debug.LogError($"[EventManager] Event not found: {eventId}");
                return;
            }

            ExecuteEvent(eventData);
        }
        #endregion

        #region Event Queries
        /// <summary>
        /// 获取所有可用的事件
        /// </summary>
        public List<EventData> GetAvailableEvents()
        {
            if (_gameState == null) return new List<EventData>();

            if (!_needsRefresh)
                return _cachedAvailableEvents;

            _cachedAvailableEvents.Clear();

            foreach (var eventData in allEvents)
            {
                if (eventData.CanTrigger(_gameState))
                {
                    _cachedAvailableEvents.Add(eventData);
                }
            }

            _needsRefresh = false;
            OnAvailableEventsChanged?.Invoke(_cachedAvailableEvents);

            return _cachedAvailableEvents;
        }

        /// <summary>
        /// 获取指定时间段的可用事件
        /// </summary>
        public List<EventData> GetAvailableEventsForTimeslot(Timeslot timeslot)
        {
            var available = GetAvailableEvents();
            return available.Where(e => e.availableTimeslots.Contains(timeslot)).ToList();
        }

        /// <summary>
        /// 获取指定类型的可用事件
        /// </summary>
        public List<EventData> GetAvailableEventsByType(EventType eventType)
        {
            var available = GetAvailableEvents();
            return available.Where(e => e.eventType == eventType).ToList();
        }

        /// <summary>
        /// 获取自动触发的事件
        /// </summary>
        public List<EventData> GetAutoTriggerEvents()
        {
            var available = GetAvailableEvents();
            return available.Where(e => e.autoTrigger).ToList();
        }

        /// <summary>
        /// 根据ID获取事件
        /// </summary>
        public EventData GetEventById(string eventId)
        {
            return allEvents.Find(e => e.eventId == eventId);
        }

        /// <summary>
        /// 检查事件是否可触发
        /// </summary>
        public bool CanTriggerEvent(string eventId)
        {
            if (_gameState == null) return false;

            EventData eventData = GetEventById(eventId);
            if (eventData == null) return false;

            return eventData.CanTrigger(_gameState);
        }

        /// <summary>
        /// 标记需要刷新可用事件列表
        /// </summary>
        public void MarkDirty()
        {
            _needsRefresh = true;
        }
        #endregion

        #region Debug
        /// <summary>
        /// 获取调试信息
        /// </summary>
        public string GetDebugInfo()
        {
            if (_gameState == null) return "EventManager: Not initialized";

            var available = GetAvailableEvents();
            var autoTrigger = GetAutoTriggerEvents();

            string info = $"=== EventManager Debug ===\n";
            info += $"Total Events: {allEvents.Count}\n";
            info += $"Available Now: {available.Count}\n";
            info += $"Auto-trigger: {autoTrigger.Count}\n";
            info += $"Completed: {_gameState.CompletedEvents.Count}\n";

            if (available.Count > 0)
            {
                info += $"\n--- Available Events ---\n";
                foreach (var evt in available.Take(5))
                {
                    info += $"• {evt.eventName} ({evt.eventType})\n";
                }
                if (available.Count > 5)
                    info += $"... and {available.Count - 5} more\n";
            }

            return info;
        }
        #endregion
    }
}
