// ===== 游戏状态管理 =====
class GameState {
    constructor() {
        this.currentDay = 1;
        this.currentTimeslot = 'morning'; // morning, afternoon, night
        this.maxDay = 15;

        // 属性系统
        this.attributes = {
            knowledge: { level: 1, exp: 0, expToNext: 100 },
            courage: { level: 1, exp: 0, expToNext: 100 },
            charm: { level: 1, exp: 0, expToNext: 100 },
            combat: { level: 1, exp: 0, expToNext: 100 }
        };

        // 关系系统
        this.relationships = {
            akira: { name: '晓', level: 1, exp: 0 },
            rei: { name: '零', level: 1, exp: 0 },
            mizuki: { name: '美月', level: 1, exp: 0 }
        };

        // 任务系统（重新设计）
        this.mainMission = {
            id: 'rescue_mizuki',
            name: '拯救美月',
            active: false,
            deadline: 15,
            startDay: 10,
            requirements: {
                combat: 3,
                clues: 3,
                ally: null // 'akira' or 'rei'
            },
            progress: {
                combat: 1,
                clues: 0,
                ally: null
            }
        };

        // 游戏标记
        this.flags = {
            tutorial_complete: false,
            ability_awakened: false,
            mission_started: false,
            akira_crisis: false,
            rei_crisis: false,
            akira_helped: false,
            rei_helped: false,
            mission_complete: false
        };

        // 完成的活动记录
        this.completedEvents = [];
        this.todayActivities = [];

        // 准备度（用于任务检查）
        this.preparedness = 0;
    }

    // 添加属性经验
    addAttributeExp(attrName, amount) {
        const attr = this.attributes[attrName];
        attr.exp += amount;

        while (attr.exp >= attr.expToNext && attr.level < 10) {
            attr.exp -= attr.expToNext;
            attr.level++;
            attr.expToNext = Math.floor(attr.expToNext * 1.5);

            showNotification(`${attrName} 升级到 Lv ${attr.level}!`, 'success');

            // 更新任务进度
            if (attrName === 'combat' && this.mainMission.active) {
                this.mainMission.progress.combat = attr.level;
            }
        }

        if (attr.level >= 10) {
            attr.exp = 0;
            attr.expToNext = 0;
        }
    }

    // 添加关系经验
    addRelationshipExp(charId, amount) {
        if (this.relationships[charId]) {
            this.relationships[charId].exp += amount;

            if (this.relationships[charId].exp >= 100) {
                this.relationships[charId].exp -= 100;
                this.relationships[charId].level++;
                showNotification(`与 ${this.relationships[charId].name} 的关系提升到 Lv ${this.relationships[charId].level}!`, 'success');
            }
        }
    }

    // 添加线索
    addClue(clueId) {
        if (!this.flags[clueId]) {
            this.flags[clueId] = true;
            this.mainMission.progress.clues++;
            showNotification('获得了关键线索！', 'success');
        }
    }

    // 推进时间
    advanceTime() {
        const timeslots = ['morning', 'afternoon', 'night'];
        const currentIndex = timeslots.indexOf(this.currentTimeslot);

        if (currentIndex < timeslots.length - 1) {
            this.currentTimeslot = timeslots[currentIndex + 1];
        } else {
            // 进入下一天
            this.currentDay++;
            this.currentTimeslot = 'morning';
            this.todayActivities = [];

            // 检查关键剧情触发
            checkDayTriggers();
        }

        // 更新准备度
        if (this.mainMission.active) {
            this.updatePreparedness();
        }
    }

    // 更新准备度
    updatePreparedness() {
        const req = this.mainMission.requirements;
        const prog = this.mainMission.progress;

        let score = 0;
        score += (prog.combat / req.combat) * 40; // 战斗力40%
        score += (prog.clues / req.clues) * 40; // 线索40%
        score += (prog.ally ? 20 : 0); // 盟友20%

        this.preparedness = Math.min(100, Math.floor(score));
    }

    // 检查游戏是否结束
    isGameOver() {
        return this.currentDay > this.maxDay || this.flags.mission_complete;
    }
}

