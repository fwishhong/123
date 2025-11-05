using UnityEngine;
using System;
using System.IO;

namespace MindLink
{
    /// <summary>
    /// 存档管理器 - 负责游戏存档的保存和加载
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        #region Singleton
        private static SaveManager _instance;
        public static SaveManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("SaveManager");
                    _instance = go.AddComponent<SaveManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 保存完成事件 (SlotIndex)
        /// </summary>
        public static event Action<int> OnSaveComplete;

        /// <summary>
        /// 加载完成事件 (SlotIndex)
        /// </summary>
        public static event Action<int> OnLoadComplete;

        /// <summary>
        /// 保存失败事件
        /// </summary>
        public static event Action<string> OnSaveError;
        #endregion

        #region Properties
        /// <summary>
        /// 存档文件夹路径
        /// </summary>
        private string SavePath => Application.persistentDataPath + "/Saves/";

        /// <summary>
        /// 最大存档槽位数
        /// </summary>
        private const int MaxSaveSlots = 5;

        /// <summary>
        /// 自动存档槽位
        /// </summary>
        private const int AutoSaveSlot = 0;

        /// <summary>
        /// 快速存档槽位
        /// </summary>
        private const int QuickSaveSlot = 1;
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

            // 确保存档文件夹存在
            EnsureSaveDirectoryExists();
        }
        #endregion

        #region Save Operations
        /// <summary>
        /// 保存游戏到指定槽位
        /// </summary>
        /// <param name="gameState">游戏状态</param>
        /// <param name="slot">槽位索引 (0-4)</param>
        /// <returns>是否保存成功</returns>
        public bool SaveGame(GameState gameState, int slot)
        {
            if (gameState == null)
            {
                Debug.LogError("[SaveManager] GameState is null!");
                OnSaveError?.Invoke("游戏状态为空");
                return false;
            }

            if (slot < 0 || slot >= MaxSaveSlots)
            {
                Debug.LogError($"[SaveManager] Invalid save slot: {slot}");
                OnSaveError?.Invoke("无效的存档槽位");
                return false;
            }

            try
            {
                // 更新存档元数据
                gameState.SaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                gameState.Version = Application.version;

                // 序列化为JSON
                string json = JsonUtility.ToJson(gameState, true);

                // 获取存档文件路径
                string filePath = GetSaveFilePath(slot);

                // 写入文件
                File.WriteAllText(filePath, json);

                Debug.Log($"[SaveManager] Game saved to slot {slot}: {filePath}");
                OnSaveComplete?.Invoke(slot);

                // 显示通知
                UIManager.Instance?.ShowNotification(
                    $"游戏已保存到槽位 {slot + 1}",
                    NotificationType.Success
                );

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to save game: {e.Message}");
                OnSaveError?.Invoke($"保存失败: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 自动保存
        /// </summary>
        public bool AutoSave(GameState gameState)
        {
            Debug.Log("[SaveManager] Auto-saving...");
            return SaveGame(gameState, AutoSaveSlot);
        }

        /// <summary>
        /// 快速保存
        /// </summary>
        public bool QuickSave(GameState gameState)
        {
            Debug.Log("[SaveManager] Quick-saving...");
            return SaveGame(gameState, QuickSaveSlot);
        }
        #endregion

        #region Load Operations
        /// <summary>
        /// 从指定槽位加载游戏
        /// </summary>
        /// <param name="slot">槽位索引</param>
        /// <returns>加载的游戏状态，失败返回null</returns>
        public GameState LoadGame(int slot)
        {
            if (slot < 0 || slot >= MaxSaveSlots)
            {
                Debug.LogError($"[SaveManager] Invalid load slot: {slot}");
                return null;
            }

            string filePath = GetSaveFilePath(slot);

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[SaveManager] Save file not found: {filePath}");
                return null;
            }

            try
            {
                // 读取文件
                string json = File.ReadAllText(filePath);

                // 反序列化
                GameState gameState = JsonUtility.FromJson<GameState>(json);

                if (gameState == null)
                {
                    Debug.LogError("[SaveManager] Failed to deserialize save data!");
                    return null;
                }

                Debug.Log($"[SaveManager] Game loaded from slot {slot}");
                OnLoadComplete?.Invoke(slot);

                return gameState;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to load game: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 加载自动存档
        /// </summary>
        public GameState LoadAutoSave()
        {
            return LoadGame(AutoSaveSlot);
        }

        /// <summary>
        /// 加载快速存档
        /// </summary>
        public GameState LoadQuickSave()
        {
            return LoadGame(QuickSaveSlot);
        }
        #endregion

        #region Save Management
        /// <summary>
        /// 删除指定槽位的存档
        /// </summary>
        public bool DeleteSave(int slot)
        {
            if (slot < 0 || slot >= MaxSaveSlots)
            {
                Debug.LogError($"[SaveManager] Invalid slot: {slot}");
                return false;
            }

            string filePath = GetSaveFilePath(slot);

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[SaveManager] Save file not found: {filePath}");
                return false;
            }

            try
            {
                File.Delete(filePath);
                Debug.Log($"[SaveManager] Deleted save in slot {slot}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to delete save: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 检查槽位是否有存档
        /// </summary>
        public bool HasSave(int slot)
        {
            if (slot < 0 || slot >= MaxSaveSlots) return false;

            string filePath = GetSaveFilePath(slot);
            return File.Exists(filePath);
        }

        /// <summary>
        /// 获取存档信息（不加载完整数据）
        /// </summary>
        public SaveInfo GetSaveInfo(int slot)
        {
            if (!HasSave(slot)) return null;

            try
            {
                string filePath = GetSaveFilePath(slot);
                FileInfo fileInfo = new FileInfo(filePath);

                // 读取基本信息
                string json = File.ReadAllText(filePath);
                GameState gameState = JsonUtility.FromJson<GameState>(json);

                return new SaveInfo
                {
                    SlotIndex = slot,
                    SaveTime = gameState.SaveTime,
                    CurrentDay = gameState.CurrentDay,
                    TotalPlayTime = gameState.TotalPlayTime,
                    FileSize = fileInfo.Length
                };
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to get save info: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取所有存档信息
        /// </summary>
        public SaveInfo[] GetAllSaveInfo()
        {
            SaveInfo[] saves = new SaveInfo[MaxSaveSlots];

            for (int i = 0; i < MaxSaveSlots; i++)
            {
                saves[i] = GetSaveInfo(i);
            }

            return saves;
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// 确保存档文件夹存在
        /// </summary>
        private void EnsureSaveDirectoryExists()
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
                Debug.Log($"[SaveManager] Created save directory: {SavePath}");
            }
        }

        /// <summary>
        /// 获取存档文件路径
        /// </summary>
        private string GetSaveFilePath(int slot)
        {
            return SavePath + $"save_{slot}.json";
        }

        /// <summary>
        /// 获取存档槽位名称
        /// </summary>
        public string GetSlotName(int slot)
        {
            if (slot == AutoSaveSlot) return "自动存档";
            if (slot == QuickSaveSlot) return "快速存档";
            return $"存档 {slot}";
        }
        #endregion

        #region Debug
        /// <summary>
        /// 打开存档文件夹
        /// </summary>
        public void OpenSaveFolder()
        {
            Application.OpenURL("file://" + SavePath);
        }

        /// <summary>
        /// 清空所有存档（调试用）
        /// </summary>
        public void DeleteAllSaves()
        {
            for (int i = 0; i < MaxSaveSlots; i++)
            {
                DeleteSave(i);
            }
            Debug.Log("[SaveManager] All saves deleted!");
        }

        /// <summary>
        /// 获取存档调试信息
        /// </summary>
        public string GetDebugInfo()
        {
            string info = $"=== Save Manager ===\n";
            info += $"Save Path: {SavePath}\n\n";

            for (int i = 0; i < MaxSaveSlots; i++)
            {
                if (HasSave(i))
                {
                    SaveInfo saveInfo = GetSaveInfo(i);
                    info += $"[Slot {i}] {GetSlotName(i)}\n";
                    info += $"  Time: {saveInfo.SaveTime}\n";
                    info += $"  Day: {saveInfo.CurrentDay}\n";
                    info += $"  Play Time: {FormatPlayTime(saveInfo.TotalPlayTime)}\n";
                    info += $"  Size: {FormatFileSize(saveInfo.FileSize)}\n\n";
                }
                else
                {
                    info += $"[Slot {i}] {GetSlotName(i)}: Empty\n\n";
                }
            }

            return info;
        }

        /// <summary>
        /// 格式化游戏时长
        /// </summary>
        private string FormatPlayTime(float seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        private string FormatFileSize(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{bytes / 1024} KB";
            return $"{bytes / (1024 * 1024)} MB";
        }
        #endregion
    }

    /// <summary>
    /// 存档信息（用于显示存档列表）
    /// </summary>
    [Serializable]
    public class SaveInfo
    {
        public int SlotIndex;
        public string SaveTime;
        public int CurrentDay;
        public float TotalPlayTime;
        public long FileSize;
    }
}
