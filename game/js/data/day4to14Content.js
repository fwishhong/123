/**
 * Day 4-14 剧情内容
 */

export const day4to14Dialogues = {
    // Day 4 - 能力测试
    'day4_ability_test': {
        id: 'day4_ability_test',
        title: 'Day 4 - 能力觉醒',
        location: '认知空间',
        autoTrigger: true,
        conditions: { day: 4, flags: ['tutorial_complete'] },
        dialogue: [
            {
                speaker: 'mysterious_voice',
                text: '你做得不错。现在，让我教你更多...',
                image: null
            },
            {
                speaker: 'player',
                text: '你到底是谁？为什么要帮我？',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: '我的身份...现在还不是时候告诉你。',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: '但我可以告诉你——认知崩溃事件，不是偶然。',
                image: null
            },
            {
                speaker: 'player',
                text: '什么意思？',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: 'NeuroCorp正在进行一个秘密计划...集体意识计划。',
                image: null
            },
            {
                speaker: 'player',
                text: '集体意识？',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: '将所有人类的意识连接在一起...听起来很美好，对吗？',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: '但代价是——失去自我，失去自由意志。',
                image: null
            },
            {
                speaker: 'player',
                text: '那些认知崩溃的人...是实验品？',
                image: null
            },
            {
                speaker: 'mysterious_voice',
                text: '正是。而你拥有的能力...可以拯救他们。',
                image: null
            }
        ],
        rewards: {
            flags: ['collective_consciousness_revealed'],
            skills: ['skill_power_strike']
        }
    },

    // Day 5 - 遇见零
    'day5_meet_zero': {
        id: 'day5_meet_zero',
        title: 'Day 5 - 黑客',
        location: '地下网吧',
        conditions: { day: 5, period: 2 },
        dialogue: [
            {
                speaker: null,
                text: '你根据线索来到一个地下网吧。',
                image: null
            },
            {
                speaker: null,
                text: '角落里，一个穿着黑色连帽衫的身影在敲击键盘。',
                image: null
            },
            {
                speaker: 'zero',
                text: '你就是那个"世界线观测者"？',
                image: 'zero_typing'
            },
            {
                speaker: 'player',
                text: '你是谁？你怎么知道...？',
                image: null
            },
            {
                speaker: 'zero',
                text: '我的代号是零。我入侵了NeuroCorp的数据库。',
                image: 'zero_smirk'
            },
            {
                speaker: 'zero',
                text: '他们在寻找一个特殊的个体...就是你。',
                image: 'zero_serious'
            },
            {
                speaker: 'player',
                text: '他们知道我的能力？',
                image: null
            },
            {
                speaker: 'zero',
                text: '还不完全确定。但他们很快就会发现。',
                image: 'zero_warning'
            },
            {
                speaker: 'zero',
                text: '我可以帮你...如果你也帮我。',
                choices: [
                    { text: '接受合作', next: 'accept', relationshipChange: 2 },
                    { text: '先观察一下', next: 'wait', relationshipChange: 0 }
                ]
            }
        ],
        branches: {
            'accept': [
                {
                    speaker: 'player',
                    text: '好，我们合作。',
                    image: null
                },
                {
                    speaker: 'zero',
                    text: '明智的选择。这是加密通讯器，有情报我会联系你。',
                    image: 'zero_smile'
                }
            ],
            'wait': [
                {
                    speaker: 'player',
                    text: '我需要先了解你的目的。',
                    image: null
                },
                {
                    speaker: 'zero',
                    text: '谨慎是好事...但时间不多了。',
                    image: 'zero_serious'
                }
            ]
        },
        rewards: {
            flags: ['met_zero'],
            unlockActivities: ['hacking_lessons', 'underground_info']
        }
    },

    // Day 7 - 遇见美月
    'day7_meet_mizuki': {
        id: 'day7_meet_mizuki',
        title: 'Day 7 - 学生会长的邀请',
        location: '学生会室',
        conditions: { day: 7, period: 1 },
        dialogue: [
            {
                speaker: 'mizuki',
                text: '你好，我是学生会长，风见美月。',
                image: 'mizuki_elegant'
            },
            {
                speaker: 'player',
                text: '学生会长？有什么事吗？',
                image: null
            },
            {
                speaker: 'mizuki',
                text: '我听说...你似乎对认知崩溃事件很感兴趣。',
                image: 'mizuki_knowing'
            },
            {
                speaker: 'player',
                text: '...你怎么知道？',
                image: null
            },
            {
                speaker: 'mizuki',
                text: '学生会需要了解校园里的一切。',
                image: 'mizuki_smile'
            },
            {
                speaker: 'mizuki',
                text: '我的家族...和NeuroCorp有些联系。我可以提供情报。',
                image: 'mizuki_serious'
            },
            {
                speaker: 'player',
                text: '代价是什么？',
                image: null
            },
            {
                speaker: 'mizuki',
                text: '聪明。我需要你...保护某个人。',
                image: 'mizuki_worried'
            },
            {
                speaker: 'mizuki',
                text: '学生会的副会长，她最近行为异常...可能是认知崩溃的前兆。',
                image: 'mizuki_concerned'
            }
        ],
        rewards: {
            flags: ['met_mizuki', 'mission_02_unlocked'],
            unlockActivities: ['student_council']
        }
    },

    // Day 10 - 第一次拯救准备
    'day10_mission_prep': {
        id: 'day10_mission_prep',
        title: 'Day 10 - 准备行动',
        location: '秘密基地',
        conditions: { day: 10, flags: ['met_zero'] },
        dialogue: [
            {
                speaker: 'zero',
                text: '我找到了Day 15目标的认知空间坐标。',
                image: 'zero_typing'
            },
            {
                speaker: 'player',
                text: '那个认知崩溃的同学...他现在怎么样？',
                image: null
            },
            {
                speaker: 'zero',
                text: '昏迷状态。如果再不救他，意识会彻底消散。',
                image: 'zero_serious'
            },
            {
                speaker: 'akatsuki',
                text: '那我们还等什么？现在就去！',
                image: 'akatsuki_determined'
            },
            {
                speaker: 'player',
                text: '等等...我们需要准备。',
                image: null
            },
            {
                speaker: 'zero',
                text: '他说得对。认知空间里的守卫很强。',
                image: 'zero_warning'
            },
            {
                speaker: 'zero',
                text: '你至少需要战斗力Lv2，还要准备一些道具。',
                image: 'zero_advice'
            },
            {
                speaker: 'akatsuki',
                text: '那我们赶紧训练！还有5天时间。',
                image: 'akatsuki_smile'
            }
        ],
        rewards: {
            flags: ['mission_01_briefed'],
            items: ['item_heal_potion', 'item_energy_drink']
        }
    },

    // Day 12 - 晓的决心
    'day12_akatsuki_resolve': {
        id: 'day12_akatsuki_resolve',
        title: 'Day 12 - 晓的训练',
        location: '训练场',
        conditions: { day: 12, relationshipLevel: { char_akatsuki: 3 } },
        dialogue: [
            {
                speaker: 'akatsuki',
                text: '呼...呼...再来一次！',
                image: 'akatsuki_sweating'
            },
            {
                speaker: 'player',
                text: '晓，你已经练了三个小时了，休息一下吧。',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '不行！Day 15就要到了，我不能拖后腿！',
                image: 'akatsuki_determined'
            },
            {
                speaker: 'player',
                text: '你已经很努力了...',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '不够...我想保护你，就像你保护我一样。',
                image: 'akatsuki_serious'
            },
            {
                speaker: 'player',
                text: '晓...',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '从小时候开始，你就一直保护我。',
                image: 'akatsuki_nostalgic'
            },
            {
                speaker: 'akatsuki',
                text: '现在轮到我了...让我和你并肩作战！',
                image: 'akatsuki_determined'
            },
            {
                speaker: 'player',
                text: '...好。那我们一起变强。',
                choices: [
                    { text: '陪她继续训练', next: 'train_together', relationshipChange: 3 },
                    { text: '劝她休息', next: 'rest', relationshipChange: 1 }
                ]
            }
        ],
        branches: {
            'train_together': [
                {
                    speaker: 'player',
                    text: '来，我陪你练。',
                    image: null
                },
                {
                    speaker: 'akatsuki',
                    text: '真的？！好！',
                    image: 'akatsuki_happy'
                },
                {
                    speaker: null,
                    text: '你们一起训练到深夜...\n\n晓的实力提升了！',
                    image: null
                }
            ],
            'rest': [
                {
                    speaker: 'player',
                    text: '过度训练会受伤的。明天再继续。',
                    image: null
                },
                {
                    speaker: 'akatsuki',
                    text: '...好吧，听你的。',
                    image: 'akatsuki_pout'
                }
            ]
        },
        rewards: {
            flags: ['akatsuki_trained'],
            combat: 2
        }
    },

    // Day 14 - 决战前夜
    'day14_eve_of_battle': {
        id: 'day14_eve_of_battle',
        title: 'Day 14 - 决战前夜',
        location: '屋顶',
        autoTrigger: true,
        conditions: { day: 14, period: 2 },
        dialogue: [
            {
                speaker: null,
                text: 'Day 14，夜晚。\n\n明天就是Day 15...第一次拯救任务。',
                image: null
            },
            {
                speaker: 'player',
                text: '（一切准备就绪了吗？）',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '睡不着吗？',
                image: 'akatsuki_smile'
            },
            {
                speaker: 'player',
                text: '晓？你怎么在这里？',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '我也睡不着...在想明天的事。',
                image: 'akatsuki_nervous'
            },
            {
                speaker: 'player',
                text: '你不用去的...太危险了。',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '我说过了，我要和你并肩作战！',
                image: 'akatsuki_determined'
            },
            {
                speaker: 'akatsuki',
                text: '而且...我相信你。你一定能拯救那个同学的。',
                image: 'akatsuki_smile'
            },
            {
                speaker: 'player',
                text: '...谢谢你，晓。',
                image: null
            },
            {
                speaker: 'akatsuki',
                text: '嗯！那么...约定好了。',
                image: 'akatsuki_happy'
            },
            {
                speaker: 'akatsuki',
                text: '明天，我们一起，平安回来！',
                image: 'akatsuki_determined'
            },
            {
                speaker: null,
                text: '星空下，你们立下了约定。\n\nDay 15，即将到来。',
                image: null
            }
        ],
        rewards: {
            flags: ['mission_01_ready'],
            relationship: { char_akatsuki: 2 }
        }
    },

    // 零的事件
    'zero_event_lv2': {
        id: 'zero_event_lv2',
        title: '零的过去',
        location: '地下网吧',
        conditions: { relationshipLevel: { char_zero: 2 } },
        dialogue: [
            {
                speaker: 'zero',
                text: '你想知道我为什么要对抗NeuroCorp？',
                image: 'zero_serious'
            },
            {
                speaker: 'player',
                text: '如果你愿意说的话。',
                image: null
            },
            {
                speaker: 'zero',
                text: '我的姐姐...三年前是NeuroCorp的研究员。',
                image: 'zero_sad'
            },
            {
                speaker: 'player',
                text: '...是吗。',
                image: null
            },
            {
                speaker: 'zero',
                text: '她发现了集体意识计划的秘密，想要揭发。',
                image: 'zero_angry'
            },
            {
                speaker: 'zero',
                text: '然后...她"意外"死了。',
                image: 'zero_sad'
            },
            {
                speaker: 'player',
                text: '对不起...',
                image: null
            },
            {
                speaker: 'zero',
                text: '所以我要揭露真相，摧毁这个邪恶的计划。',
                image: 'zero_determined'
            },
            {
                speaker: 'zero',
                text: '这就是我帮助你的理由。',
                image: 'zero_serious'
            }
        ],
        rewards: {
            flags: ['zero_backstory_revealed'],
            items: ['item_energy_drink', 'item_energy_drink']
        }
    },

    // 美月的事件
    'mizuki_event_lv2': {
        id: 'mizuki_event_lv2',
        title: '美月的秘密',
        location: '学生会室',
        conditions: { relationshipLevel: { char_mizuki: 2 } },
        dialogue: [
            {
                speaker: 'mizuki',
                text: '你知道为什么我的家族和NeuroCorp有关系吗？',
                image: 'mizuki_serious'
            },
            {
                speaker: 'player',
                text: '不知道。',
                image: null
            },
            {
                speaker: 'mizuki',
                text: '我的父亲...是NeuroCorp的董事之一。',
                image: 'mizuki_troubled'
            },
            {
                speaker: 'player',
                text: '什么？！',
                image: null
            },
            {
                speaker: 'mizuki',
                text: '但我反对他们的计划。集体意识...太可怕了。',
                image: 'mizuki_determined'
            },
            {
                speaker: 'mizuki',
                text: '所以我在暗中调查，收集证据。',
                image: 'mizuki_serious'
            },
            {
                speaker: 'player',
                text: '你一个人...不危险吗？',
                image: null
            },
            {
                speaker: 'mizuki',
                text: '危险...但必须有人这么做。',
                image: 'mizuki_smile'
            },
            {
                speaker: 'mizuki',
                text: '现在有你了...我不是一个人。',
                image: 'mizuki_grateful'
            }
        ],
        rewards: {
            flags: ['mizuki_backstory_revealed'],
            money: 500
        }
    }
};

// 更多日常小事件
export const dailyEvents = {
    'random_combat_encounter': {
        id: 'random_combat_encounter',
        title: '野外遭遇',
        location: '认知空间边缘',
        dialogue: [
            {
                speaker: null,
                text: '认知空间中，一个阴影怪物出现了！',
                image: null
            }
        ],
        rewards: {
            triggerBattle: 'enemy_shadow',
            money: 200
        }
    },

    'find_money': {
        id: 'find_money',
        title: '意外之财',
        location: '街道',
        dialogue: [
            {
                speaker: null,
                text: '你在路上捡到了一些钱。',
                image: null
            }
        ],
        rewards: {
            money: 300
        }
    },

    'skill_training': {
        id: 'skill_training',
        title: '技能顿悟',
        location: '训练场',
        dialogue: [
            {
                speaker: null,
                text: '通过刻苦训练，你掌握了新的技能！',
                image: null
            }
        ],
        rewards: {
            skills: ['skill_power_strike']
        }
    }
};