// 全局游戏状态
let gameState = new GameState();

// ===== 对话系统 =====
class DialogueSystem {
    constructor() {
        this.currentDialogue = null;
        this.dialogueQueue = [];
    }

    // 显示对话
    showDialogue(dialogue) {
        this.currentDialogue = dialogue;
        const modal = document.getElementById('dialogue-modal');
        const content = document.getElementById('dialogue-content');

        content.innerHTML = '';

        // 角色名称和头像
        if (dialogue.character) {
            const charInfo = document.createElement('div');
            charInfo.className = 'dialogue-character';
            charInfo.innerHTML = `
                <div class="dialogue-avatar">${dialogue.avatar || '👤'}</div>
                <div class="dialogue-name">${dialogue.character}</div>
            `;
            content.appendChild(charInfo);
        }

        // 对话文本
        const text = document.createElement('div');
        text.className = 'dialogue-text';
        text.textContent = dialogue.text;
        content.appendChild(text);

        // 选项
        if (dialogue.choices && dialogue.choices.length > 0) {
            const choicesDiv = document.createElement('div');
            choicesDiv.className = 'dialogue-choices';

            dialogue.choices.forEach((choice, index) => {
                const btn = document.createElement('button');
                btn.className = 'dialogue-choice-btn';
                btn.textContent = choice.text;
                btn.onclick = () => this.selectChoice(choice);
                choicesDiv.appendChild(btn);
            });

            content.appendChild(choicesDiv);
        } else {
            // 继续按钮
            const continueBtn = document.createElement('button');
            continueBtn.className = 'dialogue-continue-btn';
            continueBtn.textContent = '继续';
            continueBtn.onclick = () => this.closeDialogue();
            content.appendChild(continueBtn);
        }

        modal.classList.add('active');
    }

    // 选择对话选项
    selectChoice(choice) {
        // 执行选择的效果
        if (choice.effect) {
            choice.effect(gameState);
        }

        // 显示下一个对话或关闭
        if (choice.next) {
            this.showDialogue(choice.next);
        } else {
            this.closeDialogue();
        }
    }

    // 关闭对话
    closeDialogue() {
        document.getElementById('dialogue-modal').classList.remove('active');
        this.currentDialogue = null;

        // 如果有队列中的对话，显示下一个
        if (this.dialogueQueue.length > 0) {
            const next = this.dialogueQueue.shift();
            setTimeout(() => this.showDialogue(next), 300);
        } else {
            // 对话结束，更新UI
            updateUI();
        }
    }
}

const dialogueSystem = new DialogueSystem();

