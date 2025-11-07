# ScriptableObject Data System Guide

## 概述

本指南介绍如何使用Unity的ScriptableObject系统来创建和管理游戏内容（事件、对话、角色、任务）。

ScriptableObject的优势：
- ✅ **可视化编辑** - 在Unity编辑器中直接配置，无需写代码
- ✅ **数据驱动** - 内容和代码分离，方便迭代
- ✅ **易于管理** - 文件化管理，支持版本控制
- ✅ **即时预览** - 修改后无需编译即可测试

---

## 目录结构设置

首先，在Unity项目中创建以下文件夹结构：

```
Assets/
├── Resources/
│   ├── Events/          # 存放EventData资源
│   ├── Dialogues/       # 存放DialogueData资源
│   ├── Characters/      # 存放CharacterData资源
│   └── Missions/        # 存放MissionData资源
```

**重要**：必须使用 `Resources/` 文件夹，因为系统使用 `Resources.Load()` 来加载这些资源。

---

## 1. 创建事件 (EventData)

### 1.1 创建新事件

1. 在 `Assets/Resources/Events/` 文件夹中右键
2. 选择 `Create → MindLink → Event Data`
3. 命名文件（例如：`event_study_library`）

### 1.2 配置事件属性

#### **基本信息**
- **Event Id**: 唯一标识符，如 `study_library`
- **Event Name**: 显示名称，如 `图书馆学习`
- **Description**: 事件描述
- **Icon**: 事件图标
- **Event Type**: 事件类型（Story/Daily/Social/Study/Training等）

#### **触发条件**
- **Day Range**: 可触发的天数范围
  - Min Day: 最小天数
  - Max Day: 最大天数
- **Available Timeslots**: 可触发的时间段（Morning/Afternoon/Night）
- **Weekday Only**: 仅工作日触发
- **Weekend Only**: 仅周末触发
- **Attribute Requirements**: 需要的属性等级
- **Relationship Requirements**: 需要的关系等级
- **Required Flags**: 需要的标记
- **Blocked Flags**: 禁止的标记
- **Random Chance**: 随机触发概率 (0-1)

#### **事件效果**
- **Attribute Changes**: 属性变化
  - Attribute Type: 属性类型
  - Amount: 变化数值（经验值）
- **Relationship Changes**: 关系变化
  - Character Id: 角色ID
  - Amount: 变化数值
- **Item Rewards**: 道具奖励
- **Flag Changes**: 设置标记
- **Trigger Dialogue**: 触发的对话

#### **重复性设置**
- **Repeatable**: 是否可重复触发
- **Cooldown Days**: 冷却天数
- **Max Per Day**: 每天最多触发次数

#### **特殊设置**
- **Auto Trigger**: 是否自动触发（剧情事件）
- **Consume Timeslot**: 是否消耗时间段
- **Priority**: 优先级（自动触发时使用）

### 1.3 示例：图书馆学习事件

```
Event Id: study_library
Event Name: 图书馆学习
Description: 在图书馆学习，提升知识属性
Event Type: Study

Trigger Conditions:
  Day Range: 1-100
  Available Timeslots: Morning, Afternoon, Night

Effects:
  Attribute Changes:
    - Knowledge: +20

Repeatable: true
Consume Timeslot: true
```

---

## 2. 创建对话 (DialogueData)

### 2.1 创建新对话

1. 在 `Assets/Resources/Dialogues/` 文件夹中右键
2. 选择 `Create → MindLink → Dialogue Data`
3. 命名文件（例如：`dialogue_day1_akira`）

### 2.2 配置对话树

#### **基本信息**
- **Dialogue Id**: 唯一标识符
- **Dialogue Name**: 对话名称
- **Start Node Index**: 起始节点索引（通常是0）

#### **对话节点 (Dialogue Nodes)**

每个节点包含：

**节点信息**
- **Node Id**: 节点唯一ID（如 `node_01`）
- **Editor Note**: 编辑器备注（不会在游戏中显示）

**角色和文本**
- **Character Id**: 说话角色ID（空=旁白）
- **Character Display Name**: 角色显示名称
- **Emotion**: 角色表情
- **Text**: 对话文本内容

**流程控制**
- **Node Type**: 节点类型
  - **Normal**: 普通对话，自动继续
  - **Choice**: 选择分支，等待玩家选择
  - **End**: 对话结束
- **Next Node Id**: 下一个节点ID（Normal类型使用）
- **Choices**: 选择列表（Choice类型使用）

**条件检查**
- **Required Flags**: 需要的标记
- **Attribute Requirements**: 需要的属性等级
- **Relationship Requirements**: 需要的关系等级

