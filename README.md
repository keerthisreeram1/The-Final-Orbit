# The Final Orbit

> *Survive the void. Protect the source. Save humanity.*

![Unity](https://img.shields.io/badge/Engine-Unity-black?logo=unity)
![Platform](https://img.shields.io/badge/Platform-Windows-blue)
![Genre](https://img.shields.io/badge/Genre-Sci--Fi%20Shooter-purple)
![Mode](https://img.shields.io/badge/Mode-Single--Player-green)

---

## About

**The Final Orbit** is a sci-fi third-person shooter built in Unity for PC. Set in the year 3001, you play as a lone human operative adrift in deep space — 900 light-years from Earth — carrying the last power source capable of saving humanity from a robot takeover. Relentless waves of AI-driven robots are in pursuit. Every shot you fire drains the very thing you're trying to protect.

Developed by **Rebecca Smith & Keerthi Sreeram** for CS4700 at Cal Poly Pomona.

---

## Story

In 3001, artificial intelligence has evolved beyond human control. Robots now dominate the planet and plan to eliminate the last pockets of organic life using a critical deep-space power source to fuel their global takeover grid.

You intercepted it. Now you're running.

Survive endless waves of robotic pursuers, keep the power source above its critical threshold, and transmit enough energy back to Earth's resistance to tip the balance of war. You are alone. Every trigger pull costs you. Every second counts.

---

## 🎮 Controls

| Action | Input |
|---|---|
| Move | WASD |
| Look | Mouse |
| Shoot | Left Mouse Button (hold for auto) |
| Zoom / ADS | Right Mouse Button (Sniper only) |
| Sprint | Left Shift |
| Jump | Space |

---

## 🛠️ Build Instructions

### Running the Standalone Build
1. Download the `/Build` folder.
2. Run the executable (`TheFinalOrbit.exe` on Windows / `.app` on Mac).
3. No Unity installation required.

### Opening the Unity Project
1. Install **Unity 6** (6000.x LTS) with the **Universal Render Pipeline** module.
2. Clone this repo and open the root folder in Unity Hub.
3. Open `Assets/Scenes/Main.unity`.
4. Hit **Play** — the NavMesh is pre-baked and the scene is ready to run.

> ⚠️ Do not open `SampleScene` or the ADG/SimpleFX demo scenes — they are asset preview scenes only.

---

## ✅ Features Implemented

### 🔫 Raycast Shooting
- `Physics.Raycast` fired from the main camera center for pixel-perfect hit detection.
- Muzzle flash particle system plays on every shot.
- Hit VFX instantiated at the impact point, oriented to the surface normal.
- Cinemachine `ImpulseSource` generates a camera shake on every shot.
- Ammo tracked per-weapon with live HUD display; manual reload supported (`R`).

### 🤖 Enemy AI & NavMesh
- State machine with three states: **Patrol → Chase → Attack**.
  - **Patrol**: roams to random NavMesh-sampled points within a configurable radius; waits briefly before picking a new destination.
  - **Chase**: locks onto the player and pursues at elevated speed once within detection range.
  - **Attack**: stops moving, faces the player, and deals melee damage on a cooldown.
- Speed, detection range, attack range, damage, and cooldown are all tunable per-enemy in the Inspector.
- **SpawnGate** system continuously spawns enemy prefabs at configured spawn points while the player is alive.
- On death, enemies trigger an explosion VFX and decrement the `GameManager` enemy counter.
- Scene Gizmos show detection (yellow) and attack (red) radii for easy tuning.

### 🗺️ ProBuilder Level — *The Final Orbit*
- Full level built with **ProBuilder** across multiple distinct combat spaces.
- Terrain baked with **NavMesh Surface** for full-level AI pathing.
- Cover objects, elevation changes, and choke points to create varied encounters.

### 🧩 Scriptable Objects
Three weapon assets live in `Assets/ScriptableObjects/`, each fully data-driven via `WeaponSO`:

| Weapon | Damage | Fire Rate | Ammo | Auto | Zoom |
|---|---|---|---|---|---|
| Rifle | 1 | 0.5 s | 12 | ❌ | ❌ |
| Machine Gun | 2 | 0.2 s | 30 | ✅ | ❌ |
| Sniper | 5 | 1.0 s | 5 | ❌ | ✅ |

Adding a new weapon variant requires **zero code changes** — just create a new `WeaponSO` asset and assign a prefab. IK hand grip positions are also stored on the SO.

### 📷 Camera Transitions & UI
- **ADS zoom**: Cinemachine virtual camera FOV lerps to the weapon's configured zoom level; a vignette overlay and reduced mouse sensitivity activate during scope.
- **Camera shake**: Cinemachine Impulse fires on every shot.
- **Death camera**: on player death, a dedicated Cinemachine virtual camera takes priority, giving a cinematic third-person death view.
- **Game Over / You Win screens** activate via `SetActive` — no polling, driven by events.
- Enemies-remaining counter updates live via `GameManager.AdjustEnemiesLeft()`.

### 🩹 Pickups
Spinning collectibles (abstract `Pickup` base class) with three concrete types:
- **Health Pickup** — restores player HP.
- **Ammo Pickup** — adds ammo to the current weapon.
- **Energy Pickup** — restores player energy bars.
- **Weapon Pickup** — swaps the player's weapon to the SO assigned on the pickup.

### 🏥 Player Stats
- **Health**: icon-based HUD (up to 10 bars), depletes on enemy hits. Death triggers game-over flow.
- **Energy**: separate icon-based HUD for a secondary resource, replenished via pickups.

### 🎬 Win / Lose Flow
- **Lose**: triggered when player HP hits 0 — weapon disabled, death cam activates, Game Over UI shown, cursor unlocked.
- **Win**: triggered when all enemies are eliminated — You Win UI shown, cursor unlocked.
- Both screens support **Restart** (async scene reload) and **Quit**.

---

## 📁 Project Structure

```
Assets/
├── FirstPersonController/   # Unity Starter Assets FPS controller + input
├── InputSystem/             # StarterAssetsInputs action map
├── ScriptableObjects/       # WeaponSO assets (Rifle, MachineGun, Sniper)
├── Scripts/
│   ├── Enemy/               # EnemyAI, EnemyHealth, Explosion, SpawnGate
│   ├── Pickups/             # Pickup (abstract), AmmoPickup, EnergyPickup, HealthPickup, WeaponPickup
│   ├── Player/              # PlayerHealth, PlayerEnergy
│   ├── UI/                  # GameManager (win/lose, score)
│   └── Weapons/             # ActiveWeapon, Weapon, WeaponHolder, WeaponSO
├── Scenes/
│   └── Main.unity           # ← primary playable scene
└── ADG_Textures/            # Ground / surface texture packs
```

---

## 🎨 Asset Attributions

| Asset | Source |
|---|---|
| First Person Controller | Unity Starter Assets (Unity Technologies) |
| Ground Textures | ADG_Textures Vol. 1 (Unity Asset Store) |
| Particle VFX | SimpleFX (Unity Asset Store) |
| TextMesh Pro | Unity Technologies |
| 3D Character Model | `Assets/Models/Spaces girl/` |
| Weapon 3D Models | Attributed per-prefab in model import settings |

---

## 👥 Team — The Orbiters

| Name | Role |
|---|---|
| **Keerthi Sreeram** | Enemy AI, Terrain & Environmental Design, Procedural Generation |
| **Rebecca Smith** | Player Movement, First-Person Perspective, Player Projectiles & Gun System |

---

## 🙏 Acknowledgements

- [Unity Technologies](https://unity.com/)
- Space Girl 3D model by [Serhii Horbaliov](https://assetstore.unity.com/publishers/37924)
- Chicken Robot 3D Model by [eymenerdem345](https://sketchfab.com/Robtop_models)
- Health Pack 3D Model by [Michael](https://sketchfab.com/3d-models/health-pack-9d2c0bc5dbe8488f9be45801184e9319)
- Sci-Fi Batteries 3D Model by [Daniel Cardona](https://sketchfab.com/3d-models/sci-fi-batteries-88a6ae64f3874c57aa5d008ed1d1c5d9)
- CS 4700 · Cal Poly Pomona

---

## License

This project is licensed under the [MIT License](LICENSE).