// ===== 剧情事件数据 =====
const storyEvents = {
    // Day 1: 游戏开始
    day1_morning: {
        id: 'day1_morning',
        trigger: { day: 1, timeslot: 'morning', auto: true },
        dialogue: {
            character: '旁白',
            avatar: '📱',
            text: '2045年，新东京。你醒来时，MindLink设备传来故障警报。这个连接你大脑的装置，最近总是不太稳定...',
            choices: [
                {
                    text: '检查设备',
                    effect: (state) => { state.flags.tutorial_complete = true; },
                    next: {
                        character: '系统',
                        avatar: '⚙️',
                        text: '检测到未知波动...同步率异常...已重启设备。\n\n今天是距离"全球意识统合计划"启动还有100天。你是新东京学园的学生，过着普通的日常生活。'
                    }
                }
            ]
        }
    },

    day1_afternoon: {
        id: 'day1_afternoon',
        trigger: { day: 1, timeslot: 'afternoon', auto: true },
        dialogue: {
            character: '晓',
            avatar: '🏃',
            text: '喂！等等我！你今天看起来心不在焉的...MindLink又出问题了？我的倒是一切正常。',
            choices: [
                {
                    text: '只是有点头晕',
                    effect: (state) => { state.addRelationshipExp('akira', 10); }
                },
                {
                    text: '最近新闻里的认知崩溃事件...',
                    effect: (state) => { state.addRelationshipExp('akira', 15); state.flags.knows_crisis = true; }
                }
            ]
        }
    },

    // Day 3: 能力觉醒
    day3_night: {
        id: 'day3_night',
        trigger: { day: 3, timeslot: 'night', auto: true },
        dialogue: {
            character: '旁白',
            avatar: '🌀',
            text: '夜里，你做了一个奇怪的梦。你看到了扭曲的空间、破碎的记忆碎片，还有...一个陌生的声音。',
            choices: [
                {
                    text: '这是哪里？',
                    next: {
                        character: '？？？',
                        avatar: '👁️',
                        text: '这是认知空间...人类内心的世界。你已经觉醒了进入这里的能力。很快，你会需要这份力量。',
                        choices: [
                            {
                                text: '醒来',
                                effect: (state) => {
                                    state.flags.ability_awakened = true;
                                    showNotification('你觉醒了进入认知空间的能力！', 'success');
                                }
                            }
                        ]
                    }
                }
            ]
        }
    },

    // Day 8: 零的警告
    day8_afternoon: {
        id: 'day8_afternoon',
        trigger: { day: 8, timeslot: 'afternoon', auto: true },
        dialogue: {
            character: '零',
            avatar: '📚',
            text: '终于找到你了。我观察你很久了...你也注意到了吧？认知崩溃事件在增加。而且，下一个受害者可能就在我们身边。',
            choices: [
                {
                    text: '你是谁？怎么知道这些？',
                    effect: (state) => { state.addRelationshipExp('rei', 20); },
                    next: {
                        character: '零',
                        avatar: '📚',
                        text: '我叫零，是认知调查部的...实习生。我父亲是神代教授，他一直在研究认知崩溃。时间不多了，做好准备吧。'
                    }
                },
                {
                    text: '不关我的事',
                    effect: (state) => { state.addAttributeExp('courage', -10); }
                }
            ]
        }
    },

    // Day 10: 美月崩溃（转折点）
    day10_morning: {
        id: 'day10_morning',
        trigger: { day: 10, timeslot: 'morning', auto: true },
        dialogue: {
            character: '旁白',
            avatar: '🚨',
            text: '今天早上，学校传来紧急广播——你的同学美月突然昏迷，疑似认知崩溃。医院无法治疗，她的意识正在消散...',
            choices: [
                {
                    text: '我能救她吗？',
                    next: {
                        character: '零',
                        avatar: '📚',
                        text: '可以，但只有5天时间。你需要进入她的认知空间，解决她的"心结"。但你必须做好准备：\n\n• 战斗力至少Lv3（面对认知防御）\n• 收集3个关于她的线索\n• 找一个可以信任的同伴\n\n时间不多了，Day 15是极限！',
                        choices: [
                            {
                                text: '我会救她！',
                                effect: (state) => {
                                    state.mainMission.active = true;
                                    state.flags.mission_started = true;
                                    showNotification('任务开始：拯救美月（5天准备时间）', 'warning');
                                }
                            }
                        ]
                    }
                }
            ]
        }
    },

    // Day 12: 晓的危机
    day12_afternoon: {
        id: 'day12_afternoon',
        trigger: { day: 12, timeslot: 'afternoon', auto: true, requires: ['mission_started'] },
        dialogue: {
            character: '晓',
            avatar: '🏃',
            text: '不好了！我...我也感觉到了奇怪的波动。我可能是下一个...你能帮我吗？我害怕...',
            choices: [
                {
                    text: '我会帮你！（花费1天）',
                    effect: (state) => {
                        state.flags.akira_crisis = true;
                        state.flags.akira_helped = true;
                        state.mainMission.progress.ally = 'akira';
                        state.addRelationshipExp('akira', 50);
                        showNotification('晓愿意在Day 15协助你！', 'success');
                        // 强制跳过当前时段
                        gameState.advanceTime();
                    }
                },
                {
                    text: '抱歉，我必须专注救美月',
                    effect: (state) => {
                        state.flags.akira_crisis = true;
                        state.addRelationshipExp('akira', -30);
                    }
                }
            ]
        }
    },

    // Day 13: 零的危机
    day13_afternoon: {
        id: 'day13_afternoon',
        trigger: { day: 13, timeslot: 'afternoon', auto: true, requires: ['mission_started'] },
        dialogue: {
            character: '零',
            avatar: '📚',
            text: '我需要你的帮助。我发现了父亲的秘密实验记录...里面有关于美月的关键情报，但我需要有人帮我潜入实验室。',
            choices: [
                {
                    text: '我跟你去！（花费1天，获得线索）',
                    effect: (state) => {
                        if (!state.flags.akira_helped) {
                            state.flags.rei_crisis = true;
                            state.flags.rei_helped = true;
                            state.mainMission.progress.ally = 'rei';
                            state.addRelationshipExp('rei', 50);
                            state.addClue('clue_experiment');
                            showNotification('零愿意在Day 15协助你！获得关键线索！', 'success');
                            // 强制跳过当前时段
                            gameState.advanceTime();
                        } else {
                            showNotification('你已经答应帮助晓了，不能同时帮助零...', 'error');
                        }
                    }
                },
                {
                    text: '这太危险了',
                    effect: (state) => {
                        state.flags.rei_crisis = true;
                        state.addRelationshipExp('rei', -30);
                    }
                }
            ]
        }
    }
};

