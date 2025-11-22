# CTRL-ZED Architecture Guide

## Current GameObject Hierarchy

### Persistent Scene (Loaded Once, Never Destroyed)

```
[DontDestroyOnLoad]
│
├── GameInitiator
│   ├── CoreEventBus          ← Auto-created, persistent events
│   └── (manages all core systems)
│
├── GameManager
│   ├── PlayerDataManager
│   ├── PlayerManager
│   ├── CharacterManager
│   ├── TeamManager
│   ├── AntiVirusManager
│   ├── LevelManager
│   └── StatusEffectManager
│
├── GameStateManager
│   └── (handles scene/state transitions)
│
├── InputService
│   └── (WASD, skills, mouse input)
│
├── SoundManager
│   └── (BGM, SFX, audio pooling)
│
└── UIManager
    └── (persistent UI elements)
```

### Gameplay Scene (Loaded/Unloaded per level)

```
GameplayScene
│
├── GameplayManager
│   └── (orchestrates gameplay)
│
├── SceneEventBus              ← Auto-created, destroyed with scene
│
├── GameplaySystems
│   ├── EnemyManager
│   ├── WaveManager
│   ├── LootManager
│   ├── EnemySpawner
│   └── DamageNumberService
│
├── PlayerSystems
│   ├── FollowerManager
│   ├── PlayerGameplayManager
│   └── PlayerCharacters/
│       ├── Character_0 (PlayerCombat, PlayerMovement)
│       ├── Character_1
│       ├── Character_2
│       └── Character_3
│
├── GameplayUI
│   ├── CharacterListUI
│   ├── WaveUIController
│   ├── OSHPUI (Health)
│   ├── AnnouncementUI
│   ├── CompleteScreenUI
│   ├── LootHolder
│   └── SkillSlots
│
├── Environment
│   ├── ParallaxBackground
│   ├── Boundaries
│   └── SpawnPoints
│
└── PooledObjects/             ← Created by PoolManager
    ├── Projectiles
    ├── Enemies
    ├── DamageNumbers
    └── LootDrops
```

### Main Menu Scene

```
MainMenuScene
│
├── SceneEventBus              ← Scene-specific events
│
├── MainMenuUI
│   ├── PlayButton
│   ├── SettingsButton
│   ├── TeamSelectionUI
│   ├── CharacterSelectionUI
│   └── AntiVirusUI
│
└── MenuBackground
```

---

## Large-Scale Project Architecture

For teams of 5+ developers or projects lasting 2+ years, here's the recommended structure:

### Folder Structure (Vertical Slice Architecture)

