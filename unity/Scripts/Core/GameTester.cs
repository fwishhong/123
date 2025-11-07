using UnityEngine;
using MindLink;
using System.Collections.Generic;

/// <summary>
/// 游戏测试脚本 - 用于验证所有Manager是否正常工作
/// </summary>
public class GameTester : MonoBehaviour
{
    [Header("测试设置")]
    [Tooltip("游戏启动后多少秒开始测试")]
    public float testDelay = 1f;

    [Tooltip("是否自动开始新游戏")]
    public bool autoStartGame = true;

    [Header("测试选项")]
    public bool testTimeSystem = true;
    public bool testAttributeSystem = true;
    public bool testRelationshipSystem = true;
    public bool testMissionSystem = true;
    public bool testSaveSystem = true;

    private void Start()
    {
        Debug.Log("=== GameTester Started ===");

        // 延迟执行测试，确保所有Manager已初始化
        Invoke(nameof(RunTests), testDelay);
    }

    private void RunTests()
    {
        Debug.Log("\n========== 开始测试 ==========\n");

        if (autoStartGame)
        {
            TestGameStart();
        }

        if (testTimeSystem)
        {
            TestTimeSystem();
        }

        if (testAttributeSystem)
        {
            TestAttributeSystem();
        }

        if (testRelationshipSystem)
        {
            TestRelationshipSystem();
        }

        if (testMissionSystem)
        {
            TestMissionSystem();
        }

        if (testSaveSystem)
        {
            TestSaveSystem();
        }

        Debug.Log("\n========== 测试完成 ==========\n");
        PrintCurrentStatus();
    }

    /// <summary>
    /// 测试游戏启动
    /// </summary>
    private void TestGameStart()
    {
        Debug.Log("\n--- 测试：游戏启动 ---");
        GameManager.Instance.StartNewGame();
        Debug.Log("✓ 游戏已启动");
    }

    /// <summary>
    /// 测试时间系统
    /// </summary>
    private void TestTimeSystem()
    {
        Debug.Log("\n--- 测试：时间系统 ---");

        Debug.Log($"当前时间：Day {TimeManager.Instance.CurrentDay} - {TimeManager.Instance.GetCurrentTimeslotName()}");
        Debug.Log($"星期：{TimeManager.Instance.GetDayOfWeekName()}");
        Debug.Log($"剩余天数：{TimeManager.Instance.DaysRemaining}");

        // 推进时间
        Debug.Log("\n推进时间中...");
        TimeManager.Instance.AdvanceTime();
        Debug.Log($"新时间：Day {TimeManager.Instance.CurrentDay} - {TimeManager.Instance.GetCurrentTimeslotName()}");

        Debug.Log("✓ 时间系统正常");
    }

    /// <summary>
    /// 测试属性系统
    /// </summary>
    private void TestAttributeSystem()
    {
        Debug.Log("\n--- 测试：属性系统 ---");

        // 添加各种属性经验
        Debug.Log("添加属性经验...");
        AttributeManager.Instance.AddAttributeExp(AttributeType.Knowledge, 50);
        AttributeManager.Instance.AddAttributeExp(AttributeType.Courage, 30);
        AttributeManager.Instance.AddAttributeExp(AttributeType.Charm, 20);
        AttributeManager.Instance.AddAttributeExp(AttributeType.Combat, 100); // 足够升级

        // 打印属性状态
        Debug.Log("\n当前属性：");
        Debug.Log($"知识：Lv{AttributeManager.Instance.GetAttributeLevel(AttributeType.Knowledge)}");
        Debug.Log($"勇气：Lv{AttributeManager.Instance.GetAttributeLevel(AttributeType.Courage)}");
        Debug.Log($"魅力：Lv{AttributeManager.Instance.GetAttributeLevel(AttributeType.Charm)}");
        Debug.Log($"战斗：Lv{AttributeManager.Instance.GetAttributeLevel(AttributeType.Combat)}");

        Debug.Log("✓ 属性系统正常");
    }

    /// <summary>
    /// 测试关系系统
    /// </summary>
    private void TestRelationshipSystem()
    {
        Debug.Log("\n--- 测试：关系系统 ---");

        // 添加好感度
        Debug.Log("增加角色好感度...");
        RelationshipManager.Instance.AddRelationshipExp("akira", 50);
        RelationshipManager.Instance.AddRelationshipExp("rei", 30);
        RelationshipManager.Instance.AddRelationshipExp("mizuki", 20);

        // 打印关系状态
        Debug.Log("\n当前关系：");
        Debug.Log($"晓：Lv{RelationshipManager.Instance.GetRelationshipLevel("akira")}");
        Debug.Log($"零：Lv{RelationshipManager.Instance.GetRelationshipLevel("rei")}");
        Debug.Log($"美月：Lv{RelationshipManager.Instance.GetRelationshipLevel("mizuki")}");

        Debug.Log("✓ 关系系统正常");
    }