// ===== 日常活动数据 =====
const dailyActivities = [
    // 学习类
    {
        id: 'study_library',
        name: '图书馆学习',
        description: '提升知识，可能获得关于美月的线索。',
        icon: '📚',
        category: 'study',
        conditions: {
            timeslots: ['morning', 'afternoon', 'night']
        },
        effects: {
            knowledge: 20
        },
        randomClue: { chance: 0.3, clue: 'clue_library', day: 10 }
    },

    // 训练类
    {
        id: 'gym_training',
        name: '战斗训练',
        description: '提升战斗力，为进入认知空间做准备。',
        icon: '💪',
        category: 'training',
        conditions: {
            timeslots: ['morning', 'afternoon', 'night']
        },
        effects: {
            combat: 25,
            courage: 10
        }
    },

    {
        id: 'courage_challenge',
        name: '勇气挑战',
        description: '尝试高难度活动，大幅提升勇气。',
        icon: '⚔️',
        category: 'training',
        conditions: {
            timeslots: ['afternoon'],
            minDay: 5
        },
        effects: {
            courage: 35,
            combat: 10
        }
    },

    // 社交类
    {
        id: 'hangout_akira',
        name: '和晓一起运动',
        description: '增进与晓的关系。',
        icon: '🏃',
        category: 'social',
        conditions: {
            timeslots: ['afternoon'],
            maxDay: 11 // Day 12后晓遇到危机
        },
        effects: {
            courage: 10,
            combat: 10,
            relationships: { akira: 25 }
        }
    },

    {
        id: 'talk_rei',
        name: '和零讨论案件',
        description: '零可能有重要情报。',
        icon: '💬',
        category: 'social',
        conditions: {
            timeslots: ['afternoon', 'night'],
            minDay: 8,
            maxDay: 12
        },
        effects: {
            knowledge: 15,
            relationships: { rei: 25 }
        },
        randomClue: { chance: 0.4, clue: 'clue_rei', day: 10 }
    },

    // 调查类（Day 10+）
    {
        id: 'investigate_classroom',
        name: '调查美月的教室',
        description: '寻找关于美月的线索。',
        icon: '🔍',
        category: 'investigation',
        conditions: {
            timeslots: ['afternoon'],
            minDay: 10,
            requires: ['mission_started']
        },
        effects: {
            knowledge: 10
        },
        guaranteedClue: 'clue_classroom'
    },

    {
        id: 'investigate_rooftop',
        name: '调查天台',
        description: '美月经常去的地方，可能有线索。',
        icon: '🏢',
        category: 'investigation',
        conditions: {
            timeslots: ['afternoon', 'night'],
            minDay: 11,
            requires: ['mission_started']
        },
        effects: {
            courage: 10
        },
        guaranteedClue: 'clue_rooftop'
    },

    // 休息
    {
        id: 'rest',
        name: '休息',
        description: '恢复精力，少量提升所有属性。',
        icon: '😴',
        category: 'rest',
        conditions: {
            timeslots: ['night']
        },
        effects: {
            knowledge: 5,
            courage: 5,
            charm: 5,
            combat: 5
        }
    }
];