```
Assets/
├── _Bootstrap/                    # Entry point, nothing else
│   └── BootstrapScene.unity
│
├── Core/                          # Framework layer (rarely changes)
│   ├── DI/                        # Dependency Injection
│   │   ├── DIContainer.cs
│   │   ├── GameServices.cs
│   │   ├── CoreCompositionRoot.cs
│   │   ├── GameplayCompositionRoot.cs
│   │   └── Factories/
│   │       ├── EnemyFactory.cs
│   │       └── CharacterBattleStateFactory.cs
│   │
│   ├── Events/
│   │   ├── IEventBus.cs
│   │   ├── EventBus.cs
│   │   ├── CoreEventBus.cs        # Persistent events
│   │   ├── SceneEventBus.cs       # Scene-scoped events
│   │   ├── CoreEvents/
│   │   │   ├── GameStateEvents.cs
│   │   │   ├── PlayerEvents.cs
│   │   │   └── SettingsEvents.cs
│   │   └── GameplayEvents/
│   │       ├── WaveEvents.cs
│   │       ├── CombatEvents.cs
│   │       ├── CharacterEvents.cs
│   │       ├── EnemyEvents.cs
│   │       └── LootEvents.cs
│   │
│   ├── Commands/
│   │   ├── ICommand.cs
│   │   └── CommandQueue.cs
│   │
│   ├── StateMachine/
│   │   ├── IState.cs
│   │   ├── StateMachine.cs
│   │   └── States/
│   │       ├── GameplayStateBase.cs
│   │       ├── GameplayStartState.cs
│   │       ├── GameplayPlayingState.cs
│   │       ├── GameplayReviveState.cs
│   │       └── GameplayEndState.cs
│   │
│   ├── Pooling/
│   │   ├── IPoolable.cs
│   │   ├── ObjectPool.cs
│   │   ├── PoolManager.cs
│   │   └── PoolableProjectile.cs
│   │
│   ├── Services/
│   │   └── ServiceLocator.cs      # Legacy, for backward compatibility
│   │
│   ├── Managers/
│   │   ├── GameInitiator.cs
│   │   ├── GameManager.cs
│   │   ├── GameplayManager.cs
│   │   └── GameStateManager.cs
│   │
│   ├── Interfaces/
│   │   ├── IEnemyManager.cs
│   │   ├── IWaveManager.cs
│   │   ├── ILootManager.cs
│   │   ├── IFollowerManager.cs
│   │   ├── ISoundService.cs
│   │   ├── IInputService.cs
│   │   └── IDamageNumberService.cs
│   │
│   ├── Shared/
│   │   ├── InputService.cs
│   │   ├── SoundManager.cs
│   │   └── StatusEffects/
│   │
│   └── System/
│       └── SaveSystem/
│
├── Features/                      # Domain modules (vertical slices)
│   ├── Player/
│   │   ├── Data/
│   │   ├── Interface/
│   │   ├── Services/
│   │   │   └── PlayerService.cs
│   │   └── Gameplay/
│   │       └── Systems/
│   │           └── PlayerCombat.cs
│   │
│   ├── Characters/
│   │   ├── Data/
│   │   ├── Services/
│   │   └── Factory/
│   │
│   ├── Enemies/
│   │   ├── EnemyService.cs
│   │   ├── EnemyManager.cs
│   │   ├── EnemyFollow.cs
│   │   └── EnemySpawner.cs
│   │
│   ├── Skills/
│   │   ├── ISkill.cs
│   │   ├── SkillHolder.cs
│   │   ├── Commands/
│   │   │   └── UseSkillCommand.cs
│   │   └── Implementations/
│   │       ├── Block/
│   │       ├── Heal/
│   │       ├── Stun/
│   │       └── ...
│   │
│   ├── Weapon/
│   │   ├── WeaponHolder.cs
│   │   └── Commands/
│   │       └── FireCommand.cs
│   │
│   ├── Wave/
│   │   ├── WaveManager.cs
│   │   └── WaveService.cs
│   │
│   ├── Loots/
│   │   ├── LootManager.cs
│   │   └── LootCollect.cs
│   │
│   └── Teams/
│       └── TeamManager.cs
│
├── UI/
│   └── Views/
│       ├── Gameplay/
│       │   ├── GameplayUIController.cs
│       │   ├── Wave/
│       │   │   └── WaveUIController.cs
│       │   └── _components/
│       │       └── OSHPUI.cs
│       └── Menu/
│
└── Tests/
    ├── EditMode/                  # Unit tests
    └── PlayMode/                  # Integration tests
```

---

## Architectural Layers

```
┌─────────────────────────────────────────────────────────────┐
│                      PRESENTATION                           │
│         (UI, Views, MonoBehaviours, Input Handling)         │
│                                                             │
│  GameplayUIController, WaveUIController, OSHPUI, etc.       │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                      APPLICATION                            │
│     (Use Cases, Commands, Queries, Feature Controllers)     │
│                                                             │
│  CommandQueue, UseSkillCommand, FireCommand, StateMachine   │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                        DOMAIN                               │
│      (Entities, Business Rules, Pure C# - NO UNITY)         │
│                                                             │
│  PlayerService, CharacterService, EconomyService, etc.      │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                     INFRASTRUCTURE                          │
│    (Persistence, Networking, Platform APIs, Analytics)      │
│                                                             │
│  SaveSystem, SoundManager, InputService, ServiceLocator     │
└─────────────────────────────────────────────────────────────┘
```

### Layer Details (5+ Developer Teams)

#### 1. PRESENTATION Layer
**Responsibility**: Visual output and user input capture only.

