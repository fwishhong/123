// ===== 游戏状态管理 =====
class GameState {
    constructor() {
        this.currentDay = 1;
        this.currentTimeslot = 'morning'; // morning, afternoon, night
        this.maxDay = 15; // 原型只做Day 1-15

        // 属性系统
        this.attributes = {
            knowledge: { level: 1, exp: 0, expToNext: 100 },
            courage: { level: 1, exp: 0, expToNext: 100 },
            charm: { level: 1, exp: 0, expToNext: 100 },
            combat: { level: 1, exp: 0, expToNext: 100 }
        };

        // 关系系统
        this.relationships = {
            akira: { name: '晓', level: 2, exp: 0 },
            rei: { name: '零', level: 1, exp: 0 }
        };

        // 任务系统
        this.missions = [
            {
                id: 'mission_01',
                name: '第一个任务',
                deadline: 15,
                progress: 0,
                required: 3
            }
        ];

        // 完成的活动记录
        this.completedActivities = [];
        this.todayActivities = [];

        // 事件标记
        this.flags = [];
    }

    // 添加属性经验
    addAttributeExp(attrName, amount) {
        const attr = this.attributes[attrName];
        attr.exp += amount;

        // 检查是否升级
        while (attr.exp >= attr.expToNext && attr.level < 10) {
            attr.exp -= attr.expToNext;
            attr.level++;
            attr.expToNext = Math.floor(attr.expToNext * 1.5); // 经验需求递增

            showNotification(`${attrName} 升级到 Lv ${attr.level}!`, 'success');
        }

        // 最高10级
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
                showNotification(`与 ${this.relationships[charId].name} 的关系提升!`, 'success');
            }
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

            // 检查是否到达截止日期
            this.checkDeadlines();
        }
    }

    // 检查任务截止日期
    checkDeadlines() {
        this.missions.forEach(mission => {
            if (this.currentDay === mission.deadline) {
                showNotification(`今天是"${mission.name}"的截止日期！`, 'warning');
            }
        });
    }

    // 检查游戏是否结束
    isGameOver() {
        return this.currentDay > this.maxDay;
    }
}

// 全局游戏状态
let gameState = new GameState();

// ===== 事件数据 =====
const eventsData = [
    // 学习类
    {
        id: 'study_library',
        name: '图书馆学习',
        description: '在图书馆安静地学习，提升知识。',
        icon: '📚',
        category: 'study',
        conditions: {
            timeslots: ['morning', 'afternoon', 'night'],
            weekdayOnly: true
        },
        effects: {
            knowledge: 15
        },
        repeatable: true
    },
    {
        id: 'attend_lecture',
        name: '参加讲座',
        description: '参加学术讲座，大幅提升知识。',
        icon: '🎓',
        category: 'study',
        conditions: {
            timeslots: ['afternoon'],
            minDay: 5
        },
        effects: {
            knowledge: 25,
            charm: 5
        },
        repeatable: true,
        cooldown: 3
    },

    // 训练类
    {
        id: 'gym_training',
        name: '训练场锻炼',
        description: '在训练场进行战斗训练。',
        icon: '💪',
        category: 'training',
        conditions: {
            timeslots: ['morning', 'afternoon', 'night']
        },
        effects: {
            combat: 20,
            courage: 5
        },
        repeatable: true
    },
    {
        id: 'adventure_challenge',
        name: '冒险挑战',
        description: '尝试高难度的冒险活动，锻炼勇气。',
        icon: '⚔️',
        category: 'training',
        conditions: {
            timeslots: ['afternoon'],
            minDay: 3
        },
        effects: {
            courage: 30,
            combat: 10
        },
        repeatable: true,
        cooldown: 2
    },

    // 社交类
    {
        id: 'hangout_akira',
        name: '和晓一起运动',
        description: '和青梅竹马晓一起运动，增进关系。',
        icon: '🏃',
        category: 'social',
        conditions: {
            timeslots: ['afternoon']
        },
        effects: {
            courage: 5,
            combat: 10,
            relationships: { akira: 20 }
        },
        repeatable: true,
        cooldown: 2
    },
    {
        id: 'chat_rei',
        name: '和零在图书馆聊天',
        description: '和零讨论学术问题，增进关系。',
        icon: '💬',
        category: 'social',
        conditions: {
            timeslots: ['afternoon', 'night'],
            minDay: 2
        },
        effects: {
            knowledge: 10,
            relationships: { rei: 20 }
        },
        repeatable: true,
        cooldown: 2
    },

    // 打工类
    {
        id: 'part_time_cafe',
        name: '咖啡店打工',
        description: '在咖啡店打工，提升魅力并赚钱。',
        icon: '☕',
        category: 'work',
        conditions: {
            timeslots: ['morning', 'afternoon', 'night']
        },
        effects: {
            charm: 15
        },
        repeatable: true
    },

    // 魅力类
    {
        id: 'shopping',
        name: '购物打扮',
        description: '购买新衣服，提升魅力。',
        icon: '👔',
        category: 'charm',
        conditions: {
            timeslots: ['afternoon']
        },
        effects: {
            charm: 20
        },
        repeatable: true,
        cooldown: 3
    },
    {
        id: 'watch_movie',
        name: '观看电影',
        description: '去电影院看电影，放松心情。',
        icon: '🎬',
        category: 'charm',
        conditions: {
            timeslots: ['night']
        },
        effects: {
            charm: 15,
            knowledge: 5
        },
        repeatable: true
    },

    // 任务相关
    {
        id: 'investigate_victim',
        name: '调查受害者',
        description: '收集第一个受害者的情报。',
        icon: '🔍',
        category: 'mission',
        conditions: {
            timeslots: ['afternoon', 'night'],
            minDay: 10
        },
        effects: {
            knowledge: 10,
            courage: 10
        },
        onComplete: (gameState) => {
            gameState.missions[0].progress++;
        },
        repeatable: false
    },
    {
        id: 'explore_cognitive_space',
        name: '认知空间探索',
        description: '进入练习用的认知空间。',
        icon: '🌀',
        category: 'mission',
        conditions: {
            timeslots: ['night'],
            minDay: 12,
            minProgress: 2
        },
        effects: {
            combat: 30,
            courage: 15
        },
        onComplete: (gameState) => {
            gameState.missions[0].progress++;
        },
        repeatable: true,
        cooldown: 2
    },

    // 休息
    {
        id: 'rest',
        name: '早点睡觉',
        description: '早点休息，恢复精力。',
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
        },
        repeatable: true
    }
];

