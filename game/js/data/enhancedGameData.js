/**
 * 游戏扩展数据 - 剧情、技能、道具
 * 用于垂直切片 Day 1-15
 */

// ========== 技能系统 ==========
export const skills = [
    {
        id: 'skill_normal_attack',
        name: '普通攻击',
        description: '基础攻击',
        cost: 0,
        power: 1.0,
        type: 'physical'
    },
    {
        id: 'skill_power_strike',
        name: '强力一击',
        description: '造成150%伤害',
        cost: 10,
        power: 1.5,
        type: 'physical',
        unlockLevel: 3
    },
    {
        id: 'skill_mind_blast',
        name: '精神冲击',
        description: '造成200%伤害的精神攻击',
        cost: 20,
        power: 2.0,
        type: 'mental',
        unlockLevel: 5
    },
    {
        id: 'skill_analyze',
        name: '分析弱点',
        description: '降低敌人防御',
        cost: 15,
        effect: 'reduce_defense',
        type: 'support',
        unlockLevel: 4
    },
    {
        id: 'skill_heal',
        name: '认知修复',
        description: '恢复30%HP',
        cost: 25,
        healing: 0.3,
        type: 'support',
        unlockLevel: 6
    },
    {
        id: 'skill_ultimate',
        name: '认知觉醒',
        description: '造成300%伤害的终极攻击',
        cost: 50,
        power: 3.0,
        type: 'ultimate',
        unlockLevel: 8
    }
];

// ========== 道具系统 ==========
export const items = [
    {
        id: 'item_heal_potion',
        name: '恢复药剂',
        description: '恢复50HP',
        type: 'consumable',
        effect: 'heal',
        value: 50,
        price: 100,
        icon: '🧪'
    },
    {
        id: 'item_energy_drink',
        name: '能量饮料',
        description: '恢复20SP',
        type: 'consumable',
        effect: 'restore_sp',
        value: 20,
        price: 80,
        icon: '☕'
    },
    {
        id: 'item_attack_boost',
        name: '攻击强化剂',
        description: '本场战斗攻击力+50%',
        type: 'consumable',
        effect: 'boost_attack',
        value: 0.5,
        price: 150,
        icon: '💊'
    },
    {
        id: 'item_defense_boost',
        name: '防御强化剂',
        description: '本场战斗防御力+50%',
        type: 'consumable',
        effect: 'boost_defense',
        value: 0.5,
        price: 150,
        icon: '🛡️'
    },
    {
        id: 'item_revive',
        name: '复活药剂',
        description: '战斗失败时自动使用，恢复到50%HP',
        type: 'consumable',
        effect: 'revive',
        value: 0.5,
        price: 500,
        icon: '✨'
    }
];

