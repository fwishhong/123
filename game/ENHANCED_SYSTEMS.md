# 增强系统 - 方案A+C实现

## 📦 新增内容总览

### ✅ 已完成的新系统

1. **对话系统 (Dialogue System)** - 视觉小说风格
2. **技能系统 (Skill System)** - 战斗技能和SP管理
3. **道具系统 (Inventory System)** - 背包、商店、金钱
4. **剧情内容** - Day 1-3 完整剧情
5. **扩展活动** - 20+ 种活动
6. **任务详情** - 第一个完整任务设计

---

## 🎯 系统详解

### 1. 对话系统 (DialogueManager)

**位置**: `js/managers/DialogueManager.js`

**功能**:
- ✅ 视觉小说风格的对话显示
- ✅ 角色立绘位置管理
- ✅ 选项分支系统
- ✅ 对话历史记录
- ✅ 自动触发和条件触发

**使用示例**:
```javascript
// 开始对话
const dialogue = dialogueManager.startDialogue(dialogueData);

// 前进到下一句
const result = dialogueManager.advance();

// 做出选择
const choiceResult = dialogueManager.makeChoice(0);

// 结束对话并获取奖励
const rewards = dialogueManager.endDialogue();
```

**对话数据格式**:
```javascript
{
    id: 'day1_morning',
    title: 'Day 1 - 觉醒',
    location: '卧室',
    autoTrigger: true,
    conditions: { day: 1, period: 0 },
    dialogue: [
        {
            speaker: 'player',
            text: '对话内容...',
            image: 'character_emotion',
            choices: [
                {
                    text: '选项1',
                    next: 'branch_name',
                    relationshipChange: 2
                }
            ]
        }
    ],
    branches: {
        'branch_name': [
            // 分支对话...
        ]
    },
    rewards: {
        flags: ['flag_name'],
        items: ['item_id'],
        money: 100
    }
}
```

---

### 2. 技能系统 (SkillManager)

**位置**: `js/managers/SkillManager.js`

**功能**:
- ✅ 技能解锁系统（基于战斗力等级）
- ✅ 技能装备管理（最多4个）
- ✅ SP（技能点）管理
- ✅ 多种技能类型（物理/精神/辅助/终极）

**已定义技能** (6个):
| 技能 | 类型 | SP消耗 | 解锁等级 | 效果 |
|------|------|--------|----------|------|
| 普通攻击 | 物理 | 0 | 初始 | 100%伤害 |
| 强力一击 | 物理 | 10 | Lv3 | 150%伤害 |
| 精神冲击 | 精神 | 20 | Lv5 | 200%伤害 |
| 分析弱点 | 辅助 | 15 | Lv4 | 降低防御 |
| 认知修复 | 辅助 | 25 | Lv6 | 恢复30%HP |
| 认知觉醒 | 终极 | 50 | Lv8 | 300%伤害 |

**使用示例**:
```javascript
// 检查并解锁新技能
const newSkills = skillManager.checkUnlocks(combatLevel);

// 装备技能
skillManager.equipSkill('skill_power_strike');

// 战斗中使用技能
const result = skillManager.useSkill('skill_power_strike', target);

// 恢复SP
skillManager.restoreSP(20);
```

---

### 3. 道具系统 (InventoryManager)

**位置**: `js/managers/InventoryManager.js`

**功能**:
- ✅ 背包管理（最多50种道具）
- ✅ 金钱系统
- ✅ 商店系统（买卖道具）
- ✅ 道具使用（消耗品、装备等）

**已定义道具** (5个):
| 道具 | 类型 | 效果 | 价格 |
|------|------|------|------|
| 🧪 恢复药剂 | 消耗品 | 恢复50HP | 100 |
| ☕ 能量饮料 | 消耗品 | 恢复20SP | 80 |
| 💊 攻击强化剂 | 消耗品 | 攻击+50% | 150 |
| 🛡️ 防御强化剂 | 消耗品 | 防御+50% | 150 |
| ✨ 复活药剂 | 消耗品 | 自动复活50%HP | 500 |

**使用示例**:
```javascript
// 添加道具
inventoryManager.addItem('item_heal_potion', 3);

// 使用道具
const result = inventoryManager.useItem('item_heal_potion', target);

// 购买道具
const buyResult = inventoryManager.buyItem('item_energy_drink', 5);

// 卖出道具
const sellResult = inventoryManager.sellItem('item_heal_potion', 1);

// 检查金钱
const money = inventoryManager.getMoney();
```

---

### 4. 剧情内容

**位置**: `js/data/enhancedGameData.js`

**已创建剧情事件**:

#### Day 1 - 觉醒
- ✅ `day1_morning` - 早晨觉醒，发现能力
- ✅ `day1_meet_akatsuki` - 遇见晓，可选择坦白或隐瞒

#### Day 2 - 目击
- ✅ `day2_witness_collapse` - 目击认知崩溃事件
- ✅ 神秘声音引导

