using UnityEngine;
using System;

namespace MindLink
{
    /// <summary>
    /// 游戏总管理器 - 单例模式
    /// 负责游戏整体流程控制和各Manager的协调
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        _instance = go.AddComponent<GameManager>();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 游戏开始事件
        /// </summary>
        public static event Action OnGameStart;

        /// <summary>
        /// 游戏暂停事件
        /// </summary>
        public static event Action<bool> OnGamePause;

        /// <summary>
        /// 游戏结束事件
        /// </summary>
        public static event Action OnGameOver;

        /// <summary>
        /// 场景切换事件
        /// </summary>
        public static event Action<GameScene> OnSceneChange;
        #endregion

        #region Properties
        /// <summary>
        /// 当前游戏场景
        /// </summary>
        public GameScene CurrentScene { get; private set; } = GameScene.MainMenu;

        /// <summary>
        /// 游戏是否暂停
        /// </summary>
        public bool IsPaused { get; private set; } = false;

        /// <summary>
        /// 游戏是否正在运行
        /// </summary>
        public bool IsGameRunning { get; private set; } = false;

        /// <summary>
        /// 当前游戏状态
        /// </summary>
        public GameState CurrentGameState { get; private set; }
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            // 单例检查
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeManagers();
        }

        private void Start()
        {
            // 初始化完成后，显示主菜单
            ChangeScene(GameScene.MainMenu);
        }

        private void Update()
        {
            if (!IsGameRunning || IsPaused) return;

            // 游戏主循环更新
            // 各Manager会在自己的Update中处理逻辑
        }
        #endregion

        #region Initialization
        /// <summary>
        /// 初始化所有管理器
        /// </summary>
        private void InitializeManagers()
        {
            Debug.Log("[GameManager] Initializing all managers...");

            // 确保所有Manager都已创建
            _ = TimeManager.Instance;
            _ = EventManager.Instance;
            _ = AttributeManager.Instance;
            _ = RelationshipManager.Instance;
            _ = MissionManager.Instance;
            _ = DialogueManager.Instance;
            _ = UIManager.Instance;
            _ = AudioManager.Instance;
            _ = SaveManager.Instance;

            Debug.Log("[GameManager] All managers initialized.");
        }
        #endregion

        #region Game Control
        /// <summary>
        /// 开始新游戏
        /// </summary>
        public void StartNewGame()
        {
            Debug.Log("[GameManager] Starting new game...");

            // 创建新的游戏状态
            CurrentGameState = new GameState();

            // 初始化各系统
            TimeManager.Instance.Initialize(CurrentGameState);
            AttributeManager.Instance.Initialize(CurrentGameState);
            RelationshipManager.Instance.Initialize(CurrentGameState);
            MissionManager.Instance.Initialize(CurrentGameState);
            EventManager.Instance.Initialize(CurrentGameState);

            // 切换到游戏场景
            ChangeScene(GameScene.MainGame);

            IsGameRunning = true;
            OnGameStart?.Invoke();

            // 触发Day 1的开场事件
            EventManager.Instance.TriggerDayStart();
        }

        /// <summary>
        /// 继续游戏（从存档加载）
        /// </summary>
        /// <param name="saveSlot">存档槽位</param>
        public void ContinueGame(int saveSlot)
        {
            Debug.Log($"[GameManager] Loading game from slot {saveSlot}...");

            GameState loadedState = SaveManager.Instance.LoadGame(saveSlot);

            if (loadedState == null)
            {
                Debug.LogError("[GameManager] Failed to load game!");
                return;
            }

            CurrentGameState = loadedState;

            // 用加载的数据初始化各系统
            TimeManager.Instance.Initialize(CurrentGameState);
            AttributeManager.Instance.Initialize(CurrentGameState);
            RelationshipManager.Instance.Initialize(CurrentGameState);
            MissionManager.Instance.Initialize(CurrentGameState);
            EventManager.Instance.Initialize(CurrentGameState);

            ChangeScene(GameScene.MainGame);

            IsGameRunning = true;
            OnGameStart?.Invoke();

            // 刷新UI
            UIManager.Instance.RefreshAll();
        }

        /// <summary>
        /// 暂停游戏
        /// </summary>
        public void PauseGame()
        {
            if (!IsGameRunning) return;

            IsPaused = true;
            Time.timeScale = 0f;
            OnGamePause?.Invoke(true);

            Debug.Log("[GameManager] Game paused.");
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        public void ResumeGame()
        {
            if (!IsGameRunning) return;

            IsPaused = false;
            Time.timeScale = 1f;
            OnGamePause?.Invoke(false);

            Debug.Log("[GameManager] Game resumed.");
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        public void GameOver()
        {
            Debug.Log("[GameManager] Game over.");

            IsGameRunning = false;
            OnGameOver?.Invoke();

            // 可以在这里显示结局画面
        }

        /// <summary>
        /// 返回主菜单
        /// </summary>
        public void ReturnToMainMenu()
        {
            Debug.Log("[GameManager] Returning to main menu...");

            IsGameRunning = false;
            IsPaused = false;
            Time.timeScale = 1f;

            // 清理游戏状态
            CurrentGameState = null;

            ChangeScene(GameScene.MainMenu);
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("[GameManager] Quitting game...");

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
        #endregion

        #region Scene Management
        /// <summary>
        /// 切换场景
        /// </summary>
        public void ChangeScene(GameScene newScene)
        {
            Debug.Log($"[GameManager] Changing scene from {CurrentScene} to {newScene}");

            CurrentScene = newScene;
            OnSceneChange?.Invoke(newScene);

            // 根据场景类型加载对应UI
            UIManager.Instance.LoadSceneUI(newScene);
        }
        #endregion

        #region Save/Load
        /// <summary>
        /// 快速保存
        /// </summary>
        public void QuickSave()
        {
            if (CurrentGameState == null || !IsGameRunning) return;

            SaveManager.Instance.SaveGame(CurrentGameState, 0); // Slot 0 for quick save
            Debug.Log("[GameManager] Quick save completed.");
        }

        /// <summary>
        /// 保存到指定槽位
        /// </summary>
        public void SaveToSlot(int slot)
        {
            if (CurrentGameState == null || !IsGameRunning) return;

            SaveManager.Instance.SaveGame(CurrentGameState, slot);
            Debug.Log($"[GameManager] Saved to slot {slot}.");
        }
        #endregion
    }

    /// <summary>
    /// 游戏场景枚举
    /// </summary>
    public enum GameScene
    {
        MainMenu,       // 主菜单
        MainGame,       // 主游戏
        Battle,         // 战斗
        CognitiveSpace, // 认知空间
        Puzzle,         // 解谜
        Ending          // 结局
    }
}
