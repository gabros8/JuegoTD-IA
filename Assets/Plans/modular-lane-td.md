# Project Overview
- **Game Title:** Doodle Defense (Working Title)
- **High-Level Concept:** A 2D lane-based tower defense game where a child's sketches come to life to defend their "color" (black ink) from rival children.
- **Players:** Single-player.
- **Inspiration / Reference Games:** Plants vs. Zombies (Lanes), Battle Cats (Unit advancement/clash), Stick War (Unit types).
- **Tone / Art Direction:** Hand-drawn notebook aesthetic, sketch-like outlines, paper textures, "living doodles."
- **Target Platform:** PC (Standalone Windows), scalable for Mobile.
- **Screen Orientation / Resolution:** Landscape (1920x1080).
- **Render Pipeline:** Universal Render Pipeline (URP).

# Game Mechanics
## Core Gameplay Loop
1. **Preparation:** View lanes and energy levels.
2. **Deployment:** Spend energy to draw and deploy units into specific lanes.
3. **Clash:** Allied and enemy units move horizontally; they stop and fight when they encounter each other.
4. **Advancement:** Winners of clashes continue moving. Allied units reaching the enemy side become "Reserve Units" (free deployment) and grant upgrades.
5. **Progression:** Survive waves, collect upgrades, and defeat the level to progress to harder challenges.

## Controls and Input Methods
- **Mouse/Touch:** Click/Tap to select units from the UI and click/drag into a lane to deploy.
- **Hotkeys:** 1-5 for unit selection, Q/W/E for abilities (New Input System).

# UI
- **HUD:** Top bar for Energy (ink pot), Player Lives (hearts), and Level Progress. Bottom bar for Unit Deployment (slots with cooldowns/costs) and Ability Buttons.
- **Lane Indicators:** Visual cues for where units can be placed.
- **Upgrade Screen:** Overlay appearing when a unit reaches the enemy side, offering 3 randomized power-ups.
- **Menus:** Main Menu (Notebook cover), Pause, Victory/Defeat (Doodle-style).

# Key Asset & Context
- **ScriptableObjects:** `UnitData` (stats), `WaveData` (spawn patterns), `UpgradeData` (modifiers), `AbilityData` (logic).
- **Prefabs:** `Unit_Base`, `Lane_Base`, `VFX_InkSplat`, `UI_UnitSlot`.
- **Managers:** `GameManager` (state), `LaneManager` (lane logic), `EnergyManager` (ink regen), `WaveManager` (spawning).

# Implementation Steps

### Phase 1: Foundation & Folder Structure
1. Create the folder hierarchy as specified in the requirements.
2. Set up URP settings and a basic Camera.
3. Create `GameEvent` and `GameEventListener` ScriptableObjects for event-driven communication.

### Phase 2: Data & Core Systems
1. Implement `UnitData` SO: Health, Damage, Range, Speed, Cost, Cooldown.
2. Implement `EnergyManager`: Handle ink regeneration over time and "Force Deploy" logic.
3. Create `Lane` and `LaneManager`: Define start/end points and unit tracking per lane.

### Phase 3: Unit Framework & State Machine
1. Create `UnitController`: Base class with a State Machine (Idle, Move, Attack, Dead).
2. Implement `CombatHandler`: Range detection using `OverlapBoxAll` (filtered by lane and side) and damage application via an `IDamageable` interface.
3. Add `HealthSystem`: Handle HP, death events, and object pooling for units.

### Phase 4: Enemy & Wave System
1. Implement `WaveData` SO: List of units and spawn delays.
2. Create `WaveManager`: Sequence waves, scale difficulty, and spawn enemies into lanes.
3. Implement enemy AI logic (identical to player units but moving in the opposite direction).

### Phase 5: Reserve & Upgrade Systems
1. Implement `ReserveSystem`: Inventory for units that reached the enemy side (removing cost/cooldown).
2. Create `UpgradeManager`: Logic for applying modifiers (e.g., `unitData.attackDamage += 5`) and the selection UI.

### Phase 6: Abilities & VFX
1. Implement `AbilityManager`: Base class for Laser, Freeze, Bomb, etc.
2. Add "Hand-drawn" visual feedback: Ink splats for death, "sketching" effect for spawning.

### Phase 7: UI & Polishing
1. Build the HUD using UI Toolkit (preferred) or Canvas.
2. Connect `EnergyManager` and `WaveManager` to UI elements via events.
3. Implement Save/Load for progress and settings.

# Verification & Testing
- **Unit Test:** Verify `EnergyManager` does not allow deployment if energy is < cost (unless Force Deploy is used).
- **Combat Test:** Ensure two units in the same lane stop and damage each other correctly.
- **Lane Test:** Confirm units cannot move or detect enemies outside their assigned lane.
- **End-to-End Test:** Run a full wave, reach the enemy side with one unit, select an upgrade, and verify the stat change in the next clash.
