using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using MindLink.Data;

namespace MindLink.UI
{
    /// <summary>
    /// 对话UI - 显示对话内容和处理用户交互
    /// 支持打字机效果、分支选择、角色立绘
    /// </summary>
    public class DialogueUI : MonoBehaviour
    {
        [Header("主面板")]
        [Tooltip("对话框主面板")]
        public GameObject dialoguePanel;

        [Tooltip("背景图片（可选）")]
        public Image backgroundImage;

        [Header("角色显示")]
        [Tooltip("角色立绘容器")]
        public Transform characterContainer;

        [Tooltip("角色立绘Image")]
        public Image characterPortrait;

        [Tooltip("角色名字文本")]
        public TextMeshProUGUI characterNameText;

        [Header("对话文本")]
        [Tooltip("对话内容文本")]
        public TextMeshProUGUI dialogueText;

        [Tooltip("继续提示图标")]
        public GameObject continueIndicator;

        [Header("选择按钮")]
        [Tooltip("选择按钮容器")]
        public Transform choiceContainer;

        [Tooltip("选择按钮预制体")]
        public GameObject choiceButtonPrefab;

        [Header("控制按钮")]
        [Tooltip("跳过按钮")]
        public Button skipButton;

        [Tooltip("自动播放按钮")]
        public Button autoButton;

        [Tooltip("历史记录按钮")]
        public Button historyButton;

        [Header("设置")]
        [Tooltip("是否允许点击跳过打字机")]
        public bool allowSkipTypewriter = true;

        [Tooltip("是否允许点击继续对话")]
        public bool allowClickToContinue = true;

        [Tooltip("自动播放延迟（秒）")]
        public float autoPlayDelay = 2f;

        // 状态
        private bool _isAutoPlaying = false;
        private float _autoPlayTimer = 0f;
        private List<Button> _currentChoiceButtons = new List<Button>();

        #region Unity Lifecycle
        private void Start()
        {
            InitializeUI();
            SubscribeToEvents();
        }

        private void Update()
        {
            // 自动播放逻辑
            if (_isAutoPlaying && !DialogueManager.Instance.IsTyping &&
                DialogueManager.Instance.IsInDialogue)
            {
                _autoPlayTimer += Time.deltaTime;
                if (_autoPlayTimer >= autoPlayDelay)
                {
                    _autoPlayTimer = 0f;
                    DialogueManager.Instance.AdvanceDialogue();
                }
            }

            // 点击继续
            if (allowClickToContinue && Input.GetMouseButtonDown(0))
            {
                if (DialogueManager.Instance.IsInDialogue)
                {
                    if (DialogueManager.Instance.IsTyping && allowSkipTypewriter)
                    {
                        // 跳过打字机
                        DialogueManager.Instance.CompleteTypewriter();
                    }
                    else if (!DialogueManager.Instance.IsTyping)
                    {
                        // 继续对话（仅当不是选择节点时）
                        var currentNode = DialogueManager.Instance.GetCurrentNode();
                        if (currentNode != null && currentNode.nodeType != DialogueNodeType.Choice)
                        {
                            DialogueManager.Instance.AdvanceDialogue();
                        }
                    }
                }
            }

            // 更新继续提示
            if (continueIndicator != null)
            {
                bool shouldShow = DialogueManager.Instance.IsInDialogue &&
                                  !DialogueManager.Instance.IsTyping &&
                                  DialogueManager.Instance.GetCurrentNode()?.nodeType == DialogueNodeType.Normal;
                continueIndicator.SetActive(shouldShow);
            }
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        #endregion

        #region Initialization
        private void InitializeUI()
        {
            // 隐藏对话框
            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);

            // 隐藏继续提示
            if (continueIndicator != null)
                continueIndicator.SetActive(false);

            // 设置按钮监听
            if (skipButton != null)
                skipButton.onClick.AddListener(OnSkipClicked);

            if (autoButton != null)
                autoButton.onClick.AddListener(OnAutoClicked);

            if (historyButton != null)
                historyButton.onClick.AddListener(OnHistoryClicked);
        }
        #endregion

