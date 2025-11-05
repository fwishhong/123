using UnityEngine;
using System;

namespace MindLink
{
    /// <summary>
    /// 对话管理器 - 负责对话系统的显示和控制
    /// 这是基础框架，后续会扩展为完整的对话系统
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        #region Singleton
        private static DialogueManager _instance;
        public static DialogueManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("DialogueManager");
                    _instance = go.AddComponent<DialogueManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 对话开始事件
        /// </summary>
        public static event Action OnDialogueStart;

        /// <summary>
        /// 对话结束事件
        /// </summary>
        public static event Action OnDialogueEnd;

        /// <summary>
        /// 对话选择事件 (ChoiceIndex)
        /// </summary>
        public static event Action<int> OnDialogueChoice;
        #endregion

        #region Properties
        /// <summary>
        /// 是否正在对话中
        /// </summary>
        public bool IsInDialogue { get; private set; } = false;
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

        #region Dialogue Control
        /// <summary>
        /// 开始对话
        /// </summary>
        public void StartDialogue(string dialogueId)
        {
            if (IsInDialogue)
            {
                Debug.LogWarning("[DialogueManager] Already in dialogue!");
                return;
            }

            IsInDialogue = true;
            Debug.Log($"[DialogueManager] Starting dialogue: {dialogueId}");

            OnDialogueStart?.Invoke();

            // TODO: 加载并显示对话数据
        }

        /// <summary>
        /// 结束对话
        /// </summary>
        public void EndDialogue()
        {
            if (!IsInDialogue) return;

            IsInDialogue = false;
            Debug.Log("[DialogueManager] Dialogue ended.");

            OnDialogueEnd?.Invoke();
        }

        /// <summary>
        /// 选择对话选项
        /// </summary>
        public void SelectChoice(int choiceIndex)
        {
            if (!IsInDialogue)
            {
                Debug.LogWarning("[DialogueManager] Not in dialogue!");
                return;
            }

            Debug.Log($"[DialogueManager] Choice selected: {choiceIndex}");
            OnDialogueChoice?.Invoke(choiceIndex);

            // TODO: 处理选择的后果
        }
        #endregion
    }
}
