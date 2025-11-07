using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MindLink.Data;

namespace MindLink
{
    /// <summary>
    /// 对话管理器 - 负责对话系统的显示和控制
    /// 使用ScriptableObject数据驱动的对话系统，支持分支对话树
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
        /// 对话开始事件（参数：dialogueData）
        /// </summary>
        public static event Action<DialogueData> OnDialogueStart;

        /// <summary>
        /// 对话结束事件
        /// </summary>
        public static event Action OnDialogueEnd;

        /// <summary>
        /// 对话节点显示事件（参数：node）
        /// </summary>
        public static event Action<DialogueNode> OnDialogueNodeDisplay;

        /// <summary>
        /// 对话选择事件（参数：choiceIndex, choice）
        /// </summary>
        public static event Action<int, DialogueChoice> OnDialogueChoice;

        /// <summary>
        /// 打字机效果更新（参数：currentText）
        /// </summary>
        public static event Action<string> OnTypewriterUpdate;

        /// <summary>
        /// 打字机效果完成
        /// </summary>
        public static event Action OnTypewriterComplete;
        #endregion

        #region Properties
        private GameState _gameState;

        [Header("对话设置")]
        [Tooltip("默认打字机速度（字符/秒）")]
        public float defaultTextSpeed = 30f;

        [Tooltip("是否启用打字机效果")]
        public bool enableTypewriter = true;

        [Tooltip("在控制台显示详细日志")]
        public bool verboseLogging = true;

        /// <summary>
        /// 是否正在对话中
        /// </summary>
        public bool IsInDialogue { get; private set; } = false;

        /// <summary>
        /// 是否正在播放打字机效果
        /// </summary>
        public bool IsTyping { get; private set; } = false;

        // 当前对话状态
        private DialogueData _currentDialogue;
        private DialogueNode _currentNode;
        private Coroutine _typewriterCoroutine;

        // 对话历史
        private List<string> _dialogueHistory = new List<string>();
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