// ===== 事件检查和触发 =====
function checkDayTriggers() {
    const day = gameState.currentDay;
    const timeslot = gameState.currentTimeslot;

    // 检查是否有自动触发的剧情
    for (const [key, event] of Object.entries(storyEvents)) {
        if (event.trigger.auto &&
            event.trigger.day === day &&
            event.trigger.timeslot === timeslot &&
            !gameState.completedEvents.includes(event.id)) {

            // 检查前置条件
            if (event.trigger.requires) {
                const allMet = event.trigger.requires.every(flag => gameState.flags[flag]);
                if (!allMet) continue;
            }

            gameState.completedEvents.push(event.id);
            setTimeout(() => dialogueSystem.showDialogue(event.dialogue), 500);
            return;
        }
    }

    // Day 15: 最终任务
    if (day === 15 && timeslot === 'night' && !gameState.flags.mission_complete) {
        startFinalMission();
    }
}

// ===== 检查活动是否可用 =====
function isActivityAvailable(activity) {
    // 检查时间段
    if (activity.conditions.timeslots &&
        !activity.conditions.timeslots.includes(gameState.currentTimeslot)) {
        return false;
    }

    // 检查最小/最大天数
    if (activity.conditions.minDay && gameState.currentDay < activity.conditions.minDay) {
        return false;
    }
    if (activity.conditions.maxDay && gameState.currentDay > activity.conditions.maxDay) {
        return false;
    }

    // 检查前置标记
    if (activity.conditions.requires) {
        const allMet = activity.conditions.requires.every(flag => gameState.flags[flag]);
        if (!allMet) return false;
    }

    // 检查是否已经获得线索（避免重复）
    if (activity.guaranteedClue && gameState.flags[activity.guaranteedClue]) {
        return false;
    }

    return true;
}

// ===== UI更新函数 =====
function updateUI() {
    // 更新时间显示
    document.getElementById('current-day').textContent = gameState.currentDay;

    const timeslotNames = {
        morning: '早晨',
        afternoon: '下午',
        night: '夜晚'
    };
    const timeslotEl = document.getElementById('current-timeslot');
    timeslotEl.textContent = timeslotNames[gameState.currentTimeslot];
    timeslotEl.className = `timeslot ${gameState.currentTimeslot}`;

    // 更新属性
    updateAttributes();

    // 更新关系
    updateRelationships();

    // 更新任务
    updateMission();

    // 更新活动列表
    updateActivities();

    // 检查剧情触发
    checkDayTriggers();
}

function updateAttributes() {
    ['knowledge', 'courage', 'charm', 'combat'].forEach(attr => {
        const data = gameState.attributes[attr];
        const percentage = data.level < 10 ? (data.exp / data.expToNext) * 100 : 100;

        document.getElementById(`${attr}-bar`).style.width = `${percentage}%`;
        document.getElementById(`${attr}-value`).textContent =
            data.level < 10 ? `Lv ${data.level}` : `Lv ${data.level} (MAX)`;
    });
}

function updateRelationships() {
    const container = document.getElementById('relationships-container');
    container.innerHTML = '';

    Object.entries(gameState.relationships).forEach(([id, data]) => {
        const div = document.createElement('div');
        div.className = 'relationship-item';
        div.innerHTML = `
            <span class="relationship-name">${data.name}</span>
            <span class="relationship-level">Lv ${data.level}</span>
        `;
        container.appendChild(div);
    });
}