**效果**
- **Effects**: 显示节点时触发的效果
  - Attribute Changes
  - Relationship Changes
  - Item Rewards
  - Flag Changes

### 2.3 配置选择项 (Choices)

每个选择包含：
- **Text**: 选择文本
- **Next Node Id**: 选择后跳转的节点
- **Effects**: 选择的效果
- **Ends Dialogue**: 是否结束对话
- **条件**: Required Flags/Attribute/Relationship Requirements
- **Locked Hint**: 条件不满足时的提示

### 2.4 示例：Day 1 晓的对话

```
Dialogue Id: dialogue_day1_akira
Dialogue Name: Day 1 - 与晓相遇

Nodes:
  [0] node_greeting:
    Character: akira
    Emotion: Normal
    Text: "你好，我叫晓。你是新来的吧？"
    Node Type: Normal
    Next Node: node_question

  [1] node_question:
    Character: akira
    Emotion: Smiling
    Text: "需要我带你熟悉一下学校吗？"
    Node Type: Choice
    Choices:
      - "好啊，麻烦你了！" → node_accept
        Effects: Relationship(akira, +10)
      - "不用了，我自己看看。" → node_decline

  [2] node_accept:
    Character: akira
    Emotion: Happy
    Text: "没问题！跟我来吧~"
    Node Type: End
    Effects: Flag(akira_friendly, true)

  [3] node_decline:
    Character: akira
    Emotion: Normal
    Text: "好的，有需要随时找我。"
    Node Type: End
```

---

## 3. 创建角色 (CharacterData)

### 3.1 创建新角色

1. 在 `Assets/Resources/Characters/` 文件夹中右键
2. 选择 `Create → MindLink → Character Data`
3. 命名文件（例如：`character_akira`）

### 3.2 配置角色属性

#### **基本信息**
- **Character Id**: 角色唯一ID（如 `akira`）
- **Character Name**: 角色名称（如 `晓`）
- **Full Name**: 全名
- **Age**: 年龄
- **Gender**: 性别
- **Description**: 角色简介

#### **外观**
- **Portraits**: 角色立绘（各种表情）
  - Normal, Happy, Sad, Angry, Surprised, Worried, Thinking, Embarrassed, Serious, Smiling
- **Icon**: 角色图标（UI用）
- **Theme Color**: 主题颜色

#### **初始状态**
- **Initial Relationship Level**: 初始关系等级（1-10）
- **Initial Relationship Exp**: 初始经验值
- **Unlocked By Default**: 游戏开始时是否已解锁
- **Unlock Flag**: 解锁需要的标记

#### **关系等级解锁内容**
- **Level 2 Unlocked Events**: 等级2解锁的活动
- **Level 4 Side Quest**: 等级4解锁的支线任务
- **Level 6 Unlocked Dialogues**: 等级6解锁的对话
- **Level 8 Special Event**: 等级8解锁的特殊事件
- **Level 10 Ending Id**: 等级10解锁的结局

#### **角色特性**
- **Tags**: 角色标签（Student/Teacher/Ally/Enemy等）
- **Preferred Activities**: 喜好的活动类型（好感度1.5倍）
- **Disliked Activities**: 讨厌的活动类型（好感度0.5倍）
- **Birthday**: 生日（Day）

#### **对话和语音**
- **Default Greeting**: 默认问候对话
- **Level Up Dialogues**: 关系等级提升对话列表
- **Voice Type**: 语音音色
- **Voice Pitch**: 语音音高

#### **战斗相关**
- **Can Battle Partner**: 是否可作为战斗伙伴
- **Battle Partner Unlock Level**: 战斗伙伴解锁等级
- **Combat Stats**: 战斗属性

### 3.3 示例：晓的角色数据

```
Character Id: akira
Character Name: 晓
Age: 17
Gender: Male
Description: 学校的明星学生，性格开朗友善

Initial Relationship Level: 1
Unlocked By Default: true

Level 2 Unlocked Events: [event_akira_training]
Level 4 Side Quest: mission_akira_sidestory
Level 10 Ending: ending_akira_true

Tags: [Student, Ally, Friend]
Preferred Activities: [Training, Social]
```

---

## 4. 创建任务 (MissionData)

### 4.1 创建新任务

1. 在 `Assets/Resources/Missions/` 文件夹中右键
2. 选择 `Create → MindLink → Mission Data`
3. 命名文件（例如：`mission_rescue_mizuki`）

### 4.2 配置任务属性

#### **基本信息**
- **Mission Id**: 任务唯一ID
- **Mission Name**: 任务名称
- **Description**: 任务描述
- **Mission Type**: 任务类型（Main/Side/Character/Daily等）
- **Icon**: 任务图标