// ===== 事件冷却管理 =====
let eventCooldowns = {};

function isEventAvailable(event) {
    // 检查时间段
    if (event.conditions.timeslots &&
        !event.conditions.timeslots.includes(gameState.currentTimeslot)) {
        return false;
    }

    // 检查最小天数
    if (event.conditions.minDay && gameState.currentDay < event.conditions.minDay) {
        return false;
    }

    // 检查任务进度
    if (event.conditions.minProgress &&
        gameState.missions[0].progress < event.conditions.minProgress) {
        return false;
    }

    // 检查冷却
    if (eventCooldowns[event.id] && eventCooldowns[event.id] > 0) {
        return false;
    }

    // 检查是否可重复
    if (!event.repeatable && gameState.completedActivities.includes(event.id)) {
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
    updateMissions();

    // 更新事件列表
    updateEvents();
}

function updateAttributes() {
    ['knowledge', 'courage', 'charm', 'combat'].forEach(attr => {
        const data = gameState.attributes[attr];
        const percentage = data.level < 10 ? (data.exp / data.expToNext) * 100 : 100;

        document.getElementById(`${attr}-bar`).style.width = `${percentage}%`;
        document.getElementById(`${attr}-value`).textContent =
            data.level < 10 ? `Lv ${data.level} (${data.exp}/${data.expToNext})` : `Lv ${data.level} (MAX)`;
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

function updateMissions() {
    const container = document.getElementById('missions-container');
    container.innerHTML = '';

    gameState.missions.forEach(mission => {
        const div = document.createElement('div');
        div.className = 'mission-item';
        div.innerHTML = `
            <div class="mission-name">${mission.name}</div>
            <div>进度: ${mission.progress}/${mission.required}</div>
            <div class="mission-deadline">截止: Day ${mission.deadline}</div>
        `;
        container.appendChild(div);
    });
}

function updateEvents() {
    const container = document.getElementById('events-container');
    container.innerHTML = '';

    const availableEvents = eventsData.filter(event => isEventAvailable(event));

    if (availableEvents.length === 0) {
        container.innerHTML = '<p style="grid-column: 1/-1; text-align: center; color: #999;">当前时间段没有可用的活动</p>';
    }

    availableEvents.forEach(event => {
        const card = createEventCard(event);
        container.appendChild(card);
    });
}

function createEventCard(event) {
    const card = document.createElement('div');
    card.className = 'event-card';

    // 创建奖励标签
    let rewardsHTML = '';
    if (event.effects) {
        rewardsHTML = '<div class="event-rewards">';

        if (event.effects.knowledge) {
            rewardsHTML += `<span class="reward-tag knowledge">+${event.effects.knowledge} 知识</span>`;
        }
        if (event.effects.courage) {
            rewardsHTML += `<span class="reward-tag courage">+${event.effects.courage} 勇气</span>`;
        }
        if (event.effects.charm) {
            rewardsHTML += `<span class="reward-tag charm">+${event.effects.charm} 魅力</span>`;
        }
        if (event.effects.combat) {
            rewardsHTML += `<span class="reward-tag combat">+${event.effects.combat} 战斗</span>`;
        }
        if (event.effects.relationships) {
            Object.entries(event.effects.relationships).forEach(([char, value]) => {
                const charName = gameState.relationships[char]?.name || char;
                rewardsHTML += `<span class="reward-tag relationship">+${value} ${charName}</span>`;
            });
        }

        rewardsHTML += '</div>';
    }

    card.innerHTML = `
        <div class="event-icon">${event.icon}</div>
        <div class="event-name">${event.name}</div>
        <div class="event-description">${event.description}</div>
        ${rewardsHTML}
    `;

    card.addEventListener('click', () => executeEvent(event));

    return card;
}

// ===== 事件执行 =====
function executeEvent(event) {
    // 应用效果
    const results = [];

    if (event.effects.knowledge) {
        gameState.addAttributeExp('knowledge', event.effects.knowledge);
        results.push(`知识 +${event.effects.knowledge}`);
    }
    if (event.effects.courage) {
        gameState.addAttributeExp('courage', event.effects.courage);
        results.push(`勇气 +${event.effects.courage}`);
    }
    if (event.effects.charm) {
        gameState.addAttributeExp('charm', event.effects.charm);
        results.push(`魅力 +${event.effects.charm}`);
    }
    if (event.effects.combat) {
        gameState.addAttributeExp('combat', event.effects.combat);
        results.push(`战斗 +${event.effects.combat}`);
    }

    // 关系变化
    if (event.effects.relationships) {
        Object.entries(event.effects.relationships).forEach(([char, value]) => {
            gameState.addRelationshipExp(char, value);
            results.push(`${gameState.relationships[char].name} 好感度 +${value}`);
        });
    }

    // 执行回调
    if (event.onComplete) {
        event.onComplete(gameState);
    }

    // 记录活动
    gameState.completedActivities.push(event.id);
    gameState.todayActivities.push(event.name);

    // 设置冷却
    if (event.cooldown) {
        eventCooldowns[event.id] = event.cooldown;
    }

    // 显示结果
    showEventResult(event, results);

    // 推进时间
    gameState.advanceTime();

    // 更新UI
    updateUI();

    // 检查是否是夜晚结束
    if (gameState.currentTimeslot === 'morning') {
        showDaySummary();
    }

    // 检查游戏是否结束
    if (gameState.isGameOver()) {
        showGameOver();
    }
}

// ===== 弹窗显示 =====
function showEventResult(event, results) {
    const modal = document.getElementById('result-modal');
    document.getElementById('result-title').textContent = event.name;
    document.getElementById('result-description').textContent = event.description;

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
    gameState.todayActivities.forEach(activity => {
        const li = document.createElement('li');
        li.textContent = activity;
        activitiesList.appendChild(li);
    });

    // 属性变化（简化显示）
    const attrsContainer = document.getElementById('summary-attributes');
    attrsContainer.innerHTML = '';
    ['knowledge', 'courage', 'charm', 'combat'].forEach(attr => {
        const data = gameState.attributes[attr];
        const div = document.createElement('div');
        div.className = 'attr-change positive';
        div.innerHTML = `
            <div>${attr}</div>
            <div>Lv ${data.level}</div>
        `;
        attrsContainer.appendChild(div);
    });

    // 减少冷却
    Object.keys(eventCooldowns).forEach(id => {
        eventCooldowns[id]--;
        if (eventCooldowns[id] <= 0) {
            delete eventCooldowns[id];
        }
    });

    modal.classList.add('active');
}

function showGameOver() {
    const modal = document.getElementById('game-over-modal');

    // 最终属性
    const attrsContainer = document.getElementById('final-attributes');
    attrsContainer.innerHTML = '';
    ['knowledge', 'courage', 'charm', 'combat'].forEach(attr => {
        const data = gameState.attributes[attr];
        const div = document.createElement('div');
        div.className = 'final-attr';
        div.innerHTML = `
            <span class="final-attr-name">${attr}</span>
            <span class="final-attr-value">Lv ${data.level}</span>
        `;
        attrsContainer.appendChild(div);
    });

    // 活动统计
    document.getElementById('final-activity-count').textContent =
        `共完成 ${gameState.completedActivities.length} 个活动`;

    modal.classList.add('active');
}

function showNotification(message, type = 'info') {
    // 简单的通知实现（可以后续美化）
    console.log(`[${type.toUpperCase()}] ${message}`);
}

// ===== 事件监听 =====
document.getElementById('close-modal-btn').addEventListener('click', () => {
    document.getElementById('result-modal').classList.remove('active');
});

document.getElementById('close-summary-btn').addEventListener('click', () => {
    document.getElementById('day-end-modal').classList.remove('active');
});

document.getElementById('restart-btn').addEventListener('click', () => {
    gameState = new GameState();
    eventCooldowns = {};
    document.getElementById('game-over-modal').classList.remove('active');
    updateUI();
});

// ===== 初始化 =====
document.addEventListener('DOMContentLoaded', () => {
    updateUI();
    console.log('游戏已启动！');
    console.log('这是Day 1-15的原型，测试时间管理和选择系统。');
});
