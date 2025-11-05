# 系统架构设计
# MindLink - 认知空间

**版本**: 1.0
**日期**: 2025-11-05

---

## 目录

1. [架构概览](#架构概览)
2. [核心管理器](#核心管理器)
3. [系统交互图](#系统交互图)
4. [数据流](#数据流)
5. [技术栈](#技术栈)

---

## 架构概览

### 设计原则

1. **模块化** - 每个系统独立封装，通过接口通信
2. **数据驱动** - 游戏内容通过JSON配置，方便调整和扩展
3. **事件驱动** - 使用事件系统解耦模块间依赖
4. **可测试性** - 核心逻辑与Unity解耦，便于单元测试
5. **可扩展性** - 为DLC和更新预留接口

### 整体架构

```
┌─────────────────────────────────────────────────┐
│              Presentation Layer                  │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐        │
│  │   UI     │ │  Visual  │ │  Audio   │        │
│  │ Manager  │ │  Effects │ │  Manager │        │
│  └──────────┘ └──────────┘ └──────────┘        │
└─────────────────────────────────────────────────┘
                      ↕
┌─────────────────────────────────────────────────┐
│               Game Logic Layer                   │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐        │
│  │   Game   │ │   Time   │ │  Event   │        │
│  │  Manager │ │  Manager │ │  Manager │        │
│  └──────────┘ └──────────┘ └──────────┘        │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐        │
│  │Character │ │Attribute │ │  Battle  │        │
│  │ Manager  │ │  Manager │ │  Manager │        │
│  └──────────┘ └──────────┘ └──────────┘        │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐        │
│  │ Puzzle   │ │ Dialogue │ │   Save   │        │
│  │ Manager  │ │  Manager │ │  Manager │        │
│  └──────────┘ └──────────┘ └──────────┘        │
└─────────────────────────────────────────────────┘
                      ↕
┌─────────────────────────────────────────────────┐
│                 Data Layer                       │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐        │
│  │  Config  │ │  Runtime │ │   Save   │        │
│  │   Data   │ │   Data   │ │   Data   │        │
│  └──────────┘ └──────────┘ └──────────┘        │
└─────────────────────────────────────────────────┘
```

---

## 核心管理器

### 1. GameManager (游戏总控)

**职责**:
- 游戏初始化和生命周期管理
- 场景切换和状态管理
- 协调各个管理器

**主要接口**:
```csharp
public class GameManager : Singleton<GameManager>
{
    // 游戏状态
    public enum GameState
    {
        MainMenu,      // 主菜单
        DailyLife,     // 日常生活
        Mission,       // 任务/战斗
        Dialogue,      // 对话
        Paused         // 暂停
    }

    public GameState CurrentState { get; private set; }

    // 游戏流程控制
    public void StartNewGame();
    public void LoadGame(int saveSlot);
    public void SaveGame(int saveSlot);
    public void ChangeState(GameState newState);

    // 事件
    public event Action<GameState> OnStateChanged;
}
```

---

### 2. TimeManager (时间管理器)

**职责**:
- 管理游戏日期和时间段
- 处理时间流逝
- 触发时间相关事件

**详细设计**: 见 [TimeSystem.md](TimeSystem.md)

**主要接口**:
```csharp
public class TimeManager : Singleton<TimeManager>
{
    // 时间段枚举
    public enum TimeSlot
    {
        Morning,    // 早晨
        Afternoon,  // 下午
        Night       // 夜晚
    }

    // 当前状态
    public int CurrentDay { get; private set; }        // 1-100
    public TimeSlot CurrentTimeSlot { get; private set; }
    public int RemainingTimeSlots { get; }             // 到游戏结束还有多少时间段

    // 时间操作
    public void AdvanceTime();                         // 前进一个时间段
    public bool IsWeekend();
    public bool IsSpecialDate(int day);
    public int GetDaysUntilDeadline(string missionId);

    // 事件
    public event Action<int, TimeSlot> OnTimeAdvanced;
    public event Action<int> OnNewDay;
    public event Action OnDeadlineWarning;
}
```

---

### 3. EventManager (事件管理器)

**职责**:
- 加载和管理所有游戏事件
- 根据条件判断事件可用性
- 触发和执行事件

**详细设计**: 见 [EventSystem.md](EventSystem.md)

**主要接口**:
```csharp
public class EventManager : Singleton<EventManager>
{
    // 获取可用事件
    public List<GameEvent> GetAvailableEvents(TimeSlot timeSlot);
    public List<GameEvent> GetAvailableActivities();
    public GameEvent GetMainStoryEvent(int day);

    // 执行事件
    public void ExecuteEvent(string eventId);
    public bool CheckEventConditions(GameEvent evt);

    // 事件记录
    public void MarkEventCompleted(string eventId);
    public bool IsEventCompleted(string eventId);
    public void SetEventFlag(string flagId, bool value);
    public bool GetEventFlag(string flagId);

    // 事件
    public event Action<GameEvent> OnEventStarted;
    public event Action<GameEvent, EventResult> OnEventCompleted;
}
```

**事件数据结构**:
```csharp
public class GameEvent
{
    public string Id;                          // 唯一标识
    public string Name;                        // 显示名称
    public string Description;                 // 描述
    public EventType Type;                     // 事件类型

    public EventCondition Conditions;          // 触发条件
    public EventEffect Effects;                // 效果

    public bool IsRepeatable;                  // 是否可重复
    public int CooldownDays;                   // 冷却天数
}

public class EventCondition
{
    public int? MinDay;
    public int? MaxDay;
    public TimeSlot? RequiredTimeSlot;
    public Dictionary<string, int> RequiredAttributes;
    public Dictionary<string, int> RequiredRelationships;
    public List<string> RequiredFlags;
    public List<string> ForbiddenFlags;
    public float RandomChance;                 // 0-1
}

public class EventEffect
{
    public Dictionary<string, int> AttributeChanges;
    public Dictionary<string, int> RelationshipChanges;
    public List<string> SetFlags;
    public List<string> UnlockEvents;
    public ItemReward[] ItemRewards;
    public int MoneyCost;
    public int MoneyReward;
}
```

---

### 4. AttributeManager (属性管理器)

**职责**:
- 管理主角的所有属性
- 处理属性增长和等级提升
- 检查属性阈值条件

**详细设计**: 见 [AttributeSystem.md](AttributeSystem.md)

**主要接口**:
```csharp
public class AttributeManager : Singleton<AttributeManager>
{
    // 属性类型
    public enum AttributeType
    {
        Knowledge,   // 知识
        Courage,     // 勇气
        Charm,       // 魅力
        Combat       // 战斗力
    }

    // 属性访问
    public int GetAttributeLevel(AttributeType type);
    public int GetAttributeExp(AttributeType type);
    public float GetAttributeProgress(AttributeType type);    // 0-1

    // 属性修改
    public void AddAttributeExp(AttributeType type, int amount);
    public bool CheckAttributeRequirement(AttributeType type, int minLevel);

    // 特殊能力（基于属性解锁）
    public List<Ability> GetUnlockedAbilities();

    // 事件
    public event Action<AttributeType, int> OnAttributeLevelUp;
    public event Action<AttributeType, int> OnAttributeExpGained;
}
```

---

### 5. CharacterManager (角色关系管理器)

**职责**:
- 管理所有NPC角色
- 处理好感度系统
- 角色状态和可用性

**详细设计**: 见 [RelationshipSystem.md](RelationshipSystem.md)

**主要接口**:
```csharp
public class CharacterManager : Singleton<CharacterManager>
{
    // 角色访问
    public Character GetCharacter(string characterId);
    public List<Character> GetAllCharacters();
    public List<Character> GetAvailableCharacters(TimeSlot timeSlot);

    // 好感度管理
    public int GetRelationshipLevel(string characterId);
    public void AddRelationshipExp(string characterId, int amount);
    public bool IsCharacterRomanceable(string characterId);

    // 角色状态
    public bool IsCharacterAlive(string characterId);
    public bool IsCharacterAvailable(string characterId);
    public string GetCharacterLocation(string characterId);

    // 事件
    public event Action<string, int> OnRelationshipLevelUp;
    public event Action<string> OnCharacterUnlocked;
    public event Action<string> OnCharacterDeath;
}
```

**角色数据结构**:
```csharp
public class Character
{
    public string Id;
    public string Name;
    public CharacterRole Role;

    // 关系数据
    public int RelationshipLevel;      // 0-10
    public int RelationshipExp;

    // 状态
    public bool IsUnlocked;
    public bool IsAlive;
    public bool IsRomanceable;
    public string CurrentLocation;

    // 个性数据
    public PersonalityTraits Personality;
    public List<string> Likes;
    public List<string> Dislikes;

    // 战斗数据（如果可以作为队友）
    public BattleStats BattleStats;
}
```

---

### 6. BattleManager (战斗管理器)

**职责**:
- 管理回合制战斗流程
- 处理战斗逻辑和AI
- 战斗奖励结算

**详细设计**: 见 [BattleSystem.md](BattleSystem.md)

**主要接口**:
```csharp
public class BattleManager : Singleton<BattleManager>
{
    // 战斗控制
    public void StartBattle(BattleConfig config);
    public void EndBattle(BattleResult result);

    // 回合控制
    public void ExecutePlayerAction(BattleAction action);
    public void ProcessEnemyTurn();
    public void NextTurn();

    // 战斗状态
    public BattleState CurrentState { get; }
    public List<Combatant> PlayerParty { get; }
    public List<Combatant> EnemyParty { get; }
    public int CurrentTurn { get; }

    // 事件
    public event Action<BattleResult> OnBattleEnded;
    public event Action<Combatant> OnCombatantDefeated;
}
```

---

### 7. PuzzleManager (解谜管理器)

**职责**:
- 管理各类解谜小游戏
- 验证解谜答案
- 提供提示系统

**详细设计**: 见 [PuzzleSystem.md](PuzzleSystem.md)

**主要接口**:
```csharp
public class PuzzleManager : Singleton<PuzzleManager>
{
    // 解谜控制
    public void StartPuzzle(string puzzleId);
    public void SubmitAnswer(object answer);
    public void SkipPuzzle(string puzzleId);      // 消耗道具或条件

    // 提示系统
    public string GetHint(string puzzleId, int hintLevel);
    public int GetAvailableHints(string puzzleId);

    // 事件
    public event Action<string> OnPuzzleSolved;
    public event Action<string> OnPuzzleFailed;
}
```

---

### 8. DialogueManager (对话管理器)

**职责**:
- 显示和处理对话
- 管理对话选项和分支
- 触发对话结果

**主要接口**:
```csharp
public class DialogueManager : Singleton<DialogueManager>
{
    // 对话控制
    public void StartDialogue(string dialogueId);
    public void ShowNextLine();
    public void SelectOption(int optionIndex);
    public void EndDialogue();

    // 状态
    public bool IsDialogueActive { get; }
    public DialogueNode CurrentNode { get; }

    // 事件
    public event Action<DialogueNode> OnDialogueNodeChanged;
    public event Action<List<DialogueOption>> OnOptionsPresented;
    public event Action OnDialogueEnded;
}
```

**对话数据结构**:
```csharp
public class DialogueNode
{
    public string Id;
    public string SpeakerId;           // 说话者ID
    public string Text;                 // 对话文本
    public string[] TextVariants;       // 基于条件的文本变体

    public DialogueOption[] Options;    // 选项
    public string NextNodeId;           // 下一个节点（无选项时）

    public EventEffect Effects;         // 对话效果
    public DialogueCondition Conditions; // 显示条件
}

public class DialogueOption
{
    public string Text;
    public string NextNodeId;
    public DialogueCondition Conditions;
    public EventEffect Effects;

    // UI提示
    public AttributeType? RequiredAttribute;
    public int? RequiredLevel;
}
```

---

### 9. SaveManager (存档管理器)

**职责**:
- 保存和加载游戏数据
- 管理多存档位
- 云同步（可选）

**主要接口**:
```csharp
public class SaveManager : Singleton<SaveManager>
{
    // 存档操作
    public void SaveGame(int slot);
    public void LoadGame(int slot);
    public void DeleteSave(int slot);
    public bool DoesSaveExist(int slot);

    // 存档信息
    public SaveData GetSaveData(int slot);
    public SaveMetadata GetSaveMetadata(int slot);

    // 快照系统（关键节点自动存档）
    public void CreateSnapshot(string snapshotId);
    public void LoadSnapshot(string snapshotId);

    // 云同步
    public void SyncToCloud();
    public void LoadFromCloud();
}
```

**存档数据结构**:
```csharp
public class SaveData
{
    public SaveMetadata Metadata;
    public GameProgressData Progress;
    public PlayerData Player;
    public CharacterData[] Characters;
    public EventData[] Events;
    public InventoryData Inventory;
}

public class SaveMetadata
{
    public int SaveSlot;
    public DateTime SaveTime;
    public int CurrentDay;
    public TimeSlot CurrentTimeSlot;
    public int PlaytimeSeconds;
    public string GameVersion;
}
```

---

### 10. UIManager (UI管理器)

**职责**:
- 管理所有UI界面
- 处理界面切换和动画
- UI事件分发

**主要接口**:
```csharp
public class UIManager : Singleton<UIManager>
{
    // 界面控制
    public void ShowScreen(UIScreen screen);
    public void HideScreen(UIScreen screen);
    public void ShowPopup(string popupId, object data);
    public void ClosePopup();

    // 通知和提示
    public void ShowNotification(string message, NotificationType type);
    public void ShowAttributeGain(AttributeType type, int amount);
    public void ShowRelationshipChange(string characterId, int amount);

    // 特殊UI
    public void ShowCalendar();
    public void ShowCharacterList();
    public void ShowStatusScreen();
}
```

---

## 系统交互图

### 日常流程交互

```
Player Input
    ↓
UIManager → GameManager → TimeManager
                ↓
           EventManager → AttributeManager
                ↓           ↓
           DialogueManager  CharacterManager
                ↓
           SaveManager (自动保存)
```

### 战斗流程交互

```
EventManager (触发战斗)
    ↓
BattleManager
    ↓
    ├→ AttributeManager (获取玩家属性)
    ├→ CharacterManager (获取队友数据)
    ├→ UIManager (显示战斗界面)
    └→ SaveManager (战斗胜利后保存)
```

### 解谜流程交互

```
EventManager (进入认知空间)
    ↓
PuzzleManager
    ↓
    ├→ AttributeManager (提供提示)
    ├→ DialogueManager (显示线索)
    └→ BattleManager (解谜失败触发战斗)
```

---

## 数据流

### 游戏启动流程

```
1. GameManager.Initialize()
   ├→ 加载配置数据 (Config JSON)
   ├→ 初始化所有管理器
   └→ 显示主菜单

2. 玩家选择"新游戏" / "继续游戏"
   ├→ 新游戏: 初始化默认数据
   └→ 继续: SaveManager.LoadGame()

3. 进入游戏世界
   ├→ TimeManager 设置当前日期
   ├→ EventManager 加载可用事件
   └→ UIManager 显示主界面
```

### 时间推进流程

```
1. 玩家选择活动
   ↓
2. EventManager.ExecuteEvent()
   ├→ 检查条件
   ├→ 显示对话/过场
   ├→ 应用效果 (属性、好感度)
   └→ 标记事件完成
   ↓
3. TimeManager.AdvanceTime()
   ├→ 时间段 +1
   ├→ 触发时间事件
   └→ 检查截止日期
   ↓
4. 到达夜晚结束
   ↓
5. 显示日终总结
   ↓
6. SaveManager.AutoSave()
   ↓
7. 进入下一天
```

---

## 技术栈

### 引擎和语言
- **游戏引擎**: Unity 2022.3 LTS
- **编程语言**: C# 10.0
- **.NET版本**: .NET Standard 2.1

### 数据和序列化
- **数据格式**: JSON
- **序列化库**: Newtonsoft.Json (Json.NET)
- **本地化**: Unity Localization Package

### UI
- **UI系统**: Unity UI Toolkit (推荐) 或 uGUI
- **Tween动画**: DOTween

### 资源管理
- **图片**: Sprites (Pixel Art)
- **音频**: AudioClip (MP3/OGG)
- **字体**: TextMeshPro

### 存档
- **本地存档**: PlayerPrefs + JSON files
- **加密**: AES加密（防止篡改）
- **云同步**: Unity Cloud Save (可选)

### 性能优化
- **对象池**: 重用常用对象
- **资源加载**: Addressables System
- **内存管理**: 及时释放不用的资源

### 版本控制
- **Git**: GitHub
- **分支策略**: Git Flow
- **LFS**: 管理大型二进制文件

---

## 设计模式

### 使用的设计模式

1. **单例模式 (Singleton)** - 所有管理器
2. **观察者模式 (Observer)** - 事件系统
3. **工厂模式 (Factory)** - 事件和对象创建
4. **命令模式 (Command)** - 战斗系统的行动
5. **状态模式 (State)** - 游戏状态管理
6. **策略模式 (Strategy)** - AI行为
7. **对象池模式 (Object Pool)** - UI和战斗对象

### 代码组织

```
Scripts/
├── Core/
│   ├── GameManager.cs
│   ├── Singleton.cs
│   └── GameConfig.cs
├── Systems/
│   ├── Time/
│   │   ├── TimeManager.cs
│   │   └── TimeConfig.cs
│   ├── Event/
│   │   ├── EventManager.cs
│   │   ├── GameEvent.cs
│   │   └── EventCondition.cs
│   ├── Attribute/
│   ├── Character/
│   ├── Battle/
│   ├── Puzzle/
│   ├── Dialogue/
│   └── Save/
├── UI/
│   ├── Screens/
│   ├── Popups/
│   └── Components/
├── Data/
│   ├── Models/
│   └── Configs/
└── Utilities/
    ├── Extensions/
    └── Helpers/
```

---

## 性能考虑

### 目标性能指标

- **帧率**: 60 FPS (移动端)
- **启动时间**: < 3秒
- **场景切换**: < 1秒
- **内存占用**: < 300MB (iOS), < 400MB (Android)
- **包体大小**: < 200MB (初始下载)

### 优化策略

1. **资源加载**
   - 使用Addressables按需加载
   - 预加载常用资源
   - 场景切换时卸载无用资源

2. **UI优化**
   - 使用对象池复用UI元素
   - 避免每帧更新UI
   - 使用Canvas分组

3. **数据优化**
   - 使用轻量级数据结构
   - 避免频繁的序列化/反序列化
   - 缓存计算结果

4. **代码优化**
   - 避免在Update中进行复杂计算
   - 使用事件而非轮询
   - 合理使用协程

---

## 测试策略

### 单元测试
- 核心管理器的公共方法
- 数据验证和条件检查
- 存档加载和保存

### 集成测试
- 完整的游戏流程
- 事件触发和效果
- 战斗和解谜系统

### 性能测试
- 长时间运行测试
- 内存泄漏检测
- 帧率压力测试

### 玩家测试
- Alpha测试（内部）
- Beta测试（小范围玩家）
- 收集反馈和数据

---

## 下一步

1. ✅ 完成系统架构文档
2. ⏳ 详细设计各个子系统
3. ⏳ 定义数据格式和Schema
4. ⏳ 开始原型开发

---

**维护信息**:
- 创建日期: 2025-11-05
- 最后更新: 2025-11-05
- 版本: 1.0
