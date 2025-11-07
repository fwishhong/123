using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MindLink;

/// <summary>
/// 简单的游戏UI - 显示基本游戏信息
/// </summary>
public class SimpleGameUI : MonoBehaviour
{
    [Header("UI文本组件")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI attributesText;
    public TextMeshProUGUI relationshipsText;
    public TextMeshProUGUI missionsText;
    public TextMeshProUGUI controlsText;

    private void Start()
    {
        // 订阅事件以自动刷新UI
        TimeManager.OnTimeAdvance += (day, slot) => RefreshUI();
        AttributeManager.OnAttributeChanged += RefreshUI;
        RelationshipManager.OnRelationshipChanged += RefreshUI;

        // 延迟刷新，确保游戏已启动
        Invoke(nameof(RefreshUI), 1.5f);
    }

    private void Update()
    {
        // 每秒刷新一次
        if (Time.frameCount % 60 == 0)
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        UpdateTimeDisplay();
        UpdateAttributesDisplay();
        UpdateRelationshipsDisplay();
        UpdateMissionsDisplay();
        UpdateControlsDisplay();
    }

    private void UpdateTimeDisplay()
    {
        if (timeText != null && TimeManager.Instance != null)
        {
            timeText.text = $"<b>时间</b>\n" +
                           $"Day {TimeManager.Instance.CurrentDay} / {TimeManager.Instance.MaxDay}\n" +
                           $"{TimeManager.Instance.GetCurrentTimeslotName()}\n" +
                           $"{TimeManager.Instance.GetDayOfWeekName()}";
        }
    }

    private void UpdateAttributesDisplay()
    {
        if (attributesText != null && AttributeManager.Instance != null)
        {
            attributesText.text = $"<b>属性</b>\n" +
                                 $"📚 知识: Lv{AttributeManager.Instance.GetAttributeLevel(AttributeType.Knowledge)}\n" +
                                 $"⚔️ 勇气: Lv{AttributeManager.Instance.GetAttributeLevel(AttributeType.Courage)}\n" +
                                 $"✨ 魅力: Lv{AttributeManager.Instance.GetAttributeLevel(AttributeType.Charm)}\n" +
                                 $"⚡ 战斗: Lv{AttributeManager.Instance.GetAttributeLevel(AttributeType.Combat)}";
        }
    }

    private void UpdateRelationshipsDisplay()
    {
        if (relationshipsText != null && RelationshipManager.Instance != null)
        {
            relationshipsText.text = $"<b>关系</b>\n" +
                                    $"晓: Lv{RelationshipManager.Instance.GetRelationshipLevel("akira")}\n" +
                                    $"零: Lv{RelationshipManager.Instance.GetRelationshipLevel("rei")}\n" +
                                    $"美月: Lv{RelationshipManager.Instance.GetRelationshipLevel("mizuki")}";
        }
    }

    private void UpdateMissionsDisplay()
    {
        if (missionsText != null && MissionManager.Instance != null)
        {
            int activeCount = MissionManager.Instance.GetActiveMissionCount();
            int completedCount = MissionManager.Instance.GetCompletedMissionCount();

            string text = $"<b>任务</b>\n" +
                         $"活跃: {activeCount}\n" +
                         $"完成: {completedCount}\n";

            // 显示最紧急的任务
            var urgent = MissionManager.Instance.GetMostUrgentMission();
            if (urgent != null)
            {
                int daysLeft = MissionManager.Instance.GetMissionDaysRemaining(urgent.MissionId);
                text += $"\n<color=yellow>{urgent.Name}</color>\n";
                text += $"剩余: {daysLeft} 天\n";
                text += $"进度: {urgent.GetCompletionPercentage():P0}";
            }

            missionsText.text = text;
        }
    }

    private void UpdateControlsDisplay()
    {
        if (controlsText != null)
        {
            controlsText.text = "<b>控制键</b>\n" +
                               "<color=#88FF88>空格</color> - 推进时间\n" +
                               "<color=#88FF88>K</color> - 增加知识\n" +
                               "<color=#88FF88>C</color> - 增加战斗\n" +
                               "<color=#88FF88>A</color> - 增加晓好感\n" +
                               "<color=#88FF88>S</color> - 保存游戏\n" +
                               "<color=#88FF88>L</color> - 加载游戏\n" +
                               "<color=#88FF88>I</color> - 打印状态";
        }
    }

    private void OnDestroy()
    {
        // 取消订阅
        TimeManager.OnTimeAdvance -= (day, slot) => RefreshUI();
        if (AttributeManager.Instance != null)
            AttributeManager.OnAttributeChanged -= RefreshUI;
        if (RelationshipManager.Instance != null)
            RelationshipManager.OnRelationshipChanged -= RefreshUI;
    }
}