function updateMission() {
    const container = document.getElementById('mission-container');

    if (!gameState.mainMission.active) {
        container.innerHTML = '<p style="color: #999;">暂无紧急任务</p>';
        return;
    }

    const mission = gameState.mainMission;
    const daysLeft = mission.deadline - gameState.currentDay;
    const req = mission.requirements;
    const prog = mission.progress;

    container.innerHTML = `
        <div class="mission-active">
            <div class="mission-title">${mission.name}</div>
            <div class="mission-deadline ${daysLeft <= 2 ? 'urgent' : ''}">
                剩余时间: ${daysLeft} 天
            </div>
            <div class="mission-requirements">
                <div>战斗力: Lv${prog.combat}/${req.combat} ${prog.combat >= req.combat ? '✓' : '✗'}</div>
                <div>线索: ${prog.clues}/${req.clues} ${prog.clues >= req.clues ? '✓' : '✗'}</div>
                <div>同伴: ${prog.ally ? '✓' : '✗'} ${prog.ally ? `(${gameState.relationships[prog.ally].name})` : ''}</div>
            </div>
            <div class="mission-preparedness">
                <div class="preparedness-label">准备度: ${gameState.preparedness}%</div>
                <div class="preparedness-bar">
                    <div class="preparedness-fill" style="width: ${gameState.preparedness}%"></div>
                </div>
            </div>
        </div>
    `;
}

function updateActivities() {
    const container = document.getElementById('events-container');
    container.innerHTML = '';

    const availableActivities = dailyActivities.filter(activity => isActivityAvailable(activity));

    if (availableActivities.length === 0) {
        container.innerHTML = '<p style="grid-column: 1/-1; text-align: center; color: #999;">当前时间段没有可用的活动</p>';
        return;
    }

    availableActivities.forEach(activity => {
        const card = createActivityCard(activity);
        container.appendChild(card);
    });
}

function createActivityCard(activity) {
    const card = document.createElement('div');
    card.className = 'event-card';

    // 创建奖励标签
    let rewardsHTML = '';
    if (activity.effects) {
        rewardsHTML = '<div class="event-rewards">';

        if (activity.effects.knowledge) {
            rewardsHTML += `<span class="reward-tag knowledge">+${activity.effects.knowledge} 知识</span>`;
        }
        if (activity.effects.courage) {
            rewardsHTML += `<span class="reward-tag courage">+${activity.effects.courage} 勇气</span>`;
        }
        if (activity.effects.charm) {
            rewardsHTML += `<span class="reward-tag charm">+${activity.effects.charm} 魅力</span>`;
        }
        if (activity.effects.combat) {
            rewardsHTML += `<span class="reward-tag combat">+${activity.effects.combat} 战斗</span>`;
        }
        if (activity.effects.relationships) {
            Object.entries(activity.effects.relationships).forEach(([char, value]) => {
                const charName = gameState.relationships[char]?.name || char;
                rewardsHTML += `<span class="reward-tag relationship">+${charName}</span>`;
            });
        }

        rewardsHTML += '</div>';
    }

    card.innerHTML = `
        <div class="event-icon">${activity.icon}</div>
        <div class="event-name">${activity.name}</div>
        <div class="event-description">${activity.description}</div>
        ${rewardsHTML}
    `;

    card.addEventListener('click', () => executeActivity(activity));

    return card;
}

// ===== 执行活动 =====
function executeActivity(activity) {
    const results = [];

    // 应用属性效果
    if (activity.effects.knowledge) {
        gameState.addAttributeExp('knowledge', activity.effects.knowledge);
        results.push(`知识 +${activity.effects.knowledge}`);
    }
    if (activity.effects.courage) {
        gameState.addAttributeExp('courage', activity.effects.courage);
        results.push(`勇气 +${activity.effects.courage}`);
    }
    if (activity.effects.charm) {
        gameState.addAttributeExp('charm', activity.effects.charm);
        results.push(`魅力 +${activity.effects.charm}`);
    }
    if (activity.effects.combat) {
        gameState.addAttributeExp('combat', activity.effects.combat);
        results.push(`战斗力 +${activity.effects.combat}`);
    }

    // 关系变化
    if (activity.effects.relationships) {
        Object.entries(activity.effects.relationships).forEach(([char, value]) => {
            gameState.addRelationshipExp(char, value);
            results.push(`${gameState.relationships[char].name} 好感度 +${value}`);
        });
    }

    // 线索获取
    if (activity.guaranteedClue && !gameState.flags[activity.guaranteedClue]) {
        gameState.addClue(activity.guaranteedClue);
        results.push('🔍 获得关键线索！');
    } else if (activity.randomClue && gameState.currentDay >= activity.randomClue.day) {
        if (Math.random() < activity.randomClue.chance && !gameState.flags[activity.randomClue.clue]) {
            gameState.addClue(activity.randomClue.clue);
            results.push('🔍 意外发现了线索！');
        }
    }

    // 记录活动
    gameState.todayActivities.push(activity.name);

    // 显示结果
    showActivityResult(activity, results);

    // 推进时间
    gameState.advanceTime();

    // 更新UI
    updateUI();

    // 检查是否进入新的一天
    if (gameState.currentTimeslot === 'morning' && gameState.currentDay <= gameState.maxDay) {
        showDaySummary();
    }

    // 检查游戏是否结束
    if (gameState.isGameOver()) {
        // 结局逻辑将在最终任务后触发
    }
}

