using UnityEngine;
using System;

namespace MindLink
{
    /// <summary>
    /// UI管理器 - 负责UI的显示、隐藏和更新
    /// 这是基础框架，后续会扩展
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Singleton
        private static UIManager _instance;
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    _instance = go.AddComponent<UIManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Events
        public static event Action OnUIRefresh;
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

        #region UI Operations
        /// <summary>
        /// 刷新所有UI
        /// </summary>
        public void RefreshAll()
        {
            OnUIRefresh?.Invoke();
            Debug.Log("[UIManager] UI refreshed.");
        }

        /// <summary>
        /// 显示通知
        /// </summary>
        public void ShowNotification(string message, NotificationType type)
        {
            Debug.Log($"[UIManager] Notification ({type}): {message}");
            // TODO: 实际的UI通知显示
        }

        /// <summary>
        /// 加载场景UI
        /// </summary>
        public void LoadSceneUI(GameScene scene)
        {
            Debug.Log($"[UIManager] Loading UI for scene: {scene}");
            // TODO: 根据场景加载对应的UI
        }
        #endregion
    }

    /// <summary>
    /// 通知类型
    /// </summary>
    public enum NotificationType
    {
        Info,
        Success,
        Warning,
        Error
    }
}