        #region Event Subscription
        private void SubscribeToEvents()
        {
            DialogueManager.OnDialogueStart += OnDialogueStart;
            DialogueManager.OnDialogueEnd += OnDialogueEnd;
            DialogueManager.OnDialogueNodeDisplay += OnDialogueNodeDisplay;
            DialogueManager.OnTypewriterUpdate += OnTypewriterUpdate;
            DialogueManager.OnTypewriterComplete += OnTypewriterComplete;
        }

        private void UnsubscribeFromEvents()
        {
            if (DialogueManager.Instance != null)
            {
                DialogueManager.OnDialogueStart -= OnDialogueStart;
                DialogueManager.OnDialogueEnd -= OnDialogueEnd;
                DialogueManager.OnDialogueNodeDisplay -= OnDialogueNodeDisplay;
                DialogueManager.OnTypewriterUpdate -= OnTypewriterUpdate;
                DialogueManager.OnTypewriterComplete -= OnTypewriterComplete;
            }
        }

        private void OnDialogueStart(DialogueData dialogueData)
        {
            // 显示对话框
            if (dialoguePanel != null)
                dialoguePanel.SetActive(true);

            // 重置自动播放
            _isAutoPlaying = false;
            _autoPlayTimer = 0f;

            Debug.Log($"[DialogueUI] Dialogue started: {dialogueData.dialogueName}");
        }

        private void OnDialogueEnd()
        {
            // 隐藏对话框
            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);

            // 清空选择按钮
            ClearChoices();

            // 重置自动播放
            _isAutoPlaying = false;
            _autoPlayTimer = 0f;

            Debug.Log("[DialogueUI] Dialogue ended");
        }

        private void OnDialogueNodeDisplay(DialogueNode node)
        {
            // 重置自动播放计时器
            _autoPlayTimer = 0f;

            // 更新角色名字
            if (characterNameText != null)
            {
                string displayName = node.characterDisplayName ?? node.characterId;
                characterNameText.text = string.IsNullOrEmpty(displayName) ? "旁白" : displayName;
            }

            // 更新角色立绘
            UpdateCharacterPortrait(node);

            // 清空文本（等待打字机效果）
            if (dialogueText != null)
                dialogueText.text = "";

            // 处理选择
            if (node.nodeType == DialogueNodeType.Choice)
            {
                // 等待打字机完成后再显示选择
                // 在OnTypewriterComplete中处理
            }
            else
            {
                // 清空选择按钮
                ClearChoices();
            }
        }

        private void OnTypewriterUpdate(string currentText)
        {
            // 更新文本显示
            if (dialogueText != null)
                dialogueText.text = currentText;
        }

        private void OnTypewriterComplete()
        {
            // 如果是选择节点，显示选择按钮
            var currentNode = DialogueManager.Instance.GetCurrentNode();
            if (currentNode != null && currentNode.nodeType == DialogueNodeType.Choice)
            {
                DisplayChoices(currentNode.choices);
            }
        }
        #endregion

        #region Character Portrait
        private void UpdateCharacterPortrait(DialogueNode node)
        {
            if (characterPortrait == null) return;

            // 如果没有指定角色，隐藏立绘
            if (string.IsNullOrEmpty(node.characterId))
            {
                if (characterContainer != null)
                    characterContainer.gameObject.SetActive(false);
                return;
            }

            // 显示立绘容器
            if (characterContainer != null)
                characterContainer.gameObject.SetActive(true);

            // 尝试从Resources加载角色数据
            CharacterData character = Resources.Load<CharacterData>($"Characters/{node.characterId}");
            if (character != null)
            {
                Sprite portrait = character.GetPortrait(node.emotion);
                if (portrait != null)
                {
                    characterPortrait.sprite = portrait;
                    characterPortrait.enabled = true;
                }
                else
                {
                    characterPortrait.enabled = false;
                }
            }
            else
            {
                // 如果找不到角色数据，隐藏立绘
                characterPortrait.enabled = false;
            }
        }
        #endregion

