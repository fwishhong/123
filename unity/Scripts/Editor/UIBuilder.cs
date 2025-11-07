using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace MindLink.Editor
{
    /// <summary>
    /// UI自动生成工具 - 一键创建游戏UI结构
    /// </summary>
    public class UIBuilder : EditorWindow
    {
        [MenuItem("MindLink/UI Builder")]
        public static void ShowWindow()
        {
            GetWindow<UIBuilder>("UI Builder");
        }

        private void OnGUI()
        {
            GUILayout.Label("MindLink UI自动生成工具", EditorStyles.boldLabel);
            GUILayout.Space(10);

            EditorGUILayout.HelpBox(
                "此工具将自动创建完整的游戏UI结构。\n" +
                "确保场景中没有重名的UI元素。",
                MessageType.Info
            );

            GUILayout.Space(10);

            if (GUILayout.Button("创建完整UI结构", GUILayout.Height(40)))
            {
                CreateCompleteUI();
            }

            GUILayout.Space(10);

            GUILayout.Label("单独创建组件:", EditorStyles.boldLabel);

            if (GUILayout.Button("创建主Canvas"))
            {
                CreateMainCanvas();
            }

            if (GUILayout.Button("创建顶部信息栏"))
            {
                CreateTopBar();
            }

            if (GUILayout.Button("创建EventSelectionUI"))
            {
                CreateEventSelectionUI();
            }

            if (GUILayout.Button("创建DialogueUI"))
            {
                CreateDialogueUI();
            }

            if (GUILayout.Button("创建MissionTrackerUI"))
            {
                CreateMissionTrackerUI();
            }

            if (GUILayout.Button("创建CharacterInfoUI"))
            {
                CreateCharacterInfoUI();
            }
        }

        private void CreateCompleteUI()
        {
            // 检查是否已存在MainCanvas
            GameObject existingCanvas = GameObject.Find("MainCanvas");
            if (existingCanvas != null)
            {
                if (!EditorUtility.DisplayDialog("警告",
                    "场景中已存在MainCanvas，是否删除并重新创建？",
                    "是", "否"))
                {
                    return;
                }
                DestroyImmediate(existingCanvas);
            }

            // 创建完整UI
            GameObject mainCanvas = CreateMainCanvas();
            CreateTopBar();
            CreateEventSelectionUI();
            CreateDialogueUI();
            CreateMissionTrackerUI();
            CreateCharacterInfoUI();
            CreateNotificationPanel();

            // 添加MainGameUI组件
            MindLink.UI.MainGameUI mainGameUI = mainCanvas.AddComponent<MindLink.UI.MainGameUI>();

            // 自动配置引用（需要手动完成）
            EditorUtility.DisplayDialog("成功",
                "UI结构已创建完成！\n\n" +
                "请手动配置MainGameUI组件的引用。\n" +
                "参考UI_SETUP_GUIDE.md第8节。",
                "确定");

            Debug.Log("[UIBuilder] 完整UI结构已创建");
        }

        private GameObject CreateMainCanvas()
        {
            // 创建Canvas
            GameObject canvasObj = new GameObject("MainCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            canvasObj.AddComponent<GraphicRaycaster>();

            // 创建EventSystem（如果不存在）
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystemObj = new GameObject("EventSystem");
                eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            Debug.Log("[UIBuilder] MainCanvas已创建");
            return canvasObj;
        }

        private void CreateTopBar()
        {
            GameObject mainCanvas = GameObject.Find("MainCanvas");
            if (mainCanvas == null)
            {
                EditorUtility.DisplayDialog("错误", "请先创建MainCanvas", "确定");
                return;
            }

            // 创建TopBar
            GameObject topBar = CreatePanel("TopBar", mainCanvas.transform);
            RectTransform rt = topBar.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0.5f, 1);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(0, 80);

            // 创建DateText
            GameObject dateText = CreateText("DateText", topBar.transform, "Day 1");
            SetRectTransform(dateText, new Vector2(0, 0.5f), new Vector2(0, 0.5f),
                new Vector2(100, 0), new Vector2(200, 60));

            // 创建TimeslotText
            GameObject timeslotText = CreateText("TimeslotText", topBar.transform, "上午");
            SetRectTransform(timeslotText, new Vector2(0, 0.5f), new Vector2(0, 0.5f),
                new Vector2(320, 0), new Vector2(200, 60));

            // 创建MoneyText
            GameObject moneyText = CreateText("MoneyText", topBar.transform, "¥5000");
            SetRectTransform(moneyText, new Vector2(1, 0.5f), new Vector2(1, 0.5f),
                new Vector2(-100, 0), new Vector2(200, 60));

            // 创建QuickButtons容器
            GameObject quickButtons = new GameObject("QuickButtons");
            quickButtons.transform.SetParent(topBar.transform, false);
            SetRectTransform(quickButtons, new Vector2(1, 0.5f), new Vector2(1, 0.5f),
                new Vector2(-320, 0), new Vector2(400, 60));

            HorizontalLayoutGroup layout = quickButtons.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 10;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;

            // 创建按钮
            CreateButton("MenuButton", quickButtons.transform, "菜单");
            CreateButton("CharacterButton", quickButtons.transform, "角色");
            CreateButton("MissionButton", quickButtons.transform, "任务");
            CreateButton("SaveButton", quickButtons.transform, "保存");
            CreateButton("LoadButton", quickButtons.transform, "读取");

            Debug.Log("[UIBuilder] TopBar已创建");
        }

        private void CreateEventSelectionUI()
        {
            GameObject mainCanvas = GameObject.Find("MainCanvas");
            if (mainCanvas == null)
            {
                EditorUtility.DisplayDialog("错误", "请先创建MainCanvas", "确定");
                return;
            }

            // 创建EventSelectionPanel
            GameObject panel = CreatePanel("EventSelectionPanel", mainCanvas.transform);
            SetRectTransformStretch(panel, 50, 50, 150, 50);
            panel.SetActive(false); // 默认隐藏

            // 创建FilterContainer
            GameObject filterContainer = new GameObject("FilterContainer");
            filterContainer.transform.SetParent(panel.transform, false);
            SetRectTransform(filterContainer, new Vector2(0, 1), new Vector2(1, 1),
                new Vector2(0, -40), new Vector2(0, 60));

            HorizontalLayoutGroup filterLayout = filterContainer.AddComponent<HorizontalLayoutGroup>();
            filterLayout.spacing = 10;
            filterLayout.padding = new RectOffset(20, 20, 10, 10);

            // 创建ScrollView
            GameObject scrollView = CreateScrollView("EventScrollView", panel.transform);
            SetRectTransformStretch(scrollView, 10, 400, 80, 10);

            // 创建DetailPanel
            GameObject detailPanel = CreatePanel("DetailPanel", panel.transform);
            SetRectTransform(detailPanel, new Vector2(1, 0), new Vector2(1, 1),
                new Vector2(-200, 0), new Vector2(380, 0));
            detailPanel.GetComponent<RectTransform>().offsetMin = new Vector2(detailPanel.GetComponent<RectTransform>().offsetMin.x, 10);
            detailPanel.GetComponent<RectTransform>().offsetMax = new Vector2(-10, -80);

            // 在DetailPanel中创建文本
            CreateText("DetailNameText", detailPanel.transform, "事件名称");
            CreateText("DetailDescriptionText", detailPanel.transform, "事件描述");
            CreateText("DetailEffectsText", detailPanel.transform, "效果");
            CreateText("DetailRequirementsText", detailPanel.transform, "需求");

            // 创建按钮
            CreateButton("ConfirmButton", detailPanel.transform, "确认");
            CreateButton("CancelButton", detailPanel.transform, "取消");

            // 添加EventSelectionUI组件
            panel.AddComponent<MindLink.UI.EventSelectionUI>();

            Debug.Log("[UIBuilder] EventSelectionUI已创建");
        }

        private void CreateDialogueUI()
        {
            GameObject mainCanvas = GameObject.Find("MainCanvas");
            if (mainCanvas == null)
            {
                EditorUtility.DisplayDialog("错误", "请先创建MainCanvas", "确定");
                return;
            }

            // 创建DialoguePanel
            GameObject panel = CreatePanel("DialoguePanel", mainCanvas.transform);
            SetRectTransformStretch(panel, 0, 0, 0, 0);
            panel.SetActive(false); // 默认隐藏

            // 背景设置为半透明黑色
            Image panelImage = panel.GetComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.8f);

            // 创建CharacterContainer
            GameObject charContainer = new GameObject("CharacterContainer");
            charContainer.transform.SetParent(panel.transform, false);
            SetRectTransform(charContainer, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(-400, 0), new Vector2(600, 1080));

            GameObject charPortrait = new GameObject("CharacterPortrait");
            charPortrait.transform.SetParent(charContainer.transform, false);
            charPortrait.AddComponent<Image>();
            SetRectTransformStretch(charPortrait, 0, 0, 0, 0);

            // 创建DialogueBox
            GameObject dialogueBox = CreatePanel("DialogueBox", panel.transform);
            SetRectTransform(dialogueBox, new Vector2(0, 0), new Vector2(1, 0),
                new Vector2(0, 140), new Vector2(0, 250));
            dialogueBox.GetComponent<RectTransform>().offsetMin = new Vector2(50, 30);
            dialogueBox.GetComponent<RectTransform>().offsetMax = new Vector2(-50, 30);

            // 创建CharacterNameText
            CreateText("CharacterNameText", dialogueBox.transform, "角色名");

            // 创建DialogueText
            CreateText("DialogueText", dialogueBox.transform, "对话内容...");

            // 创建ContinueIndicator
            GameObject indicator = new GameObject("ContinueIndicator");
            indicator.transform.SetParent(dialogueBox.transform, false);
            indicator.AddComponent<Image>();
            SetRectTransform(indicator, new Vector2(1, 0), new Vector2(1, 0),
                new Vector2(-30, 30), new Vector2(40, 40));

            // 创建ChoiceContainer
            GameObject choiceContainer = new GameObject("ChoiceContainer");
            choiceContainer.transform.SetParent(panel.transform, false);
            SetRectTransform(choiceContainer, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0, 100), new Vector2(800, 400));

            VerticalLayoutGroup choiceLayout = choiceContainer.AddComponent<VerticalLayoutGroup>();
            choiceLayout.spacing = 20;
            choiceLayout.childAlignment = TextAnchor.MiddleCenter;

            // 创建控制按钮
            CreateButton("SkipButton", panel.transform, "跳过");
            CreateButton("AutoButton", panel.transform, "自动");
            CreateButton("HistoryButton", panel.transform, "历史");

            // 添加DialogueUI组件
            panel.AddComponent<MindLink.UI.DialogueUI>();

            Debug.Log("[UIBuilder] DialogueUI已创建");
        }

        private void CreateMissionTrackerUI()
        {
            GameObject mainCanvas = GameObject.Find("MainCanvas");
            if (mainCanvas == null)
            {
                EditorUtility.DisplayDialog("错误", "请先创建MainCanvas", "确定");
                return;
            }

            // 创建MissionPanel
            GameObject panel = CreatePanel("MissionPanel", mainCanvas.transform);
            SetRectTransform(panel, new Vector2(1, 0), new Vector2(1, 1),
                new Vector2(-270, 0), new Vector2(500, 0));
            panel.GetComponent<RectTransform>().offsetMin = new Vector2(panel.GetComponent<RectTransform>().offsetMin.x, 20);
            panel.GetComponent<RectTransform>().offsetMax = new Vector2(-20, -120);
            panel.SetActive(false); // 默认隐藏

            // 创建标题
            CreateText("TitleText", panel.transform, "任务列表");

            // 创建ScrollView
            GameObject scrollView = CreateScrollView("MissionScrollView", panel.transform);
            SetRectTransformStretch(scrollView, 10, 10, 60, 10);

            // 添加MissionTrackerUI组件
            panel.AddComponent<MindLink.UI.MissionTrackerUI>();

            Debug.Log("[UIBuilder] MissionTrackerUI已创建");
        }

        private void CreateCharacterInfoUI()
        {
            GameObject mainCanvas = GameObject.Find("MainCanvas");
            if (mainCanvas == null)
            {
                EditorUtility.DisplayDialog("错误", "请先创建MainCanvas", "确定");
                return;
            }

            // 创建CharacterPanel
            GameObject panel = CreatePanel("CharacterPanel", mainCanvas.transform);
            SetRectTransform(panel, new Vector2(0, 0), new Vector2(0, 1),
                new Vector2(270, 0), new Vector2(500, 0));
            panel.GetComponent<RectTransform>().offsetMin = new Vector2(20, 20);
            panel.GetComponent<RectTransform>().offsetMax = new Vector2(panel.GetComponent<RectTransform>().offsetMax.x, -120);
            panel.SetActive(false); // 默认隐藏

            // 创建AttributeContainer
            GameObject attrContainer = new GameObject("AttributeContainer");
            attrContainer.transform.SetParent(panel.transform, false);
            SetRectTransform(attrContainer, new Vector2(0, 1), new Vector2(1, 1),
                new Vector2(0, -150), new Vector2(0, 300));

            VerticalLayoutGroup attrLayout = attrContainer.AddComponent<VerticalLayoutGroup>();
            attrLayout.spacing = 10;
            attrLayout.padding = new RectOffset(10, 10, 10, 10);

            // 创建4个属性显示
            string[] attributes = { "Knowledge", "Combat", "Charisma", "Courage" };
            foreach (string attr in attributes)
            {
                GameObject attrItem = new GameObject(attr);
                attrItem.transform.SetParent(attrContainer.transform, false);

                // 名称和进度条
                CreateText($"{attr}Text", attrItem.transform, $"{attr} Lv1");
                GameObject slider = new GameObject($"{attr}Slider");
                slider.transform.SetParent(attrItem.transform, false);
                slider.AddComponent<Slider>();
            }

            // 创建RelationshipScrollView
            GameObject scrollView = CreateScrollView("RelationshipScrollView", panel.transform);
            SetRectTransformStretch(scrollView, 10, 200, 320, 10);

            // 添加CharacterInfoUI组件
            panel.AddComponent<MindLink.UI.CharacterInfoUI>();

            Debug.Log("[UIBuilder] CharacterInfoUI已创建");
        }

        private void CreateNotificationPanel()
        {
            GameObject mainCanvas = GameObject.Find("MainCanvas");
            if (mainCanvas == null) return;

            // 创建NotificationPanel
            GameObject panel = CreatePanel("NotificationPanel", mainCanvas.transform);
            SetRectTransform(panel, new Vector2(0.5f, 1), new Vector2(0.5f, 1),
                new Vector2(0, -150), new Vector2(600, 100));
            panel.SetActive(false); // 默认隐藏

            // 半透明背景
            Image panelImage = panel.GetComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.7f);

            // 创建NotificationText
            GameObject text = CreateText("NotificationText", panel.transform, "通知内容");
            SetRectTransformStretch(text, 10, 10, 10, 10);

            TMP_Text tmpText = text.GetComponent<TMP_Text>();
            tmpText.alignment = TextAlignmentOptions.Center;
            tmpText.fontSize = 32;

            Debug.Log("[UIBuilder] NotificationPanel已创建");
        }

        #region Helper Methods
        private GameObject CreatePanel(string name, Transform parent)
        {
            GameObject panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            Image image = panel.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            return panel;
        }

        private GameObject CreateText(string name, Transform parent, string content)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent, false);
            TMP_Text tmpText = textObj.AddComponent<TextMeshProUGUI>();
            tmpText.text = content;
            tmpText.fontSize = 24;
            tmpText.color = Color.white;
            return textObj;
        }

        private GameObject CreateButton(string name, Transform parent, string label)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent, false);

            Image image = buttonObj.AddComponent<Image>();
            image.color = Color.white;

            Button button = buttonObj.AddComponent<Button>();

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            TMP_Text tmpText = textObj.AddComponent<TextMeshProUGUI>();
            tmpText.text = label;
            tmpText.fontSize = 20;
            tmpText.color = Color.black;
            tmpText.alignment = TextAlignmentOptions.Center;

            SetRectTransformStretch(textObj, 0, 0, 0, 0);

            return buttonObj;
        }

        private GameObject CreateScrollView(string name, Transform parent)
        {
            GameObject scrollView = new GameObject(name);
            scrollView.transform.SetParent(parent, false);

            Image image = scrollView.AddComponent<Image>();
            image.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>();

            // Viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollView.transform, false);
            viewport.AddComponent<RectMask2D>();
            SetRectTransformStretch(viewport, 0, 0, 0, 0);

            // Content
            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);
            content.AddComponent<RectTransform>();

            VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 10;
            layout.padding = new RectOffset(10, 10, 10, 10);

            ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            scrollRect.content = content.GetComponent<RectTransform>();
            scrollRect.viewport = viewport.GetComponent<RectTransform>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;

            return scrollView;
        }

        private void SetRectTransform(GameObject obj, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            RectTransform rt = obj.GetComponent<RectTransform>();
            if (rt == null)
                rt = obj.AddComponent<RectTransform>();

            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.anchoredPosition = anchoredPosition;
            rt.sizeDelta = sizeDelta;
        }

        private void SetRectTransformStretch(GameObject obj, float left, float right, float top, float bottom)
        {
            RectTransform rt = obj.GetComponent<RectTransform>();
            if (rt == null)
                rt = obj.AddComponent<RectTransform>();

            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(left, bottom);
            rt.offsetMax = new Vector2(-right, -top);
        }
        #endregion
    }
}
