# MindLink - Unity Scripts

这个文件夹包含所有可以直接导入Unity项目的C#脚本。

## 📁 文件夹结构

```
unity/
├── Scripts/
│   ├── Core/                  # 核心类
│   ├── Managers/              # 管理器系统 ✅
│   │   ├── GameManager.cs     # 游戏总控
│   │   ├── TimeManager.cs     # 时间管理
│   │   ├── AttributeManager.cs # 属性管理
│   │   ├── RelationshipManager.cs # 关系管理
│   │   ├── MissionManager.cs   # 任务管理
│   │   ├── EventManager.cs    # 事件管理（基础）
│   │   ├── DialogueManager.cs # 对话管理（基础）
│   │   ├── SaveManager.cs     # 存档系统
│   │   ├── AudioManager.cs    # 音频管理
│   │   └── UIManager.cs       # UI管理（基础）
│   ├── Models/                # 数据模型 ✅
│   │   └── GameState.cs       # 游戏状态数据
│   ├── Data/                  # ScriptableObjects（待创建）
│   ├── Systems/               # 各系统实现（待创建）
│   ├── UI/                    # UI组件（待创建）
│   └── Utilities/             # 工具类（待创建）
└── README.md                  # 本文件
```

## 🚀 快速开始

### 步骤1：创建Unity项目

1. 打开Unity Hub
2. 创建新项目：
   - 名称：MindLink
   - 模板：2D 或 3D（推荐2D）
   - Unity版本：2022.3 LTS 或更高

### 步骤2：导入脚本

1. 将整个 `unity/Scripts` 文件夹复制到Unity项目的 `Assets/` 目录
2. Unity会自动编译脚本
3. 如果有编译错误，等待Unity编辑器完全加载后再检查

### 步骤3：设置场景

1. 在Hierarchy中创建空GameObject，命名为"GameManager"
2. 添加 `GameManager.cs` 组件
3. 运行游戏 - 其他Manager会自动创建

## 📚 核心系统说明

### GameManager - 游戏总控

**功能**：
- 游戏生命周期管理（开始、暂停、结束）
- 场景切换
- 管理器初始化
- 快速存档/读档

**使用方法**：
```csharp
// 开始新游戏
GameManager.Instance.StartNewGame();

// 继续游戏
GameManager.Instance.ContinueGame(slotIndex);

// 暂停/继续
GameManager.Instance.PauseGame();
GameManager.Instance.ResumeGame();

// 快速保存
GameManager.Instance.QuickSave();
```

### GameState - 游戏数据

**包含**：
- 时间数据（Day, Timeslot）
- 4种属性（Knowledge, Courage, Charm, Combat）
- 角色关系
- 任务进度
- 事件记录
- 标记系统
- 道具背包

**访问**：
```csharp
GameState state = GameManager.Instance.CurrentGameState;

// 设置标记
state.SetFlag("mission_started", true);

// 检查标记
bool completed = state.GetFlag("tutorial_complete");

// 添加道具
state.AddItem("key_01", 1);
```

### TimeManager - 时间管理

**功能**：
- 时间推进（Morning → Afternoon → Night → Next Day）
- 星期几计算
- 倒计时系统
- 冷却管理

**使用**：
```csharp
// 推进时间
TimeManager.Instance.AdvanceTime();

// 获取当前信息
int day = TimeManager.Instance.CurrentDay;
Timeslot slot = TimeManager.Instance.CurrentTimeslot;
string dayName = TimeManager.Instance.GetDayOfWeekName();

// 订阅事件
TimeManager.OnDayStart += (day) => {
    Debug.Log($"New day: {day}");
};
```

### AttributeManager - 属性管理

**功能**：
- 属性经验值管理
- 自动升级计算
- 升级通知
- 属性要求检查

**使用**：
```csharp
// 添加经验
AttributeManager.Instance.AddAttributeExp(AttributeType.Combat, 25);

// 检查等级
int level = AttributeManager.Instance.GetAttributeLevel(AttributeType.Knowledge);

// 检查要求
var requirements = new Dictionary<AttributeType, int> {
    { AttributeType.Combat, 3 }
};
bool canDo = AttributeManager.Instance.CheckAttributeRequirements(requirements);
```

### RelationshipManager - 关系管理

**功能**：
- 好感度管理
- 自动升级
- 关系解锁（Lv2专属活动，Lv4支线，Lv10结局）

**使用**：
```csharp
// 添加好感度
RelationshipManager.Instance.AddRelationshipExp("akira", 20);

// 检查等级
int level = RelationshipManager.Instance.GetRelationshipLevel("rei");

// 添加新角色
RelationshipManager.Instance.AddCharacter("mizuki", "美月", 1);
```