| What belongs here | What does NOT belong here |
|-------------------|---------------------------|
| MonoBehaviours that display data | Business logic |
| UI Controllers (button handlers) | Data calculations |
| Input capture (mouse, keyboard) | Direct service calls |
| Animations, VFX triggers | Database/save operations |
| View binding (data → display) | State management logic |

**Rules**:
- Views are **dumb** - they only know how to display data
- Subscribe to events, never call services directly
- One-way data flow: Application → Presentation
- Use **Presenters** or **ViewModels** to format data

```csharp
// ✅ GOOD - Presentation layer
public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void OnEnable() => CoreEventBus.Subscribe<PlayerHealthChangedEvent>(OnHealthChanged);
    private void OnDisable() => CoreEventBus.Unsubscribe<PlayerHealthChangedEvent>(OnHealthChanged);

    private void OnHealthChanged(PlayerHealthChangedEvent e)
    {
        healthSlider.value = (float)e.CurrentHealth / e.MaxHealth;
    }
}

// ❌ BAD - Business logic in presentation
public class HealthBarUI : MonoBehaviour
{
    private void Update()
    {
        var player = GameManager.Instance.PlayerManager.playerService;
        var health = player.GetPlayerData().currentHealth;
        var maxHealth = player.CalculateMaxHealth(); // Business logic!
        healthSlider.value = health / maxHealth;
    }
}
```

---

#### 2. APPLICATION Layer
**Responsibility**: Orchestrate use cases without containing business rules.

| What belongs here | What does NOT belong here |
|-------------------|---------------------------|
| Commands (user actions) | Core business rules |
| Queries (data retrieval) | UI/rendering code |
| State machines | Direct Unity API calls |
| Use case orchestration | Data persistence |
| Event publishing | Complex calculations |

**Rules**:
- Commands are the **entry point** for all user actions
- No business logic - just coordinate domain services
- Can call multiple domain services in sequence
- Should be **unit testable** without Unity

```csharp
// ✅ GOOD - Application layer command
public class PurchaseItemCommand : ICommand
{
    private readonly IEconomyService _economy;
    private readonly IInventoryService _inventory;
    private readonly ItemData _item;

    public PurchaseItemCommand(IEconomyService economy, IInventoryService inventory, ItemData item)
    {
        _economy = economy;
        _inventory = inventory;
        _item = item;
    }

    public bool CanExecute() => _economy.CanAfford(_item.Price);

    public void Execute()
    {
        _economy.Spend(_item.Price);      // Delegate to domain
        _inventory.AddItem(_item);         // Delegate to domain
    }
}

// ❌ BAD - Business logic in application layer
public class PurchaseItemCommand : ICommand
{
    public void Execute()
    {
        // Calculating discount is DOMAIN logic!
        var discount = player.Level > 10 ? 0.1f : 0f;
        var finalPrice = item.Price * (1 - discount);
        player.Coins -= finalPrice;
    }
}
```

---

#### 3. DOMAIN Layer
**Responsibility**: All business rules and game logic. **Pure C# - NO Unity dependencies.**

| What belongs here | What does NOT belong here |
|-------------------|---------------------------|
| Game rules/formulas | MonoBehaviour |
| Services (PlayerService, etc.) | Transform, Vector3, GameObject |
| Entities (PlayerData, etc.) | Coroutines |
| Value calculations | Scene management |
| Validation logic | Input handling |

**Rules**:
- **Zero Unity references** - pure C# only
- Contains all formulas (damage calc, level curves, etc.)
- Should be **100% unit testable**
- Publishes events, never subscribes to Unity events
- Domain services are **injected**, never use singletons

```csharp
// ✅ GOOD - Pure domain service
public class DamageCalculator
{
    public int Calculate(int baseDamage, int attackerLevel, int defenderArmor, bool isCritical)
    {
        var levelBonus = 1 + (attackerLevel * 0.05f);
        var armorReduction = 1 - (defenderArmor / (100f + defenderArmor));
        var critMultiplier = isCritical ? 2f : 1f;

        return Mathf.RoundToInt(baseDamage * levelBonus * armorReduction * critMultiplier);
    }
}

// ❌ BAD - Unity code in domain
public class DamageCalculator
{
    public int Calculate(GameObject attacker, GameObject defender)
    {
        var distance = Vector3.Distance(attacker.transform.position, defender.transform.position);
        // This couples domain to Unity!
    }
}
```

