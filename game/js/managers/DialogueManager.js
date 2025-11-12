/**
 * DialogueManager - 对话系统管理器
 * 处理视觉小说风格的对话和剧情
 */

export class DialogueManager {
    constructor() {
        this.currentDialogue = null;
        this.currentIndex = 0;
        this.dialogueHistory = [];
        this.choices = [];
        this.branch = null;
    }

    /**
     * 初始化
     */
    init() {
        console.log('[DialogueManager] Initialized');
    }

    /**
     * 开始对话事件
     */
    startDialogue(dialogueData) {
        this.currentDialogue = dialogueData;
        this.currentIndex = 0;
        this.branch = null;
        this.choices = [];

        console.log('[Dialogue] Started:', dialogueData.title);

        return {
            location: dialogueData.location,
            title: dialogueData.title,
            firstLine: this.getCurrentLine()
        };
    }

    /**
     * 获取当前对话行
     */
    getCurrentLine() {
        if (!this.currentDialogue) return null;

        let dialogue = this.currentDialogue.dialogue;

        // 如果在分支中，使用分支对话
        if (this.branch && this.currentDialogue.branches && this.currentDialogue.branches[this.branch]) {
            dialogue = this.currentDialogue.branches[this.branch];
        }

        if (this.currentIndex >= dialogue.length) {
            return null; // 对话结束
        }

        const line = dialogue[this.currentIndex];

        return {
            speaker: line.speaker,
            text: line.text,
            image: line.image,
            choices: line.choices || null,
            hasNext: this.currentIndex < dialogue.length - 1 || (line.choices && line.choices.length > 0)
        };
    }

    /**
     * 前进到下一行
     */
    advance() {
        if (!this.currentDialogue) return null;

        this.currentIndex++;
        const currentLine = this.getCurrentLine();

        // 如果对话结束，返回奖励
        if (!currentLine) {
            return {
                finished: true,
                rewards: this.currentDialogue.rewards
            };
        }

        return {
            finished: false,
            line: currentLine
        };
    }

    /**
     * 选择选项
     */
    makeChoice(choiceIndex) {
        const currentLine = this.getCurrentLine();
        if (!currentLine || !currentLine.choices) {
            console.error('[Dialogue] No choices available');
            return null;
        }

        const choice = currentLine.choices[choiceIndex];
        if (!choice) {
            console.error('[Dialogue] Invalid choice index:', choiceIndex);
            return null;
        }

        // 记录选择
        this.dialogueHistory.push({
            dialogue: this.currentDialogue.id,
            choiceIndex: choiceIndex,
            choiceText: choice.text
        });

        // 如果有分支，切换到分支
        if (choice.next) {
            this.branch = choice.next;
            this.currentIndex = 0; // 重置到分支的开始
        } else {
            this.currentIndex++; // 否则继续主线
        }

        return {
            relationshipChange: choice.relationshipChange || 0,
            nextLine: this.getCurrentLine()
        };
    }

    /**
     * 检查对话是否应该触发
     */
    checkTrigger(dialogueId, currentDay, currentPeriod, flags, relationships) {
        const dialogue = this.getDialogueData(dialogueId);
        if (!dialogue) return false;

        const cond = dialogue.conditions;
        if (!cond) return false;

        // 检查日期
        if (cond.day !== undefined && cond.day !== currentDay) {
            return false;
        }

        // 检查时间段
        if (cond.period !== undefined && cond.period !== currentPeriod) {
            return false;
        }

        // 检查标志
        if (cond.flags) {
            for (const flag of cond.flags) {
                if (!flags.has(flag)) {
                    return false;
                }
            }
        }

        // 检查好感度
        if (cond.relationshipLevel) {
            for (const [charId, required] of Object.entries(cond.relationshipLevel)) {
                if (relationships[charId] < required) {
                    return false;
                }
            }
        }

        return true;
    }

    /**
     * 获取对话数据（需要在初始化时注入）
     */
    getDialogueData(dialogueId) {
        // 这个方法需要访问游戏数据
        // 在实际使用中会从外部注入
        return null;
    }

    /**
     * 是否在对话中
     */
    isInDialogue() {
        return this.currentDialogue !== null;
    }

    /**
     * 结束对话
     */
    endDialogue() {
        const rewards = this.currentDialogue?.rewards || {};
        this.currentDialogue = null;
        this.currentIndex = 0;
        this.branch = null;
        this.choices = [];
        return rewards;
    }

    /**
     * 获取对话历史
     */
    getHistory() {
        return this.dialogueHistory;
    }

    /**
     * 获取存档数据
     */
    getSaveData() {
        return {
            dialogueHistory: this.dialogueHistory
        };
    }

    /**
     * 加载数据
     */
    load(data) {
        if (data) {
            this.dialogueHistory = data.dialogueHistory || [];
        }
    }
}
