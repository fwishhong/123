using UnityEngine;
using System;
using System.Collections.Generic;

namespace MindLink
{
    /// <summary>
    /// 事件管理器 - 负责游戏事件的触发和管理
    /// 这是基础框架，后续会扩展为完整的事件系统
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
        public static event Action<string> OnEventTriggered;
        public static event Action<string> OnEventCompleted;
        #endregion

        #region Properties
        private GameState _gameState;
        // TODO: 事件数据库将通过ScriptableObject加载
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
        public void Initialize(GameState gameState)
        {
            _gameState = gameState;
            Debug.Log("[EventManager] Initialized.");
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
            Debug.Log($"[EventManager] Checking Day {day} events...");

            // TODO: 检查并触发当天的自动事件
        }

        /// <summary>
        /// 检查时间段事件
        /// </summary>
        public void CheckTimeslotEvents()
        {
            if (_gameState == null) return;

            // TODO: 检查当前时间段的事件
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        public void ExecuteEvent(string eventId)
        {
            if (_gameState == null) return;

            Debug.Log($"[EventManager] Executing event: {eventId}");

            // 标记为已完成
            if (!_gameState.CompletedEvents.Contains(eventId))
            {
                _gameState.CompletedEvents.Add(eventId);
            }

            OnEventTriggered?.Invoke(eventId);

            // TODO: 实际的事件执行逻辑
        }
        #endregion

        #region Event Conditions
        /// <summary>
        /// 检查事件是否可触发
        /// </summary>
        public bool CanTriggerEvent(string eventId)
        {
            if (_gameState == null) return false;

            // 检查是否已完成
            if (_gameState.CompletedEvents.Contains(eventId))
            {
                return false;
            }

            // TODO: 检查其他条件（属性、好感度、标记等）

            return true;
        }
        #endregion
    }
}
