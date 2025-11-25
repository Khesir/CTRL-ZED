# GlobalTooltipUI - Sliding Notification System

A global tooltip/notification UI system that slides from top or bottom to display messages. Perfect for showing unsuccessful transactions, errors, warnings, and general notifications via an event bus.

## Features

✅ Slide animations (up or down) with DOTween
✅ Event bus integration for decoupled messaging
✅ Message queue system (handles multiple messages gracefully)
✅ Four message types: Error, Warning, Info, Success
✅ Color-coded backgrounds for message types
✅ Optional icon support with pulse animation
✅ Optional typewriter text effect
✅ Configurable durations and animations
✅ Works with unscaled time (pause-friendly)
✅ Singleton pattern for global access

---

## Setup

### 1. Create the UI in Unity

1. **Create Canvas** (if you don't have one):
   - Right-click Hierarchy → UI → Canvas
   - Set Canvas Scaler to "Scale with Screen Size"

2. **Create Tooltip Container**:
   - Right-click Canvas → UI → Panel
   - Rename to "GlobalTooltip"
   - Set anchors to **top-center** (for slide down) or **bottom-center** (for slide up)
   - Set pivot to match anchor position
   - Example position for top: `(0, -75, 0)` with width `800`, height `100`

3. **Add Background Image**:
   - The Panel already has an Image component
   - Rename it to "Background"
   - This will be color-coded based on message type

4. **Add Text**:
   - Right-click Panel → UI → Text - TextMeshPro
   - Rename to "MessageText"
   - Center align, white color, appropriate font size (18-24)

5. **Optional - Add Icon**:
   - Right-click Panel → UI → Image
   - Rename to "Icon"
   - Position on the left side of the text
   - This is optional but recommended

6. **Add Component**:
   - Select the Panel
   - Add Component → `GlobalTooltipUI`
   - Assign references:
     - Tooltip Container: The Panel's RectTransform
     - Message Text: The TextMeshPro component
     - Background Image: The Background Image
     - Icon Image: The Icon Image (if you created one)

### 2. Configure Settings

**Animation Settings:**
- `Slide Direction`: Down (slides from top) or Up (slides from bottom)
- `Slide Distance`: How far to slide (default: 150)
- `Slide Duration`: Animation speed (default: 0.4s)
- `Slide In Ease`: OutBack gives a nice bouncy effect
- `Slide Out Ease`: InBack for smooth exit

**Display Settings:**
- `Default Display Duration`: 3 seconds
- `Min Display Duration`: 1.5 seconds
- `Max Display Duration`: 8 seconds

**Colors** (already set to good defaults):
- Error: Red
- Warning: Yellow/Orange
- Info: Blue
- Success: Green

**Optional Effects:**
- `Use Icon Pulse`: Icon pops in with animation
- `Use Text Typewriter`: Text types out character-by-character
- `Typewriter Speed`: Delay between characters

### 3. Make it Persist (Important!)

The GlobalTooltipUI uses `DontDestroyOnLoad`, so it will persist between scenes. Make sure it's in your main scene or a scene that loads first.

---

## Usage

### Method 1: Event Bus (Recommended)

The event bus allows any script to send tooltip messages without direct references:

```csharp
using Core.Shared.Events;

// Simple error
TooltipEventBus.PublishError("Not enough coins!");

// Simple warning
TooltipEventBus.PublishWarning("Your drives are low!");

// Simple info
TooltipEventBus.PublishInfo("Wave 5 incoming!");

// Simple success
TooltipEventBus.PublishSuccess("Character deployed!");

// Custom message with duration
TooltipEventBus.Publish(new TooltipMessage(
    "Boss wave incoming!",
    TooltipType.Warning,
    duration: 5f
));

// Custom message with icon
Sprite icon = Resources.Load<Sprite>("Icons/CoinIcon");
TooltipEventBus.Publish(new TooltipMessage(
    "Double coins event active!",
    TooltipType.Success,
    duration: 4f,
    icon: icon
));
```

### Method 2: Direct Call

```csharp
using Core.Shared.UI;

// Direct call to singleton
GlobalTooltipUI.Instance.ShowError("Not enough coins!");
GlobalTooltipUI.Instance.ShowWarning("Cooldown active!");
GlobalTooltipUI.Instance.ShowInfo("Wave starting...");
GlobalTooltipUI.Instance.ShowSuccess("Level up!");

// Custom message
GlobalTooltipUI.Instance.ShowMessage(new TooltipMessage(
    "Custom message",
    TooltipType.Error,
    duration: 3f
));
```

---

## Integration Examples

### Economy Service - Failed Transactions

```csharp
public class EconomyService : IEconomyService
{
    private int coins;

    public bool TryPurchase(int cost)
    {
        if (coins < cost)
        {
            TooltipEventBus.PublishError($"Not enough coins! Need {cost}, have {coins}");
            return false;
        }

        coins -= cost;
        TooltipEventBus.PublishSuccess($"Purchase successful! -{cost} coins");
        return true;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        TooltipEventBus.PublishSuccess($"+{amount} coins!");
    }
}
```

### Resource Service - Insufficient Resources

```csharp
public class ResourceService : IResourceService
{
    public bool TrySpendResource(string resourceType, int amount)
    {
        if (!HasEnough(resourceType, amount))
        {
            TooltipEventBus.PublishError($"Not enough {resourceType}!");
            return false;
        }

        Spend(resourceType, amount);
        return true;
    }
}
```

### Drives Service - Action Points

```csharp
public class DrivesService : IDrivesService
{
    private int currentDrives;

    public bool TrySpendDrives(int cost)
    {
        if (currentDrives < cost)
        {
            TooltipEventBus.PublishWarning($"Not enough drives! Need {cost}");
            return false;
        }

        currentDrives -= cost;
        return true;
    }
}
```

### Character Deployment Validation

```csharp
public class CharacterManager
{
    public bool TryDeployCharacter(Character character)
    {
        if (IsTeamFull())
        {
            TooltipEventBus.PublishError("Team is full!");
            return false;
        }

        if (IsInCombat())
        {
            TooltipEventBus.PublishWarning("Cannot deploy during combat!");
            return false;
        }

        DeployCharacter(character);
        TooltipEventBus.PublishSuccess($"{character.name} deployed!");
        return true;
    }
}
```

### Skill System - Cooldowns & Costs

```csharp
public class SkillService
{
    public bool TryUseSkill(Skill skill)
    {
        if (skill.IsOnCooldown)
        {
            TooltipEventBus.PublishWarning($"{skill.Name} is on cooldown!");
            return false;
        }

        if (!HasEnoughResources(skill.Cost))
        {
            TooltipEventBus.PublishError($"Not enough resources for {skill.Name}!");
            return false;
        }

        UseSkill(skill);
        TooltipEventBus.PublishInfo($"{skill.Name} activated!");
        return true;
    }
}
```

### Wave System - Notifications

```csharp
public class WaveManager
{
    public void StartWave(int waveNumber)
    {
        TooltipEventBus.PublishInfo($"Wave {waveNumber} starting!");
    }

    public void OnWaveComplete(int waveNumber, int coinsEarned)
    {
        TooltipEventBus.PublishSuccess($"Wave {waveNumber} cleared! +{coinsEarned} coins");
    }

    public void OnBossWave()
    {
        TooltipEventBus.Publish(new TooltipMessage(
            "BOSS WAVE INCOMING!",
            TooltipType.Warning,
            duration: 5f
        ));
    }
}
```

---

## Message Queue Behavior

When multiple messages are sent in quick succession:

1. First message displays immediately
2. Subsequent messages are queued
3. Each message displays for its full duration
4. Next message slides in after previous slides out
5. No messages are lost

Example:
```csharp
// Send 3 messages rapidly
TooltipEventBus.PublishError("Error 1");
TooltipEventBus.PublishWarning("Warning 2");
TooltipEventBus.PublishSuccess("Success 3");

// They will display in order: Error → Warning → Success
// Each with its full animation and duration
```

---

## API Reference

### TooltipEventBus (Static)

**Publishing:**
- `PublishError(string message)` - Quick error message
- `PublishWarning(string message)` - Quick warning message
- `PublishInfo(string message)` - Quick info message
- `PublishSuccess(string message)` - Quick success message
- `Publish(TooltipMessage message)` - Custom message with full control

**Subscribing (Advanced):**
- `Subscribe(Action<TooltipMessage> callback)` - Subscribe to all messages
- `Unsubscribe(Action<TooltipMessage> callback)` - Unsubscribe
- `ClearAllSubscribers()` - Clear all (use with caution)

### GlobalTooltipUI (Singleton)

**Direct Methods:**
- `ShowError(string message)` - Display error
- `ShowWarning(string message)` - Display warning
- `ShowInfo(string message)` - Display info
- `ShowSuccess(string message)` - Display success
- `ShowMessage(TooltipMessage message)` - Display custom message
- `ClearAll()` - Clear queue and hide immediately

### TooltipMessage (Struct)

```csharp
public struct TooltipMessage
{
    public string Message;
    public TooltipType Type;
    public float Duration;  // 0 = use default
    public Sprite Icon;     // Optional

    // Factory methods
    public static TooltipMessage Error(string message, float duration = 0f);
    public static TooltipMessage Warning(string message, float duration = 0f);
    public static TooltipMessage Info(string message, float duration = 0f);
    public static TooltipMessage Success(string message, float duration = 0f);
}
```

---

## Design Recommendations

### UI Layout Suggestions

**For Top Slide Down:**
- Anchor: Top-Center
- Pivot: (0.5, 1)
- Position: Y = -75 (adjust based on your design)
- Width: 600-800px
- Height: 80-120px

**For Bottom Slide Up:**
- Anchor: Bottom-Center
- Pivot: (0.5, 0)
- Position: Y = 75
- Width: 600-800px
- Height: 80-120px

### Color Palette

Default colors are optimized for visibility:
- **Error Red**: `(0.8, 0.2, 0.2, 0.95)` - Critical failures
- **Warning Orange**: `(0.9, 0.7, 0.2, 0.95)` - Cautions
- **Info Blue**: `(0.2, 0.5, 0.8, 0.95)` - General information
- **Success Green**: `(0.2, 0.7, 0.3, 0.95)` - Positive feedback

Feel free to adjust in the Inspector to match your game's theme!

---

## Testing

Use the Context Menu (right-click component in Inspector) to test:

- **Test Error Message** - Shows error example
- **Test Warning Message** - Shows warning example
- **Test Info Message** - Shows info example
- **Test Success Message** - Shows success example
- **Test Multiple Messages** - Tests message queue

---

## Tips

1. **Use Event Bus for most cases** - Keeps code decoupled
2. **Keep messages short** - 1-2 sentences max for readability
3. **Use appropriate types** - Error for failures, Success for achievements
4. **Don't spam** - Too many notifications can overwhelm players
5. **Test durations** - 3s is good default, 5s for important warnings
6. **Icons are optional** - Only use if they add clarity
7. **Consider sound** - Pair with audio cues for better feedback

---

## Requirements

- DOTween
- UniTask
- TextMesh Pro
- Unity UI
