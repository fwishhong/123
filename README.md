# MindLink - 认知空间 (Cognitive Space)

> 一款结合《女神异闻录5》时间管理系统和《命运石之门》深度剧情的近未来科幻冒险游戏

## 🎮 游戏概述

**类型**: 时间管理 + 回合制战斗 + 解谜 + 视觉小说
**平台**: 移动端（iOS/Android）
**开发引擎**: Unity
**美术风格**: 2D像素风 + 角色立绘
**游戏时长**: 单周目 8-12 小时，多周目解锁完整剧情

### 核心玩法

- **100天时间线**: 玩家有100天时间来准备和完成任务
- **选择与取舍**: 每天只能做有限的事情，无法完成所有内容
- **多维成长**: 知识、勇气、魅力、战斗力等属性系统
- **人际关系**: 8个可攻略角色，好感度影响剧情和战斗
- **大型任务**: 每15-20天一个大任务，结合战斗与解谜
- **多重结局**: 15+ 结局，基于选择、属性和好感度

## 📁 项目结构

```
MindLink/
├── docs/               # 设计文档
│   ├── design/        # 游戏设计文档
│   ├── systems/       # 系统设计文档
│   ├── data/          # 数据结构设计
│   ├── content/       # 内容设计（剧情、事件）
│   └── assets/        # 美术需求文档
├── src/               # 源代码
│   ├── scripts/       # 游戏脚本
│   ├── data/          # 游戏数据（JSON）
│   └── config/        # 配置文件
├── assets/            # 游戏资源
│   ├── sprites/       # 精灵图
│   ├── audio/         # 音频文件
│   └── ui/            # UI资源
└── README.md          # 本文件
```

## 🎯 开发阶段

### Phase 1: 设计阶段 ✅ (当前)
- [x] 核心概念设计
- [ ] 详细系统设计文档
- [ ] 数据结构设计
- [ ] 内容规划

### Phase 2: 原型开发 (Week 1-4)
- [ ] 时间管理系统
- [ ] 基础UI
- [ ] 属性系统
- [ ] 简单事件系统

### Phase 3: 核心系统 (Week 5-10)
- [ ] 战斗系统
- [ ] 解谜系统
- [ ] 好感度系统
- [ ] 存档系统

### Phase 4: 内容制作 (Week 11-20)
- [ ] 100天事件编写
- [ ] 角色剧情
- [ ] 大任务设计
- [ ] 结局制作

### Phase 5: 优化与测试 (Week 21-24)
- [ ] 性能优化
- [ ] 平衡性调整
- [ ] Bug修复
- [ ] 多语言本地化

## 📚 文档索引

### 设计文档
- [游戏设计概览](docs/design/GDD.md) - 游戏整体设计文档
- [世界观与故事](docs/design/Story.md) - 剧情设定与世界观
- [角色设计](docs/design/Characters.md) - 角色背景与关系

### 系统文档
- [时间管理系统](docs/systems/TimeSystem.md)
- [属性与成长系统](docs/systems/AttributeSystem.md)
- [好感度系统](docs/systems/RelationshipSystem.md)
- [战斗系统](docs/systems/BattleSystem.md)
- [解谜系统](docs/systems/PuzzleSystem.md)
- [结局系统](docs/systems/EndingSystem.md)

### 数据结构
- [数据格式说明](docs/data/DataStructures.md)
- [事件配置格式](docs/data/EventSchema.md)
- [角色数据格式](docs/data/CharacterSchema.md)

### 内容设计
- [100天时间线](docs/content/Timeline.md)
- [事件列表](docs/content/Events.md)
- [对话脚本](docs/content/Dialogues.md)

## 🛠️ 技术栈

- **引擎**: Unity 2022 LTS
- **语言**: C#
- **数据格式**: JSON
- **版本控制**: Git
- **UI框架**: Unity UI Toolkit
- **本地化**: Unity Localization

## 📝 开发日志

### 2025-11-05
- 项目初始化
- 创建项目结构
- 开始编写设计文档

## 📄 License

TBD

## 🤝 Contributing

当前为设计阶段，暂不接受外部贡献。

---

**开发状态**: 🔵 设计阶段
**最后更新**: 2025-11-05
