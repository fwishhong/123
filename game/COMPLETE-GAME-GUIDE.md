# MindLink 完整版游戏指南

## 🎮 版本说明

**complete-game-v2.html** - Day 1-100 完整体验版

### ✨ 已实现功能

#### 1. ✅ 完整战斗系统
- **回合制战斗UI**
  - 敌人HP条和动画
  - 玩家HP条和SP显示
  - 战斗日志实时更新
  - 技能按钮动态启用/禁用

- **战斗机制**
  - 普通攻击（无SP消耗）
  - 技能攻击（消耗SP，威力更强）
  - 使用道具（恢复HP）
  - 敌人AI自动回合

- **战斗奖励**
  - 经验值提升战斗力
  - 金钱奖励
  - 任务物品掉落

#### 2. ✅ 商店系统
- **可购买道具**
  - 🧪 恢复药剂 - $100 (恢复50HP)
  - ☕ 能量饮料 - $80 (恢复20SP)
  - 💊 攻击强化 - $150 (攻击力+50%)
  - 🛡️ 防御强化 - $150 (防御力+50%)
  - ✨ 复活药剂 - $500 (复活并恢复50%HP)

- **购买机制**
  - 实时金钱显示
  - 拥有数量统计
  - 购买成功/失败反馈

#### 3. ✅ 技能管理系统
- **技能列表**
  - ⚔️ 普通攻击 (SP: 0, 威力100%)
  - 💥 强力一击 (SP: 10, 威力150%, Lv2解锁)
  - ✨ 精神冲击 (SP: 20, 威力200%, Lv4解锁)
  - 🔍 分析弱点 (SP: 15, 降低防御, Lv3解锁)
  - 💚 认知修复 (SP: 25, 恢复30%HP, Lv5解锁)
  - 💫 认知觉醒 (SP: 50, 威力300%, Lv7解锁)

- **装备系统**
  - 最多装备4个技能
  - 拖动装备/卸下
  - 技能槽位可视化
  - 战斗力等级自动解锁新技能

#### 4. ✅ 视觉增强
- **角色立绘系统**
  - 👧 晓 - 青梅竹马
  - 👨‍💻 零 - 神秘黑客
  - 👩 美月 - 学生会长
  - 👨‍🔬 顾寒川 - 最终BOSS

- **动画效果**
  - 角色浮动动画
  - 敌人抖动效果
  - HP条平滑过渡
  - 按钮悬停效果

- **UI设计**
  - 赛博朋克主题配色
  - 霓虹蓝边框
  - 渐变背景
  - 半透明面板

#### 5. ✅ 音效系统（框架）
- **音效分类**
  - game_start - 游戏开始
  - dialogue - 对话推进
  - choice - 选择分支
  - battle_start - 战斗开始
  - attack - 普通攻击
  - skill - 技能释放
  - hit - 受到伤害
  - victory - 战斗胜利
  - defeat - 战斗失败
  - buy - 购买道具
  - equip - 装备技能
  - save/load - 存读档
  - activity - 执行活动
  - error - 错误提示

- **音效控制**
  - 右上角音效开关 🔊/🔇
  - 一键静音/取消静音
  - 状态持久化

#### 6. ✅ Day 1-100 完整剧情
- **Day 1-15** (已完整实现)
  - Day 1: 觉醒 - 获得世界线观测者能力
  - Day 2: 认知崩溃目击
  - Day 3: 教程战斗
  - Day 15: 第一次拯救任务

- **Day 16-35** (剧情数据已创建)
  - Day 16: 拯救成功的余波
  - Day 17: 零提供新情报
  - Day 18: 副会长出现症状
  - Day 19: 神秘声音揭示倒计时
  - Day 20: 战略选择
  - Day 25: 第二次任务 - 拯救副会长
  - Day 28: 零的姐姐真相
  - Day 30: 政府求助选择
  - Day 35: 第三次任务 - 掩护零潜入

- **Day 36-60** (剧情数据已创建)
  - Day 37: 真相揭露 - 收割计划
  - Day 45: 第四次任务 - 潜入地下实验室
  - Day 60: 第五次任务 - 拯救晓

- **Day 61-85** (剧情数据已创建)
  - Day 71: 神秘声音身份揭露
  - Day 80: 倒计时确认