#### Day 3 - 教程战斗
- ✅ `day3_tutorial_battle` - 第一次进入认知空间
- ✅ 教程战斗：击败阴影

#### 角色事件
- ✅ `akatsuki_event_lv2` - 晓的担心（好感度Lv2）
- ✅ `akatsuki_event_lv4` - 晓的过去（好感度Lv4，揭示背景故事）

**剧情特点**:
- 📝 约3000字Day 1-3剧情
- 🎭 多个选项分支
- 💕 影响角色好感度
- 🎁 解锁新活动和能力

---

### 5. 扩展活动

**位置**: `js/data/enhancedGameData.js` - `expandedActivities`

**新增活动** (18+个):

#### 早晨 (Morning)
- 🧘 早晨冥想 - 提升知识+战斗力
- 📰 阅读新闻 - 了解世界观
- 🍳 和晓一起吃早餐 - 提升魅力+晓好感度

#### 下午 (Afternoon)
- 📚 图书馆调查 - 深入研究认知空间
- 🏛️ 参加学生会 - 提升魅力+知识
- ⚔️ 战斗模拟训练 - 大幅提升战斗力（花费50）
- 💼 打工 - 赚钱（+200）
- 💻 黑客课程 - 零教授黑客技术

#### 夜晚 (Night)
- 🌙 认知空间探索 - 提升战斗力+知识
- 🦇 夜间巡逻 - 提升勇气+战斗力
- 🕵️ 情报收集 - 从地下渠道获取情报（花费100）
- 💕 约会（晓）- 提升魅力+晓好感度
- 🏋️ 深夜特训 - 大幅提升战斗力+勇气

#### 特殊
- 🛒 访问商店 - 购买道具
- 💬 角色交谈 - 提升好感度

**活动特性**:
- 📅 时间段限制
- 🔒 条件解锁（属性、好感度、标志）
- 💰 金钱成本/收入
- 🎯 剧情提示

---

### 6. 第一个完整任务

**任务**: `mission_01` - "第一次拯救"

**详情**:
- 📅 截止日期: Day 15
- 🔓 解锁日期: Day 5
- 📋 要求: 战斗力Lv2 + 完成教程

**地下城结构**:
```
扭曲的教室
├─ 第1层: 阴影怪物 (HP 50, ATK 10)
├─ 第2层: 阴影怪物 (HP 50, ATK 10)
├─ 第3层: 噩梦实体 (HP 80, ATK 15)
└─ BOSS: 弱化守卫 (HP 120, ATK 20)
```

**奖励**:
- 💰 1000金钱
- 🎁 恢复药剂 x1
- 🎁 能量饮料 x1
- 🎁 攻击强化剂 x1
- 💕 晓好感度 +2

**剧情**:
- ✅ 任务前对话 - 与晓的约定
- ✅ 任务后对话 - 拯救成功

---

## 🔧 集成指南

### 如何在现有游戏中使用这些系统

#### 步骤1: 导入新管理器

```javascript
import { DialogueManager } from './managers/DialogueManager.js';
import { SkillManager } from './managers/SkillManager.js';
import { InventoryManager } from './managers/InventoryManager.js';
import {
    skills,
    items,
    dialogueEvents,
    expandedActivities
} from './data/enhancedGameData.js';
```

#### 步骤2: 初始化管理器

```javascript
// 在GameManager.init()中添加
this.managers.dialogue = new DialogueManager();
this.managers.skills = new SkillManager();
this.managers.inventory = new InventoryManager();

this.managers.dialogue.init();
this.managers.skills.init(skills);
this.managers.inventory.init(items);
```

#### 步骤3: 更新BattleManager

在战斗系统中集成技能和道具：

```javascript
// 添加技能选择UI
showSkillMenu() {
    const skills = this.managers.skills.getEquippedSkills();
    // 显示技能列表
}

// 使用技能
useSkill(skillId) {
    const result = this.managers.skills.useSkill(skillId, {
        baseAttack: this.playerStats.attack,
        maxHP: this.playerStats.maxHP
    });

    if (result.success && result.damage) {
        this.currentEnemy.currentHP -= result.damage;
    }
}

// 使用道具
useItem(itemId) {
    const result = this.managers.inventory.useItem(itemId, this.playerStats);

    if (result.success && result.healing) {
        this.playerStats.currentHP += result.healing;
    }
}
```

#### 步骤4: 添加对话UI

```javascript
// 在UIManager中添加对话显示方法
showDialogue(dialogueData) {
    // 创建对话框UI
    const dialogueBox = document.createElement('div');
    dialogueBox.className = 'dialogue-box';

    // 显示当前对话
    const line = this.managers.dialogue.getCurrentLine();
    dialogueBox.innerHTML = `
        <div class="speaker">${line.speaker || 'System'}</div>
        <div class="text">${line.text}</div>
    `;

    // 如果有选项，显示选项按钮
    if (line.choices) {
        // 添加选项按钮
    }
}
```

#### 步骤5: 检查对话触发