// ========== 对话事件 ==========
export const dialogueEvents = {
    // Day 1 - 游戏开始
    'day1_morning': {
        id: 'day1_morning',
        title: 'Day 1 - 觉醒',
        location: '卧室',
        autoTrigger: true,
        conditions: { day: 1, period: 0 },
        dialogue: [
            {
                speaker: null,
                text: '2045年11月1日，早晨6:30。',
                image: null
            },
            {
                speaker: null,
                text: '你从梦中醒来。\n\n头痛欲裂。',
                image: null
            },
            {
                speaker: 'player',
                text: '（又是那个梦...为什么我总是梦到那些奇怪的景象？）',
                image: null
            },
            {
                speaker: null,
                text: '你的MindLink设备在耳后微微发烫。\n\n这是第三次出现这种异常了。',
                image: null
            },
            {
                speaker: 'player',
                text: '（难道是设备故障？但诊断程序显示一切正常...）',
                image: null
            },
            {
                speaker: null,
                text: '突然，你的右手背出现了一道淡蓝色的光芒。\n\n三条交织的线条，像是某种符文。',
                image: null
            },
            {
                speaker: 'player',
                text: '这是...什么？！',
                image: null
            },
            {
                speaker: null,
                text: '光芒持续了几秒，然后消失了。\n\n但你知道，有什么改变了。',
                image: null
            },
            {
                speaker: 'player',
                text: '（我需要搞清楚这是怎么回事...）',
                image: null
            }
        ],
        rewards: {
            flags: ['awakening_complete']
        }
    },

    // Day 1 - 遇见晓
    'day1_meet_akatsuki': {
        id: 'day1_meet_akatsuki',
        title: '青梅竹马',
        location: '学校门口',
        conditions: { day: 1, flags: ['awakening_complete'] },
        dialogue: [
            {
                speaker: null,
                text: '学校门口，你看到一个熟悉的身影。',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '早啊！又睡过头了吧？',
                image: 'akatsuki_smile'
            },
            {
                speaker: 'player',
                text: '晓...没有，只是起晚了一点。',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '哈哈，还是老样子。对了，你的脸色看起来不太好？',
                image: 'akatsuki_concern'
            },
            {
                speaker: null,
                text: '晓，你的青梅竹马。\n\n从小一起长大的邻居，总是这么开朗活泼。',
                image: null
            },
            {
                speaker: 'player',
                text: '没什么，只是...做了个奇怪的梦。',
                choices: [
                    { text: '告诉她手背上的光芒', next: 'tell_truth', relationshipChange: 2 },
                    { text: '隐瞒这件事', next: 'hide_truth', relationshipChange: 0 }
                ]
            }
        ],
        branches: {
            'tell_truth': [
                {
                    speaker: 'player',
                    text: '其实...今天早上发生了一件奇怪的事。我的手背上出现了光芒。',
                    image: null
                },
                {
                    speaker: 'akatsuki',
                    text: '光芒？你是说...MindLink的故障吗？',
                    image: 'akatsuki_surprise'
                },
                {
                    speaker: 'player',
                    text: '我也不确定。但感觉不像是故障...',
                    image: null
                },
                {
                    speaker: 'akatsuki',
                    text: '最近新闻上说有很多认知崩溃事件...你不会也...？',
                    image: 'akatsuki_worry'
                },
                {
                    speaker: 'player',
                    text: '应该不是。我感觉很清醒，只是...好像获得了什么能力。',
                    image: null
                },
                {
                    speaker: 'akatsuki',
                    text: '能力？听起来像科幻小说...不过如果你需要帮助，随时告诉我！',
                    image: 'akatsuki_smile'
                },
                {
                    speaker: null,
                    text: '晓的好感度上升了！\n\n（诚实会增进关系）',
                    image: null
                }
            ],
            'hide_truth': [
                {
                    speaker: 'player',
                    text: '就是普通的噩梦而已。',
                    image: null
                },
                {
                    speaker: 'akatsuki',
                    text: '是吗...那好吧。不过你有什么事一定要告诉我哦！',
                    image: 'akatsuki_smile'
                },
                {
                    speaker: 'player',
                    text: '（也许现在还不是说的时候...）',
                    image: null
                }
            ]
        },
        rewards: {
            flags: ['met_akatsuki'],
            unlockActivities: ['hangout_akatsuki']
        }
    },

    // Day 2 - 认知崩溃目击
    'day2_witness_collapse': {
        id: 'day2_witness_collapse',
        title: '第一次目击',
        location: '教室',
        autoTrigger: true,
        conditions: { day: 2, period: 1 },
        dialogue: [
            {
                speaker: null,
                text: 'Day 2，下午。\n\n正常的一天，直到——',
                image: null
            },
            {
                speaker: null,
                text: '教室里突然传来一声尖叫。',
                image: null
            },
            {
                speaker: 'student_a',
                text: '啊啊啊！救命！',
                image: null
            },
            {
                speaker: null,
                text: '一个同学突然倒地，身体剧烈颤抖。\n\n他的眼睛失去焦点，瞳孔散大。',
                image: null
            },
            {
                speaker: 'teacher',
                text: '快叫救护车！这是认知崩溃！',
                image: null
            },
            {
                speaker: 'player',
                text: '（认知崩溃...新闻里说的那个？）',
                image: null
            },
            {
                speaker: null,
                text: '就在这时，你的右手背再次发光。\n\n这次你看清楚了——三条线，每条都指向不同的方向。',
                image: null
            },
            {
                speaker: null,
                text: '你的视野突然改变了。\n\n你能"看到"那个同学周围有黑色的雾气。',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: '——你看到了，对吗？',
                image: null
            },
            {
                speaker: 'player',
                text: '谁？！',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: '认知空间...那是人类内心的世界。\n\n而你，拥有进入那个世界的能力。',
                image: null
            },
            {
                speaker: 'player',
                text: '（这个声音...从哪里来的？）',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: '如果你想拯救他们...就学会使用这份力量吧。',
                image: null
            },
            {
                speaker: null,
                text: '声音消失了。\n\n周围的骚动还在继续。\n\n你知道，你的人生从此改变了。',
                image: null
            }
        ],
        rewards: {
            flags: ['cognitive_space_revealed', 'mysterious_voice_heard'],
            unlockActivities: ['meditation', 'cognitive_training']
        }
    },

    // Day 3 - 教程战斗
    'day3_tutorial_battle': {
        id: 'day3_tutorial_battle',
        title: '第一次进入',
        location: '认知空间',
        conditions: { day: 3, flags: ['cognitive_space_revealed'] },
        dialogue: [
            {
                speaker: null,
                text: 'Day 3，夜晚。\n\n你决定尝试进入认知空间。',
                image: null
            },
            {
                speaker: 'player',
                text: '（专注...就像那个声音说的...）',
                image: null
            },
            {
                speaker: null,
                text: '世界开始扭曲。\n\n你的意识被拉入了另一个维度。',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: '很好。这就是认知空间的入口。',
                image: null
            },
            {
                speaker: 'player',
                text: '这里是...？',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: '你自己的认知空间。在这里，你可以训练你的能力。',
                image: null
            },
            {
                speaker: null,
                text: '一个阴影从黑暗中浮现。',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: '那是你内心的阴影...击败它。',
                image: null
            }
        ],
        rewards: {
            flags: ['tutorial_complete'],
            triggerBattle: 'enemy_shadow',
            items: ['item_heal_potion', 'item_energy_drink']
        }
    },

    // 晓 好感度事件
    'akatsuki_event_lv2': {
        id: 'akatsuki_event_lv2',
        title: '晓的担心',
        location: '咖啡厅',
        conditions: { relationshipLevel: { char_akatsuki: 2 } },
        dialogue: [
            {
                speaker: 'akatsuki',
                text: '最近你好像经常发呆...真的没问题吗？',
                image: 'akatsuki_worry'
            },
            {
                speaker: 'player',
                text: '只是在想一些事情。',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '你知道吗，我们从小一起长大，你的每个表情我都看得出来。',
                image: 'akatsuki_smile'
            },
            {
                speaker: 'player',
                text: '晓...',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '不管发生什么，我都会支持你的。这是约定哦！',
                image: 'akatsuki_determined'
            }
        ],
        rewards: {
            flags: ['akatsuki_promise'],
            items: ['item_heal_potion']
        }
    },

    'akatsuki_event_lv4': {
        id: 'akatsuki_event_lv4',
        title: '晓的过去',
        location: '公园',
        conditions: { relationshipLevel: { char_akatsuki: 4 } },
        dialogue: [
            {
                speaker: 'akatsuki',
                text: '其实...我也有秘密。',
                image: 'akatsuki_serious'
            },
            {
                speaker: 'player',
                text: '秘密？',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '我的父亲...是NeuroCorp的研究员。',
                image: 'akatsuki_sad'
            },
            {
                speaker: 'player',
                text: '什么？！',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '三年前，他失踪了。官方说是实验事故...但我不相信。',
                image: 'akatsuki_determined'
            },
            {
                speaker: 'player',
                text: '所以你一直在调查...？',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '嗯。也许...我们可以一起寻找真相。',
                image: 'akatsuki_smile'
            }
        ],
        rewards: {
            flags: ['akatsuki_backstory_revealed'],
            unlockActivities: ['investigate_neurocorp']
        }
    }
};