**Domain Folder Structure**:
```
Features/
├── Player/
│   ├── Data/
│   │   └── PlayerData.cs           # Entity (pure data)
│   ├── Services/
│   │   ├── PlayerService.cs        # Orchestrates sub-services
│   │   ├── ExpService.cs           # Leveling formulas
│   │   ├── HealthService.cs        # HP calculations
│   │   └── EconomyService.cs       # Currency logic
│   └── Interfaces/
│       └── IPlayerService.cs
```

---

#### 4. INFRASTRUCTURE Layer
**Responsibility**: External concerns - persistence, networking, platform APIs.

| What belongs here | What does NOT belong here |
|-------------------|---------------------------|
| SaveSystem (JSON, PlayerPrefs) | Game rules |
| Network clients (REST, WebSocket) | UI code |
| Platform SDKs (Steam, Mobile) | Business calculations |
| Analytics wrappers | Command orchestration |
| Audio systems | State machines |
| Input abstraction | Domain entities |

**Rules**:
- Implements interfaces defined in Domain layer
- Can use Unity APIs (ScriptableObjects, PlayerPrefs, etc.)
- Handles serialization/deserialization
- Abstracts platform differences

```csharp
// Domain defines the contract
public interface ISaveService
{
    void Save(PlayerData data);
    PlayerData Load();
}

// Infrastructure implements it
public class JsonSaveService : ISaveService
{
    private readonly string _savePath;

    public void Save(PlayerData data)
    {
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(_savePath, json);
    }

    public PlayerData Load()
    {
        if (!File.Exists(_savePath)) return new PlayerData();
        var json = File.ReadAllText(_savePath);
        return JsonUtility.FromJson<PlayerData>(json);
    }
}
```

---

### Dependency Flow (Critical Rule)

```
Presentation → Application → Domain ← Infrastructure
     ↓              ↓           ↑           ↑
   Views       Commands     Services    Persistence

NEVER: Presentation → Infrastructure (skip layers)
NEVER: Domain → Presentation (wrong direction)
NEVER: Domain → Infrastructure (use interfaces)
```

**The Dependency Rule**:
- Inner layers (Domain) know NOTHING about outer layers
- Outer layers depend on inner layers, never reverse
- Infrastructure implements Domain interfaces (Dependency Inversion)

---

### Team Ownership (5+ Developers)

| Layer | Team/Role | Changes Frequency |
|-------|-----------|-------------------|
| Presentation | UI/UX Developer | High (visual polish) |
| Application | Feature Developer | Medium (new features) |
| Domain | Core/Senior Dev | Low (stable rules) |
| Infrastructure | Platform/Backend | Low (rarely changes) |

This separation allows:
- UI team can reskin without touching game logic
- Feature developers can add new commands without breaking core
- Domain changes require review (affects everything)
- Platform team can swap save systems without touching gameplay

---

## Scene Strategy (Additive Scenes)

```
Scenes/
├── _Persistent/                   # Always loaded
│   ├── BootstrapScene.unity       # Entry, loads everything else
│   ├── CoreScene.unity            # GameManager, Audio, Input
│   └── UISystemScene.unity        # Modal system, popups, loading screen
│
├── Levels/                        # Gameplay scenes (additive)
│   ├── Level_Tutorial.unity
│   ├── Level_01.unity
│   └── Level_02.unity
│
└── Menus/                         # Menu scenes (swap in/out)
    ├── MainMenuScene.unity
    ├── SettingsScene.unity
    └── InventoryScene.unity
```

### Loading Flow

```
Bootstrap → Load CoreScene (persistent)
          → Load UISystemScene (persistent)
          → Load MainMenuScene (swappable)

On Play → Unload MainMenuScene
        → Load Level_01 (additive)
        → Load GameplayUIScene (additive)
```

---

## Event Architecture (Scoped Events)