```javascript
// 在时间推进后检查对话触发
checkDialogueTriggers() {
    const time = this.managers.time.getCurrentTime();
    const flags = this.managers.events.activeFlags;
    const rels = this.managers.relationships.getAll();

    for (const [id, dialogue] of Object.entries(dialogueEvents)) {
        if (this.managers.dialogue.checkTrigger(id, time.day, time.period, flags, rels)) {
            this.managers.dialogue.startDialogue(dialogue);
            break;
        }
    }
}
```

---

## 📊 数据统计

### 当前内容量

| 类型 | 数量 | 总字数/行数 |
|------|------|------------|
| 对话事件 | 6个 | ~3000字 |
| 技能 | 6个 | - |
| 道具 | 5个 | - |
| 活动 | 20+个 | - |
| 任务 | 1个完整 | ~500字 |

### 估算完整Day 1-15内容

要完成完整的Day 1-15垂直切片，还需要：

- [ ] Day 4-14 日常事件（约10个）
- [ ] 更多角色互动事件（零、美月等）
- [ ] Day 15任务的完整地下城实现
- [ ] 更多战斗敌人变种
- [ ] 商店UI实现
- [ ] 技能/道具UI实现

**估算工作量**:
- 对话内容: 再增加~5000字
- 代码实现: 需要UI增强
- 测试调试: 2-3小时

---

## 🎨 UI需求

### 需要添加的UI元素

#### 1. 对话框 (Dialogue Box)
```css
.dialogue-box {
    position: fixed;
    bottom: 20px;
    left: 50%;
    transform: translateX(-50%);
    width: 80%;
    max-width: 800px;
    background: rgba(21, 25, 50, 0.95);
    border: 2px solid var(--accent-blue);
    padding: 20px;
}

.dialogue-speaker {
    color: var(--accent-blue);
    font-size: 1.2rem;
    margin-bottom: 10px;
}

.dialogue-text {
    color: var(--text-primary);
    line-height: 1.8;
    margin-bottom: 15px;
}

.dialogue-choices {
    display: flex;
    flex-direction: column;
    gap: 10px;
}
```

#### 2. 技能选择UI
- 显示已装备的4个技能
- 显示当前SP
- 技能图标和名称
- SP消耗提示

#### 3. 背包UI
- 网格布局显示道具
- 道具图标、名称、数量
- 使用/丢弃按钮
- 金钱显示

#### 4. 商店UI
- 可购买道具列表
- 价格显示
- 购买/卖出按钮
- 金钱余额

---

## 🚀 下一步建议

### 优先级1：核心功能集成
1. ✅ 创建对话系统
2. ✅ 创建技能系统
3. ✅ 创建道具系统
4. ⏳ 更新UI支持新系统
5. ⏳ 集成到主游戏循环

### 优先级2：内容补充
1. ⏳ 完成Day 4-14剧情
2. ⏳ 添加更多角色事件
3. ⏳ 实现第一个完整地下城

### 优先级3：系统优化
1. ⏳ 添加动画效果
2. ⏳ 音效集成
3. ⏳ 性能优化

---

## 💡 使用建议

### 对于开发者

**如果想快速测试**:
```javascript
// 在控制台中测试新系统
const dm = new DialogueManager();
const sm = new SkillManager();
const im = new InventoryManager();

// 测试对话
dm.init();
dm.startDialogue(dialogueEvents.day1_morning);

// 测试技能
sm.init(skills);
sm.unlockSkill('skill_power_strike');
sm.equipSkill('skill_power_strike');

// 测试道具
im.init(items);
im.addMoney(1000);
im.buyItem('item_heal_potion', 5);
```

**如果想完整集成**:
1. 参考上面的"集成指南"
2. 更新`index-standalone.html`或模块化版本
3. 添加对应的UI元素
4. 连接事件触发逻辑

### 对于内容创作者

**添加新对话**:
1. 在`enhancedGameData.js`的`dialogueEvents`中添加
2. 遵循现有格式
3. 设置触发条件
4. 定义奖励

**添加新技能**:
1. 在`skills`数组中添加
2. 设置解锁等级
3. 定义效果和消耗

**添加新道具**:
1. 在`items`数组中添加
2. 设置价格和效果
3. 选择道具类型

---

## 📝 总结

已完成的系统为游戏提供了：
- ✅ **深度剧情**: 视觉小说风格的叙事
- ✅ **策略战斗**: 技能选择和资源管理
- ✅ **经济系统**: 金钱、道具、商店
- ✅ **内容框架**: Day 1-3完整体验

这些系统已经可以独立运行和测试，只需要UI集成就能完全发挥作用！

---

**文件位置**:
- `js/managers/DialogueManager.js` - 对话管理器
- `js/managers/SkillManager.js` - 技能管理器
- `js/managers/InventoryManager.js` - 背包管理器
- `js/data/enhancedGameData.js` - 扩展游戏数据

**创建日期**: 2025-11-12
**状态**: ✅ 核心系统完成，等待UI集成