// ========== 扩展活动 ==========
export const expandedActivities = [
    // 早晨活动
    {
        id: 'morning_meditation',
        name: '早晨冥想',
        description: '清晨的冥想，提升精神力',
        period: 0,
        effects: { knowledge: 3, combat: 2 },
        conditions: { flags: ['cognitive_space_revealed'] }
    },
    {
        id: 'morning_news',
        name: '阅读新闻',
        description: '了解最新的认知崩溃事件',
        period: 0,
        effects: { knowledge: 4 },
        storyHint: '获取世界观信息'
    },
    {
        id: 'breakfast_with_akatsuki',
        name: '和晓一起吃早餐',
        description: '和青梅竹马共进早餐',
        period: 0,
        effects: { charm: 2 },
        relationship: { char_akatsuki: 1 },
        conditions: { flags: ['met_akatsuki'] }
    },

    // 下午活动
    {
        id: 'library_research',
        name: '图书馆调查',
        description: '调查认知空间的相关资料',
        period: 1,
        effects: { knowledge: 6 },
        conditions: { flags: ['cognitive_space_revealed'] }
    },
    {
        id: 'student_council',
        name: '参加学生会',
        description: '参与学生会活动',
        period: 1,
        effects: { charm: 4, knowledge: 2 },
        relationship: { char_mizuki: 1 }
    },
    {
        id: 'combat_simulation',
        name: '战斗模拟训练',
        description: '在认知空间中进行模拟战斗',
        period: 1,
        effects: { combat: 8 },
        conditions: { flags: ['tutorial_complete'] },
        money: -50
    },
    {
        id: 'part_time_job',
        name: '打工',
        description: '在咖啡厅打工赚钱',
        period: 1,
        effects: { charm: 2 },
        money: 200
    },
    {
        id: 'hacking_lessons',
        name: '黑客课程',
        description: '零教你黑客技术',
        period: 1,
        effects: { knowledge: 5, courage: 2 },
        relationship: { char_zero: 1 },
        conditions: { relationshipLevel: { char_zero: 2 } }
    },

    // 夜晚活动
    {
        id: 'cognitive_exploration',
        name: '认知空间探索',
        description: '深入探索认知空间',
        period: 2,
        effects: { combat: 5, knowledge: 3 },
        conditions: { flags: ['tutorial_complete'] }
    },
    {
        id: 'night_patrol',
        name: '夜间巡逻',
        description: '寻找认知崩溃的受害者',
        period: 2,
        effects: { courage: 5, combat: 3 },
        conditions: { flags: ['tutorial_complete'] }
    },
    {
        id: 'underground_info',
        name: '情报收集',
        description: '从地下渠道收集情报',
        period: 2,
        effects: { knowledge: 4, courage: 3 },
        conditions: { relationshipLevel: { char_zero: 2 } },
        money: -100
    },
    {
        id: 'date_akatsuki',
        name: '约会（晓）',
        description: '和晓一起度过夜晚',
        period: 2,
        effects: { charm: 3 },
        relationship: { char_akatsuki: 2 },
        conditions: { relationshipLevel: { char_akatsuki: 3 } }
    },
    {
        id: 'night_training',
        name: '深夜特训',
        description: '进行高强度训练',
        period: 2,
        effects: { combat: 9, courage: 4 },
        conditions: { attributeLevel: { combat: 5 } }
    },

    // 商店
    {
        id: 'shop_visit',
        name: '访问商店',
        description: '购买道具和装备',
        period: -1, // 任何时间段
        special: 'open_shop'
    },

    // 角色互动
    {
        id: 'talk_zero',
        name: '和零交谈',
        description: '与神秘黑客交流',
        period: 2,
        relationship: { char_zero: 1 },
        conditions: { flags: ['met_zero'] }
    },
    {
        id: 'talk_mizuki',
        name: '和美月交谈',
        description: '与学生会长交流',
        period: 1,
        relationship: { char_mizuki: 1 },
        conditions: { flags: ['met_mizuki'] }
    }
];