            if (verboseLogging)
                Debug.Log("[DialogueManager] Initialized.");
        }
        #endregion

        #region Dialogue Control
        /// <summary>
        /// 开始对话（通过DialogueData）
        /// </summary>
        public void StartDialogue(DialogueData dialogueData)
        {
            if (dialogueData == null)
            {
                Debug.LogError("[DialogueManager] DialogueData is null!");
                return;
            }

            if (IsInDialogue)
            {
                if (verboseLogging)
                    Debug.LogWarning("[DialogueManager] Already in dialogue! Ending current dialogue first.");
                EndDialogue();
            }

            // 验证数据
            string error;
            if (!dialogueData.Validate(out error))
            {
                Debug.LogError($"[DialogueManager] Invalid dialogue data: {error}");
                return;
            }

            IsInDialogue = true;
            _currentDialogue = dialogueData;

            if (verboseLogging)
                Debug.Log($"[DialogueManager] Starting dialogue: {dialogueData.dialogueName} ({dialogueData.dialogueId})");

            OnDialogueStart?.Invoke(dialogueData);

            // 显示起始节点
            DialogueNode startNode = dialogueData.GetStartNode();
            if (startNode != null)
            {
                ShowNode(startNode);
            }
            else
            {
                Debug.LogError("[DialogueManager] No start node found!");
                EndDialogue();
            }
        }

        /// <summary>
        /// 开始对话（通过ID从Resources加载）
        /// </summary>
        public void StartDialogue(string dialogueId)
        {
            DialogueData dialogueData = Resources.Load<DialogueData>($"Dialogues/{dialogueId}");

            if (dialogueData == null)
            {
                Debug.LogError($"[DialogueManager] Dialogue not found: {dialogueId}");
                return;
            }

            StartDialogue(dialogueData);
        }

        /// <summary>
        /// 显示对话节点
        /// </summary>
        private void ShowNode(DialogueNode node)
        {
            if (node == null)
            {
                Debug.LogError("[DialogueManager] Node is null!");
                EndDialogue();
                return;
            }

            // 检查节点条件
            if (!node.CheckConditions(_gameState))
            {
                if (verboseLogging)
                    Debug.LogWarning($"[DialogueManager] Node {node.nodeId} conditions not met. Skipping.");

                // 尝试跳到下一个节点
                if (!string.IsNullOrEmpty(node.nextNodeId))
                {
                    AdvanceToNode(node.nextNodeId);
                }
                else
                {
                    EndDialogue();
                }
                return;
            }

            _currentNode = node;

            if (verboseLogging)
                Debug.Log($"[DialogueManager] Showing node: {node.nodeId}");

            // 应用节点效果
            node.effects?.Apply(_gameState);

            // 播放音效
            if (node.sfxClip != null)
            {
                AudioManager.Instance?.PlaySFX(node.sfxClip);
            }

            // 播放语音
            if (node.voiceClip != null)
            {
                AudioManager.Instance?.PlaySFX(node.voiceClip);
            }

            // 触发节点显示事件
            OnDialogueNodeDisplay?.Invoke(node);

            // 显示文本（打字机效果）
            if (enableTypewriter && _currentDialogue != null)
            {
                ShowTextWithTypewriter(node.text, _currentDialogue.textSpeed);
            }
            else
            {
                ShowTextInstant(node.text);
            }

            // 记录到历史
            if (_currentDialogue != null && _currentDialogue.saveToHistory)
            {
                string historyEntry = $"{node.characterDisplayName ?? node.characterId}: {node.text}";
                _dialogueHistory.Add(historyEntry);
            }
        }

        /// <summary>
        /// 前进到下一个节点
        /// </summary>
        public void AdvanceDialogue()
        {
            if (!IsInDialogue || _currentNode == null) return;

            // 如果正在打字，完成打字
            if (IsTyping)
            {
                CompleteTypewriter();
                return;
            }

            // 根据节点类型处理
            switch (_currentNode.nodeType)
            {
                case DialogueNodeType.Normal:
                    // 自动继续到下一个节点
                    if (!string.IsNullOrEmpty(_currentNode.nextNodeId))
                    {
                        AdvanceToNode(_currentNode.nextNodeId);
                    }
                    else
                    {
                        EndDialogue();
                    }
                    break;

                case DialogueNodeType.Choice:
                    // 等待用户选择，不自动前进
                    if (verboseLogging)
                        Debug.Log("[DialogueManager] Waiting for player choice...");
                    break;

                case DialogueNodeType.End:
                    // 结束对话
                    EndDialogue();
                    break;
            }
        }

        /// <summary>
        /// 跳转到指定节点
        /// </summary>
        private void AdvanceToNode(string nodeId)
        {
            if (_currentDialogue == null) return;

            DialogueNode nextNode = _currentDialogue.GetNodeById(nodeId);

            if (nextNode == null)
            {
                Debug.LogError($"[DialogueManager] Node not found: {nodeId}");
                EndDialogue();
                return;
            }

            ShowNode(nextNode);
        }

        /// <summary>
        /// 选择对话选项
        /// </summary>
        public void SelectChoice(int choiceIndex)
        {
            if (!IsInDialogue || _currentNode == null)
            {
                Debug.LogWarning("[DialogueManager] Not in dialogue!");
                return;
            }

            if (_currentNode.nodeType != DialogueNodeType.Choice)
            {
                Debug.LogWarning("[DialogueManager] Current node is not a choice node!");
                return;
            }

            if (choiceIndex < 0 || choiceIndex >= _currentNode.choices.Count)
            {
                Debug.LogError($"[DialogueManager] Invalid choice index: {choiceIndex}");
                return;
            }

            DialogueChoice choice = _currentNode.choices[choiceIndex];

            // 检查选择是否可用
            if (!choice.IsAvailable(_gameState))
            {
                Debug.LogWarning($"[DialogueManager] Choice {choiceIndex} is not available!");
                return;
            }

            if (verboseLogging)
                Debug.Log($"[DialogueManager] Choice selected: {choice.text}");

            OnDialogueChoice?.Invoke(choiceIndex, choice);

            // 应用选择效果
            choice.effects?.Apply(_gameState);

            // 处理选择后的流程
            if (choice.endsDialogue)
            {
                EndDialogue();
            }
            else if (!string.IsNullOrEmpty(choice.nextNodeId))
            {
                AdvanceToNode(choice.nextNodeId);
            }
            else
            {
                EndDialogue();
            }
        }

        /// <summary>
        /// 结束对话
        /// </summary>
        public void EndDialogue()
        {
            if (!IsInDialogue) return;

            if (verboseLogging)
                Debug.Log("[DialogueManager] Dialogue ended.");

            // 停止打字机
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                _typewriterCoroutine = null;
            }

            IsInDialogue = false;
            IsTyping = false;
            _currentDialogue = null;
            _currentNode = null;

            OnDialogueEnd?.Invoke();
        }
        #endregion

        #region Typewriter Effect
        /// <summary>
        /// 使用打字机效果显示文本
        /// </summary>
        private void ShowTextWithTypewriter(string text, float speed)
        {
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
            }

            _typewriterCoroutine = StartCoroutine(TypewriterCoroutine(text, speed));
        }

        /// <summary>
        /// 立即显示完整文本
        /// </summary>
        private void ShowTextInstant(string text)
        {
            OnTypewriterUpdate?.Invoke(text);
            OnTypewriterComplete?.Invoke();
        }

        /// <summary>
        /// 打字机协程
        /// </summary>
        private IEnumerator TypewriterCoroutine(string text, float speed)
        {
            IsTyping = true;

            string displayText = "";
            float delay = 1f / speed;

            for (int i = 0; i < text.Length; i++)
            {
                displayText += text[i];
                OnTypewriterUpdate?.Invoke(displayText);

                yield return new WaitForSeconds(delay);
            }

            IsTyping = false;
            OnTypewriterComplete?.Invoke();

            _typewriterCoroutine = null;
        }

        /// <summary>
        /// 完成打字机效果（立即显示全部文本）
        /// </summary>
        public void CompleteTypewriter()
        {
            if (!IsTyping || _currentNode == null) return;

            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                _typewriterCoroutine = null;
            }

            IsTyping = false;
            ShowTextInstant(_currentNode.text);
        }
        #endregion

        #region Queries
        /// <summary>
        /// 获取当前对话数据
        /// </summary>
        public DialogueData GetCurrentDialogue()
        {
            return _currentDialogue;
        }

        /// <summary>
        /// 获取当前节点
        /// </summary>
        public DialogueNode GetCurrentNode()
        {
            return _currentNode;
        }

        /// <summary>
        /// 获取对话历史
        /// </summary>
        public List<string> GetDialogueHistory()
        {
            return new List<string>(_dialogueHistory);
        }

        /// <summary>
        /// 清除对话历史
        /// </summary>
        public void ClearHistory()
        {
            _dialogueHistory.Clear();
        }
        #endregion

        #region Debug
        /// <summary>
        /// 获取调试信息
        /// </summary>
        public string GetDebugInfo()
        {
            string info = "=== DialogueManager Debug ===\n";
            info += $"In Dialogue: {IsInDialogue}\n";
            info += $"Is Typing: {IsTyping}\n";

            if (_currentDialogue != null)
            {
                info += $"Current Dialogue: {_currentDialogue.dialogueName}\n";
                info += $"Current Node: {_currentNode?.nodeId ?? "None"}\n";
                info += $"Node Type: {_currentNode?.nodeType.ToString() ?? "None"}\n";
            }

            info += $"History Entries: {_dialogueHistory.Count}\n";

            return info;
        }
        #endregion
    }
}
