# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

CTRL-ZED is a 2D tactical battle game built with Unity (URP 14.0.11) and C#. It features tower defense-style gameplay with character deployment, team management, wave-based enemy encounters, and a skill/leveling system.

## Build & Development

- **IDE**: Visual Studio with `CTRL-ZED.sln`
- **Unity Version**: 2022.x (URP)
- **Build**: Unity Editor → File → Build Settings → Build
- **Play**: Unity Editor Play button (Ctrl+P)
- **Tests**: Unity Test Runner (Window → Testing → Test Runner)

### Key Dependencies
- **UniTask** (`com.cysharp.unitask`) - Async/await pattern for Unity
- **Cinemachine** - Camera management
- **TextMesh Pro** - UI text rendering

## Architecture

### Service-Oriented Architecture with SOLID Principles

The codebase follows Interface Segregation and Dependency Inversion patterns. Each domain is encapsulated in a service with a focused interface.

**Core pattern example:**
```csharp
// Services are injected, not directly instantiated
playerService = new PlayerService(data, expService, healthService, economyService, resourceService, drivesService);

// PlayerService implements multiple narrow interfaces
public class PlayerService : IResourceService, IEconomyService, IExpService, IHealthService, IDrivesService
```

### Key Architectural Patterns
- **Singleton Managers**: GameManager.Instance, GameStateManager.Instance, InputService.Instance
- **Factory Pattern**: CharacterFactory for character instantiation
- **Event System**: Decoupled communication via events (OnHealthChanged, OnLevelUp, OnCoinsChange)
- **State Machine**: GameState enum (Initial, MainMenu, Gameplay, Credits, Loading)

### Initialization Flow
1. **GameInitiator** (prefab singleton) → Entry point, initializes all managers
2. **GameManager** → Central orchestrator holding references to all domain managers
3. **GameplayManager** → Gameplay-specific logic (waves, spawning, combat)

## Project Structure

```
Assets/
├── Core/                      # Infrastructure & global systems
│   ├── Managers/              # GameInitiator, GameManager, GameStateManager
│   ├── Shared/                # InputService, SoundManager, StatusEffects, Stats
│   ├── System/                # Save/load system
│   └── Interfaces/            # Core interface definitions
│
├── Features/                  # Domain modules (each has Data/, Interface/, Services/)
│   ├── Player/                # Player services (Economy, Health, Exp, Resources, Drives)
│   ├── Characters/            # Character management and factory
│   ├── Enemies/               # Enemy AI and spawning
│   ├── Skills/                # Skill implementations (Block, Heal, Stun, etc.)
│   ├── Teams/                 # Team composition and switching
│   ├── Wave/                  # Wave/round management
│   └── Weapon/                # Weapons and projectiles
```

## Conventions

### Naming
- **Services**: `*Service` (PlayerService, EconomyService)
- **Managers**: `*Manager` (GameManager, CharacterManager)
- **Data Classes**: `*Data` (PlayerData, CharacterData)
- **Interfaces**: `I*` prefix (IEconomyService, IInputService)
- **Configs**: `*Config` (CharacterConfigs, WeaponConfig)

### Async Patterns
Use UniTask instead of coroutines:
```csharp
async UniTask DoSomethingAsync() { ... }
async UniTaskVoid FireAndForget() { ... }
```

### Service Design Rule
Ask: "Is this player-specific only, or does it interact with the game world?"
- Player-specific → Keep inside PlayerService
- Shared/global → Move to its own service or manager

## Game Systems

- **Resources**: 5 types - Coins, Technology, Energy, Intelligence, Food
- **Drives**: Action points that cost resources to spend
- **Skills**: Block, Disable, Firewall, Frenzy, Heal, Stealth, Stun, BirdsEye
- **Antivirus**: Currency upgrade system with buffs/debuffs
- **Status Effects**: Stun, Disable, Block, Firewall managed by StatusEffectManager

## Input (InputService)
- WASD/Arrow keys - Movement
- Q/E/R - Skills
- Space/LeftShift - Dash
- Mouse LMB - Fire