        #region Choice Display
        private void DisplayChoices(List<DialogueChoice> choices)
        {
            if (choiceContainer == null || choiceButtonPrefab == null)
                return;

            // 清空现有按钮
            ClearChoices();

            // 创建选择按钮
            GameState gameState = GameManager.Instance.CurrentGameState;

            for (int i = 0; i < choices.Count; i++)
            {
                DialogueChoice choice = choices[i];
                bool isAvailable = choice.IsAvailable(gameState);

                GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);
                Button button = buttonObj.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null)
                {
                    if (isAvailable)
                    {
                        buttonText.text = choice.text;
                    }
                    else
                    {
                        buttonText.text = $"{choice.text} [已锁定]";
                        buttonText.color = Color.gray;
                    }
                }

                if (button != null)
                {
                    if (isAvailable)
                    {
                        int choiceIndex = i;
                        button.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
                        _currentChoiceButtons.Add(button);
                    }
                    else
                    {
                        button.interactable = false;

                        // 显示锁定提示
                        button.onClick.AddListener(() => {
                            Debug.Log($"[DialogueUI] Choice locked: {choice.lockedHint}");
                        });
                    }
                }
            }
        }

        private void ClearChoices()
        {
            if (choiceContainer == null) return;

            foreach (Transform child in choiceContainer)
            {
                Destroy(child.gameObject);
            }

            _currentChoiceButtons.Clear();
        }

        private void OnChoiceSelected(int choiceIndex)
        {
            Debug.Log($"[DialogueUI] Choice selected: {choiceIndex}");

            // 清空选择按钮
            ClearChoices();

            // 通知DialogueManager
            DialogueManager.Instance.SelectChoice(choiceIndex);
        }
        #endregion

        #region Button Callbacks
        private void OnSkipClicked()
        {
            if (DialogueManager.Instance.IsTyping)
            {
                DialogueManager.Instance.CompleteTypewriter();
            }
            else
            {
                // 跳过整个对话（可选功能）
                Debug.Log("[DialogueUI] Skip dialogue (not implemented)");
            }
        }

        private void OnAutoClicked()
        {
            _isAutoPlaying = !_isAutoPlaying;
            _autoPlayTimer = 0f;

            // 更新按钮显示
            if (autoButton != null)
            {
                ColorBlock colors = autoButton.colors;
                colors.normalColor = _isAutoPlaying ? Color.green : Color.white;
                autoButton.colors = colors;
            }

            Debug.Log($"[DialogueUI] Auto play: {_isAutoPlaying}");
        }

        private void OnHistoryClicked()
        {
            // 显示对话历史（需要单独的HistoryUI）
            var history = DialogueManager.Instance.GetDialogueHistory();
            Debug.Log($"[DialogueUI] Show history ({history.Count} entries)");

            // TODO: 实现历史UI
            foreach (var entry in history)
            {
                Debug.Log(entry);
            }
        }
        #endregion

        #region Public API
        /// <summary>
        /// 手动显示对话框
        /// </summary>
        public void Show()
        {
            if (dialoguePanel != null)
                dialoguePanel.SetActive(true);
        }

        /// <summary>
        /// 手动隐藏对话框
        /// </summary>
        public void Hide()
        {
            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);
        }

        /// <summary>
        /// 设置是否允许点击继续
        /// </summary>
        public void SetAllowClickToContinue(bool allow)
        {
            allowClickToContinue = allow;
        }

        /// <summary>
        /// 设置自动播放状态
        /// </summary>
        public void SetAutoPlay(bool auto)
        {
            _isAutoPlaying = auto;
            _autoPlayTimer = 0f;
        }
        #endregion
    }
}