#### **触发条件**
- **Trigger Day Range**: 触发任务的天数范围
- **Required Flags**: 需要的标记
- **Attribute Requirements**: 需要的属性等级
- **Relationship Requirements**: 需要的关系等级
- **Prerequisite Missions**: 前置任务
- **Auto Trigger**: 满足条件时自动开始

#### **时间限制**
- **Has Deadline**: 是否有截止日期
- **Duration Days**: 持续天数
- **Game Over On Failure**: 失败后是否Game Over

#### **任务目标 (Objectives)**

每个目标包含：
- **Objective Id**: 目标唯一ID
- **Description**: 目标描述
- **Objective Type**: 目标类型
  - Custom, AttributeLevel, RelationshipLevel, CollectItems, CompleteEvents, WinBattles, SolvePuzzles, TalkToCharacter, ReachDay, SetFlag
- **Target Value**: 目标值
- **Initial Value**: 初始值
- **Is Optional**: 是否为可选目标
- **Auto Track Type**: 自动追踪类型（用于自动更新进度）

#### **奖励**
- **Attribute Rewards**: 属性经验奖励
- **Relationship Rewards**: 关系经验奖励
- **Item Rewards**: 道具奖励
- **Flag Rewards**: 设置的标记
- **Unlock Content**: 解锁的内容

#### **对话**
- **Accept Dialogue**: 接受任务时的对话
- **Progress Dialogue**: 任务进行中的对话
- **Complete Dialogue**: 完成任务时的对话
- **Fail Dialogue**: 任务失败时的对话

### 4.3 示例：拯救美月任务

```
Mission Id: rescue_mizuki
Mission Name: 拯救美月
Description: 美月被困在认知空间，必须在15天内拯救她
Mission Type: Main

Trigger Day Range: 10-12
Auto Trigger: true

Has Deadline: true
Duration Days: 5
Game Over On Failure: true

Objectives:
  [0] combat_level:
    Description: "战斗力达到Lv3"
    Objective Type: AttributeLevel
    Target Value: 3
    Auto Track: AttributeLevel
    Tracked Attribute: Combat

  [1] collect_clues:
    Description: "收集3个线索"
    Objective Type: CollectItems
    Target Value: 3

  [2] find_ally:
    Description: "找到同伴"
    Objective Type: Custom
    Target Value: 1

Attribute Rewards:
  - Combat: +50
  - Courage: +30

Flag Rewards:
  - mizuki_rescued: true
```

---

## 5. 在代码中使用

### 5.1 触发事件

```csharp
// 通过EventManager获取可用事件
List<EventData> availableEvents = EventManager.Instance.GetAvailableEvents();

// 执行事件
EventData event = EventManager.Instance.GetEventById("study_library");
EventManager.Instance.ExecuteEvent(event);
```

### 5.2 播放对话

```csharp
// 通过ID播放对话
DialogueManager.Instance.StartDialogue("dialogue_day1_akira");

// 或者直接传入DialogueData
DialogueData dialogue = ...;
DialogueManager.Instance.StartDialogue(dialogue);

// 前进对话
DialogueManager.Instance.AdvanceDialogue();

// 选择选项
DialogueManager.Instance.SelectChoice(0);
```

### 5.3 获取角色信息

```csharp
// 从Resources加载
CharacterData character = Resources.Load<CharacterData>("Characters/character_akira");

// 获取立绘
Sprite portrait = character.GetPortrait(CharacterEmotion.Happy);

// 获取解锁内容
CharacterUnlockInfo unlockInfo = character.GetUnlockInfo(4);
```

### 5.4 开始任务

```csharp
// 从Resources加载
MissionData mission = Resources.Load<MissionData>("Missions/mission_rescue_mizuki");

// 检查是否可接受
if (mission.CanAccept(gameState))
{
    // 创建任务进度
    MissionProgress progress = mission.CreateMissionProgress(currentDay);

    // 添加到活跃任务
    MissionManager.Instance.StartMission(
        mission.missionId,
        mission.missionName,
        mission.durationDays,
        progress.Objectives
    );
}
```

---

## 6. 事件系统集成

### 6.1 EventManager自动加载

EventManager会在启动时自动从 `Resources/Events/` 加载所有EventData：

```csharp
EventData[] loadedEvents = Resources.LoadAll<EventData>("Events");
```

### 6.2 事件触发时机

- **Day Start**: `EventManager.Instance.TriggerDayStart()` - 自动触发剧情事件
- **Timeslot Change**: `EventManager.Instance.CheckTimeslotEvents()` - 更新可用事件
- **Manual**: `EventManager.Instance.ExecuteEvent(eventData)` - 玩家选择活动

### 6.3 订阅事件通知

