# Unity UI 设置指南

## 概述

本指南将详细说明如何在Unity中搭建MindLink游戏的完整UI系统。

**预计时间**: 1-2小时
**前置要求**:
- Unity 2021.3+ (已安装TextMeshPro)
- MindLink项目已导入所有脚本

---

## 目录

1. [准备工作](#1-准备工作)
2. [创建主Canvas](#2-创建主canvas)
3. [创建顶部信息栏](#3-创建顶部信息栏)
4. [创建EventSelectionUI](#4-创建eventselectionui)
5. [创建DialogueUI](#5-创建dialogueui)
6. [创建MissionTrackerUI](#6-创建missiontrackerui)
7. [创建CharacterInfoUI](#7-创建characterinfoui)
8. [配置MainGameUI](#8-配置maingameui)
9. [测试UI](#9-测试ui)

---

## 1. 准备工作

### 1.1 导入TextMeshPro

1. 打开Unity项目
2. 在任意场景中右键创建 `UI → Text - TextMeshPro`
3. 会弹出导入窗口，点击 **Import TMP Essentials**
4. 等待导入完成

### 1.2 创建UI场景

1. 在Project面板中，创建文件夹 `Assets/Scenes/`
2. 创建新场景：`MainGameScene`
3. 在场景中找到GameManager GameObject（如果没有，创建一个空GameObject并添加GameManager组件）

---

## 2. 创建主Canvas

### 2.1 创建Canvas

1. Hierarchy右键 → `UI → Canvas`
2. 重命名为 `MainCanvas`
3. 选中MainCanvas，在Inspector中设置：
   - **Canvas**:
     - Render Mode: `Screen Space - Overlay`
   - **Canvas Scaler**:
     - UI Scale Mode: `Scale With Screen Size`
     - Reference Resolution: `1920 x 1080`
     - Screen Match Mode: `Match Width Or Height`
     - Match: `0.5`

### 2.2 创建EventSystem

如果场景中没有EventSystem：
1. Hierarchy右键 → `UI → Event System`

---

## 3. 创建顶部信息栏

### 3.1 创建TopBar Panel

1. 在MainCanvas下创建空GameObject，命名为 `TopBar`
2. 添加 `Image` 组件（背景）
3. Rect Transform设置：
   - Anchor: 顶部拉伸（Top-Stretch）
   - Pos Y: `0`
   - Height: `80`

### 3.2 创建日期文本

1. 在TopBar下创建 `UI → Text - TextMeshPro`，命名为 `DateText`
2. Rect Transform:
   - Anchor: Left-Center
   - Pos X: `100`, Pos Y: `0`
   - Width: `200`, Height: `60`
3. TextMeshPro设置：
   - Text: `Day 1`
   - Font Size: `36`
   - Alignment: Middle-Left

### 3.3 创建时间段文本

1. 在TopBar下创建 `UI → Text - TextMeshPro`，命名为 `TimeslotText`
2. Rect Transform:
   - Anchor: Left-Center
   - Pos X: `320`, Pos Y: `0`
   - Width: `200`, Height: `60`
3. TextMeshPro设置：
   - Text: `上午`
   - Font Size: `32`
   - Alignment: Middle-Left

### 3.4 创建金钱文本

1. 在TopBar下创建 `UI → Text - TextMeshPro`，命名为 `MoneyText`
2. Rect Transform:
   - Anchor: Right-Center
   - Pos X: `-100`, Pos Y: `0`
   - Width: `200`, Height: `60`
3. TextMeshPro设置：
   - Text: `¥5000`
   - Font Size: `32`
   - Alignment: Middle-Right

### 3.5 创建快捷按钮容器

1. 在TopBar下创建空GameObject，命名为 `QuickButtons`
2. Rect Transform:
   - Anchor: Right-Center
   - Pos X: `-320`, Pos Y: `0`
   - Width: `400`, Height: `60`
3. 添加 `Horizontal Layout Group` 组件：
   - Spacing: `10`
   - Child Alignment: Middle-Center

### 3.6 创建快捷按钮

在QuickButtons下创建5个按钮，分别命名为：
- `MenuButton` (文本: "菜单")
- `CharacterButton` (文本: "角色")
- `MissionButton` (文本: "任务")
- `SaveButton` (文本: "保存")
- `LoadButton` (文本: "读取")

每个按钮：
1. 右键QuickButtons → `UI → Button - TextMeshPro`
2. 调整按钮大小（自动通过Layout Group控制）
3. 修改按钮内的Text文本

---

## 4. 创建EventSelectionUI

### 4.1 创建EventSelection Panel

1. 在MainCanvas下创建空GameObject，命名为 `EventSelectionPanel`
2. 添加 `Image` 组件
3. Rect Transform:
   - Anchor: 中心拉伸（Stretch-Stretch）
   - Left: `50`, Right: `50`
   - Top: `150`, Bottom: `50`

### 4.2 创建FilterContainer

1. 在EventSelectionPanel下创建空GameObject，命名为 `FilterContainer`
2. Rect Transform:
   - Anchor: 顶部拉伸（Top-Stretch）
   - Height: `60`
   - Top: `10`
3. 添加 `Horizontal Layout Group`：
   - Spacing: `10`
   - Padding: Left/Right: `20`

### 4.3 创建EventListContainer

1. 在EventSelectionPanel下创建 `UI → Scroll View`
2. 重命名为 `EventScrollView`
3. Rect Transform:
   - Anchor: 拉伸（Stretch-Stretch）
   - Top: `80`, Bottom: `10`
   - Left: `10`, Right: `400`
4. 找到 `Content` 子对象（在Viewport下）：
   - 添加 `Vertical Layout Group`
   - Spacing: `10`
   - Padding: `10`
   - 添加 `Content Size Fitter`：
     - Vertical Fit: `Preferred Size`

### 4.4 创建DetailPanel

1. 在EventSelectionPanel下创建空GameObject，命名为 `DetailPanel`
2. 添加 `Image` 组件
3. Rect Transform:
   - Anchor: Right-Stretch
   - Width: `380`
   - Right: `10`, Top: `80`, Bottom: `10`

在DetailPanel下创建以下文本组件（全部使用TextMeshPro）：
- `DetailNameText` (大标题)
- `DetailDescriptionText` (描述)
- `DetailEffectsText` (效果)
- `DetailRequirementsText` (需求)

### 4.5 创建确认/取消按钮

在DetailPanel底部创建：
- `ConfirmButton` (文本: "确认")
- `CancelButton` (文本: "取消")

### 4.6 创建EventButton预制体

1. 在EventScrollView的Content下创建 `UI → Button - TextMeshPro`
2. 命名为 `EventButton`
3. 设置尺寸：Width: `Auto`, Height: `80`
4. 调整布局，包含：
   - 图标 (Image)
   - 名称 (TextMeshPro)
   - 类型标签 (TextMeshPro)
5. 拖拽EventButton到Project面板，创建预制体：`Assets/Prefabs/UI/EventButton.prefab`
6. 删除场景中的EventButton实例

### 4.7 添加EventSelectionUI组件

1. 选中EventSelectionPanel
2. Add Component → `EventSelectionUI`
3. 配置引用：
   - Event List Container: `EventScrollView/Viewport/Content`
   - Event Button Prefab: 拖入EventButton预制体
   - Detail Panel: 拖入DetailPanel
   - Detail Name Text: 拖入DetailNameText
   - Detail Description Text: 拖入DetailDescriptionText
   - Detail Effects Text: 拖入DetailEffectsText
   - Detail Requirements Text: 拖入DetailRequirementsText
   - Filter Container: 拖入FilterContainer
   - Confirm Button: 拖入ConfirmButton
   - Cancel Button: 拖入CancelButton

---

## 5. 创建DialogueUI

### 5.1 创建Dialogue Panel

1. 在MainCanvas下创建空GameObject，命名为 `DialoguePanel`
2. 添加 `Image` 组件（半透明黑色背景）
3. Rect Transform:
   - Anchor: 拉伸（Stretch-Stretch）
   - 全0

### 5.2 创建背景图片（可选）

1. 在DialoguePanel下创建 `UI → Image`，命名为 `BackgroundImage`
2. Rect Transform: 全屏拉伸

### 5.3 创建角色立绘容器

1. 在DialoguePanel下创建空GameObject，命名为 `CharacterContainer`
2. Rect Transform:
   - Anchor: Center
   - Pos X: `-400`, Pos Y: `0`
   - Width: `600`, Height: `1080`
3. 在CharacterContainer下创建 `UI → Image`，命名为 `CharacterPortrait`
   - Rect Transform: 拉伸填充

### 5.4 创建对话框

1. 在DialoguePanel下创建空GameObject，命名为 `DialogueBox`
2. 添加 `Image` 组件
3. Rect Transform:
   - Anchor: 底部拉伸（Bottom-Stretch）
   - Height: `250`
   - Bottom: `30`, Left: `50`, Right: `50`

### 5.5 创建角色名字

1. 在DialogueBox下创建 `UI → Text - TextMeshPro`，命名为 `CharacterNameText`
2. Rect Transform:
   - Anchor: Top-Left
   - Pos X: `30`, Pos Y: `-30`
   - Width: `300`, Height: `50`
3. TextMeshPro设置：
   - Font Size: `32`
   - Font Style: Bold

### 5.6 创建对话文本

1. 在DialogueBox下创建 `UI → Text - TextMeshPro`，命名为 `DialogueText`
2. Rect Transform:
   - Anchor: 拉伸（Stretch-Stretch）
   - Left: `30`, Right: `30`, Top: `80`, Bottom: `30`
3. TextMeshPro设置：
   - Font Size: `28`
   - Overflow: `Overflow`

### 5.7 创建继续提示

1. 在DialogueBox下创建 `UI → Image`，命名为 `ContinueIndicator`
2. Rect Transform:
   - Anchor: Bottom-Right
   - Pos X: `-30`, Pos Y: `30`
   - Width: `40`, Height: `40`
3. 可以使用三角形或箭头图标

### 5.8 创建选择容器

1. 在DialoguePanel下创建空GameObject，命名为 `ChoiceContainer`
2. Rect Transform:
   - Anchor: Center
   - Pos Y: `100`
   - Width: `800`, Height: `400`
3. 添加 `Vertical Layout Group`:
   - Spacing: `20`
   - Child Alignment: Middle-Center

### 5.9 创建ChoiceButton预制体

1. 在ChoiceContainer下创建 `UI → Button - TextMeshPro`
2. 命名为 `ChoiceButton`
3. 设置尺寸：Width: `780`, Height: `80`
4. 拖拽到Project创建预制体：`Assets/Prefabs/UI/ChoiceButton.prefab`
5. 删除场景实例

### 5.10 创建控制按钮

在DialoguePanel右上角创建3个小按钮：
- `SkipButton` (文本: "跳过")
- `AutoButton` (文本: "自动")
- `HistoryButton` (文本: "历史")

### 5.11 添加DialogueUI组件

1. 选中DialoguePanel
2. Add Component → `DialogueUI`
3. 配置所有引用（按照组件上的Tooltip提示）

---

## 6. 创建MissionTrackerUI

### 6.1 创建Mission Panel

1. 在MainCanvas下创建空GameObject，命名为 `MissionPanel`
2. 添加 `Image` 组件
3. Rect Transform:
   - Anchor: Right-Stretch
   - Width: `500`
   - Right: `20`, Top: `120`, Bottom: `20`

### 6.2 创建标题

1. 在MissionPanel下创建 `UI → Text - TextMeshPro`，命名为 `TitleText`
2. 文本设置为 "任务列表"

### 6.3 创建MissionListContainer

1. 创建 `UI → Scroll View`
2. 配置Content的Vertical Layout Group

### 6.4 创建MissionItem预制体

1. 创建包含以下元素的按钮：
   - 任务名称 (TextMeshPro)
   - 进度文本 (TextMeshPro)
   - 截止日期 (TextMeshPro)
   - 进度条 (Slider)
2. 创建预制体：`Assets/Prefabs/UI/MissionItem.prefab`

### 6.5 创建任务详情面板

类似EventSelectionUI的DetailPanel

### 6.6 添加MissionTrackerUI组件

配置所有引用

---

## 7. 创建CharacterInfoUI

### 7.1 创建Character Panel

1. 在MainCanvas下创建空GameObject，命名为 `CharacterPanel`
2. Rect Transform: 左侧面板，类似MissionPanel但在左边

### 7.2 创建属性显示区域

创建4个属性条，每个包含：
- 属性名称 (TextMeshPro)
- 等级文本 (TextMeshPro)
- 进度条 (Slider)

排列：
- Knowledge
- Combat
- Charisma
- Courage

### 7.3 创建关系列表

1. 创建Scroll View
2. 配置RelationshipListContainer

### 7.4 创建RelationshipItem预制体

包含：
- 角色图标 (Image)
- 角色名称 (TextMeshPro)
- 关系等级 (TextMeshPro)
- 进度条 (Slider)

创建预制体：`Assets/Prefabs/UI/RelationshipItem.prefab`

### 7.5 创建角色详情面板

包含：
- 大立绘 (Image)
- 角色名称 (TextMeshPro)
- 角色描述 (TextMeshPro)
- 关系等级 (TextMeshPro)
- 关系进度条 (Slider)
- 解锁内容 (TextMeshPro, 支持滚动)

### 7.6 添加CharacterInfoUI组件

配置所有引用

---

## 8. 配置MainGameUI

### 8.1 创建通知系统

1. 在MainCanvas下创建空GameObject，命名为 `NotificationPanel`
2. Rect Transform:
   - Anchor: Top-Center
   - Pos Y: `-150`
   - Width: `600`, Height: `100`
3. 添加Image组件（半透明背景）
4. 在NotificationPanel下创建 `UI → Text - TextMeshPro`，命名为 `NotificationText`
   - 居中显示，较大字体

### 8.2 添加MainGameUI组件

1. 选中MainCanvas（或创建新的GameObject叫MainGameController）
2. Add Component → `MainGameUI`
3. 配置所有引用：

**UI组件引用**:
- Event Selection UI: 拖入EventSelectionPanel
- Dialogue UI: 拖入DialoguePanel
- Mission Tracker UI: 拖入MissionPanel
- Character Info UI: 拖入CharacterPanel

**顶部信息栏**:
- Date Text: 拖入DateText
- Timeslot Text: 拖入TimeslotText
- Money Text: 拖入MoneyText

**快捷按钮**:
- Menu Button: 拖入MenuButton
- Character Button: 拖入CharacterButton
- Mission Button: 拖入MissionButton
- Save Button: 拖入SaveButton
- Load Button: 拖入LoadButton

**通知系统**:
- Notification Text: 拖入NotificationText
- Notification Panel: 拖入NotificationPanel
- Notification Duration: `3`

### 8.3 初始可见性设置

在Scene中设置默认可见性：
- TopBar: ✅ 激活
- EventSelectionPanel: ❌ 未激活
- DialoguePanel: ❌ 未激活
- MissionPanel: ❌ 未激活
- CharacterPanel: ❌ 未激活
- NotificationPanel: ❌ 未激活

---

## 9. 测试UI

### 9.1 测试准备

1. 确保场景中有GameManager GameObject
2. GameManager上有所有必需的组件：
   - GameManager
   - TimeManager
   - AttributeManager
   - RelationshipManager
   - MissionManager
   - EventManager
   - DialogueManager
   - SaveManager
   - AudioManager

### 9.2 运行测试

1. 按Play运行游戏
2. 查看Console是否有错误
3. 测试快捷键：
   - **C** - 打开/关闭角色面板
   - **M** - 打开/关闭任务面板
   - **F5** - 保存
   - **F9** - 读取

### 9.3 常见问题

#### 问题1: NullReferenceException
- **原因**: UI引用未配置
- **解决**: 检查MainGameUI和各个UI组件的引用是否全部配置

#### 问题2: UI不显示
- **原因**: GameObject未激活或RectTransform配置错误
- **解决**: 检查GameObject的Active状态和RectTransform的Anchor/Position

#### 问题3: 按钮无响应
- **原因**: EventSystem缺失或Button组件未配置
- **解决**: 确保场景中有EventSystem，检查Button的OnClick事件

#### 问题4: TextMeshPro显示异常
- **原因**: TMP Essentials未导入
- **解决**: 重新导入TMP Essentials

---

## 10. 样式美化（可选）

### 10.1 创建UI Sprite

1. 创建文件夹 `Assets/UI/Sprites/`
2. 在Photoshop/GIMP中创建UI元素：
   - 按钮背景（normal/highlighted/pressed）
   - 面板背景
   - 进度条背景和填充
   - 图标
3. 导入Unity并设置Texture Type为 `Sprite (2D and UI)`

### 10.2 应用样式

1. 选中所有Panel，设置统一的背景色或Sprite
2. 选中所有Button，使用Sprite Swap过渡
3. 统一字体、颜色方案

### 10.3 动画效果（高级）

可以为UI添加动画：
- 面板淡入/淡出
- 按钮hover效果
- 通知滑入/滑出

使用Animation窗口或DOTween插件

---

## 11. 保存和组织

### 11.1 保存场景

1. File → Save Scene
2. 保存为 `Assets/Scenes/MainGameScene.unity`

### 11.2 创建预制体

将以下GameObject转为预制体：
- `Assets/Prefabs/UI/MainCanvas.prefab`（整个Canvas）
- 或者单独保存各个Panel

### 11.3 项目结构

```
Assets/
├── Prefabs/
│   └── UI/
│       ├── EventButton.prefab
│       ├── ChoiceButton.prefab
│       ├── MissionItem.prefab
│       ├── RelationshipItem.prefab
│       └── MainCanvas.prefab
├── Scenes/
│   └── MainGameScene.unity
├── UI/
│   └── Sprites/
│       ├── button_normal.png
│       ├── button_hover.png
│       ├── panel_bg.png
│       └── ...
└── Scripts/
    └── (已存在)
```

---

## 12. 下一步

UI搭建完成后，你可以：

1. **创建示例ScriptableObject数据** - 创建一些事件、对话、角色来测试UI
2. **美化UI** - 添加图片、调整颜色、字体
3. **添加音效** - 为按钮点击、对话显示添加音效
4. **继续开发其他系统** - 战斗系统、解谜系统等

---

## 附录：快速参考

### UI层级结构速览

```
MainCanvas
├── TopBar
│   ├── DateText
│   ├── TimeslotText
│   ├── MoneyText
│   └── QuickButtons
│       ├── MenuButton
│       ├── CharacterButton
│       ├── MissionButton
│       ├── SaveButton
│       └── LoadButton
├── EventSelectionPanel (EventSelectionUI)
│   ├── FilterContainer
│   ├── EventScrollView
│   │   └── Viewport
│   │       └── Content (放EventButton实例)
│   └── DetailPanel
│       ├── DetailNameText
│       ├── DetailDescriptionText
│       ├── DetailEffectsText
│       ├── DetailRequirementsText
│       ├── ConfirmButton
│       └── CancelButton
├── DialoguePanel (DialogueUI)
│   ├── BackgroundImage
│   ├── CharacterContainer
│   │   └── CharacterPortrait
│   ├── DialogueBox
│   │   ├── CharacterNameText
│   │   ├── DialogueText
│   │   └── ContinueIndicator
│   ├── ChoiceContainer (放ChoiceButton实例)
│   ├── SkipButton
│   ├── AutoButton
│   └── HistoryButton
├── MissionPanel (MissionTrackerUI)
│   ├── TitleText
│   ├── MissionScrollView
│   │   └── Content (放MissionItem实例)
│   └── DetailPanel
├── CharacterPanel (CharacterInfoUI)
│   ├── AttributeContainer
│   │   ├── Knowledge (名称+等级+进度条)
│   │   ├── Combat
│   │   ├── Charisma
│   │   └── Courage
│   ├── RelationshipScrollView
│   │   └── Content (放RelationshipItem实例)
│   └── DetailPanel
│       ├── CharacterPortrait
│       ├── CharacterNameText
│       ├── CharacterDescriptionText
│       ├── RelationshipLevelText
│       ├── RelationshipProgressSlider
│       └── UnlockContentText
└── NotificationPanel
    └── NotificationText
```

### 常用快捷键

- **C** - 角色信息
- **M** - 任务列表
- **F5** - 快速保存
- **F9** - 快速读取
- **ESC** - 菜单
- **Space** - 推进时间（测试用）
- **鼠标左键** - 继续对话

### 组件配置检查清单

- [ ] MainGameUI所有引用已配置
- [ ] EventSelectionUI所有引用已配置
- [ ] DialogueUI所有引用已配置
- [ ] MissionTrackerUI所有引用已配置
- [ ] CharacterInfoUI所有引用已配置
- [ ] 所有按钮OnClick事件已配置
- [ ] 所有预制体已创建
- [ ] EventSystem存在于场景中
- [ ] Canvas Scaler已正确配置

---

**版本**: 1.0
**最后更新**: 2025-11-07
**作者**: Claude (Anthropic)
