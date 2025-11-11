/**
 * 游戏数据 - 活动、事件、任务
 */

// 活动列表
export const activities = [
    {
        id: 'study_library',
        name: '图书馆学习',
        description: '在图书馆学习，提升知识',
        conditions: {
            time_period: 1 // 下午
        },
        effects: {
            attributes: {
                knowledge: 5
            }
        }
    },
    {
        id: 'train_combat',
        name: '战斗训练',
        description: '进行战斗训练，提升战斗力',
        conditions: {
            time_period: 1 // 下午
        },
        effects: {
            attributes: {
                combat: 5
            }
        }
    },
    {
        id: 'social_activity',
        name: '社交活动',
        description: '参加社交活动，提升魅力',
        conditions: {
            time_period: 2 // 夜晚
        },
        effects: {
            attributes: {
                charm: 5
            }
        }
    },
    {
        id: 'adventure_activity',
        name: '冒险活动',
        description: '尝试冒险活动，提升勇气',
        conditions: {
            time_period: 2 // 夜晚
        },
        effects: {
            attributes: {
                courage: 5
            }
        }
    },
    {
        id: 'cafe_rest',
        name: '咖啡厅休息',
        description: '在咖啡厅放松，恢复精力',
        conditions: {},
        effects: {
            attributes: {
                charm: 2
            }
        }
    },
    {
        id: 'early_morning_run',
        name: '晨跑',
        description: '早晨跑步锻炼身体',
        conditions: {
            time_period: 0 // 早晨
        },
        effects: {
            attributes: {
                combat: 3,
                courage: 2
            }
        }
    },
    {
        id: 'lecture_attend',
        name: '参加讲座',
        description: '听取学术讲座，增长见识',
        conditions: {
            time_period: 1
        },
        effects: {
            attributes: {
                knowledge: 4,
                charm: 1
            }
        }
    },
    {
        id: 'explore_city',
        name: '探索城市',
        description: '探索新东京的街道',
        conditions: {
            time_period: 2
        },
        effects: {
            attributes: {
                courage: 3,
                knowledge: 2
            }
        }
    },
    {
        id: 'meditation',
        name: '冥想训练',
        description: '进行认知空间冥想训练',
        conditions: {
            required_attributes: {
                knowledge: 2
            }
        },
        effects: {
            attributes: {
                knowledge: 3,
                combat: 2
            }
        }
    },
    {
        id: 'advanced_combat',
        name: '高级战斗训练',
        description: '更高强度的战斗训练',
        conditions: {
            required_attributes: {
                combat: 3,
                courage: 2
            }
        },
        effects: {
            attributes: {
                combat: 7,
                courage: 3
            }
        }
    }
];

// 角色列表
export const characters = [
    {
        id: 'char_akatsuki',
        name: '晓',
        title: '青梅竹马',
        avatar: '🌸',
        description: '从小一起长大的青梅竹马，开朗活泼'
    },
    {
        id: 'char_zero',
        name: '零',
        title: '神秘黑客',
        avatar: '💻',
        description: '神秘的黑客，掌握着许多秘密'
    },
    {
        id: 'char_mizuki',
        name: '美月',
        title: '学生会长',
        avatar: '👑',
        description: '优雅的学生会长，背后隐藏着秘密'
    },
    {
        id: 'char_takuya',
        name: '拓也',
        avatar: '⚡',
        title: '热血运动员',
        description: '性格开朗的运动健将'
    },
    {
        id: 'char_sakura',
        name: '咲良',
        title: '天才少女',
        avatar: '🔬',
        description: '天才科学家，研究认知空间'
    },
    {
        id: 'char_rin',
        name: '凛',
        title: '冷酷杀手',
        avatar: '🗡️',
        description: '神秘的能力者，目的不明'
    },
    {
        id: 'char_yuma',
        name: '悠真',
        title: '艺术家',
        avatar: '🎨',
        description: '敏感的艺术家，能感知认知空间'
    },
    {
        id: 'char_hibiki',
        name: '响',
        title: '音乐家',
        avatar: '🎵',
        description: '才华横溢的音乐家'
    }
];

// 任务列表
export const missions = [
    {
        id: 'mission_01',
        name: '第一次拯救',
        description: '拯救第一个认知崩溃受害者',
        deadline: 15,
        requirements: {
            attributes: {
                combat: 2
            }
        }
    },
    {
        id: 'mission_02',
        name: '学生会的危机',
        description: '调查学生会成员的认知崩溃',
        deadline: 25,
        requirements: {
            attributes: {
                knowledge: 3,
                combat: 3
            }
        }
    },
    {
        id: 'mission_03',
        name: '教师的秘密',
        description: '解救认知崩溃的教师',
        deadline: 35,
        requirements: {
            attributes: {
                knowledge: 4,
                courage: 3
            }
        }
    },
    {
        id: 'mission_04',
        name: '潜入NeuroCorp',
        description: '潜入NeuroCorp实验室调查真相',
        deadline: 45,
        requirements: {
            attributes: {
                combat: 5,
                courage: 4,
                knowledge: 4
            }
        }
    },
    {
        id: 'mission_05',
        name: '阻止叛徒',
        description: '阻止内部叛徒的计划',
        deadline: 70,
        requirements: {
            attributes: {
                combat: 6,
                courage: 5
            }
        }
    },
    {
        id: 'mission_06',
        name: '最终决战',
        description: '阻止集体意识计划',
        deadline: 100,
        requirements: {
            attributes: {
                combat: 8,
                knowledge: 7,
                courage: 7,
                charm: 6
            }
        }
    }
];

// 敌人列表
export const enemies = [
    {
        id: 'enemy_shadow',
        name: '阴影怪物',
        maxHP: 50,
        attack: 10,
        defense: 5,
        sprite: '👥'
    },
    {
        id: 'enemy_nightmare',
        name: '噩梦实体',
        maxHP: 80,
        attack: 15,
        defense: 8,
        sprite: '😈'
    },
    {
        id: 'enemy_guardian',
        name: '认知守卫',
        maxHP: 120,
        attack: 20,
        defense: 15,
        sprite: '🛡️'
    },
    {
        id: 'enemy_final_boss',
        name: '集体意识核心',
        maxHP: 300,
        attack: 35,
        defense: 25,
        sprite: '🌀'
    }
];

// 事件列表（示例）
export const events = [
    {
        id: 'event_intro_01',
        name: '游戏开始',
        type: 'story',
        conditions: {
            date_range: [1, 1]
        },
        once: true
    }
];
