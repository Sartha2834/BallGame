# 🎯 Ball Game – 2D Slingshot Puzzle

A fun **2D slingshot physics puzzle game** built with **Unity** — inspired by the classic Angry Birds style gameplay. Pull back, aim, and launch your ball to hit targets and reach the goal!

---

## 🕹️ Gameplay

- **Drag** the ball to pull back the slingshot
- **Release** to launch the ball with physics-based force
- **Hit targets** along the way to earn bonus points
- **Reach the goal** to complete each level
- **Avoid** falling out of bounds — or it's a fail!

---

## ✨ Features

| Feature | Description |
|---|---|
| 🏹 Slingshot mechanic | Drag-and-release with animated rubber bands |
| 📐 Trajectory preview | Dotted arc preview while aiming |
| 🎯 Target system | Destructible targets with score bonuses |
| 🏆 Score system | Points for targets + level completion |
| ✅ Win / Fail panels | Visual feedback on success or failure |
| 🔢 2 Levels | Level 1 → Level 2 progression |
| 🔄 Retry / Next Level | Buttons to replay or advance |
| 📱 Android build | Exported `.apk` included |

---

## 🗂️ Project Structure

```
Assets/
├── BallLauncher.cs     # Slingshot drag, trajectory & launch logic
├── GameManager.cs      # Win/fail state, level transitions
├── ScoreManager.cs     # Score tracking and UI updates
├── Goal.cs             # Goal trigger — completes the level
├── Target.cs           # Destructible targets that add score
├── FailTrigger.cs      # Out-of-bounds / fail zone trigger
├── Scenes/
│   ├── Level1.unity
│   └── Level2.unity
└── Settings/           # URP render pipeline settings
```

---

## 🎮 Controls

| Input | Action |
|---|---|
| 🖱️ Click & Drag on Ball | Pull slingshot |
| 🖱️ Release | Launch ball |
| `R` key | Restart current level |

---

## 🛠️ Built With

- **Unity 2D** (Universal Render Pipeline)
- **C#** scripting
- **TextMesh Pro** for UI text
- **Unity Input System**
- **Physics2D** — Rigidbody2D, triggers, raycasts

---

## 🚀 How to Run

### In Unity Editor
1. Open the project folder in **Unity Hub**
2. Open `Assets/Scenes/Level1.unity`
3. Press **Play** ▶️

### Android APK
A pre-built APK is included: `Ballgame.apk`
1. Transfer to your Android device
2. Enable **Install from unknown sources**
3. Install and play!

---

## 📸 Screenshots

> *(Add screenshots of your game here!)*

---

## 👤 Author

Made with ❤️ using Unity.

---

## 📄 License

This project is open source. Feel free to use it as a learning reference!
