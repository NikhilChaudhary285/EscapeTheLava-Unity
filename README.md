# Escape The Lava

A 16×8 grid-tapping arcade game built in Unity, developed as the Round 1 technical assignment for **FOG (Future of Gaming)**. Collect every diamond before the 30-second timer runs out — avoid the lava, or lose a life.

**▶️ Gameplay Demo:** [Watch on Google Drive](https://drive.google.com/file/d/1kl97PguLLynVB5UhuTNFQgPKyaEAgNOf/view?usp=sharing)

---

## Overview

The player is presented with a 16×8 grid of tiles — Lava, Diamonds, and safe Islands. Tapping a diamond collects it and adds to the score; tapping lava costs a life. Collect every diamond on the board before the timer hits zero to win. Lose all 5 lives, or run out of time with diamonds still on the board, and it's game over.

The assignment brief explicitly weighted **animation and visual polish** as heavily as the core mechanic, so beyond the required rules, this implementation includes idle tile animations, tap-position score popups, particle VFX, screen shake, and fully animated win/loss sequences.

## Features

- **16×8 grid (128 tiles)** — Lava, Diamond, and Island tile types, randomly distributed each round
- **30-second countdown timer**, **5 lives**, live score/diamonds-collected tracking
- **Idle animations** — diamonds gently float, lava continuously glows/pulses
- **Tap-position score popups** — color-coded `+1` (diamond) / `-1` (lava) floating text spawning exactly where the player tapped
- **Particle VFX** — a directional splash/burn burst on lava hits, a sparkle burst on diamond collection
- **Screen shake** on lava impact for extra tactile feedback
- **Animated Win/Loss screen** — distinct gold/red stone-framed banners, scale+fade entrance animation, correct final score, one-tap Restart
- **Input safety** — tiles cannot be double-collected/double-damaged from a single tap, and all input is locked the instant a round ends
- **Responsive UI** — built with a Canvas Scaler for portrait mobile, verified at 1080×1920 and 1080×2400
- **Final production art** — a consistent, custom-generated stone-framed tile/UI art set (not placeholder shapes)

## Controls

| Action | Effect |
|---|---|
| Tap/click a **Diamond** tile | Collects it — score +1 |
| Tap/click a **Lava** tile | Costs 1 life |
| Tap/click an **Island** tile | Nothing happens (safe) |
| Tap **Restart** (after Win/Loss) | Starts a fresh round |

## How to Run

**Requirements:** Unity **2022.3 LTS** (or a compatible 2022.3.x patch), Universal Render Pipeline (2D Renderer) — included in the project.

```
git clone https://github.com/NikhilChaudhary285/EscapeTheLava-Unity.git
```

1. Open **Unity Hub** → **Add project from disk** → select the cloned folder.
2. Let Unity import (first open may take a minute).
3. Open `Assets/_Project/Scenes/Game.unity`.
4. Press **Play**.

## Technical Implementation

Built with a lightweight, event-driven architecture — one authoritative state manager, dumb/self-contained tiles, and decoupled UI that only reacts to events rather than owning game logic:

```
GameManager        — single source of truth for round state (Ready / Playing / Won / Lost);
                      owns win/loss decision logic and the global input-lock guard
GridManager         — spawns and holds the 16x8 board, forwards tile taps to GameManager
Tile                — per-cell type/state, idle animation (float/glow), tap detection
TimerController      — 30s countdown, raises OnTimerExpired
LivesController       — 5 lives, raises OnLivesDepleted
ScoreController        — score/diamond tracking, raises OnAllDiamondsCollected
UIManager               — HUD (timer/score/lives), listens only, no gameplay logic
ScorePopupSpawner         — floating +1/-1 text at tap position
VfxManager                 — lava splash + diamond sparkle particle effects
GameOverPanel               — CanvasGroup-driven show/hide with entrance animation,
                              subscribes to GameManager's round-end event
```

Communication runs through C# events/UnityEvents rather than direct cross-references, keeping `Tile` and the UI dumb/reactive and `GameManager` as the only place round outcomes are ever decided — this is also what guarantees a tile can't be double-tapped and input can't leak through after a round ends.

**Stack:** Unity 2022.3 LTS · URP (2D Renderer) · TextMeshPro · Unity UI (uGUI)

## Project Structure

```
Assets/_Project/
  Scenes/         Game.unity
  Scripts/
    Core/          GameManager, GameState
    Grid/           GridManager, Tile, TileType
    Systems/        TimerController, LivesController, ScoreController
    UI/              UIManager, ScorePopup, ScorePopupSpawner, GameOverPanel
    VFX/              VfxManager
  Prefabs/
    Tiles/           Tile, LavaSplashVFX, DiamondSparkleVFX
    UI/               Heart, ScorePopup
  Art/Sprites/         Final tile/UI artwork (Lava, Diamond, Island, Heart,
                       Win/Loss panel backgrounds, Restart button)
```

## Known Limitations

- Verified via mouse/click input in the Unity Editor and Game view; not tested on a physical touchscreen device build. Unity's standard click/tap handling (`OnMouseDown`) is expected to behave equivalently on touch, but this hasn't been confirmed on-device.
- Tablet-range aspect ratios (e.g. 4:3) show minor extra spacing versus the primary phone-portrait target — a known, accepted trade-off rather than a bug, since the assignment's tap/mobile framing prioritizes phone aspect ratios.

---

## About the Developer

**Nikhil Chaudhary**

- Portfolio: https://nikhilchaudhary285.github.io/
- GitHub: https://github.com/NikhilChaudhary285
- LinkedIn: https://www.linkedin.com/in/nikhilchaudhary285/

**Published work:** *Alnahsha Run* — [Google Play](https://play.google.com/store/apps/details?id=com.moderndoctors.alnahsharun) · [App Store](https://apps.apple.com/tr/app/%D8%A7%D9%84%D9%86%D8%AD%D8%B4%D8%A9-run/id1473582650)
