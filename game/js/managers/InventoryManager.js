/**
 * InventoryManager - 背包/道具系统管理器
 */

export class InventoryManager {
    constructor() {
        this.inventory = {}; // { itemId: count }
        this.money = 500; // 初始金钱
        this.maxSlots = 50; // 最大道具种类
    }

    /**
     * 初始化
     */
    init(items) {
        this.items = items || [];
        console.log('[InventoryManager] Initialized with', this.items.length, 'item types');
    }

    /**
     * 重置
     */
    reset() {
        this.inventory = {};
        this.money = 500;
    }

    /**
     * 添加道具
     */
    addItem(itemId, count = 1) {
        const item = this.getItemData(itemId);
        if (!item) {
            console.error('[Inventory] Invalid item:', itemId);
            return false;
        }

        if (!this.inventory[itemId]) {
            // 检查背包空间
            if (Object.keys(this.inventory).length >= this.maxSlots) {
                return { success: false, message: '背包已满' };
            }
            this.inventory[itemId] = 0;
        }

        this.inventory[itemId] += count;

        console.log('[Inventory] Added', count, 'x', item.name);

        return {
            success: true,
            item: item,
            count: count,
            totalCount: this.inventory[itemId]
        };
    }

    /**
     * 移除道具
     */
    removeItem(itemId, count = 1) {
        if (!this.inventory[itemId] || this.inventory[itemId] < count) {
            return { success: false, message: '道具数量不足' };
        }

        this.inventory[itemId] -= count;

        if (this.inventory[itemId] <= 0) {
            delete this.inventory[itemId];
        }

        return {
            success: true,
            count: count,
            remaining: this.inventory[itemId] || 0
        };
    }

    /**
     * 使用道具
     */
    useItem(itemId, target) {
        const item = this.getItemData(itemId);
        if (!item) {
            return { success: false, message: '道具不存在' };
        }

        if (!this.hasItem(itemId)) {
            return { success: false, message: '没有此道具' };
        }

        // 根据道具类型执行效果
        const result = {
            success: true,
            item: item,
            effect: item.effect
        };

        switch (item.effect) {
            case 'heal':
                result.healing = item.value;
                result.message = `恢复了 ${item.value} HP`;
                break;

            case 'restore_sp':
                result.spRestore = item.value;
                result.message = `恢复了 ${item.value} SP`;
                break;

            case 'boost_attack':
                result.attackBoost = item.value;
                result.message = `攻击力提升 ${item.value * 100}%`;
                break;

            case 'boost_defense':
                result.defenseBoost = item.value;
                result.message = `防御力提升 ${item.value * 100}%`;
                break;

            case 'revive':
                result.revive = item.value;
                result.message = `复活并恢复 ${item.value * 100}% HP`;
                break;

            default:
                result.message = `使用了 ${item.name}`;
        }

        // 消耗道具
        if (item.type === 'consumable') {
            this.removeItem(itemId, 1);
            result.consumed = true;
        }

        return result;
    }

    /**
     * 检查是否拥有道具
     */
    hasItem(itemId, count = 1) {
        return this.inventory[itemId] && this.inventory[itemId] >= count;
    }

    /**
     * 获取道具数量
     */
    getItemCount(itemId) {
        return this.inventory[itemId] || 0;
    }

    /**
     * 购买道具
     */
    buyItem(itemId, count = 1) {
        const item = this.getItemData(itemId);
        if (!item) {
            return { success: false, message: '道具不存在' };
        }

        const totalCost = item.price * count;

        if (this.money < totalCost) {
            return { success: false, message: '金钱不足' };
        }

        // 扣除金钱
        this.money -= totalCost;

        // 添加道具
        const addResult = this.addItem(itemId, count);

        if (addResult.success) {
            return {
                success: true,
                item: item,
                count: count,
                cost: totalCost,
                moneyRemaining: this.money
            };
        } else {
            // 如果添加失败，退还金钱
            this.money += totalCost;
            return addResult;
        }
    }

    /**
     * 卖出道具
     */
    sellItem(itemId, count = 1) {
        const item = this.getItemData(itemId);
        if (!item) {
            return { success: false, message: '道具不存在' };
        }

        if (!this.hasItem(itemId, count)) {
            return { success: false, message: '道具数量不足' };
        }

        const sellPrice = Math.floor(item.price * 0.5); // 卖出价格是购买价格的一半
        const totalValue = sellPrice * count;

        // 移除道具
        this.removeItem(itemId, count);

        // 增加金钱
        this.money += totalValue;

        return {
            success: true,
            item: item,
            count: count,
            value: totalValue,
            moneyTotal: this.money
        };
    }

    /**
     * 添加金钱
     */
    addMoney(amount) {
        this.money += amount;
        return this.money;
    }

    /**
     * 扣除金钱
     */
    removeMoney(amount) {
        if (this.money < amount) {
            return { success: false, message: '金钱不足' };
        }

        this.money -= amount;
        return { success: true, remaining: this.money };
    }

    /**
     * 获取金钱
     */
    getMoney() {
        return this.money;
    }

    /**
     * 获取所有道具
     */
    getAllItems() {
        const result = [];

        for (const [itemId, count] of Object.entries(this.inventory)) {
            const itemData = this.getItemData(itemId);
            if (itemData) {
                result.push({
                    ...itemData,
                    count: count
                });
            }
        }

        return result;
    }

    /**
     * 获取道具数据
     */
    getItemData(itemId) {
        return this.items.find(i => i.id === itemId);
    }

    /**
     * 获取可购买的道具列表
     */
    getShopItems() {
        return this.items.filter(item => item.price && item.price > 0);
    }

    /**
     * 获取存档数据
     */
    getSaveData() {
        return {
            inventory: { ...this.inventory },
            money: this.money
        };
    }

    /**
     * 加载数据
     */
    load(data) {
        if (data) {
            this.inventory = data.inventory || {};
            this.money = data.money || 500;
        }
    }
}
