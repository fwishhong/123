# Unity 快速设置指南

从蓝屏到可玩游戏的完整步骤！

---

## ✅ 你已完成的步骤

- [x] 创建Unity项目
- [x] 导入Scripts文件夹
- [x] 创建GameManager GameObject
- [x] 添加GameManager组件
- [x] 运行游戏（看到蓝屏）

---

## 🎮 下一步：添加测试脚本

### 步骤1：添加GameTester组件

1. **停止游戏**（如果正在运行）
2. 在Hierarchy中选中 `GameManager` GameObject
3. 在Inspector面板中，点击 **Add Component**
4. 搜索 `GameTester`
5. 点击添加

✅ 你会看到GameTester组件出现在Inspector中，带有各种测试选项

### 步骤2：运行测试

1. **点击播放按钮▶️**
2. **打开Console标签**（Window > General > Console）
3. 你会看到一大堆测试日志！

预期输出：
```
=== GameTester Started ===
[GameManager] Initializing all managers...
[TimeManager] Initialized. Current Day: 1, Timeslot: Morning
...
========== 开始测试 ==========

--- 测试：游戏启动 ---
✓ 游戏已启动

--- 测试：时间系统 ---
当前时间：Day 1 - 早晨
✓ 时间系统正常

--- 测试：属性系统 ---
知识：Lv1
战斗：Lv2  (升级了!)
✓ 属性系统正常
...
```

✅ 如果看到这些，说明所有系统正常！

### 步骤3：测试按键控制

游戏运行时，按以下按键：

| 按键 | 功能 |
|------|------|
| **空格** | 推进时间 |
| **K** | 增加知识经验 +25 |
| **C** | 增加战斗经验 +25 |
| **A** | 增加晓的好感度 +20 |
| **S** | 保存游戏 |
| **L** | 加载游戏 |
| **I** | 打印完整状态信息 |

在Console中会看到实时反馈！

---

## 🎨 可选：添加简单UI显示

如果你想看到可视化的UI而不只是Console日志：

### 步骤1：安装TextMeshPro

1. Unity会提示你导入TMP Essentials
2. 如果没有提示，去 **Window > TextMeshPro > Import TMP Essential Resources**

### 步骤2：创建Canvas

1. Hierarchy右键 > **UI > Canvas**
2. Canvas会自动创建（包含EventSystem）

### 步骤3：创建UI文本

在Canvas下创建5个TextMeshPro文本：

#### 时间显示（左上角）
1. Canvas右键 > **UI > Text - TextMeshPro**
2. 命名为 `TimeText`
3. 设置Rect Transform：
   - Anchor Presets: 点击左上角预设
   - Pos X: 20, Pos Y: -20
   - Width: 200, Height: 100
4. 设置Text组件：
   - Font Size: 18
   - Alignment: Left Top

#### 属性显示
1. 创建 `AttributesText`
2. Rect Transform：
   - Anchor: 左上
   - Pos X: 20, Pos Y: -140
   - Width: 200, Height: 120

#### 关系显示
1. 创建 `RelationshipsText`
2. Rect Transform：
   - Anchor: 左上
   - Pos X: 20, Pos Y: -280
   - Width: 200, Height: 100

#### 任务显示
1. 创建 `MissionsText`
2. Rect Transform：
   - Anchor: 左上
   - Pos X: 20, Pos Y: -400
   - Width: 250, Height: 150

#### 控制说明（右下角）
1. 创建 `ControlsText`
2. Rect Transform：
   - Anchor: 右下
   - Pos X: -20, Pos Y: 20
   - Width: 250, Height: 200
3. Text:
   - Font Size: 14
   - Alignment: Right Bottom

### 步骤4：添加SimpleGameUI组件

1. 在Canvas上 **Add Component**
2. 搜索 `SimpleGameUI`
3. 在Inspector中，将对应的Text拖到相应字段：
   - Time Text → TimeText
   - Attributes Text → AttributesText
   - Relationships Text → RelationshipsText
   - Missions Text → MissionsText
   - Controls Text → ControlsText

### 步骤5：运行游戏

现在你应该看到：
- 左侧显示时间、属性、关系、任务
- 右下角显示控制键说明
- 按空格键会实时更新显示！

---

## 🎯 测试清单

运行游戏后，逐一测试：

### ✅ 时间系统
- [ ] 按空格键，看Day和时间段变化
- [ ] Morning → Afternoon → Night → Day 2
- [ ] Console显示时间推进日志

### ✅ 属性系统
- [ ] 按K键5次，知识应该升到Lv2
- [ ] 按C键5次，战斗应该升到Lv2
- [ ] Console显示升级通知

### ✅ 关系系统
- [ ] 按A键5次，晓的好感度应该提升
- [ ] Console显示好感度增加

### ✅ 任务系统
- [ ] 在Console中找到"测试任务：拯救美月"
- [ ] 显示进度和剩余天数
- [ ] 任务完成度应该在UI中显示

### ✅ 存档系统
- [ ] 按S键保存游戏
- [ ] Console显示"保存成功"
- [ ] 修改一些数据（按K、C、空格等）
- [ ] 按L键加载
- [ ] 数据应该恢复到保存时的状态

---

## 🐛 常见问题

### Q1: Console没有任何输出
**A**:
- 确保GameManager上有GameTester组件
- 确保Console的日志过滤没有关闭（Info/Warning/Error都要勾选）

### Q2: 看到很多NullReferenceException错误
**A**:
- 停止游戏
- 在GameManager的Inspector中，确保GameManager组件在最上面
- 重新运行

### Q3: UI不显示或显示错误
**A**:
- 确保导入了TextMeshPro
- 确保SimpleGameUI组件的所有Text字段都已连接
- 检查Canvas的Render Mode是Screen Space - Overlay

### Q4: 按键没反应
**A**:
- 确保Game窗口是焦点（点击一下Game标签）
- 检查Console是否有"[手动操作]"的日志输出

---

## 📊 预期结果

完成所有设置后，你应该看到：

```
游戏画面：
┌─────────────────────────┐
│ 时间                     │
│ Day 1 / 100             │
│ 早晨                     │
│ 星期一                   │
│                         │
│ 属性                     │
│ 📚 知识: Lv1            │
│ ⚔️ 勇气: Lv1            │
│ ✨ 魅力: Lv1            │
│ ⚡ 战斗: Lv1            │
│                         │
│ 关系                     │
│ 晓: Lv1                 │
│ 零: Lv1                 │
│ 美月: Lv1               │
│                         │
│ 任务                     │
│ 活跃: 1                 │
│ 测试任务：拯救美月        │  控制键
│ 剩余: 14 天              │  空格 - 推进时间
│ 进度: 33%                │  K - 增加知识
└─────────────────────────┘  ...
```

---

## 🎉 成功！你现在可以：

✅ 看到游戏系统正常运行
✅ 通过按键控制游戏
✅ 实时查看游戏状态
✅ 保存和加载游戏
✅ 验证所有Manager工作正常

---

## 🚀 下一步

系统验证完成后，我可以继续开发：

1. **完整的UI系统**（漂亮的界面，而不是简单文本）
2. **事件和对话系统**（ScriptableObject数据）
3. **战斗系统**
4. **解谜系统**

告诉我你想先做哪个！或者如果有任何问题，随时问我。

---

**需要帮助？** 把Console的错误信息告诉我，我会帮你解决！