// ===== 显示活动结果 =====
function showActivityResult(activity, results) {
    const modal = document.getElementById('result-modal');
    document.getElementById('result-title').textContent = activity.name;
    document.getElementById('result-description').textContent = activity.description;

    const rewardsContainer = document.getElementById('result-rewards');
    rewardsContainer.innerHTML = '';
    results.forEach(result => {
        const span = document.createElement('span');
        span.className = 'reward-item';
        span.textContent = result;
        rewardsContainer.appendChild(span);
    });

    modal.classList.add('active');
}

function showDaySummary() {
    const modal = document.getElementById('day-end-modal');
    document.getElementById('summary-day').textContent = gameState.currentDay - 1;

    // 今日活动
    const activitiesList = document.getElementById('summary-activities');
    activitiesList.innerHTML = '';

    if (gameState.todayActivities.length === 0) {
        const li = document.createElement('li');
        li.textContent = '（剧情日）';
        li.style.color = '#999';
        activitiesList.appendChild(li);
    } else {
        gameState.todayActivities.forEach(activity => {
            const li = document.createElement('li');
            li.textContent = activity;
            activitiesList.appendChild(li);
        });
    }

    // 属性变化
    const attrsContainer = document.getElementById('summary-attributes');
    attrsContainer.innerHTML = '';
    ['knowledge', 'courage', 'charm', 'combat'].forEach(attr => {
        const data = gameState.attributes[attr];
        const div = document.createElement('div');
        div.className = 'attr-change positive';
        div.innerHTML = `
            <div class="attr-name">${attr}</div>
            <div class="attr-value">Lv ${data.level}</div>
        `;
        attrsContainer.appendChild(div);
    });

    modal.classList.add('active');
}

// ===== 最终任务 =====
function startFinalMission() {
    const req = gameState.mainMission.requirements;
    const prog = gameState.mainMission.progress;

    // 检查准备度
    const combatReady = prog.combat >= req.combat;
    const cluesReady = prog.clues >= req.clues;
    const allyReady = prog.ally !== null;

    let dialogue = {
        character: '旁白',
        avatar: '🌀',
        text: '',
        choices: []
    };

    if (combatReady && cluesReady && allyReady) {
        // 完美结局
        dialogue.text = `你已经做好了充分的准备。战斗力Lv${prog.combat}，掌握了${prog.clues}条关键线索，${gameState.relationships[prog.ally].name}在你身边。\n\n你们一起进入了美月的认知空间...`;
        dialogue.choices = [{
            text: '进入认知空间',
            next: showPerfectEnding
        }];
    } else if (combatReady && cluesReady) {
        // 成功结局（缺少同伴）
        dialogue.text = `你的战斗力和线索都足够了，但没有同伴协助。这将是一场艰难的战斗...`;
        dialogue.choices = [{
            text: '独自进入',
            next: showGoodEnding
        }];
    } else if (combatReady || cluesReady) {
        // 勉强结局
        dialogue.text = `你的准备不够充分...${!combatReady ? '战斗力不足' : ''}${!cluesReady ? '线索不够' : ''}${!allyReady ? '没有同伴' : ''}。但时间已经不多了，你必须尝试。`;
        dialogue.choices = [{
            text: '强行进入',
            next: showNormalEnding
        }];
    } else {
        // 失败结局
        dialogue.text = `时间到了，但你完全没有准备好。战斗力不足，线索也不够，更没有可以信任的同伴...\n\n你还是冲进了美月的认知空间，但结果...`;
        dialogue.choices = [{
            text: '...',
            next: showBadEnding
        }];
    }

    dialogueSystem.showDialogue(dialogue);
}

