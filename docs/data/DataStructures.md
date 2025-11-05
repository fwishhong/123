# 数据结构设计
# MindLink - 认知空间

**版本**: 1.0
**日期**: 2025-11-05

---

## 目录

1. [数据组织](#数据组织)
2. [配置数据](#配置数据)
3. [运行时数据](#运行时数据)
4. [存档数据](#存档数据)
5. [数据验证](#数据验证)

---

## 数据组织

### 文件结构

```
src/data/
├── config/                 # 静态配置数据（只读）
│   ├── characters.json     # 角色定义
│   ├── events.json         # 事件配置
│   ├── dialogues.json      # 对话脚本
│   ├── skills.json         # 技能配置
│   ├── items.json          # 道具配置
│   ├── enemies.json        # 敌人配置
│   ├── missions.json       # 任务配置
│   └── game_config.json    # 游戏常量
│
├── localization/          # 多语言文本
│   ├── en.json            # 英语
│   ├── zh-CN.json         # 简体中文
│   ├── ja.json            # 日语
│   └── ko.json            # 韩语
│
└── saves/                 # 存档文件（运行时生成）
    ├── save_slot_1.json
    ├── save_slot_2.json
    ├── save_slot_3.json
    └── autosave.json
```

### 数据格式约定

- **格式**: JSON (UTF-8编码)
- **命名**: snake_case (字段名)
- **日期**: ISO 8601 格式
- **ID**: 使用前缀区分类型 (如 `char_01`, `event_01`, `skill_01`)

---

## 配置数据

### 1. 角色配置 (characters.json)

```json
{
  "characters": [
    {
      "id": "char_01",
      "name_key": "char_01_name",
      "role": "childhood_friend",
      "gender": "female",
      "age": 17,

      "description_key": "char_01_desc",
      "personality": {
        "traits": ["cheerful", "loyal", "impulsive"],
        "likes": ["sports", "action_movies", "spicy_food"],
        "dislikes": ["studying", "formality", "insects"]
      },

      "initial_state": {
        "unlocked": true,
        "alive": true,
        "relationship_level": 2,
        "location": "school_courtyard"
      },

      "battle_stats": {
        "role": "attacker",
        "base_hp": 120,
        "base_sp": 40,
        "base_atk": 15,
        "base_def": 8,
        "base_mag": 6,
        "base_res": 7,
        "base_spd": 65,
        "base_luk": 12
      },

      "skills": {
        "initial": ["skill_basic_attack"],
        "unlock_conditions": [
          {
            "skill_id": "skill_flame_kick",
            "relationship_level": 6
          },
          {
            "skill_id": "skill_ultimate_strike",
            "relationship_level": 10
          }
        ]
      },

      "events": {
        "relationship_events": [
          "event_char01_lv2",
          "event_char01_lv4",
          "event_char01_lv6",
          "event_char01_lv8",
          "event_char01_lv10"
        ],
        "crisis_event": "event_char01_crisis",
        "romance_event": "event_char01_romance"
      },

      "voice_actor": "TBD",
      "sprite_id": "char_01_sprite",
      "portrait_id": "char_01_portrait"
    }
  ]
}
```

### 2. 事件配置 (events.json)

```json
{
  "events": [
    {
      "id": "event_study_library",
      "type": "activity",
      "category": "attribute_training",

      "name_key": "event_study_library_name",
      "description_key": "event_study_library_desc",
      "location": "library",

      "conditions": {
        "day_range": {
          "min": 1,
          "max": 100
        },
        "time_slots": ["afternoon", "night"],
        "weekday_only": true,
        "required_attributes": {},
        "required_relationships": {},
        "required_flags": [],
        "forbidden_flags": [],
        "random_chance": 1.0
      },

      "effects": {
        "time_cost": 1,
        "attribute_changes": {
          "knowledge": 15
        },
        "relationship_changes": {},
        "money_change": 0,
        "item_rewards": [],
        "set_flags": [],
        "unlock_events": []
      },

      "repeatable": true,
      "cooldown_days": 0,
      "priority": 1
    },

    {
      "id": "event_main_story_day10",
      "type": "main_story",
      "category": "forced",

      "name_key": "event_main_day10_name",
      "description_key": "event_main_day10_desc",

      "conditions": {
        "day_range": {
          "min": 10,
          "max": 10
        },
        "time_slots": ["night"],
        "required_flags": ["tutorial_completed"]
      },

      "effects": {
        "time_cost": 0,
        "dialogue_id": "dialogue_main_day10",
        "set_flags": ["first_mission_unlocked"],
        "unlock_events": ["event_mission_01_prepare"]
      },

      "repeatable": false,
      "auto_trigger": true,
      "skippable": false
    },

    {
      "id": "event_hangout_char01",
      "type": "social",
      "category": "character_interaction",

      "name_key": "event_hangout_char01_name",
      "description_key": "event_hangout_char01_desc",
      "related_character": "char_01",

      "conditions": {
        "day_range": {
          "min": 5,
          "max": 100
        },
        "time_slots": ["afternoon"],
        "weekend_only": true,
        "required_relationships": {
          "char_01": 2
        }
      },

      "effects": {
        "time_cost": 1,
        "dialogue_id": "dialogue_hangout_char01_01",
        "relationship_changes": {
          "char_01": 20
        },
        "attribute_changes": {
          "charm": 5
        }
      },

      "repeatable": true,
      "cooldown_days": 3,
      "variations": [
        "dialogue_hangout_char01_01",
        "dialogue_hangout_char01_02",
        "dialogue_hangout_char01_03"
      ]
    }
  ]
}
```

### 3. 对话配置 (dialogues.json)

```json
{
  "dialogues": [
    {
      "id": "dialogue_main_day10",
      "title_key": "dialogue_main_day10_title",

      "nodes": [
        {
          "id": "node_01",
          "speaker": "char_01",
          "text_key": "dialogue_main_day10_node01",
          "emotion": "worried",
          "next": "node_02"
        },
        {
          "id": "node_02",
          "speaker": "player",
          "text_key": "dialogue_main_day10_node02",
          "next": "node_03"
        },
        {
          "id": "node_03",
          "speaker": "char_01",
          "text_key": "dialogue_main_day10_node03",
          "emotion": "determined",
          "next": "choice_01"
        },
        {
          "id": "choice_01",
          "type": "choice",
          "options": [
            {
              "text_key": "dialogue_main_day10_choice01_opt1",
              "next": "node_04_agree",
              "effects": {
                "relationship_changes": {
                  "char_01": 10
                },
                "set_flags": ["agreed_to_help"]
              },
              "requirements": {
                "courage": 2
              }
            },
            {
              "text_key": "dialogue_main_day10_choice01_opt2",
              "next": "node_04_hesitate",
              "effects": {
                "relationship_changes": {
                  "char_01": -5
                }
              }
            }
          ]
        },
        {
          "id": "node_04_agree",
          "speaker": "char_01",
          "text_key": "dialogue_main_day10_node04_agree",
          "emotion": "happy",
          "next": "end"
        },
        {
          "id": "node_04_hesitate",
          "speaker": "char_01",
          "text_key": "dialogue_main_day10_node04_hesitate",
          "emotion": "disappointed",
          "next": "end"
        }
      ]
    }
  ]
}
```

### 4. 技能配置 (skills.json)

```json
{
  "skills": [
    {
      "id": "skill_basic_attack",
      "name_key": "skill_basic_attack_name",
      "description_key": "skill_basic_attack_desc",

      "sp_cost": 0,
      "type": "attack",
      "element": "physical",
      "target": "single_enemy",

      "power": 100,
      "accuracy": 95,
      "critical_bonus": 0,

      "effects": [],
      "animation": "attack_slash",
      "icon": "icon_attack"
    },

    {
      "id": "skill_lightning_strike",
      "name_key": "skill_lightning_name",
      "description_key": "skill_lightning_desc",

      "sp_cost": 15,
      "type": "attack",
      "element": "digital",
      "target": "single_enemy",

      "power": 150,
      "accuracy": 85,
      "critical_bonus": 10,

      "effects": [
        {
          "type": "bonus_damage_vs_type",
          "target_type": "mechanical",
          "multiplier": 1.5
        }
      ],

      "animation": "skill_lightning",
      "icon": "icon_lightning",
      "unlock_level": 4
    },

    {
      "id": "skill_heal",
      "name_key": "skill_heal_name",
      "description_key": "skill_heal_desc",

      "sp_cost": 12,
      "type": "support",
      "element": "emotional",
      "target": "single_ally",

      "power": 0,
      "accuracy": 100,

      "effects": [
        {
          "type": "heal",
          "amount": 80,
          "is_percentage": false
        }
      ],

      "animation": "skill_heal_glow",
      "icon": "icon_heal"
    }
  ]
}
```

### 5. 道具配置 (items.json)

```json
{
  "items": [
    {
      "id": "item_health_potion",
      "name_key": "item_health_potion_name",
      "description_key": "item_health_potion_desc",
      "category": "consumable",

      "price": 50,
      "sell_price": 25,

      "usable_in_battle": true,
      "usable_in_field": true,
      "target": "single_ally",

      "effects": [
        {
          "type": "heal_hp",
          "amount": 100,
          "is_percentage": false
        }
      ],

      "icon": "icon_potion_red",
      "max_stack": 99
    },

    {
      "id": "item_weapon_basic_sword",
      "name_key": "item_weapon_basic_sword_name",
      "description_key": "item_weapon_basic_sword_desc",
      "category": "equipment_weapon",

      "price": 200,
      "sell_price": 100,

      "equip_slot": "weapon",
      "stat_bonuses": {
        "atk": 10,
        "spd": 5
      },

      "icon": "icon_sword_basic",
      "max_stack": 1
    }
  ]
}
```

### 6. 敌人配置 (enemies.json)

```json
{
  "enemies": [
    {
      "id": "enemy_memory_fragment",
      "name_key": "enemy_memory_fragment_name",
      "description_key": "enemy_memory_fragment_desc",

      "type": "mob",
      "category": "memory",

      "stats": {
        "hp": 80,
        "sp": 20,
        "atk": 8,
        "def": 5,
        "mag": 6,
        "res": 4,
        "spd": 50,
        "luk": 5
      },

      "element": "memory",
      "weaknesses": ["emotional"],
      "resistances": ["physical"],
      "immunities": [],

      "skills": ["skill_memory_shard", "skill_confuse"],
      "ai_pattern": "aggressive",

      "rewards": {
        "exp": 30,
        "money": 20,
        "item_drops": [
          {
            "item_id": "item_memory_shard",
            "chance": 0.3
          }
        ]
      },

      "sprite": "sprite_memory_fragment",
      "size": "small"
    },

    {
      "id": "boss_forgotten_wall",
      "name_key": "boss_forgotten_wall_name",
      "description_key": "boss_forgotten_wall_desc",

      "type": "boss",
      "category": "defense_mechanism",

      "stats": {
        "hp": 800,
        "sp": 100,
        "atk": 18,
        "def": 15,
        "mag": 12,
        "res": 10,
        "spd": 40,
        "luk": 8
      },

      "phases": [
        {
          "hp_threshold": 1.0,
          "weaknesses": ["emotional"],
          "skills": ["skill_boss_summon", "skill_memory_barrage"]
        },
        {
          "hp_threshold": 0.6,
          "weaknesses": ["physical"],
          "skills": ["skill_boss_defense_mode", "skill_memory_lockdown"],
          "state_changes": {
            "def": 25
          }
        },
        {
          "hp_threshold": 0.3,
          "weaknesses": [],
          "skills": ["skill_boss_berserk", "skill_complete_forgetting"],
          "state_changes": {
            "atk": 30
          }
        }
      ],

      "rewards": {
        "exp": 800,
        "money": 500,
        "guaranteed_items": ["item_memory_core"]
      },

      "sprite": "sprite_boss_forgotten_wall",
      "size": "large",
      "battle_bgm": "bgm_boss_01"
    }
  ]
}
```

### 7. 任务配置 (missions.json)

```json
{
  "missions": [
    {
      "id": "mission_01",
      "name_key": "mission_01_name",
      "description_key": "mission_01_desc",

      "unlock_day": 10,
      "deadline_day": 15,
      "required_event": "event_main_story_day10",

      "target": {
        "type": "save_victim",
        "target_character": "npc_victim_01"
      },

      "phases": [
        {
          "id": "phase_investigation",
          "name_key": "mission_01_phase1_name",
          "objectives": [
            {
              "type": "collect_clues",
              "required_count": 3,
              "clue_events": [
                "event_mission01_clue1",
                "event_mission01_clue2",
                "event_mission01_clue3"
              ]
            }
          ]
        },
        {
          "id": "phase_exploration",
          "name_key": "mission_01_phase2_name",
          "objectives": [
            {
              "type": "enter_cognitive_space",
              "space_id": "cognitive_space_01"
            },
            {
              "type": "solve_puzzle",
              "puzzle_id": "puzzle_01"
            }
          ]
        },
        {
          "id": "phase_confrontation",
          "name_key": "mission_01_phase3_name",
          "objectives": [
            {
              "type": "defeat_boss",
              "boss_id": "boss_forgotten_wall"
            }
          ]
        }
      ],

      "rewards": {
        "exp": 500,
        "money": 1000,
        "items": ["item_memory_key"],
        "unlock_events": ["event_char02_unlock"],
        "unlock_area": "area_downtown"
      },

      "failure_consequences": {
        "character_death": "npc_victim_01",
        "story_branch": "bad_ending_path",
        "set_flags": ["mission_01_failed"]
      }
    }
  ]
}
```

### 8. 游戏配置 (game_config.json)

```json
{
  "game_config": {
    "version": "1.0.0",

    "time_system": {
      "total_days": 100,
      "time_slots_per_day": 3,
      "time_slot_names": ["morning", "afternoon", "night"]
    },

    "attribute_system": {
      "max_level": 10,
      "exp_curve": [0, 100, 200, 400, 700, 1200, 2000, 3200, 5000, 8000],
      "attribute_types": ["knowledge", "courage", "charm", "combat"]
    },

    "relationship_system": {
      "max_level": 10,
      "exp_per_level": 100,
      "decay_rate": 0,
      "max_characters": 8
    },

    "battle_system": {
      "max_party_size": 3,
      "escape_base_chance": 0.5,
      "critical_damage_multiplier": 1.5
    },

    "economy": {
      "starting_money": 500,
      "max_money": 999999
    },

    "difficulty_settings": {
      "easy": {
        "enemy_hp_multiplier": 0.7,
        "enemy_damage_multiplier": 0.7,
        "money_multiplier": 1.2
      },
      "normal": {
        "enemy_hp_multiplier": 1.0,
        "enemy_damage_multiplier": 1.0,
        "money_multiplier": 1.0
      },
      "hard": {
        "enemy_hp_multiplier": 1.5,
        "enemy_damage_multiplier": 1.3,
        "money_multiplier": 0.8
      }
    }
  }
}
```

---

## 运行时数据

### GameState (游戏运行时状态)

```json
{
  "game_state": {
    "session_id": "uuid-here",
    "game_version": "1.0.0",
    "difficulty": "normal",
    "playtime_seconds": 3600,

    "time": {
      "current_day": 25,
      "current_time_slot": "afternoon",
      "is_weekend": false
    },

    "player": {
      "name": "Player Name",
      "attributes": {
        "knowledge": {
          "level": 4,
          "exp": 350,
          "exp_to_next": 700
        },
        "courage": {
          "level": 3,
          "exp": 200,
          "exp_to_next": 400
        },
        "charm": {
          "level": 5,
          "exp": 600,
          "exp_to_next": 1200
        },
        "combat": {
          "level": 6,
          "exp": 1000,
          "exp_to_next": 2000
        }
      },

      "battle_stats": {
        "hp": 180,
        "max_hp": 180,
        "sp": 75,
        "max_sp": 75,
        "atk": 28,
        "def": 16,
        "mag": 20,
        "res": 15,
        "spd": 60,
        "luk": 10
      },

      "skills": [
        "skill_basic_attack",
        "skill_lightning_strike",
        "skill_memory_resonance",
        "skill_heal"
      ],

      "equipment": {
        "weapon": "item_weapon_basic_sword",
        "armor": "item_armor_school_uniform",
        "accessory": "item_acc_lucky_charm"
      }
    },

    "characters": {
      "char_01": {
        "unlocked": true,
        "alive": true,
        "relationship_level": 6,
        "relationship_exp": 50,
        "location": "school_courtyard",
        "availability": true,
        "last_interaction_day": 24
      },
      "char_02": {
        "unlocked": true,
        "alive": true,
        "relationship_level": 4,
        "relationship_exp": 80,
        "location": "library",
        "availability": true,
        "last_interaction_day": 22
      }
    },

    "inventory": {
      "money": 1250,
      "items": [
        {
          "item_id": "item_health_potion",
          "quantity": 5
        },
        {
          "item_id": "item_sp_drink",
          "quantity": 3
        },
        {
          "item_id": "item_memory_core",
          "quantity": 1
        }
      ]
    },

    "events": {
      "completed_events": [
        "event_main_story_day10",
        "event_study_library",
        "event_hangout_char01"
      ],
      "active_cooldowns": {
        "event_hangout_char01": 2
      }
    },

    "flags": {
      "tutorial_completed": true,
      "first_mission_completed": true,
      "agreed_to_help": true,
      "char_01_romance_unlocked": false
    },

    "missions": {
      "active_missions": [
        {
          "mission_id": "mission_02",
          "current_phase": "phase_investigation",
          "progress": {
            "clues_collected": 2,
            "required_clues": 3
          },
          "days_remaining": 8
        }
      ],
      "completed_missions": ["mission_01"],
      "failed_missions": []
    }
  }
}
```

---

## 存档数据

### SaveData (存档文件格式)

```json
{
  "save_data": {
    "metadata": {
      "save_slot": 1,
      "save_time": "2025-11-05T10:30:00Z",
      "game_version": "1.0.0",
      "playtime_seconds": 3600,
      "current_day": 25,
      "current_time_slot": "afternoon",
      "save_location": "school",
      "player_name": "Player Name",
      "is_autosave": false
    },

    "game_state": {
      "...": "完整的 GameState 数据"
    },

    "statistics": {
      "battles_won": 45,
      "battles_lost": 2,
      "puzzles_solved": 8,
      "events_completed": 67,
      "total_damage_dealt": 12500,
      "total_damage_taken": 4200,
      "endings_unlocked": ["ending_normal_01"]
    },

    "new_game_plus": {
      "is_new_game_plus": false,
      "previous_playthroughs": 0,
      "carry_over_data": null
    }
  }
}
```

---

## 数据验证

### 验证规则

#### 事件配置验证

```typescript
// 伪代码
function validateEvent(event: Event): ValidationResult {
  // 必填字段
  if (!event.id || !event.type) {
    return error("Missing required fields");
  }

  // ID格式
  if (!/^event_[a-z0-9_]+$/.test(event.id)) {
    return error("Invalid event ID format");
  }

  // 日期范围
  if (event.conditions.day_range) {
    if (event.conditions.day_range.min < 1 ||
        event.conditions.day_range.max > 100) {
      return error("Day range must be between 1-100");
    }
  }

  // 引用完整性
  if (event.related_character) {
    if (!characterExists(event.related_character)) {
      return error(`Character ${event.related_character} not found`);
    }
  }

  return success();
}
```

### 数据工具

建议创建以下开发工具：

1. **数据验证器** - 启动时检查所有JSON配置
2. **事件编辑器** - 可视化编辑事件和对话
3. **数据查看器** - 查看和调试运行时数据
4. **存档编辑器** - 测试用，快速修改存档

---

## 示例数据集

为了快速原型开发，提供以下最小数据集：

### MVP数据集内容

- **角色**: 3个（主角 + 2个队友）
- **事件**: 20个（5个主线，10个日常活动，5个社交）
- **对话**: 15个场景
- **技能**: 10个
- **道具**: 5种消耗品，3种装备
- **敌人**: 5种（2种小怪，2种精英，1个BOSS）
- **任务**: 1个完整任务

这足以支撑Day 1-15的完整体验。

---

## 本地化策略

### 文本键命名规范

```
<category>_<id>_<field>

示例:
- char_01_name
- char_01_desc
- event_study_library_name
- event_study_library_desc
- dialogue_main_day10_node01
- skill_lightning_name
```

### 本地化文件示例 (en.json)

```json
{
  "characters": {
    "char_01_name": "Akira",
    "char_01_desc": "Your energetic childhood friend."
  },
  "events": {
    "event_study_library_name": "Study at Library",
    "event_study_library_desc": "Hit the books and improve your knowledge."
  },
  "ui": {
    "button_continue": "Continue",
    "button_new_game": "New Game",
    "label_day": "Day {0}",
    "label_money": "Money: ${0}"
  }
}
```

---

**文档状态**: ✅ 完成
**下一步**: 创建示例数据文件