```
┌─────────────────────────────────────────────────────────────┐
│                    CoreEventBus (Persistent)                 │
├─────────────────────────────────────────────────────────────┤
│ PlayerHealthChangedEvent  → Any UI in any scene             │
│ PlayerDataChangedEvent    → Menu UI, HUD                    │
│ PlayerLevelUpEvent        → Notifications, effects          │
│ GameStateChangedEvent     → Scene transitions               │
│ SettingsChangedEvent      → Audio, display updates          │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                  SceneEventBus (Per Scene)                   │
├─────────────────────────────────────────────────────────────┤
│ EnemyDefeatedEvent        → EnemyManager, WaveManager       │
│ WaveStartedEvent          → WaveUIController, GameplayUI    │
│ WaveCompletedEvent        → Rewards, next wave              │
│ WaveProgressUpdatedEvent  → Wave slider UI                  │
│ LootCollectedEvent        → LootHolder UI                   │
│ DamageDealtEvent          → Damage numbers, sounds          │
└─────────────────────────────────────────────────────────────┘
```

### When to Use Which Event Bus

| Event Type | Bus | Why |
|------------|-----|-----|
| Player data changes | `CoreEventBus` | UI in any scene needs it |
| Player health | `CoreEventBus` | Persists across sessions |
| Game state changes | `CoreEventBus` | Affects entire game |
| Settings changed | `CoreEventBus` | Global effect |
| Enemy dies | `SceneEventBus` | Only gameplay cares |
| Wave events | `SceneEventBus` | Only gameplay cares |
| Damage dealt | `SceneEventBus` | Visual feedback only |
| Loot collected | `SceneEventBus` | Scene-specific UI |

---

## Dependency Injection Flow

```
┌─────────────────────────────────────────────────────────────┐
│                      GameInitiator                           │
│                         (Awake)                              │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                      DIContainer                             │
│              GameServices.Initialize(container)              │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                  CoreCompositionRoot                         │
│         Register: ISoundService, IInputService,              │
│         GameManager, GameStateManager, etc.                  │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼ (On Gameplay Scene Load)
┌─────────────────────────────────────────────────────────────┐
│                GameplayCompositionRoot                       │
│         Register: IEnemyManager, IWaveManager,               │
│         ILootManager, IFollowerManager, etc.                 │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                    Usage Anywhere                            │
│         var sound = GameServices.Get<ISoundService>();       │
│         var enemy = GameServices.Get<IEnemyManager>();       │
└─────────────────────────────────────────────────────────────┘
```

---

## Design Patterns Summary

| Pattern | Location | Purpose |
|---------|----------|---------|
| **Service Locator** | `ServiceLocator.cs` | Legacy service access |
| **Dependency Injection** | `DIContainer.cs`, `GameServices.cs` | Clean dependency management |
| **Event Bus** | `CoreEventBus.cs`, `SceneEventBus.cs` | Decoupled communication |
| **Command** | `ICommand.cs`, `CommandQueue.cs` | Encapsulate actions |
| **State Machine** | `StateMachine.cs`, `IState.cs` | Clean state transitions |
| **Object Pool** | `ObjectPool.cs`, `PoolManager.cs` | Performance optimization |
| **Factory** | `EnemyFactory.cs`, `CharacterBattleStateFactory.cs` | Object creation with DI |
| **Singleton** | Various managers | Single instance access |
| **Composition Root** | `CoreCompositionRoot.cs`, `GameplayCompositionRoot.cs` | Wire dependencies |

---

## Key Principles

| Principle | Implementation |
|-----------|----------------|
| **Feature Isolation** | Vertical slices in Features/ folder |
| **Domain Separation** | Pure C# services (no Unity deps where possible) |
| **Explicit Dependencies** | DI over Singletons, interfaces over concrete |
| **Scene Scoping** | SceneEventBus auto-cleans on unload |
| **Data Flow** | Events up, Commands down |
| **Testability** | Interfaces enable mocking |

---

## Migration Path

If you need to gradually migrate existing code:

1. **ServiceLocator → DI**: Both work side-by-side
2. **Direct calls → Events**: Add event publishing alongside existing calls
3. **Switch statements → State Machine**: States call existing methods internally
4. **Instantiate → Pool**: Pool wrapper calls Instantiate if pool not set up

This allows incremental adoption without breaking existing functionality.