function showPerfectEnding() {
    return {
        character: '结局',
        avatar: '✨',
        text: `【完美结局】\n\n凭借充足的准备和${gameState.relationships[gameState.mainMission.progress.ally].name}的帮助，你成功解开了美月心中的结。她的认知空间恢复稳定，意识重新归来。\n\n美月醒来后，紧紧抱住了你。"谢谢你...救了我。"\n\n但这只是开始。认知崩溃事件背后的真相，还等待着你去揭开...\n\n【原型结束 - 感谢游玩！】`,
        choices: [{
            text: '重新开始',
            effect: () => {
                gameState.flags.mission_complete = true;
                setTimeout(() => restartGame(), 2000);
            }
        }]
    };
}

function showGoodEnding() {
    return {
        character: '结局',
        avatar: '⭐',
        text: `【成功结局】\n\n虽然独自作战很艰难，但你最终还是救回了美月。她的意识恢复了，但过程中你也受了不轻的伤...\n\n"你为什么要这么做？"美月问。\n"因为...我不想失去任何人。"你回答。\n\n认知崩溃事件的谜团还没有解开，但至少，你救下了一个人。\n\n【原型结束 - 感谢游玩！】`,
        choices: [{
            text: '重新开始',
            effect: () => {
                gameState.flags.mission_complete = true;
                setTimeout(() => restartGame(), 2000);
            }
        }]
    };
}

function showNormalEnding() {
    return {
        character: '结局',
        avatar: '💔',
        text: `【勉强结局】\n\n准备不足的你，在认知空间中苦战。美月的心结比想象中更深，你差点迷失在扭曲的空间里...\n\n最后关头，美月自己的意志帮助了你。她醒来了，但记忆出现了缺失。\n\n"你是...谁？"她看着你，眼神茫然。\n\n你救下了她的生命，但或许...失去了更重要的东西。\n\n【原型结束 - 感谢游玩！】`,
        choices: [{
            text: '重新开始',
            effect: () => {
                gameState.flags.mission_complete = true;
                setTimeout(() => restartGame(), 2000);
            }
        }]
    };
}

function showBadEnding() {
    return {
        character: '结局',
        avatar: '💀',
        text: `【失败结局】\n\n完全没有准备的你，在认知空间中迷失了方向。美月的心结太过复杂，你根本无法理解，更无法解开...\n\n当你被强制退出时，美月已经陷入了永久昏迷。\n\n"我早就警告过你了。"零冷冷地说，"时间管理很重要。你浪费了太多时间。"\n\n你失败了。\n\n【原型结束 - 请重新尝试】`,
        choices: [{
            text: '重新开始',
            effect: () => {
                gameState.flags.mission_complete = true;
                setTimeout(() => restartGame(), 2000);
            }
        }]
    };
}

function restartGame() {
    gameState = new GameState();
    document.getElementById('dialogue-modal').classList.remove('active');
    updateUI();
}

function showNotification(message, type = 'info') {
    // 简单的通知实现
    console.log(`[${type.toUpperCase()}] ${message}`);

    // TODO: 可以添加更好的视觉通知
}

// ===== 事件监听 =====
document.getElementById('close-modal-btn').addEventListener('click', () => {
    document.getElementById('result-modal').classList.remove('active');
});

document.getElementById('close-summary-btn').addEventListener('click', () => {
    document.getElementById('day-end-modal').classList.remove('active');
});

// ===== 初始化 =====
document.addEventListener('DOMContentLoaded', () => {
    updateUI();
    console.log('=== MindLink - 认知空间 原型 v2.0 ===');
    console.log('Day 1-15 完整剧情体验');
    console.log('核心机制：时间压力 + 剧情选择 + 任务准备');
});
