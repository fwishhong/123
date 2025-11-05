# 美术资源需求文档
# MindLink - 认知空间

**版本**: 1.0
**日期**: 2025-11-05
**状态**: 资源需求定义

---

## 目录

1. [资源概览](#资源概览)
2. [角色立绘](#角色立绘)
3. [像素精灵](#像素精灵)
4. [UI资源](#ui资源)
5. [场景背景](#场景背景)
6. [CG插画](#cg插画)
7. [视觉特效](#视觉特效)
8. [图标系统](#图标系统)
9. [技术规范](#技术规范)
10. [制作优先级](#制作优先级)

---

## 资源概览

### 总量统计

| 类型 | 数量 | 总文件数（含变体） |
|------|------|-------------------|
| 角色立绘 | 10人 | ~120张 |
| 像素角色精灵 | 10人 | ~40张 |
| 像素敌人精灵 | 15种 | ~30张 |
| UI元素 | - | ~80张 |
| 场景背景 | 15个 | ~30张 |
| CG插画 | 50张 | 50张 |
| 特效动画 | 30组 | ~150帧 |
| 图标 | 100个 | 100张 |
| **总计** | - | **~600个文件** |

### 美术风格

**整体风格**: 混搭风格
- **角色**: 日式高清立绘（类似《女神异闻录》）
- **场景/精灵**: 像素风格（16-bit 风格，类似《Stardew Valley》）
- **UI**: 现代简洁，科幻感

**色彩基调**:
- 日常场景: 温暖色调
- 认知空间: 冷色调 + 霓虹效果
- 战斗界面: 高对比度

---

## 角色立绘

### 规格要求

| 属性 | 规格 |
|------|------|
| **格式** | PNG（透明背景） |
| **分辨率** | 2048x2048 px |
| **导出尺寸** | 1024x1024 px（游戏内使用） |
| **色彩模式** | RGBA |
| **文件大小** | < 500KB/张（压缩后） |
| **命名规范** | `char_[ID]_[emotion]_[variant].png` |

### 角色列表

#### 主角（性别可选）

**char_00_player_male / char_00_player_female**

**立绘变体**:
- 基础表情（12种）：
  - `neutral` - 中性
  - `happy` - 开心
  - `sad` - 悲伤
  - `angry` - 愤怒
  - `surprised` - 惊讶
  - `worried` - 担心
  - `determined` - 坚定
  - `embarrassed` - 尴尬
  - `thinking` - 思考
  - `shocked` - 震惊
  - `smiling` - 微笑
  - `crying` - 哭泣

**特殊服装**（可选）:
- `school_uniform` - 校服（默认）
- `casual` - 便装
- `battle_gear` - 战斗装备

**总数**: 12表情 × 1服装 = **12张** （男女各12张，共24张）

---

#### 角色 #1: 晓 (Akira)

**ID**: `char_01`

**外貌描述**:
- 年龄: 17岁
- 性别: 女
- 发型: 短发，棕色，略显凌乱
- 身材: 运动型，中等身高
- 服装: 运动夹克 + 牛仔裤 + 运动鞋
- 特征: 明亮的眼睛，充满活力

**立绘变体**:
- 基础表情（12种）- 同上
- 特殊立绘:
  - `char_01_sports` - 运动服
  - `char_01_crying_serious` - 剧情专用（父母离婚）
  - `char_01_wedding` - 结局专用（恋爱路线）

**总数**: 12 + 3特殊 = **15张**

---

#### 角色 #2: 零 (Rei)

**ID**: `char_02`

**外貌描述**:
- 年龄: 18岁
- 性别: 男
- 发型: 整齐的短发，黑色
- 身材: 瘦高，斯文
- 服装: 衬衫 + 马甲 + 眼镜
- 特征: 总是在看书，冷静的表情

**立绘变体**:
- 基础表情（12种）
- 特殊立绘:
  - `char_02_no_glasses` - 摘下眼镜
  - `char_02_past` - 回忆场景（年轻版）

**总数**: 12 + 2特殊 = **14张**

---

#### 角色 #3: 美月 (Mizuki)

**ID**: `char_03`

**外貌描述**:
- 年龄: 16岁
- 性别: 女
- 发型: 长发，精致，浅色
- 身材: 纤细，优雅
- 服装: 精致的学生装 + 蝴蝶结
- 特征: 完美的外表，优雅的姿态

**立绘变体**:
- 基础表情（12种）
- 特殊立绘:
  - `char_03_stage_dress` - 演出服装
  - `char_03_casual_free` - 解放后的便装（更自由的形象）

**总数**: 12 + 2特殊 = **14张**

---

#### 角色 #4-8: 拓也、咲良、凛、悠真、响

每个角色同样需要：
- 基础表情（12种）
- 特殊立绘（2-3张）

**每个角色**: 12-15张
**5个角色总计**: **约65张**

---

#### 支援角色

**神代教授** (`char_support_01`):
- 基础表情（6种简化版）
- **6张**

**学生会长** (`char_support_02`):
- 基础表情（6种简化版）
- **6张**

**商人老K** (`char_support_03`):
- 基础表情（6种简化版）
- **6张**

---

#### 反派角色

**Dr.天城** (`char_villain_01`):
- 基础表情（8种）
- 特殊立绘:
  - `char_villain_01_young` - 年轻版（回忆）
  - `char_villain_01_final` - 最终战

**总数**: 8 + 2特殊 = **10张**

---

### 角色立绘总计

| 角色类型 | 数量 | 立绘总数 |
|---------|------|---------|
| 主角（男女） | 2 | 24张 |
| 可攻略角色 | 8 | 110张 |
| 支援角色 | 3 | 18张 |
| 反派角色 | 2 | 15张 |
| **总计** | **15人** | **~170张** |

---

## 像素精灵

### 规格要求

| 属性 | 规格 |
|------|------|
| **格式** | PNG（透明背景） |
| **基础尺寸** | 32x32 px（角色），64x64 px（大型敌人） |
| **导出倍率** | 1x, 2x, 3x（适配不同分辨率） |
| **色彩模式** | RGBA |
| **风格** | 16-bit 像素风格 |
| **动画帧数** | 4-8帧（行走、攻击等） |
| **命名规范** | `sprite_[type]_[id]_[action].png` |

### 角色行走精灵

**用途**: 探索场景、地图移动

**需求**:
- 主角行走: 4方向 × 4帧动画 = 16帧
- 8个可攻略角色: 各4方向 × 4帧 = 128帧
- 3个支援NPC: 简化版，静态 + 1个动画 = 6帧

**总数**: **约150帧**

**示例命名**:
```
sprite_char_00_walk_down_01.png
sprite_char_00_walk_down_02.png
sprite_char_00_walk_up_01.png
sprite_char_01_walk_left_01.png
```

---

### 战斗精灵（侧视图）

**用途**: 战斗画面显示

**需求**:
- 主角 + 8角色: 各3帧（待机、攻击、受伤）= 27帧
- 简化版可以用1帧静态图

**总数**: **约30帧**

---

### 敌人精灵

#### 杂兵类型（5种）

| ID | 名称 | 描述 | 尺寸 |
|----|------|------|------|
| `sprite_enemy_01` | 记忆碎片 | 破碎的几何体，漂浮 | 32x32 |
| `sprite_enemy_02` | 压力幻影 | 扭曲的人形阴影 | 32x48 |
| `sprite_enemy_03` | 数据噪音 | 像素化故障效果 | 32x32 |
| `sprite_enemy_04` | 情感泡沫 | 彩色的漂浮球体 | 24x24 |
| `sprite_enemy_05` | 记忆回声 | 半透明的重影 | 32x32 |

**每种**: 待机2帧 + 攻击2帧 = 4帧
**总数**: 5种 × 4帧 = **20帧**

---

#### 精英敌人（5种）

| ID | 名称 | 描述 | 尺寸 |
|----|------|------|------|
| `sprite_enemy_elite_01` | 防御机制 | 机械护卫，装甲 | 48x48 |
| `sprite_enemy_elite_02` | 恐惧实体 | 黑暗怪物，多触手 | 48x64 |
| `sprite_enemy_elite_03` | 执念具现 | 重复的影像 | 48x48 |
| `sprite_enemy_elite_04` | 认知壁障 | 墙状敌人 | 64x48 |
| `sprite_enemy_elite_05` | 混乱使者 | 多色混沌体 | 48x48 |

**每种**: 待机3帧 + 攻击3帧 + 特殊动作2帧 = 8帧
**总数**: 5种 × 8帧 = **40帧**

---

#### BOSS精灵（6个）

| ID | 名称 | 描述 | 尺寸 |
|----|------|------|------|
| `sprite_boss_01` | 遗忘之墙 | 巨大的屏障，裂痕 | 128x128 |
| `sprite_boss_02` | 压抑之心 | 心脏形状，黑暗能量 | 96x96 |
| `sprite_boss_03` | 记忆守护者 | 守护者形态 | 96x128 |
| `sprite_boss_04` | 系统核心 | 科技风格，多层结构 | 128x96 |
| `sprite_boss_05` | 影山（人形） | 人形BOSS | 64x96 |
| `sprite_boss_06` | Dr.天城最终形态 | 巨大的认知投影 | 128x128 |
| `sprite_boss_secret` | ??? (隐藏BOSS) | 神秘的存在 | 128x128 |

**每个**: 待机4帧 + 攻击4帧 + 阶段变化2帧 + 受伤2帧 = 12帧
**总数**: 7个 × 12帧 = **84帧**

---

### 像素精灵总计

| 类型 | 数量 | 总帧数 |
|------|------|--------|
| 角色行走精灵 | 12人 | 150帧 |
| 战斗精灵 | 9人 | 30帧 |
| 杂兵敌人 | 5种 | 20帧 |
| 精英敌人 | 5种 | 40帧 |
| BOSS | 7个 | 84帧 |
| **总计** | - | **~324帧** |

---

## UI资源

### 规格要求

| 属性 | 规格 |
|------|------|
| **格式** | PNG（部分透明） |
| **分辨率** | 矢量导出（多尺寸） |
| **色彩模式** | RGBA |
| **风格** | 现代简洁 + 科幻元素 |
| **适配** | 16:9, 18:9, 19.5:9 屏幕 |

### UI元素清单

#### 1. 主菜单界面

| 文件名 | 描述 | 尺寸 |
|--------|------|------|
| `ui_main_bg.png` | 主菜单背景 | 1920x1080 |
| `ui_main_logo.png` | 游戏Logo | 800x300 |
| `ui_button_newgame.png` | "新游戏"按钮 | 400x80 |
| `ui_button_continue.png` | "继续游戏"按钮 | 400x80 |
| `ui_button_load.png` | "读取存档"按钮 | 400x80 |
| `ui_button_options.png` | "设置"按钮 | 400x80 |
| `ui_button_quit.png` | "退出"按钮 | 400x80 |

**总数**: **7个文件**

---

#### 2. 日历/时间系统UI

| 文件名 | 描述 | 尺寸 |
|--------|------|------|
| `ui_calendar_bg.png` | 日历背景框 | 800x600 |
| `ui_calendar_day_normal.png` | 普通日期格子 | 80x80 |
| `ui_calendar_day_today.png` | 今天的日期（高亮） | 80x80 |
| `ui_calendar_day_event.png` | 有事件的日期 | 80x80 |
| `ui_calendar_day_deadline.png` | 截止日期（警告） | 80x80 |
| `ui_timeslot_morning.png` | 早晨图标 | 64x64 |
| `ui_timeslot_afternoon.png` | 下午图标 | 64x64 |
| `ui_timeslot_night.png` | 夜晚图标 | 64x64 |
| `ui_date_display.png` | 日期显示框 | 300x60 |

**总数**: **9个文件**

---

#### 3. 事件选择界面

| 文件名 | 描述 | 尺寸 |
|--------|------|------|
| `ui_event_card_bg.png` | 事件卡片背景 | 400x200 |
| `ui_event_card_border.png` | 卡片边框 | 400x200 |
| `ui_event_icon_study.png` | 学习图标 | 64x64 |
| `ui_event_icon_social.png` | 社交图标 | 64x64 |
| `ui_event_icon_battle.png` | 战斗图标 | 64x64 |
| `ui_event_icon_mission.png` | 任务图标 | 64x64 |
| `ui_location_marker.png` | 地点标记 | 48x48 |

**总数**: **7个文件**

---

#### 4. 战斗UI

| 文件名 | 描述 | 尺寸 |
|--------|------|------|
| `ui_battle_bg.png` | 战斗界面背景 | 1920x1080 |
| `ui_battle_player_panel.png` | 玩家状态面板 | 600x150 |
| `ui_battle_enemy_panel.png` | 敌人状态面板 | 300x100 |
| `ui_battle_hp_bar_bg.png` | HP条背景 | 200x20 |
| `ui_battle_hp_bar_fill.png` | HP条填充 | 200x20 |
| `ui_battle_sp_bar_bg.png` | SP条背景 | 200x20 |
| `ui_battle_sp_bar_fill.png` | SP条填充 | 200x20 |
| `ui_battle_button_attack.png` | 攻击按钮 | 120x120 |
| `ui_battle_button_skill.png` | 技能按钮 | 120x120 |
| `ui_battle_button_item.png` | 道具按钮 | 120x120 |
| `ui_battle_button_defend.png` | 防御按钮 | 120x120 |
| `ui_battle_button_escape.png` | 逃跑按钮 | 120x120 |
| `ui_battle_turn_indicator.png` | 回合指示器 | 100x100 |

**总数**: **13个文件**

---

#### 5. 对话系统UI

| 文件名 | 描述 | 尺寸 |
|--------|------|------|
| `ui_dialogue_box.png` | 对话框背景 | 1800x400 |
| `ui_dialogue_name_plate.png` | 角色名牌 | 300x60 |
| `ui_dialogue_choice_button.png` | 选项按钮 | 800x80 |
| `ui_dialogue_next_arrow.png` | 继续箭头动画（3帧） | 48x48 |
| `ui_dialogue_auto_icon.png` | 自动播放图标 | 32x32 |
| `ui_dialogue_skip_icon.png` | 跳过图标 | 32x32 |

**总数**: **8个文件**（含动画帧）

---

#### 6. 状态界面

| 文件名 | 描述 | 尺寸 |
|--------|------|------|
| `ui_status_bg.png` | 状态界面背景 | 1920x1080 |
| `ui_status_attribute_panel.png` | 属性面板 | 400x500 |
| `ui_status_relationship_panel.png` | 关系面板 | 400x500 |
| `ui_status_inventory_panel.png` | 背包面板 | 600x800 |
| `ui_status_tab_attributes.png` | 属性标签 | 150x50 |
| `ui_status_tab_relationships.png` | 关系标签 | 150x50 |
| `ui_status_tab_inventory.png` | 背包标签 | 150x50 |
| `ui_status_tab_missions.png` | 任务标签 | 150x50 |

**总数**: **8个文件**

---

#### 7. 通用UI组件

| 文件名 | 描述 | 尺寸 |
|--------|------|------|
| `ui_button_normal.png` | 通用按钮（普通） | 200x60 |
| `ui_button_hover.png` | 通用按钮（悬停） | 200x60 |
| `ui_button_pressed.png` | 通用按钮（按下） | 200x60 |
| `ui_button_disabled.png` | 通用按钮（禁用） | 200x60 |
| `ui_panel_bg.png` | 通用面板背景 | 800x600 |
| `ui_popup_bg.png` | 弹窗背景 | 600x400 |
| `ui_notification_bg.png` | 通知背景 | 400x100 |
| `ui_tooltip_bg.png` | 提示框背景 | 300x80 |
| `ui_loading_spinner.png` | 加载动画（8帧） | 64x64 |
| `ui_checkmark.png` | 勾选标记 | 32x32 |
| `ui_cross.png` | 关闭标记 | 32x32 |

**总数**: **19个文件**（含动画帧）

---

### UI资源总计

| 类别 | 文件数 |
|------|--------|
| 主菜单 | 7 |
| 日历/时间 | 9 |
| 事件选择 | 7 |
| 战斗UI | 13 |
| 对话UI | 8 |
| 状态界面 | 8 |
| 通用组件 | 19 |
| **总计** | **71个文件** |

---

## 场景背景

### 规格要求

| 属性 | 规格 |
|------|------|
| **格式** | PNG/JPG |
| **分辨率** | 1920x1080 px（横屏），1080x1920 px（竖屏） |
| **导出尺寸** | 多尺寸适配 |
| **色彩模式** | RGB |
| **风格** | 像素风格（场景），高清（认知空间） |

### 场景列表

#### 日常场景（像素风）

| ID | 场景名 | 描述 | 时段变体 |
|----|--------|------|----------|
| `bg_school_courtyard` | 学校庭院 | 学生活动区域 | 早/午/晚 |
| `bg_school_classroom` | 教室 | 主角的教室 | 早/午/晚 |
| `bg_library` | 图书馆 | 安静的学习场所 | 午/晚 |
| `bg_training_ground` | 训练场 | 锻炼和战斗练习 | 午/晚 |
| `bg_cafeteria` | 食堂 | 用餐和社交 | 午 |
| `bg_street` | 街道 | 城市街景 | 早/午/晚 |
| `bg_shopping_district` | 商业区 | 购物和社交 | 午/晚 |
| `bg_underground_market` | 地下市场 | 拓也的据点 | 晚 |
| `bg_player_room` | 主角房间 | 私人空间 | 早/晚 |
| `bg_cafe` | 咖啡厅 | 打工和约会地点 | 午/晚 |

**每个场景**: 1-3个时段变体
**总数**: **约25张**

---

#### 认知空间场景（高清/混合风格）

| ID | 场景名 | 描述 | 主题色 |
|----|--------|------|--------|
| `bg_cognitive_tutorial` | 教学认知空间 | 梦境般的抽象空间 | 蓝紫色 |
| `bg_cognitive_victim01` | 遗忘之域 | 灰白色，记忆碎片漂浮 | 灰白色 |
| `bg_cognitive_victim02` | 压抑之心 | 黑暗，心脏血管状 | 暗红色 |
| `bg_cognitive_victim03` | 记忆迷宫 | 走廊和分岔路 | 金色 |
| `bg_cognitive_neurocorp` | NeuroCorp内部 | 科技感，数据流 | 青色 |
| `bg_cognitive_akira` | 燃烧的体育馆 | 火焰和运动器材 | 橙红色 |
| `bg_cognitive_rei` | 燃烧的图书馆 | 书籍和火焰 | 红黑色 |
| `bg_cognitive_mizuki` | 华丽牢笼 | 金色笼子，鸟 | 金色 |
| `bg_cognitive_final` | 集体意识空间 | 无限的光 | 白色 |
| `bg_cognitive_true` | 因果尽头 | 多时间线重叠 | 彩虹色 |

**总数**: **10张**

---

### 场景背景总计

| 类型 | 数量 |
|------|------|
| 日常场景 | 25张 |
| 认知空间 | 10张 |
| **总计** | **35张** |

---

## CG插画

### 规格要求

| 属性 | 规格 |
|------|------|
| **格式** | PNG/JPG |
| **分辨率** | 2560x1440 px |
| **色彩模式** | RGB |
| **风格** | 高清插画，日式动画风格 |
| **文件大小** | < 2MB/张（压缩后） |

### CG分类

#### 主线剧情CG（20张）

| ID | 描述 | 场景 |
|----|------|------|
| `cg_opening` | 开场：主角醒来 | Day 1 |
| `cg_first_collapse` | 第一次目睹认知崩溃 | Day 2 |
| `cg_power_awaken` | 能力觉醒 | Day 3 |
| `cg_first_dive` | 第一次进入认知空间 | Day 10 |
| `cg_mission01_clear` | 第一次拯救成功 | Day 15 |
| `cg_team_assemble` | 团队集结 | Day 30 |
| `cg_truth_reveal` | 真相揭示 | Day 60 |
| `cg_character_death` | 角色死亡（如果发生） | Day 50-75 |
| `cg_betrayal` | 内部叛徒揭露 | Day 85 |
| `cg_final_choice` | 最终选择 | Day 99 |
| `cg_ending_bad` | Bad Ending | Day 100 |
| `cg_ending_normal` | Normal Ending | Day 100 |
| `cg_ending_good_01` | Good Ending 1 | Day 100 |
| `cg_ending_good_02` | Good Ending 2 | Day 100 |
| `cg_ending_ideal_01` | Ideal Ending 1 | Day 100 |
| `cg_ending_ideal_02` | Ideal Ending 2 | Day 100 |
| `cg_ending_true_01` | True Ending 1 | Day 100 |
| `cg_ending_true_02` | True Ending 2 | Day 100 |
| `cg_ending_dark` | Dark Ending | Day 100 |
| `cg_ending_joke` | Joke Ending | Day 100 |

**总数**: **20张**

---

#### 角色剧情CG（16张，每角色2张）

每个可攻略角色（8人）:
- 1张: 个人危机事件CG
- 1张: 结局CG（恋爱/友情）

**示例**:
- `cg_char01_crisis` - 晓的危机
- `cg_char01_ending` - 晓的结局
- `cg_char02_crisis` - 零的危机
- `cg_char02_ending` - 零的结局
- ...

**总数**: **16张**

---

#### 特殊事件CG（10张）

| ID | 描述 |
|----|------|
| `cg_school_festival` | 学园祭 |
| `cg_cultural_festival` | 文化祭 |
| `cg_beach_event` | 海滩事件（夏日） |
| `cg_christmas` | 圣诞节（如果玩到） |
| `cg_neurocorp_infiltration` | 潜入NeuroCorp |
| `cg_boss_fight_epic` | 史诗BOSS战 |
| `cg_all_out_attack` | 总攻击演出 |
| `cg_time_rewind` | 时间回溯 |
| `cg_secret_room` | 秘密房间 |
| `cg_group_photo` | 全员合照（Good Ending+） |

**总数**: **10张**

---

#### 回忆/闪回CG（4张）

| ID | 描述 |
|----|------|
| `cg_flashback_amagi` | 天城的过去 |
| `cg_flashback_rei` | 零的弟弟事件 |
| `cg_flashback_player` | 主角的昏迷 |
| `cg_flashback_origin` | MindLink的诞生 |

**总数**: **4张**

---

### CG总计

| 类型 | 数量 |
|------|------|
| 主线剧情 | 20张 |
| 角色剧情 | 16张 |
| 特殊事件 | 10张 |
| 回忆闪回 | 4张 |
| **总计** | **50张** |

---

## 视觉特效

### 规格要求

| 属性 | 规格 |
|------|------|
| **格式** | PNG序列帧 / Sprite Sheet |
| **分辨率** | 根据用途，64x64 到 512x512 |
| **帧数** | 4-16帧/动画 |
| **色彩模式** | RGBA（透明） |
| **导出** | Sprite Atlas（优化性能） |

### 特效列表

#### 战斗特效（20组）

| ID | 名称 | 描述 | 帧数 |
|----|------|------|------|
| `vfx_attack_slash` | 斩击 | 白色弧线 | 6帧 |
| `vfx_attack_punch` | 拳击 | 冲击波 | 4帧 |
| `vfx_skill_fire` | 火焰攻击 | 火球爆炸 | 8帧 |
| `vfx_skill_ice` | 冰冻攻击 | 冰晶扩散 | 8帧 |
| `vfx_skill_lightning` | 雷电攻击 | 闪电链 | 8帧 |
| `vfx_skill_digital` | 数字攻击 | 数据流 | 8帧 |
| `vfx_skill_emotional` | 情感攻击 | 心形波纹 | 8帧 |
| `vfx_skill_memory` | 记忆攻击 | 碎片旋转 | 8帧 |
| `vfx_heal` | 治疗效果 | 绿色光芒 | 8帧 |
| `vfx_buff` | BUFF效果 | 向上箭头 | 6帧 |
| `vfx_debuff` | DEBUFF效果 | 向下箭头 | 6帧 |
| `vfx_critical` | 暴击特效 | 爆炸星星 | 8帧 |
| `vfx_weak_hit` | 弱点命中 | 红色感叹号 | 6帧 |
| `vfx_down` | 倒地 | 眩晕图标 | 4帧循环 |
| `vfx_all_out_attack` | 总攻击 | 全屏特效 | 16帧 |
| `vfx_hit_impact` | 命中冲击 | 白色闪光 | 4帧 |
| `vfx_dodge` | 闪避 | 残影 | 6帧 |
| `vfx_guard` | 防御 | 盾牌 | 6帧 |
| `vfx_death_enemy` | 敌人死亡 | 消散 | 8帧 |
| `vfx_level_up` | 升级 | 光芒上升 | 12帧 |

**总数**: **20组 ≈ 150帧**

---

#### UI特效（5组）

| ID | 名称 | 描述 | 帧数 |
|----|------|------|------|
| `vfx_ui_notification` | 通知提示 | 弹出动画 | 8帧 |
| `vfx_ui_attribute_gain` | 属性增加 | 数字上升 | 6帧 |
| `vfx_ui_relationship_up` | 好感度提升 | 心形上升 | 8帧 |
| `vfx_ui_transition` | 场景转场 | 淡入淡出 | 6帧 |
| `vfx_ui_cursor_click` | 点击反馈 | 涟漪效果 | 4帧 |

**总数**: **5组 ≈ 32帧**

---

#### 环境特效（5组）

| ID | 名称 | 描述 | 帧数 |
|----|------|------|------|
| `vfx_env_rain` | 雨 | 雨滴下落 | 8帧循环 |
| `vfx_env_snow` | 雪 | 雪花飘落 | 8帧循环 |
| `vfx_env_particles` | 粒子 | 光点漂浮 | 8帧循环 |
| `vfx_env_glitch` | 故障效果 | 认知空间特效 | 8帧循环 |
| `vfx_env_portal` | 传送门 | 旋转能量 | 12帧循环 |

**总数**: **5组 ≈ 44帧**

---

### 特效总计

| 类型 | 组数 | 总帧数 |
|------|------|--------|
| 战斗特效 | 20 | ~150帧 |
| UI特效 | 5 | ~32帧 |
| 环境特效 | 5 | ~44帧 |
| **总计** | **30组** | **~226帧** |

---

## 图标系统

### 规格要求

| 属性 | 规格 |
|------|------|
| **格式** | PNG（透明背景） |
| **分辨率** | 128x128 px（导出多尺寸） |
| **色彩模式** | RGBA |
| **风格** | 简洁图标，线性风格 |

### 图标列表

#### 属性图标（4个）

| ID | 名称 |
|----|------|
| `icon_attribute_knowledge` | 知识（书本） |
| `icon_attribute_courage` | 勇气（剑） |
| `icon_attribute_charm` | 魅力（星星） |
| `icon_attribute_combat` | 战斗力（拳头） |

---

#### 技能图标（30个）

分类:
- 攻击技能: 10个（火、冰、雷、物理等）
- 辅助技能: 10个（治疗、BUFF等）
- 特殊技能: 10个（角色专属）

**示例**:
- `icon_skill_basic_attack` - 普通攻击
- `icon_skill_lightning_strike` - 雷霆一击
- `icon_skill_heal` - 治疗
- `icon_skill_flame_kick` - 烈焰飞踢（晓）
- ...

---

#### 道具图标（30个）

分类:
- 消耗品: 10个（药水、食物等）
- 装备: 15个（武器、防具、饰品）
- 任务道具: 5个（特殊物品）

**示例**:
- `icon_item_health_potion` - 治疗药水（红色瓶子）
- `icon_item_sp_drink` - 能量饮料（蓝色罐子）
- `icon_item_weapon_sword` - 剑
- `icon_item_memory_fragment` - 记忆碎片
- ...

---

#### 状态图标（15个）

| ID | 名称 |
|----|------|
| `icon_status_poisoned` | 中毒 |
| `icon_status_stunned` | 眩晕 |
| `icon_status_burning` | 燃烧 |
| `icon_status_frozen` | 冰冻 |
| `icon_status_confused` | 混乱 |
| `icon_status_atk_up` | 攻击提升 |
| `icon_status_def_up` | 防御提升 |
| `icon_status_spd_up` | 速度提升 |
| `icon_status_atk_down` | 攻击下降 |
| `icon_status_def_down` | 防御下降 |
| `icon_status_regen` | 再生 |
| `icon_status_barrier` | 护盾 |
| `icon_status_immune` | 免疫 |
| `icon_status_berserk` | 狂暴 |
| `icon_status_sleep` | 睡眠 |

---

#### 系统图标（20个）

**示例**:
- `icon_calendar` - 日历
- `icon_save` - 保存
- `icon_load` - 读取
- `icon_settings` - 设置
- `icon_map` - 地图
- `icon_mission` - 任务
- `icon_inventory` - 背包
- `icon_relationship` - 关系
- `icon_battle` - 战斗
- `icon_shop` - 商店
- `icon_hint` - 提示
- `icon_skip` - 跳过
- `icon_auto` - 自动
- `icon_log` - 记录
- `icon_gallery` - 图鉴
- ...

---

### 图标总计

| 类型 | 数量 |
|------|------|
| 属性图标 | 4 |
| 技能图标 | 30 |
| 道具图标 | 30 |
| 状态图标 | 15 |
| 系统图标 | 20 |
| **总计** | **99个** |

---

## 技术规范

### 文件命名规范

**统一格式**: `[category]_[type]_[id]_[variant].ext`

**示例**:
- `char_01_happy.png` - 角色立绘
- `sprite_enemy_01_walk_01.png` - 敌人精灵
- `ui_button_normal.png` - UI元素
- `bg_school_courtyard_afternoon.png` - 背景
- `cg_ending_good_01.png` - CG
- `vfx_attack_slash_03.png` - 特效帧
- `icon_skill_heal.png` - 图标

---

### 图层组织（PSD/源文件）

**推荐图层结构**:
```
文件名.psd
├─ Background（背景）
├─ Character（角色主体）
│  ├─ Body（身体）
│  ├─ Head（头部）
│  ├─ Eyes（眼睛）
│  ├─ Mouth（嘴巴）
│  └─ Hair（头发）
├─ Effects（特效）
└─ Export（导出标记）
```

**变体管理**:
- 使用图层组管理不同表情
- 使用智能对象便于批量修改
- 保留源文件（PSD/AI）

---

### 导出设置

#### 角色立绘
```
格式: PNG-24
透明: 是
分辨率: 2048x2048 px（源）, 1024x1024 px（游戏用）
压缩: TinyPNG 或 pngquant
色彩配置: sRGB
```

#### 像素精灵
```
格式: PNG-8 或 PNG-24（需要透明度）
抗锯齿: 关闭（保持像素锐利）
分辨率: 32x32, 48x48, 64x64, 128x128
导出倍率: @1x, @2x, @3x
```

#### UI元素
```
格式: PNG-24
九宫格切片: 是（可拉伸元素）
分辨率: 原始尺寸 + @2x, @3x
命名: name.png, name@2x.png, name@3x.png
```

#### CG插画
```
格式: JPG（背景）, PNG（需透明）
质量: 90%
分辨率: 2560x1440 px
色彩配置: sRGB
文件大小: < 2MB
```

---

### 色彩规范

#### 主色调

| 用途 | 颜色代码 | 说明 |
|------|---------|------|
| 主色 | `#00A8E8` | 科技蓝 |
| 辅色 | `#FF6B6B` | 警告红 |
| 强调色 | `#FFD93D` | 高亮黄 |
| 成功 | `#6BCF7F` | 成功绿 |
| 文本主色 | `#2D3436` | 深灰 |
| 文本辅色 | `#636E72` | 中灰 |
| 背景 | `#F5F6FA` | 浅灰 |

#### 属性颜色

| 属性 | 颜色 |
|------|------|
| 知识 | 蓝色 `#0984E3` |
| 勇气 | 红色 `#D63031` |
| 魅力 | 粉色 `#FD79A8` |
| 战斗力 | 橙色 `#E17055` |

#### 元素颜色

| 元素 | 颜色 |
|------|------|
| 物理 | 灰色 `#95A5A6` |
| 数字 | 青色 `#00CEC9` |
| 情感 | 粉色 `#FD79A8` |
| 记忆 | 金色 `#FDCB6E` |
| 精神 | 紫色 `#6C5CE7` |

---

## 制作优先级

### Phase 1: 核心原型（最高优先级）

**需要立即制作**:
- [ ] 主角立绘（6个基础表情）
- [ ] 晓、零立绘（各6个基础表情）
- [ ] 像素主角行走精灵（4方向）
- [ ] 3种基础敌人精灵
- [ ] 第一个BOSS精灵
- [ ] 核心UI（主菜单、对话框、战斗界面）
- [ ] 3个日常场景背景
- [ ] 2个认知空间背景
- [ ] 20个基础图标
- [ ] 5个基础战斗特效

**预计**: ~100个文件

---

### Phase 2: 垂直切片（高优先级）

**Day 1-15完整内容**:
- [ ] 补充主角、晓、零的完整表情
- [ ] 美月立绘（基础表情）
- [ ] 6个日常场景
- [ ] 3个认知空间场景
- [ ] 第一个任务的CG（3张）
- [ ] 完整战斗UI
- [ ] 50个图标
- [ ] 15个战斗特效

**预计**: ~150个文件

---

### Phase 3: 全量开发（中优先级）

**完整游戏内容**:
- [ ] 全部8个可攻略角色立绘
- [ ] 全部支援和反派角色立绘
- [ ] 全部敌人和BOSS精灵
- [ ] 全部场景背景
- [ ] 主线CG（20张）
- [ ] 全部UI组件
- [ ] 全部图标
- [ ] 全部特效

**预计**: ~500个文件

---

### Phase 4: 打磨完善（低优先级）

**锦上添花**:
- [ ] 角色CG（16张）
- [ ] 特殊事件CG（10张）
- [ ] 角色特殊服装立绘
- [ ] 环境特效
- [ ] UI动画优化
- [ ] 额外变体

**预计**: ~100个文件

---

## 资源制作建议

### 外包策略

**推荐外包内容**:
1. **角色立绘** - 找专业立绘画师（重要！）
2. **CG插画** - 找CG画师（与立绘画师可以是同一人）
3. **像素美术** - 找像素艺术家（相对便宜）
4. **UI设计** - 找UI设计师

**自制内容**（如果预算有限）:
- 简单图标（可用工具生成）
- 基础UI组件
- 特效（可用工具或购买素材包）

---

### 预算估算

| 项目 | 数量 | 单价参考（USD） | 小计 |
|------|------|----------------|------|
| 角色立绘 | 170张 | $30-80/张 | $5,100-13,600 |
| CG插画 | 50张 | $100-300/张 | $5,000-15,000 |
| 像素精灵 | 324帧 | $10-30/帧 | $3,240-9,720 |
| 场景背景 | 35张 | $50-150/张 | $1,750-5,250 |
| UI资源 | 71个 | $10-30/个 | $710-2,130 |
| 特效 | 226帧 | $5-15/帧 | $1,130-3,390 |
| 图标 | 99个 | $5-15/个 | $495-1,485 |
| **总计** | - | - | **$17,425-50,575** |

**现实建议**:
- MVP阶段: $3,000-5,000（复用素材+简化）
- 完整版本: $15,000-25,000（部分外包+素材包）
- 高品质版: $40,000+（全部定制）

---

### 素材包推荐

**可购买现成素材**（节省成本）:

1. **itch.io** - 像素素材包（$5-50）
2. **Unity Asset Store** - UI包、特效包（$10-100）
3. **Envato Elements** - 订阅制素材库（$16.50/月）
4. **OpenGameArt** - 免费素材（需检查许可证）

**AI生成工具**（原型阶段）:
- **Midjourney/Stable Diffusion** - 生成立绘占位图
- **DALL-E** - 生成概念图
- 注意：商用需要检查版权

---

## 下一步

- [ ] 根据优先级开始制作
- [ ] 建立美术风格指南（Style Guide）
- [ ] 准备外包Brief文档
- [ ] 建立资源管理流程

---

**文档状态**: ✅ 完成
**总资源数**: ~600个文件
**预估成本**: $17,000-50,000（全定制）