// ========== 任务定义 ==========
export const missionDetails = {
    'mission_01': {
        id: 'mission_01',
        name: '第一次拯救',
        description: '拯救认知崩溃的同学',
        deadline: 15,
        unlockDay: 5,
        requirements: {
            combat: 2,
            flags: ['tutorial_complete']
        },
        dungeon: {
            name: '扭曲的教室',
            floors: 3,
            enemies: ['enemy_shadow', 'enemy_shadow', 'enemy_nightmare'],
            boss: 'enemy_guardian_weak',
            rewards: {
                money: 1000,
                items: ['item_heal_potion', 'item_energy_drink', 'item_attack_boost'],
                relationship: { char_akatsuki: 2 }
            }
        },
        storyBefore: 'mission_01_intro',
        storyAfter: 'mission_01_complete'
    }
};

// ========== 任务剧情 ==========
export const missionStories = {
    'mission_01_intro': {
        dialogue: [
            {
                speaker: null,
                text: 'Day 14，夜晚。\n\n明天就是截止日了。',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '你真的要去吗？',
                image: 'akatsuki_worry'
            },
            {
                speaker: 'player',
                text: '我必须去。他还在等我救他。',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '那...我和你一起去！',
                image: 'akatsuki_determined'
            },
            {
                speaker: 'player',
                text: '不行，太危险了。',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '我说过的...不管发生什么，我都会支持你！',
                image: 'akatsuki_smile'
            },
            {
                speaker: 'player',
                text: '...谢谢你，晓。',
                image: null
            },
            {
                speaker: null,
                text: 'Day 15，即将开始。',
                image: null
            }
        ]
    },
    'mission_01_complete': {
        dialogue: [
            {
                speaker: null,
                text: '任务完成。\n\n那个同学被成功救出了。',
                image: null
            },
            {
                speaker: 'student_a',
                text: '谢...谢谢你...',
                image: null
            },
            {
                speaker: 'player',
                text: '没事就好。',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '你做到了！我就知道你可以的！',
                image: 'akatsuki_happy'
            },
            {
                speaker: 'mysterious_voice',
                text: '做得不错。但这只是开始...',
                image: null
            },
            {
                speaker: 'player',
                text: '（还有更多人需要拯救...）',
                image: null
            }
        ]
    }
};
