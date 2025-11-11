# MindLink - 认知空间 (Web版)

基于完整GDD的可玩Web游戏实现

## 🎮 如何运行

### 方法1: 直接打开（推荐）
```bash
# 在项目根目录下
cd game
# 直接用浏览器打开 index.html
open index.html  # macOS
xdg-open index.html  # Linux
start index.html  # Windows
```

### 方法2: 使用本地服务器
```bash
# 使用Python
cd game
python3 -m http.server 8000

# 使用Node.js (需要先安装 http-server)
npm install -g http-server
cd game
http-server -p 8000

# 然后在浏览器访问: http://localhost:8000
```

## 🎯 游戏特性

### 核心机制
- ✅ **时间管理系统**: 100天，每天3个时间段（早晨/下午/夜晚）
- ✅ **属性系统**: 知识、勇气、魅力、战斗力 (等级1-10)
- ✅ **好感度系统**: 8个角色，等级0-10，带衰减机制
- ✅ **活动系统**: 10种活动，条件触发，属性提升
- ✅ **任务系统**: 6个主线任务，带截止日期
- ✅ **战斗系统**: 回合制战斗，4种行动（攻击/技能/防御/道具）
- ✅ **存档系统**: 使用LocalStorage自动保存

### UI界面
- 📅 **活动视图**: 选择并执行活动
- 👥 **关系视图**: 查看角色好感度
- 📆 **日程视图**: 日历和截止日期提醒
- ⚔️ **战斗视图**: 回合制战斗界面
- 💾 **菜单系统**: 保存/加载/设置

### 美术风格
- 🌌 赛博朋克/科幻主题
- 🎨 蓝紫色调配色方案
- ✨ 霓虹光效和动画
- 📱 响应式设计，支持移动端

## 🎲 游戏玩法

### 第一步: 开始游戏
1. 点击"开始游戏"进入新游戏
2. 或点击"继续游戏"加载存档

### 第二步: 选择活动
- 每个时间段可以选择一个活动
- 不同活动提升不同属性
- 某些活动需要属性达到要求才能解锁

### 活动示例
| 活动 | 时间段 | 效果 |
|------|--------|------|
| 图书馆学习 | 下午 | +5 知识 |
| 战斗训练 | 下午 | +5 战斗力 |
| 社交活动 | 夜晚 | +5 魅力 |
| 冒险活动 | 夜晚 | +5 勇气 |
| 晨跑 | 早晨 | +3 战斗力, +2 勇气 |

### 第三步: 完成任务
- 查看日程表，注意任务截止日期
- 在截止日期前完成任务
- 任务需要特定属性等级

### 主线任务
| 任务 | 截止日 | 要求 |
|------|--------|------|
| 第一次拯救 | Day 15 | 战斗力 Lv2 |
| 学生会的危机 | Day 25 | 知识 Lv3, 战斗力 Lv3 |
| 教师的秘密 | Day 35 | 知识 Lv4, 勇气 Lv3 |
| 潜入NeuroCorp | Day 45 | 战斗力 Lv5, 勇气 Lv4, 知识 Lv4 |
| 阻止叛徒 | Day 70 | 战斗力 Lv6, 勇气 Lv5 |
| 最终决战 | Day 100 | 所有属性高等级 |

### 第四步: 战斗
- 进入认知空间后会触发战斗
- 选择行动：攻击/技能/防御/道具
- 击败敌人完成任务

### 结局系统
游戏到Day 100后根据以下条件判定结局：
- **真结局**: 完成所有6个任务 + 总好感度≥50
- **好结局**: 完成4+个任务
- **普通结局**: 完成2-3个任务
- **坏结局**: 完成少于2个任务

## 🛠️ 技术架构

### 文件结构
```
game/
├── index.html              # 主HTML文件
├── css/
│   └── main.css           # 完整样式表 (600+ 行)
├── js/
│   ├── main.js            # 应用入口
│   ├── managers/          # 管理器系统
│   │   ├── GameManager.js
│   │   ├── TimeManager.js
│   │   ├── AttributeManager.js
│   │   ├── RelationshipManager.js
│   │   ├── EventManager.js
│   │   └── BattleManager.js
│   ├── ui/
│   │   └── UIManager.js   # UI控制器
│   └── data/
│       └── gameData.js    # 游戏数据
└── README.md              # 本文件
```

### 技术栈
- **前端**: 纯HTML5 + CSS3 + JavaScript (ES6+)
- **架构**: 模块化管理器系统
- **存储**: LocalStorage
- **框架**: 无（原生实现）