```csharp
// 订阅事件触发通知
EventManager.OnEventTriggered += (eventId, eventData) => {
    Debug.Log($"Event triggered: {eventData.eventName}");
};

// 订阅对话开始通知
DialogueManager.OnDialogueStart += (dialogueData) => {
    // 显示对话UI
};

// 订阅打字机更新
DialogueManager.OnTypewriterUpdate += (text) => {
    // 更新UI文本
};
```

---

## 7. 最佳实践

### 7.1 命名规范

- **Event Id**: `event_<category>_<name>`
  - 例：`event_story_day1`, `event_training_gym`
- **Dialogue Id**: `dialogue_<context>_<character>`
  - 例：`dialogue_day1_akira`, `dialogue_levelup_rei`
- **Character Id**: 使用小写英文名
  - 例：`akira`, `rei`, `mizuki`
- **Mission Id**: `mission_<type>_<name>`
  - 例：`mission_main_rescue`, `mission_side_investigation`

### 7.2 Flag命名规范

- 剧情标记：`story_<event>`
  - 例：`story_day1_complete`
- 角色标记：`<character>_<state>`
  - 例：`akira_helped`, `rei_unlocked`
- 系统标记：`system_<feature>`
  - 例：`system_tutorial_complete`

### 7.3 数据组织

按功能分类创建子文件夹：

```
Resources/
├── Events/
│   ├── Story/          # 主线剧情事件
│   ├── Daily/          # 日常活动
│   ├── Character/      # 角色专属事件
│   └── Special/        # 特殊事件
├── Dialogues/
│   ├── Main/           # 主线对话
│   ├── Character/      # 角色对话
│   └── System/         # 系统对话
└── Missions/
    ├── Main/           # 主线任务
    ├── Side/           # 支线任务
    └── Character/      # 角色任务
```

### 7.4 版本控制

- ScriptableObject文件是文本格式（YAML），适合Git
- 大型立绘/音频资源使用Git LFS
- 定期备份 Resources 文件夹

---

## 8. 调试技巧

### 8.1 验证数据完整性

所有ScriptableObject都有 `Validate()` 方法：

```csharp
EventData eventData = ...;
string error;
if (!eventData.Validate(out error))
{
    Debug.LogError($"Invalid event: {error}");
}
```

### 8.2 打印调试信息

```csharp
// EventManager调试
Debug.Log(EventManager.Instance.GetDebugInfo());

// DialogueManager调试
Debug.Log(DialogueManager.Instance.GetDebugInfo());
```

### 8.3 测试单个对话/事件

在Unity编辑器中：
1. 选中ScriptableObject文件
2. Inspector面板显示所有字段
3. 可以临时修改测试，Play Mode后会恢复

---

## 9. 常见问题

### Q: 为什么必须放在Resources文件夹？

A: 因为系统使用 `Resources.Load()` 来动态加载资源。如果不想用Resources，可以改用Addressables系统。

### Q: 如何创建循环对话？

A: 让最后一个节点的 `Next Node Id` 指向前面的节点即可。注意避免无限循环，应该有退出条件。

### Q: 事件不触发怎么办？

A: 检查：
1. Day Range 是否包含当前天数
2. Available Timeslots 是否包含当前时间段
3. 所有条件（属性、关系、标记）是否满足
4. Random Chance 是否太低
5. 使用 `EventManager.Instance.GetDebugInfo()` 查看可用事件

### Q: 对话选择显示不出来？

A: 检查：
1. Node Type 是否设为 Choice
2. Choices 列表是否为空
3. 选择的条件是否满足
4. UI是否正确订阅了 `OnDialogueNodeDisplay` 事件

### Q: 如何实现条件分支对话？

A: 使用节点的条件检查：
1. 创建两个节点，内容不同
2. 设置不同的 Required Flags
3. 前一个节点指向条件节点
4. 系统会自动跳过条件不满足的节点

---

## 10. 下一步

完成ScriptableObject系统后，下一步应该：

1. **创建UI系统** - 显示事件列表、对话框、任务追踪
2. **创建示例数据** - Day 1-3的剧情事件和对话
3. **集成音频** - 添加BGM和音效支持
4. **战斗系统** - 基于属性的战斗系统
5. **存档集成** - 确保所有数据可正确保存/加载

---

## 总结

ScriptableObject系统为MindLink提供了强大的数据驱动内容创作工具：

✅ **事件系统** - 复杂的触发条件和效果
✅ **对话系统** - 分支对话树和选择
✅ **角色系统** - 关系等级和解锁内容
✅ **任务系统** - 目标追踪和奖励

现在可以在Unity编辑器中可视化地创建所有游戏内容，无需编写代码！

---

**版本**: 1.0
**最后更新**: 2025-11-07
**作者**: Claude (Anthropic)
