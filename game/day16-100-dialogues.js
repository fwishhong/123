/**
 * Day 16-100 完整剧情对话数据
 * 基于 STORY_OUTLINE_DAY16-100.md
 */

const day16to100Dialogues = {
    // ========== 第二章：真相浮现 (Day 16-35) ==========

    // Day 16 - 拯救成功的余波
    'day16_aftermath': {
        id: 'day16_aftermath',
        location: '学校',
        day: 16,
        period: 1,
        lines: [
            { speaker: null, text: 'Day 16。第一次拯救任务成功后的第二天。' },
            { speaker: '晓', text: '大家都在议论昨天的事...那个同学醒过来了！', sprite: '👧' },
            { speaker: '你', text: '太好了...我们成功了。' },
            { speaker: '晓', text: '但是...', sprite: '👧' },
            { speaker: '你', text: '怎么了？' },
            { speaker: '晓', text: '学校里又出现了三个认知崩溃的案例。', sprite: '👧' },
            { speaker: '你', text: '什么？！这么快？' },
            { speaker: null, text: '你意识到，这只是开始...' }
        ],
        rewards: { flags: ['mission1_aftermath'], courage: 5 }
    },

    // Day 17 - 零提供新情报
    'day17_zero_intel': {
        id: 'day17_zero_intel',
        location: '地下网吧',
        day: 17,
        period: 2,
        lines: [
            { speaker: '零', text: '我破解了NeuroCorp的内部邮件。', sprite: '👨‍💻' },
            { speaker: '你', text: '发现了什么？' },
            { speaker: '零', text: '他们在加速实验。原计划是Day 100启动，但现在...可能提前。', sprite: '👨‍💻' },
            { speaker: '你', text: '提前？提前到什么时候？' },
            { speaker: '零', text: '我还不确定。但至少...我们的时间不多了。', sprite: '👨‍💻' },
            { speaker: '你', text: '那我们必须加快行动。' },
            { speaker: '零', text: '我会继续收集情报。你...继续提升实力吧。', sprite: '👨‍💻' }
        ],
        rewards: { flags: ['neurocorp_accelerating'], knowledge: 8, relationship: { zero: 2 } }
    },

    // Day 18 - 美月的副会长出现症状
    'day18_vice_president': {
        id: 'day18_vice_president',
        location: '学生会室',
        day: 18,
        period: 1,
        lines: [
            { speaker: '美月', text: '...你能来一趟吗？', sprite: '👩' },
            { speaker: '你', text: '怎么了？你的声音听起来不太对。' },
            { speaker: '美月', text: '是副会长...她开始出现症状了。', sprite: '👩' },
            { speaker: '你', text: '认知崩溃？' },
            { speaker: '美月', text: '嗯。她说自己经常听到奇怪的声音...看到不存在的东西。', sprite: '👩' },
            { speaker: '美月', text: '我需要你的帮助。拜托了。', sprite: '👩' },
            {
                speaker: '你',
                text: '...',
                choices: [
                    { text: '我会帮你的', value: 'accept', relationship: { mizuki: 3 } },
                    { text: '这次可能更危险...', value: 'hesitate', relationship: { mizuki: 1 } }
                ]
            }
        ],
        branches: {
            'accept': [
                { speaker: '你', text: '我会帮你的。我们一起救她。' },
                { speaker: '美月', text: '谢谢...真的，谢谢你。', sprite: '👩' },
                { speaker: null, text: '美月的好感度大幅上升！' }
            ],
            'hesitate': [
                { speaker: '你', text: '这次可能更危险...但我会尽力的。' },
                { speaker: '美月', text: '嗯...我明白。只要你尽力就好。', sprite: '👩' }
            ]
        },
        rewards: { flags: ['mission2_unlocked', 'vice_president_collapse'], unlockActivities: ['prepare_mission2'] }
    },

    // Day 19 - 神秘声音揭示倒计时
    'day19_countdown': {
        id: 'day19_countdown',
        location: '认知空间',
        day: 19,
        period: 2,
        lines: [
            { speaker: '神秘声音', text: '你做得很好...但还不够。' },
            { speaker: '你', text: '你到底是谁？为什么要帮我？' },
            { speaker: '神秘声音', text: '我是...未来。' },
            { speaker: '你', text: '什么意思？' },
            { speaker: '神秘声音', text: '世界线α...世界线β...你现在所在的，是世界线γ。' },
            { speaker: '神秘声音', text: '前两次，我都失败了。这是最后的机会。' },
            { speaker: '你', text: '失败？你是说...这已经发生过两次了？' },
            { speaker: '神秘声音', text: '是的。Day 100，集体意识计划启动。α线，所有人都死了。β线，你活了下来，但晓...', sprite: '👧' },
            { speaker: '你', text: '晓怎么了？！' },
            { speaker: '神秘声音', text: '...她牺牲了。为了救你。' },
            { speaker: '你', text: '...我不会让这发生的。' },
            { speaker: '神秘声音', text: '那就变强吧。只有足够强，才能改变命运。' }
        ],
        rewards: { flags: ['worldline_revealed', 'future_self_hint'], combat: 10, courage: 10 }
    },

    // Day 20 - 决定：主动出击还是防守
    'day20_decision': {
        id: 'day20_decision',
        location: '秘密基地',
        day: 20,
        period: 2,
        lines: [
            { speaker: '零', text: '我们需要做个决定。', sprite: '👨‍💻' },
            { speaker: '晓', text: '什么决定？', sprite: '👧' },
            { speaker: '零', text: '是主动出击，潜入NeuroCorp总部...还是继续防守，救出更多受害者？', sprite: '👨‍💻' },
            { speaker: '美月', text: '主动出击太危险了...我们的实力还不够。', sprite: '👩' },
            { speaker: '晓', text: '但如果一直等下去，会有更多人受害！', sprite: '👧' },
            {
                speaker: '你',
                text: '我认为...',
                choices: [
                    { text: '主动出击，尽快终结这一切', value: 'offensive', flags: ['strategy_offensive'] },
                    { text: '先救人，同时积蓄实力', value: 'defensive', flags: ['strategy_defensive'] }
                ]
            }
        ],
        branches: {
            'offensive': [
                { speaker: '你', text: '我们应该主动出击。拖得越久，变数越多。' },
                { speaker: '零', text: '有魄力。那我会制定潜入计划。', sprite: '👨‍💻' },
                { speaker: '晓', text: '那我们一起战斗！', sprite: '👧' },
                { speaker: null, text: '选择了进攻路线！' }
            ],
            'defensive': [
                { speaker: '你', text: '我们还需要更强。先救人，同时积蓄实力。' },
                { speaker: '美月', text: '我同意。稳扎稳打比较安全。', sprite: '👩' },
                { speaker: '零', text: '明白了。那我会继续收集情报。', sprite: '👨‍💻' },
                { speaker: null, text: '选择了防御路线！' }
            ]
        },
        rewards: { flags: ['strategic_choice_made'], relationship: { zero: 1, akatsuki: 1, mizuki: 1 } }
    },

    // Day 25 - 第二次任务：拯救副会长
    'day25_mission2_start': {
        id: 'day25_mission2_start',
        location: '学生会室认知空间入口',
        day: 25,
        period: 2,
        lines: [
            { speaker: null, text: 'Day 25。第二次拯救任务。' },
            { speaker: '美月', text: '副会长的情况越来越糟糕了...', sprite: '👩' },
            { speaker: '你', text: '我们今天就进去。' },
            { speaker: '晓', text: '我也要去！', sprite: '👧' },
            { speaker: '你', text: '晓，这次可能更危险...'},
            { speaker: '晓', text: '正因为危险，我才更要和你一起！', sprite: '👧' },
            { speaker: '零', text: '我会在外面支援。给你们标记出敌人位置。', sprite: '👨‍💻' },
            { speaker: '你', text: '那...我们出发！' },
            { speaker: null, text: '进入学生会室认知空间...' }
        ],
        rewards: { battle: 'nightmare', flags: ['mission2_started'] }
    },

    'day25_mission2_complete': {
        id: 'day25_mission2_complete',
        location: '学生会室',
        day: 25,
        period: 2,
        afterBattle: true,
        lines: [
            { speaker: null, text: '副会长被成功救出了。' },
            { speaker: '副会长', text: '谢谢...谢谢你们...' },
            { speaker: '美月', text: '你做到了！太好了！', sprite: '👩' },
            { speaker: '晓', text: '我们做到了！', sprite: '👧' },
            { speaker: '你', text: '但是...' },
            { speaker: '零', text: '刚才的战斗暴露了我们的位置。', sprite: '👨‍💻' },
            { speaker: '零', text: 'NeuroCorp已经知道有人在干扰他们的计划了。', sprite: '👨‍💻' },
            { speaker: '你', text: '意思是...他们会开始追踪我们？' },
            { speaker: '零', text: '是的。从现在开始，我们必须更加小心。', sprite: '👨‍💻' }
        ],
        rewards: { flags: ['mission2_complete', 'neurocorp_noticed'], money: 1500, items: ['heal_potion', 'energy_drink', 'attack_boost'], relationship: { mizuki: 3 } }
    },

    // Day 28 - 零的姐姐真相
    'day28_zero_sister': {
        id: 'day28_zero_sister',
        location: '地下网吧',
        day: 28,
        period: 2,
        lines: [
            { speaker: '零', text: '...我找到她了。', sprite: '👨‍💻' },
            { speaker: '你', text: '找到谁？' },
            { speaker: '零', text: '我姐姐。三年前认知崩溃的那个人。', sprite: '👨‍💻' },
            { speaker: '你', text: '她还活着？' },
            { speaker: '零', text: '是...也不是。她的身体死了，但她的意识...', sprite: '👨‍💻' },
            { speaker: '零', text: '被困在一个叫"伊甸园"的地方。', sprite: '👨‍💻' },
            { speaker: '你', text: '伊甸园？' },
            { speaker: '零', text: 'NeuroCorp的核心设施。囚禁所有认知崩溃受害者意识的地方。', sprite: '👨‍💻' },
            { speaker: '零', text: '我必须...救她出来。', sprite: '👨‍💻' },
            { speaker: '你', text: '我会帮你的。' },
            { speaker: '零', text: '...谢谢。', sprite: '👨‍💻' }
        ],
        rewards: { flags: ['zero_sister_revealed', 'eden_discovered'], relationship: { zero: 5 } }
    },

    // Day 30 - 选择：是否向政府求助
    'day30_government': {
        id: 'day30_government',
        location: '秘密基地',
        day: 30,
        period: 2,
        lines: [
            { speaker: '美月', text: '我父亲说...政府已经注意到认知崩溃事件了。', sprite: '👩' },
            { speaker: '晓', text: '政府？他们会帮我们吗？', sprite: '👧' },
            { speaker: '美月', text: '也许。但是...NeuroCorp和政府有很深的联系。', sprite: '👩' },
            { speaker: '零', text: '向政府求助可能会打草惊蛇。', sprite: '👨‍💻' },
            { speaker: '你', text: '但靠我们自己...真的能对抗整个NeuroCorp吗？' },
            {
                speaker: '晓',
                text: '你们觉得呢？',
                sprite: '👧',
                choices: [
                    { text: '向政府寻求帮助', value: 'government', flags: ['seek_government'] },
                    { text: '继续独立行动', value: 'independent', flags: ['stay_independent'] }
                ]
            }
        ],
        branches: {
            'government': [
                { speaker: '你', text: '我们应该向政府寻求帮助。' },
                { speaker: '美月', text: '我会联系我认识的官员。', sprite: '👩' },
                { speaker: '零', text: '希望这个决定是对的...', sprite: '👨‍💻' }
            ],
            'independent': [
                { speaker: '你', text: '我们继续独立行动。不能冒险。' },
                { speaker: '晓', text: '那我们就靠自己！', sprite: '👧' },
                { speaker: '零', text: '明白了。我会加强情报网络。', sprite: '👨‍💻' }
            ]
        },
        rewards: { courage: 10, knowledge: 10 }
    },

    // Day 35 - 第三次任务：掩护零的潜入
    'day35_mission3': {
        id: 'day35_mission3',
        location: 'NeuroCorp外围',
        day: 35,
        period: 2,
        lines: [
            { speaker: null, text: 'Day 35。零的潜入行动开始。' },
            { speaker: '零', text: '我需要10分钟。你们掩护我。', sprite: '👨‍💻' },
            { speaker: '你', text: '交给我们。' },
            { speaker: '晓', text: '小心点，零。', sprite: '👧' },
            { speaker: '零', text: '...谢谢。我会的。', sprite: '👨‍💻' },
            { speaker: null, text: '突然，警报响起！' },
            { speaker: '你', text: '糟糕！是认知猎人！' },
            { speaker: null, text: '第一次遭遇NeuroCorp的清除者...' }
        ],
        rewards: { battle: 'hunter', flags: ['mission3_started', 'met_hunter'] }
    },

    // ========== 第三章：崩溃加速 (Day 36-60) ==========

    // Day 37 - 震惊真相
    'day37_truth': {
        id: 'day37_truth',
        location: '秘密基地',
        day: 37,
        period: 1,
        lines: [
            { speaker: '零', text: '我拿到了...核心数据。', sprite: '👨‍💻' },
            { speaker: '你', text: '太好了！集体意识计划的真相是什么？' },
            { speaker: '零', text: '...', sprite: '👨‍💻' },
            { speaker: '晓', text: '零？你怎么了？', sprite: '👧' },
            { speaker: '零', text: '这不是连接...是收割。', sprite: '👨‍💻' },
            { speaker: '你', text: '什么意思？' },
            { speaker: '零', text: '集体意识计划的真正目的...不是让人类意识连接，而是...', sprite: '👨‍💻' },
            { speaker: '零', text: '收割所有人的意识，创造一个人工"神"。', sprite: '👨‍💻' },
            { speaker: '晓', text: '什么？！', sprite: '👧' },
            { speaker: '美月', text: '这...这太疯狂了！', sprite: '👩' },
            { speaker: '你', text: '谁策划了这一切？' },
            { speaker: '零', text: '项目主管...顾寒川博士。', sprite: '👨‍💻' },
            { speaker: '美月', text: '...！', sprite: '👩' },
            { speaker: '你', text: '美月？你认识他？' },
            { speaker: '美月', text: '他是...我父亲的合作伙伴。', sprite: '👩' }
        ],
        rewards: { flags: ['truth_revealed', 'gu_discovered'], knowledge: 20, courage: 10 }
    },

    // Day 45 - 第四次任务：潜入地下实验室
    'day45_mission4': {
        id: 'day45_mission4',
        location: 'NeuroCorp地下实验室',
        day: 45,
        period: 2,
        lines: [
            { speaker: null, text: 'Day 45。潜入NeuroCorp地下实验室。' },
            { speaker: '你', text: '这里就是...伊甸园的入口？' },
            { speaker: '零', text: '是的。我的姐姐就在里面。', sprite: '👨‍💻' },
            { speaker: '晓', text: '还有晓的父亲...', sprite: '👧' },
            { speaker: '你', text: '我们一定要把他们救出来。' },
            { speaker: null, text: '突然，一个身影出现了。' },
            { speaker: '顾寒川', text: '欢迎...世界线观测者。', sprite: '👨‍🔬' },
            { speaker: '你', text: '顾寒川！' },
            { speaker: '顾寒川', text: '你们的行动，从一开始就在我的监视之下。', sprite: '👨‍🔬' },
            { speaker: '顾寒川', text: '不过...能走到这一步，你确实很优秀。', sprite: '👨‍🔬' },
            { speaker: '你', text: '为什么要做这些？！' },
            { speaker: '顾寒川', text: '因为...我要复活她。', sprite: '👨‍🔬' },
            { speaker: '顾寒川', text: '我的妻子。她因为认知崩溃而死。', sprite: '👨‍🔬' },
            { speaker: '顾寒川', text: '所以我创造了这个计划...收集所有意识，创造一个"神"...', sprite: '👨‍🔬' },
            { speaker: '顾寒川', text: '一个能够改写生死的"神"。', sprite: '👨‍🔬' },
            { speaker: '你', text: '你疯了！' },
            { speaker: '顾寒川', text: '疯了？也许吧。但是...', sprite: '👨‍🔬' },
            { speaker: '顾寒川', text: '如果你能复活你最爱的人，你不会这么做吗？', sprite: '👨‍🔬' },
            { speaker: null, text: '战斗开始！' }
        ],
        rewards: { battle: 'hunter', flags: ['mission4_complete', 'gu_confrontation'] }
    },

    // Day 60 - 第五次任务：拯救晓
    'day60_mission5': {
        id: 'day60_mission5',
        location: 'NeuroCorp核心区',
        day: 60,
        period: 2,
        lines: [
            { speaker: null, text: 'Day 60。晓被认知猎人抓走了。' },
            { speaker: '零', text: '我定位到了晓的位置...但有陷阱。', sprite: '👨‍💻' },
            { speaker: '你', text: '不管有什么陷阱，我都要去救她！' },
            { speaker: '美月', text: '我们一起去！', sprite: '👩' },
            { speaker: '零', text: '...好。我会破解安全系统。', sprite: '👨‍💻' },
            { speaker: null, text: '突袭NeuroCorp核心区...' },
            { speaker: '你', text: '晓！你在哪里？！' },
            { speaker: '晓', text: '...这里...', sprite: '👧' },
            { speaker: '你', text: '晓！！' },
            { speaker: null, text: '晓被困在认知空间的深处，意识正在崩溃...' },
            { speaker: '你', text: '我一定要救你...等我！' }
        ],
        rewards: { battle: 'guardian', flags: ['mission5_started', 'akatsuki_captured'] }
    },

    // ========== 第四章：终末倒计时 (Day 61-85) ==========

    // Day 71 - 神秘声音身份揭露
    'day71_identity': {
        id: 'day71_identity',
        location: '认知空间深层',
        day: 71,
        period: 2,
        lines: [
            { speaker: '神秘声音', text: '是时候告诉你真相了。' },
            { speaker: '你', text: '你到底是谁？' },
            { speaker: '神秘声音', text: '我是...你。' },
            { speaker: '你', text: '什么？！' },
            { speaker: '神秘声音', text: '来自世界线β的你。' },
            { speaker: null, text: '一个身影出现了...和你一模一样的人。' },
            { speaker: '未来的你', text: '在β线，我活了下来，但晓死了。' },
            { speaker: '未来的你', text: '在α线，我们都死了。' },
            { speaker: '未来的你', text: '现在是γ线...最后的机会。' },
            { speaker: '你', text: '世界线观测者...原来是这个意思。' },
            { speaker: '未来的你', text: '是的。你拥有观测世界线的能力。' },
            { speaker: '未来的你', text: 'Day 100，你会面临最终选择。' },
            { speaker: '未来的你', text: '记住...不要重蹈我的覆辙。' },
            { speaker: null, text: '身影消失了。但你获得了新的力量...' }
        ],
        rewards: { flags: ['identity_revealed', 'worldline_power'], combat: 20, knowledge: 20, skills: ['ultimate'] }
    },

    // Day 80 - 倒计时
    'day80_countdown': {
        id: 'day80_countdown',
        location: '秘密基地',
        day: 80,
        period: 1,
        lines: [
            { speaker: '零', text: '确认了。Day 100是计划启动日。', sprite: '👨‍💻' },
            { speaker: '美月', text: '还有20天...', sprite: '👩' },
            { speaker: '晓', text: '我们能阻止吗？', sprite: '👧' },
            { speaker: '你', text: '一定能。我们已经走到这一步了。' },
            { speaker: '零', text: '我找到了系统核心的位置。', sprite: '👨‍💻' },
            { speaker: '零', text: 'NeuroCorp总部顶楼。', sprite: '👨‍💻' },
            { speaker: '美月', text: '那里戒备森严...', sprite: '👩' },
            { speaker: '晓', text: '不管多危险，我们都要去！', sprite: '👧' },
            { speaker: '你', text: '那就...准备最终决战吧。' }
        ],
        rewards: { flags: ['final_countdown'], courage: 20 }
    },

    // ========== 终章：终末与新生 (Day 86-100) ==========

    // Day 90 - 顾寒川的真相
    'day90_gu_truth': {
        id: 'day90_gu_truth',
        location: 'NeuroCorp核心',
        day: 90,
        period: 2,
        lines: [
            { speaker: '顾寒川', text: '你们终于来了。', sprite: '👨‍🔬' },
            { speaker: '你', text: '顾寒川！我们要阻止你！' },
            { speaker: '顾寒川', text: '阻止我？你们知道我为什么这么做吗？', sprite: '👨‍🔬' },
            { speaker: '顾寒川', text: '10年前，我的妻子...她是第一个认知崩溃的受害者。', sprite: '👨‍🔬' },
            { speaker: '顾寒川', text: '我眼睁睁看着她的意识消散...什么都做不了。', sprite: '👨‍🔬' },
            { speaker: '顾寒川', text: '从那天起，我发誓...一定要找到复活她的方法。', sprite: '👨‍🔬' },
            { speaker: '你', text: '但你不能为了一个人，牺牲所有人！' },
            { speaker: '顾寒川', text: '如果是你最爱的人呢？你会怎么选择？', sprite: '👨‍🔬' },
            { speaker: '你', text: '...' },
            { speaker: '顾寒川', text: '看吧。你也无法回答。', sprite: '👨‍🔬' },
            { speaker: '顾寒川', text: '所以...不要阻止我。让我完成这一切。', sprite: '👨‍🔬' }
        ],
        rewards: { flags: ['gu_backstory'], knowledge: 25 }
    },

    // Day 96 - 最终选择前夜
    'day96_final_choice': {
        id: 'day96_final_choice',
        location: '系统核心前',
        day: 96,
        period: 2,
        lines: [
            { speaker: '零', text: '我破解了系统...', sprite: '👨‍💻' },
            { speaker: '你', text: '太好了！那我们可以摧毁它了！' },
            { speaker: '零', text: '等等...有个问题。', sprite: '👨‍💻' },
            { speaker: '你', text: '什么问题？' },
            { speaker: '零', text: '如果直接摧毁系统...所有MindLink用户都会死。', sprite: '👨‍💻' },
            { speaker: '晓', text: '什么？！', sprite: '👧' },
            { speaker: '零', text: '现在全世界有10亿MindLink用户。系统和他们的大脑已经深度绑定了。', sprite: '👨‍💻' },
            { speaker: '美月', text: '那...那怎么办？', sprite: '👩' },
            { speaker: '零', text: '还有一个方法...改写系统，而不是摧毁它。', sprite: '👨‍💻' },
            { speaker: '零', text: '但这需要...利用你的世界线观测者能力。', sprite: '👨‍💻' },
            { speaker: '你', text: '会有什么风险？' },
            { speaker: '零', text: '你可能会...失去这个能力。或者...失去关于世界线的记忆。', sprite: '👨‍💻' },
            { speaker: '你', text: '...' },
            { speaker: '晓', text: '不要！这太危险了！', sprite: '👧' },
            { speaker: '你', text: '但这是拯救所有人的唯一方法...对吗？' },
            { speaker: '零', text: '...是的。', sprite: '👨‍💻' },
            { speaker: null, text: 'Day 100即将到来...你必须做出选择。' }
        ],
        rewards: { flags: ['final_choice_revealed'], knowledge: 30, courage: 30 }
    },

    // Day 99 - 最终对决
    'day99_final_battle': {
        id: 'day99_final_battle',
        location: '集体意识核心',
        day: 99,
        period: 2,
        lines: [
            { speaker: null, text: 'Day 99，夜晚。' },
            { speaker: null, text: '距离Day 100还有几个小时。' },
            { speaker: '顾寒川', text: '你们还是来了...', sprite: '👨‍🔬' },
            { speaker: '你', text: '我们要阻止你，顾寒川！' },
            { speaker: '顾寒川', text: '阻止我...然后呢？让我的妻子永远消失？', sprite: '👨‍🔬' },
            { speaker: '你', text: '你的妻子...她会希望你这么做吗？' },
            { speaker: '顾寒川', text: '...', sprite: '👨‍🔬' },
            { speaker: '你', text: '牺牲10亿人来复活一个人...这不是爱，是自私！' },
            { speaker: '顾寒川', text: '闭嘴！你什么都不懂！', sprite: '👨‍🔬' },
            { speaker: '顾寒川', text: '系统启动！', sprite: '👨‍🔬' },
            { speaker: null, text: '最终决战...开始！' }
        ],
        rewards: { battle: 'boss_gu', flags: ['final_battle'] }
    }
};

// 导出数据
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { day16to100Dialogues };
}
