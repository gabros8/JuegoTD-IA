# Project Technical Documentation: Modular Lane TD

## 1. Project Description
This project is a **Modular Lane-Based Tower Defense (TD)** game where the player defends against waves of enemies by deploying units across multiple parallel lanes. Unlike traditional static TD games, this experience focuses on unit deployment, resource management (Energy), and a "Reserve" system where units that successfully traverse the lane are returned to a pool for later use. The core experience is built around strategic lane management, ability usage, and wave-based progression.

## 2. Gameplay Flow / User Loop
1.  **Initialization**: The `GameManager` sets up the initial state (lives, game state), and `LaneManager` identifies available lanes.
2.  **Wave Start**: `WaveManager` begins spawning enemies according to `WaveData` configurations after a set delay.
3.  **Player Action**: 
    *   **Deploy Units**: Players spend Energy (managed by `EnergyManager`) to spawn allied units into lanes.
    *   **Use Abilities**: Players trigger special abilities (e.g., Lasers) using Energy and managing cooldowns.
4.  **Unit Interaction**: Allied and enemy units move towards opposite ends of their assigned lanes. When they detect an opponent within range (via `CombatHandler`), they stop to attack.
5.  **Lane Traversal**:
    *   **Enemy Reaches End**: The player loses a life; the unit is destroyed.
    *   **Ally Reaches End**: The unit is added to the `ReserveManager` and may trigger upgrades via `UpgradeManager`.
6.  **Game End**: The game ends in **Victory** if all waves are cleared or **Defeat** if lives reach zero.

## 3. Architecture
The project follows a **Manager-centric architecture** with a heavy reliance on **ScriptableObject-based Event Systems** to decouple components.
*   **Singleton Pattern**: Used for global managers (`GameManager`, `WaveManager`, `LaneManager`, `PoolManager`, `EnergyManager`, `AbilityManager`) to provide easy access to core systems.
*   **Observer Pattern**: Implemented via `GameEvent` and `GameEventListener` ScriptableObjects, allowing systems like the UI or Game Logic to respond to state changes (e.g., `onLivesChanged`) without direct references.
*   **Object Pooling**: `PoolManager` handles unit and VFX instantiation/destruction to optimize performance.
*   **Data-Driven Design**: Unit stats, wave compositions, and abilities are defined as `ScriptableObject` assets, allowing for rapid iteration without code changes.

`Location: Assets/Scripts/Core`

## 4. Game Systems & Domain Concepts

### Lane System
Manages the spatial organization of the game. Each lane acts as a self-contained combat corridor with its own spawn and target points.
*   `LaneManager`: Maintains a list of all active lanes and provides access to them.
*   `Lane`: Tracks units currently within its bounds and provides spatial data (spawn/target points) for unit navigation.
*   **Extension**: New lane types can be created by adding unique logic to the `Lane` class or creating specialized lane prefabs.
`Location: Assets/Scripts/Lanes`

### Unit & Combat System
Handles the movement, detection, and interaction of all entities.
*   `UnitController`: The state machine governing unit behavior (Idle, Moving, Attacking, Dead).
*   `CombatHandler`: Manages target scanning using the `Lane` unit registry and applies damage.
*   `HealthSystem`: Implements `IDamageable` to track health and trigger death events.
*   **Design Pattern**: **State Machine** (Unit States) and **Interface-based interaction** (`IDamageable`).
`Location: Assets/Scripts/Units`, `Assets/Scripts/Combat`

### Wave & Spawning System
Controls the progression of difficulty and enemy flow.
*   `WaveManager`: Executes a Coroutine-based routine to spawn units based on wave definitions.
*   `WaveData`: A ScriptableObject containing `WaveEntry` data (unit type, delay, lane index).
`Location: Assets/Scripts/Waves`

### Ability System
Allows for player-triggered interventions that consume resources.
*   `AbilityManager`: Tracks ability cooldowns and validates energy costs.
*   `AbilityData`: Abstract base for all abilities (e.g., `LaserAbilityData`).
*   **Extension**: Create a new class inheriting from `AbilityData` and implement the `Execute` method.
`Location: Assets/Scripts/Abilities`

## 5. Scene Overview
*   **SampleScene**: The primary gameplay scene. It contains the `Lane` setup, the `Main Camera`, the `Global Volume` for post-processing, and the `Manager` hierarchy where all singleton managers reside.
*   **Loading/Flow**: The project currently uses a single-scene structure. Scene transitions (e.g., Main Menu to Game) are intended to be handled by the `GameManager` or a dedicated Scene Loader (not currently implemented).

`Location: Assets/Scenes`

## 6. UI System
*(Note: While script files for UI are not present in the taxonomy, the project structure includes a UI folder and UGUI dependency.)*
*   **Framework**: Based on **Unity UI (UGUI)** as per the package list.
*   **Binding Logic**: Intended to use `GameEventListener` components to update UI elements (Health bars, Energy counters, Wave text) based on `GameEvent` triggers from the core managers.
*   **Structure**: Expected to have a `Canvas` containing sub-panels for HUD, Upgrade Shop, and Game Over screens.

`Location: Assets/Scripts/UI` (Structure reserved for implementation)

## 7. Asset & Data Model
*   **Unit Definitions**: `UnitData` assets store health, damage, speed, and prefab references.
*   **Wave Definitions**: `WaveData` assets define enemy sequences and timing.
*   **Abilities**: `AbilityData` assets define costs and effects.
*   **Prefabs**: 
    *   Units: Found in `Assets/Prefabs`, containing `UnitController`, `CombatHandler`, and `HealthSystem`.
    *   Lanes: Modular lane segments used to build the battlefield.
*   **Organization**: Assets are strictly categorized into folders (Animations, Art, Audio, Materials, Prefabs, ScriptableObjects, Scripts, VFX).

`Location: Assets/ScriptableObjects`, `Assets/Prefabs`

## 8. Notes, Caveats & Gotchas
*   **Lane Index -1**: In `WaveData`, a lane index of `-1` indicates that the unit should spawn in a random lane.
*   **Reserve System**: Units reaching the end of a lane are not destroyed; they are "recycled" via `ReserveManager`. Ensure `ReserveManager` is updated if unit logic changes.
*   **Scale and Physics**: The game uses a 2D-in-3D approach (using `Vector3.right` for movement in a 3D environment). Movement is currently translation-based rather than physics-based.
*   **Time Scale**: `GameManager` sets `Time.timeScale = 0` on Victory or Defeat. Any systems relying on `Time.deltaTime` will pause automatically.
*   **PoolManager Dependency**: The `UnitController` relies on `PoolManager` to return to the pool upon death. If the pooler is missing, units will fallback to `gameObject.SetActive(false)`.