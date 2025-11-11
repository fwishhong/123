# 快速开始 - 立即游玩！

## ✅ **推荐方式：直接双击打开（无需服务器）**

**文件**：`index-standalone.html`

这是一个单文件版本，所有代码打包在一个HTML文件中，**可以直接用浏览器打开**！

### Windows:
1. 进入 `game` 文件夹
2. 双击 `index-standalone.html`
3. 用浏览器打开即可开始游戏！

### Mac/Linux:
```bash
cd game
open index-standalone.html    # Mac
xdg-open index-standalone.html  # Linux
```

## 🌐 方式2：使用本地服务器（适合开发）

**文件**：`index.html`（模块化版本）

如果你想修改代码或开发，使用这个版本：

```bash
cd game

# 使用Python
python3 -m http.server 8000

# 或使用Node.js
npx http-server -p 8000

# 然后访问: http://localhost:8000
```

## 🎮 游戏说明

### 开始游戏
1. 点击"开始游戏"进入新游戏
2. 或点击"继续游戏"加载存档

### 基本玩法
- **选择活动**：每个时间段选择一个活动提升属性
- **查看关系**：点击底部"关系"查看角色好感度
- **查看日程**：点击"日程"查看任务截止日期
- **保存游戏**：点击右上角菜单 ☰ → 保存游戏

### 目标
在100天内完成尽可能多的任务，获得更好的结局！

## 🐛 调试命令

打开浏览器控制台（F12），输入：

```javascript
// 查看当前状态
debugGame.showStats()

// 快速测试
debugGame.setDay(50)              // 跳到第50天
debugGame.setAttribute("combat", 8) // 提升战斗力
debugGame.startBattle("enemy_shadow") // 开始战斗

// 保存/加载
debugGame.save()
debugGame.load()
```

## 📁 文件说明

| 文件 | 说明 | 使用场景 |
|------|------|---------|
| `index-standalone.html` | 单文件版本 | ✅ **直接双击打开**（推荐） |
| `index.html` | 模块化版本 | 需要本地服务器（开发用） |
| `README.md` | 完整文档 | 查看详细信息 |

## ⚠️ 常见问题

### Q: 为什么有两个 index 文件？
A:
- `index-standalone.html` - 单文件，可直接打开，**推荐使用**
- `index.html` - 模块化，需要服务器，适合开发

### Q: 双击打不开？
A:
1. 确保使用现代浏览器（Chrome/Firefox/Edge/Safari）
2. 右键 → 打开方式 → 选择浏览器
3. 或直接把文件拖到浏览器窗口

### Q: 点按钮没反应？
A:
- 打开控制台（F12）查看是否有错误
- 确认使用的是 `index-standalone.html` 而不是 `index.html`

### Q: 进度会保存吗？
A: 会的！游戏使用 LocalStorage 自动保存。

---

**立即开始你的认知空间之旅！** 🚀

直接双击 `index-standalone.html` 即可！