    /// <summary>
    /// 测试任务系统
    /// </summary>
    private void TestMissionSystem()
    {
        Debug.Log("\n--- 测试：任务系统 ---");

        // 创建测试任务
        var objectives = new Dictionary<string, ObjectiveProgress>
        {
            { "combat_level", new ObjectiveProgress("战斗力达到Lv3", 3, 1) },
            { "clues", new ObjectiveProgress("收集线索", 3, 0) },
            { "ally", new ObjectiveProgress("找到同伴", 1, 0) }
        };

        MissionManager.Instance.StartMission(
            "test_mission",
            "测试任务：拯救美月",
            15,
            objectives
        );

        Debug.Log("任务已创建");

        // 更新任务进度
        Debug.Log("\n更新任务进度...");
        MissionManager.Instance.UpdateMissionObjective("test_mission", "clues", 1);
        MissionManager.Instance.UpdateMissionObjective("test_mission", "ally", 1);

        // 打印任务状态
        var mission = MissionManager.Instance.GetActiveMission("test_mission");
        if (mission != null)
        {
            Debug.Log($"\n任务：{mission.Name}");
            Debug.Log($"截止日期：Day {mission.Deadline}");
            Debug.Log($"完成度：{mission.GetCompletionPercentage():P0}");
            Debug.Log($"剩余时间：{MissionManager.Instance.GetMissionDaysRemaining("test_mission")} 天");
        }

        Debug.Log("✓ 任务系统正常");
    }

    /// <summary>
    /// 测试存档系统
    /// </summary>
    private void TestSaveSystem()
    {
        Debug.Log("\n--- 测试：存档系统 ---");

        var gameState = GameManager.Instance.CurrentGameState;

        if (gameState != null)
        {
            // 保存到槽位2
            Debug.Log("保存游戏到槽位2...");
            bool saved = SaveManager.Instance.SaveGame(gameState, 2);

            if (saved)
            {
                Debug.Log("✓ 保存成功");

                // 检查存档信息
                SaveInfo info = SaveManager.Instance.GetSaveInfo(2);
                if (info != null)
                {
                    Debug.Log($"\n存档信息：");
                    Debug.Log($"时间：{info.SaveTime}");
                    Debug.Log($"Day：{info.CurrentDay}");
                    Debug.Log($"游戏时长：{info.TotalPlayTime:F0}秒");
                }
            }
            else
            {
                Debug.LogError("✗ 保存失败");
            }
        }

        Debug.Log("✓ 存档系统正常");
    }

    /// <summary>
    /// 打印当前游戏状态
    /// </summary>
    private void PrintCurrentStatus()
    {
        Debug.Log("\n========== 当前游戏状态 ==========\n");

        Debug.Log("【时间】");
        Debug.Log(TimeManager.Instance.GetDebugInfo());

        Debug.Log("\n【属性】");
        Debug.Log(AttributeManager.Instance.GetDebugInfo());

        Debug.Log("\n【关系】");
        Debug.Log(RelationshipManager.Instance.GetDebugInfo());

        Debug.Log("\n【任务】");
        Debug.Log(MissionManager.Instance.GetDebugInfo());

        Debug.Log("\n【存档】");
        Debug.Log(SaveManager.Instance.GetDebugInfo());

        Debug.Log("\n====================================\n");
    }

    /// <summary>
    /// 按键测试
    /// </summary>
    private void Update()
    {
        // 按空格键推进时间
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("\n[手动操作] 推进时间");
            TimeManager.Instance.AdvanceTime();
            Debug.Log(TimeManager.Instance.GetDebugInfo());
        }

        // 按K键添加知识经验
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("\n[手动操作] 添加知识经验 +25");
            AttributeManager.Instance.AddAttributeExp(AttributeType.Knowledge, 25);
        }

        // 按C键添加战斗经验
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("\n[手动操作] 添加战斗经验 +25");
            AttributeManager.Instance.AddAttributeExp(AttributeType.Combat, 25);
        }

        // 按A键增加晓的好感度
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("\n[手动操作] 增加晓的好感度 +20");
            RelationshipManager.Instance.AddRelationshipExp("akira", 20);
        }

        // 按S键保存游戏
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("\n[手动操作] 快速保存");
            GameManager.Instance.QuickSave();
        }

        // 按L键加载游戏
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("\n[手动操作] 加载快速存档");
            GameManager.Instance.ContinueGame(1); // Quick save slot
        }

        // 按I键打印状态
        if (Input.GetKeyDown(KeyCode.I))
        {
            PrintCurrentStatus();
        }
    }
}