### 核心类
```javascript
GameManager        // 游戏主控制器
TimeManager        // 时间系统
AttributeManager   // 属性管理
RelationshipManager // 好感度管理
EventManager       // 事件/活动管理
BattleManager      // 战斗系统
UIManager          // UI渲染和交互
```

## 🐛 调试命令

打开浏览器控制台（F12），可以使用以下调试命令：

```javascript
// 显示当前状态
debugGame.showStats()

// 设置天数
debugGame.setDay(50)

// 设置属性等级
debugGame.setAttribute("knowledge", 8)
debugGame.setAttribute("combat", 7)

// 设置好感度
debugGame.setRelationship("char_akatsuki", 10)

// 开始战斗
debugGame.startBattle("enemy_shadow")    // 阴影怪物
debugGame.startBattle("enemy_nightmare") // 噩梦实体
debugGame.startBattle("enemy_guardian")  // 认知守卫
debugGame.startBattle("enemy_final_boss") // 最终BOSS

// 保存游戏
debugGame.save()

// 加载游戏
debugGame.load()

// 清除存档
debugGame.clearSave()
```

## 📊 游戏数据

### 角色列表
| ID | 名字 | 称号 | 头像 |
|----|------|------|------|
| char_akatsuki | 晓 | 青梅竹马 | 🌸 |
| char_zero | 零 | 神秘黑客 | 💻 |
| char_mizuki | 美月 | 学生会长 | 👑 |
| char_takuya | 拓也 | 热血运动员 | ⚡ |
| char_sakura | 咲良 | 天才少女 | 🔬 |
| char_rin | 凛 | 冷酷杀手 | 🗡️ |
| char_yuma | 悠真 | 艺术家 | 🎨 |
| char_hibiki | 响 | 音乐家 | 🎵 |

### 敌人列表
| 名称 | HP | 攻击 | 防御 |
|------|-----|------|------|
| 阴影怪物 | 50 | 10 | 5 |
| 噩梦实体 | 80 | 15 | 8 |
| 认知守卫 | 120 | 20 | 15 |
| 集体意识核心 | 300 | 35 | 25 |

## 🎯 开发状态

### ✅ 已完成
- [x] 核心游戏循环
- [x] 时间管理系统
- [x] 属性和好感度系统
- [x] 活动系统
- [x] 任务系统
- [x] 战斗系统
- [x] UI界面
- [x] 存档系统
- [x] 调试工具

### 🚧 待完善
- [ ] 更多活动内容
- [ ] 角色专属事件
- [ ] 完整的剧情对话
- [ ] 更多战斗技能
- [ ] 音效和背景音乐
- [ ] 角色立绘和CG
- [ ] 更复杂的结局判定
- [ ] 多周目系统

### 🎨 美术资源（待添加）
- [ ] 角色立绘
- [ ] 背景图片
- [ ] UI图标
- [ ] 战斗特效
- [ ] CG插画

### 🎵 音频资源（待添加）
- [ ] BGM（25首）
- [ ] 音效（80个）
- [ ] 角色语音（可选）

## 📝 代码质量

- **模块化**: 清晰的管理器分离
- **可扩展**: 易于添加新内容
- **数据驱动**: 游戏内容配置化
- **调试友好**: 完整的调试命令
- **注释完整**: 每个函数都有说明

## 🔧 自定义内容

### 添加新活动
编辑 `js/data/gameData.js`:

```javascript
{
    id: 'my_activity',
    name: '我的活动',
    description: '活动描述',
    conditions: {
        time_period: 1  // 0=早晨, 1=下午, 2=夜晚
    },
    effects: {
        attributes: {
            knowledge: 5  // 增加属性
        }
    }
}
```

### 添加新敌人
```javascript
{
    id: 'my_enemy',
    name: '新敌人',
    maxHP: 100,
    attack: 20,
    defense: 10,
    sprite: '👹'
}
```

### 修改配色
编辑 `css/main.css` 的 `:root` 变量:

```css
:root {
    --primary-bg: #0a0e27;
    --accent-blue: #00d4ff;
    --accent-purple: #9d4edd;
    /* ... */
}
```

## 📚 参考资料

- [完整GDD文档](../docs/design/GDD.md)
- [系统架构设计](../docs/systems/SystemArchitecture.md)
- [数据结构定义](../docs/data/DataStructures.md)

## 🤝 贡献

这是一个原型实现，欢迎：
- 添加更多游戏内容
- 优化UI/UX
- 添加美术资源
- 改进战斗系统
- Bug修复

## 📄 许可证

本项目为原型演示，基于GDD文档实现。

---

**开始你的认知空间之旅吧！** 🚀