- **Day 86-100** (剧情数据已创建)
  - Day 90: 顾寒川的真相
  - Day 96: 最终选择前夜
  - Day 99: 最终对决
  - Day 100: 结局（多重结局）

## 📖 游戏玩法

### 基础流程
1. 点击"开始游戏"触发Day 1剧情
2. 观看对话，做出选择影响好感度
3. 每天分为3个时间段：早晨/下午/夜晚
4. 选择活动提升属性
5. 特定日期触发剧情事件和战斗
6. Day 100达成结局

### 属性系统
- **知识** - 影响技能效果和剧情选择
- **勇气** - 影响战斗表现
- **魅力** - 影响角色好感度
- **战斗力** - 影响战斗HP和伤害，解锁新技能
- **金钱** - 购买道具和装备
- **SP** - 释放技能消耗，每天恢复

### 战斗技巧
1. **前期** (Lv1-3)
   - 多用普通攻击节省SP
   - 买几瓶恢复药剂备用
   - 优先提升战斗力

2. **中期** (Lv4-6)
   - 解锁强力技能后合理使用
   - 装备恢复技能应急
   - 购买强化道具

3. **后期** (Lv7+)
   - 解锁终极技能
   - 技能组合搭配
   - 准备充足道具挑战BOSS

### 好感度系统
- **晓** - 选择诚实，保护她
- **零** - 帮助调查，支持行动
- **美月** - 接受任务，理解立场

高好感度解锁专属剧情和结局

## 🎯 任务攻略

### Day 1-15: 第一章
**目标**: 完成教程，救出第一个同学

**推荐路线**:
- Day 1-3: 多训练战斗力，至少达到Lv2
- Day 4-10: 平衡发展各项属性
- Day 11-14: 准备道具，战斗力提升到Lv3
- Day 15: 挑战第一个BOSS

**必需条件**:
- 战斗力 Lv2+
- 至少1瓶恢复药剂

### Day 16-35: 第二章
**目标**: 拯救副会长，选择战略路线

**推荐路线**:
- Day 16-20: 提升战斗力到Lv4
- Day 21-24: 准备第二次任务
- Day 25: 拯救副会长（战斗力Lv3+, 知识Lv3+）
- Day 26-35: 准备第三次任务

**关键选择**:
- Day 18: 答应帮助美月（好感度+3）
- Day 20: 选择战略路线（影响后续剧情）

### Day 36-60: 第三章
**目标**: 揭露真相，救出晓

**推荐路线**:
- Day 36-44: 战斗力提升到Lv6
- Day 45: 第四次任务（战斗力Lv5+）
- Day 46-59: 全力提升属性
- Day 60: 拯救晓（战斗力Lv6+, 魅力Lv5+）

### Day 61-85: 第四章
**目标**: 集结力量，准备决战

**推荐路线**:
- Day 61-70: 战斗力提升到Lv8
- Day 71: 获得终极能力
- Day 76-85: 全属性提升到Lv8+

### Day 86-100: 终章
**目标**: 最终决战，达成结局

**推荐准备**:
- 所有属性Lv8+
- 装备4个强力技能
- 至少3瓶恢复药剂
- 至少2瓶强化道具
- 充足金钱（1000+）

**结局条件**:
- True Ending: 所有属性Lv10, 所有角色好感度Lv10
- Good Ending: 战斗力Lv8+, 主要角色好感度Lv8+
- Normal Ending: 达成最低要求
- Bad Ending: 失败
- Hidden Ending: 发现世界线秘密（需特定选择）

## 🛠️ 技术说明

### 文件结构
```
game/
├── complete-game-v2.html          # 完整游戏（推荐）
├── day16-100-dialogues.js         # Day 16-100剧情数据
├── demo-day1-15.html              # Day 1-15演示版
├── index-standalone.html          # 基础独立版
└── COMPLETE-GAME-GUIDE.md         # 本指南
```

### 如何运行

**方法1: 直接双击（推荐）**
1. 找到 `complete-game-v2.html`
2. 双击打开
3. 开始游戏

**方法2: 本地服务器**
```bash
# 进入game目录
cd game

# 启动简单服务器
python -m http.server 8000

# 浏览器访问
http://localhost:8000/complete-game-v2.html
```

### 存档系统
- 自动保存到浏览器 localStorage
- 点击"保存"手动存档
- 点击"读取"读取存档
- 支持跨会话保存

### 音效集成（可选）