### MissionManager - 任务管理

**功能**：
- 任务创建和追踪
- 目标进度管理
- 截止日期提醒
- 自动失败判定

**使用**：
```csharp
// 创建任务
var objectives = new Dictionary<string, ObjectiveProgress> {
    { "combat_level", new ObjectiveProgress("战斗力达到Lv3", 3, 1) },
    { "clues", new ObjectiveProgress("收集线索", 3, 0) }
};
MissionManager.Instance.StartMission("mission_01", "拯救美月", 15, objectives);

// 更新进度
MissionManager.Instance.UpdateMissionObjective("mission_01", "clues", 1);

// 检查剩余时间
int daysLeft = MissionManager.Instance.GetMissionDaysRemaining("mission_01");
```

### SaveManager - 存档系统

**功能**：
- JSON序列化
- 5个存档槽位
- 自动存档/快速存档
- 存档信息查询

**使用**：
```csharp
// 保存游戏
SaveManager.Instance.SaveGame(gameState, slotIndex);

// 加载游戏
GameState loadedState = SaveManager.Instance.LoadGame(slotIndex);

// 快速保存
SaveManager.Instance.QuickSave(gameState);

// 检查存档
bool hasSave = SaveManager.Instance.HasSave(slotIndex);
SaveInfo info = SaveManager.Instance.GetSaveInfo(slotIndex);
```

### AudioManager - 音频管理

**功能**：
- BGM播放和淡入淡出
- SFX音效池
- 音量控制
- 静音功能

**使用**：
```csharp
// 播放BGM
AudioManager.Instance.PlayBGM(bgmClip, fadeIn: true);

// 播放音效
AudioManager.Instance.PlaySFX(sfxClip);

// 设置音量
AudioManager.Instance.BGMVolume = 0.7f;
AudioManager.Instance.SetSFXVolume(0.8f);
```

## 📝 事件系统

所有Manager都使用事件系统进行通信：

```csharp
// 订阅时间推进事件
TimeManager.OnTimeAdvance += (day, timeslot) => {
    Debug.Log($"Time: Day {day} - {timeslot}");
};

// 订阅属性升级事件
AttributeManager.OnAttributeLevelUp += (type, level) => {
    Debug.Log($"{type} leveled up to {level}!");
};

// 订阅任务事件
MissionManager.OnMissionComplete += (missionId, success) => {
    if (success) {
        Debug.Log($"Mission {missionId} completed!");
    }
};
```

## 🔧 调试功能

所有Manager都提供调试信息：

```csharp
// 时间信息
Debug.Log(TimeManager.Instance.GetDebugInfo());
// Output: Day 15/100 - 夜晚 (星期一)

// 属性信息
Debug.Log(AttributeManager.Instance.GetDebugInfo());
// Output: Knowledge: Lv3 (50/150) ...

// 关系信息
Debug.Log(RelationshipManager.Instance.GetDebugInfo());
// Output: 晓: Lv4 (30/100) ...

// 任务信息
Debug.Log(MissionManager.Instance.GetDebugInfo());
// Output: Active: 1, Completed: 2 ...

// 存档信息
Debug.Log(SaveManager.Instance.GetDebugInfo());
// Output: [Slot 0] 自动存档, Day: 15 ...
```

## ⚠️ 注意事项

1. **Manager依赖关系**：
   - GameManager必须最先创建
   - 其他Manager会自动作为GameManager的子对象

2. **事件订阅**：
   - 记得在OnDestroy中取消订阅，避免内存泄漏

3. **GameState**：
   - 所有数据修改都应通过Manager进行
   - 直接修改GameState可能导致事件不触发

4. **存档路径**：
   - 存档保存在 `Application.persistentDataPath/Saves/`
   - 可以用 `SaveManager.Instance.OpenSaveFolder()` 打开文件夹

## 🚧 待完成

以下系统还需要实现：

- [ ] EventManager完整实现（ScriptableObject事件数据）
- [ ] DialogueManager完整实现（对话树、打字机效果）
- [ ] UIManager完整实现（所有UI面板）
- [ ] BattleManager（战斗系统）
- [ ] PuzzleManager（解谜系统）
- [ ] ScriptableObject数据定义
- [ ] UI Prefab组件

## 📖 下一步

1. 在Unity中测试现有Manager
2. 创建ScriptableObject数据结构
3. 实现UI系统
4. 添加战斗和解谜系统

## 📞 问题反馈

如果遇到问题：
1. 检查Unity Console的错误信息
2. 确认所有Manager都已正确初始化
3. 使用Debug功能查看当前状态

---

**版本**: 1.0
**最后更新**: 2025-11-05
**状态**: Phase 1完成 - 核心系统就绪