要添加真实音效，创建 `sounds/` 目录并添加音频文件：

```
game/
└── sounds/
    ├── game_start.mp3
    ├── dialogue.mp3
    ├── battle_start.mp3
    ├── attack.mp3
    ├── skill.mp3
    ├── victory.mp3
    └── ...
```

然后取消注释 `complete-game-v2.html` 中的音频代码：
```javascript
playSound(soundId) {
    if (!this.soundEnabled) return;
    const audio = new Audio(`sounds/${soundId}.mp3`);
    audio.play().catch(e => console.log('Audio play failed:', e));
}
```

## 📊 数据集成

### 添加Day 16-100剧情

在 `complete-game-v2.html` 中添加剧情数据:

```html
<!-- 在 </body> 前添加 -->
<script src="day16-100-dialogues.js"></script>

<!-- 然后在游戏数据中合并 -->
<script>
// 在 GameData.dialogues 定义后添加
if (typeof day16to100Dialogues !== 'undefined') {
    Object.assign(GameData.dialogues, day16to100Dialogues);
}
</script>
```

### 敌人数据扩展

在 `GameData.enemies` 中添加更多敌人：

```javascript
enemies: {
    'shadow': { id: 'shadow', name: '阴影', sprite: '👤', maxHP: 50, attack: 10, exp: 20 },
    'nightmare': { id: 'nightmare', name: '噩梦', sprite: '😈', maxHP: 80, attack: 15, exp: 40 },
    'guardian': { id: 'guardian', name: '守卫者', sprite: '🤖', maxHP: 150, attack: 25, exp: 100 },
    'hunter': { id: 'hunter', name: '认知猎人', sprite: '🦾', maxHP: 200, attack: 35, exp: 200 },
    'boss_gu': { id: 'boss_gu', name: '顾寒川', sprite: '👨‍🔬', maxHP: 500, attack: 50, exp: 1000 }
}
```

## 🎨 自定义

### 修改配色
在CSS中调整颜色变量：
```css
:root {
    --primary-bg: #0a0e27;      /* 主背景 */
    --secondary-bg: #151932;    /* 次背景 */
    --accent-blue: #00d4ff;     /* 强调色 */
    --accent-purple: #9d4edd;   /* 紫色 */
    --accent-pink: #ff006e;     /* 粉色 */
}
```

### 添加新技能
```javascript
GameData.skills['new_skill'] = {
    id: 'new_skill',
    name: '新技能',
    icon: '🌟',
    cost: 30,
    power: 2.5,
    desc: '描述',
    unlockLevel: 6
};
```

### 添加新道具
```javascript
GameData.items['new_item'] = {
    id: 'new_item',
    name: '新道具',
    icon: '🎁',
    price: 200,
    effect: 'heal',
    value: 100,
    desc: '描述'
};
```

## 🐛 已知问题

1. **Day 16-100剧情需要手动集成**
   - 剧情数据已创建在 `day16-100-dialogues.js`
   - 需要在HTML中引入并合并到 GameData

2. **音效为占位符**
   - 当前只有控制台输出
   - 需要添加真实音频文件

3. **角色立绘使用emoji**
   - 可以替换为真实图片
   - 修改 `.dialogue-character` 样式

## 📝 更新日志

### v2.0 (当前版本)
- ✅ 完整战斗系统UI
- ✅ 商店系统
- ✅ 技能管理UI
- ✅ 视觉增强（角色立绘、动画）
- ✅ 音效系统框架
- ✅ Day 1-15完整剧情
- ✅ Day 16-100剧情数据
- ✅ 存档系统
- ✅ 底部导航栏
- ✅ 通知系统

### v1.0
- ✅ 基础游戏框架
- ✅ 对话系统
- ✅ 时间管理
- ✅ 属性系统

## 🎮 快速开始

1. 打开 `complete-game-v2.html`
2. 点击"开始游戏"
3. 体验Day 1-15完整剧情
4. 提升属性，解锁技能
5. 挑战战斗，购买道具
6. 培养角色好感度
7. 朝着Day 100进发！

## 💡 提示

- 经常保存游戏
- 战斗前确保HP和道具充足
- 平衡发展各项属性
- 注意角色好感度对剧情的影响
- 多尝试不同选择，探索分支剧情
- 战斗力每提升2级可解锁新技能

祝你在MindLink的世界中玩得愉快！🎮✨
