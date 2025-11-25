# FillBarAnimator - DOTween Fill Bar System

A generalized, reusable fill bar component using DOTween for smooth animations. Perfect for HP bars, experience bars, level progression, resource gauges, and any other fill-based UI.

## Features

✅ Smooth DOTween animations with configurable easing
✅ Optional color gradient transitions
✅ Optional punch scale effect on change
✅ Support for value overflow (exp bar leveling)
✅ Flash effects for damage/heal feedback
✅ Works with scaled and unscaled time
✅ Fully configurable in Inspector

---

## Setup

### 1. Create Fill Bar UI

1. Create a UI Image in your Canvas
2. Set Image Type to **Filled**
3. Set Fill Method to **Horizontal** (or your preference)
4. Add the `FillBarAnimator` component to the Image

### 2. Configure in Inspector

**References:**
- `Fill Image` - Auto-assigned, but can override

**Animation Settings:**
- `Animation Duration` - How long the fill animation takes (default: 0.3s)
- `Ease Type` - DOTween ease curve (OutQuad, Linear, etc.)
- `Use Unscaled Time` - If true, animations ignore Time.timeScale (useful for pause menus)

**Optional Color Transitions:**
- `Use Color Transition` - Enable color changes based on fill amount
- `Color Gradient` - Define colors at different fill percentages (0% = red, 100% = green)

**Optional Effects:**
- `Punch Scale On Change` - Bar "pops" slightly when value changes
- `Punch Scale` - Scale multiplier (default: 1.1)
- `Punch Duration` - How long the punch lasts

---

## Usage Examples

### Health Bar

```csharp
public class HealthBarController : MonoBehaviour
{
    [SerializeField] private FillBarAnimator healthBar;

    private void Start()
    {
        // Initialize with max HP and current HP
        healthBar.Initialize(maxHealth: 100f, startingValue: 100f);
    }

    public void OnHealthChanged(float newHealth)
    {
        healthBar.UpdateValue(newHealth);
    }

    public void OnTakeDamage(float newHealth)
    {
        healthBar.UpdateValue(newHealth);
        healthBar.Flash(Color.red); // Flash red on damage
    }
}
```

### Experience Bar

```csharp
public class ExperienceBarController : MonoBehaviour
{
    [SerializeField] private FillBarAnimator expBar;

    private void Start()
    {
        expBar.Initialize(maxValue: expToNextLevel, startingValue: 0f);
    }

    public async void OnExperienceGained(float currentExp, float maxExp)
    {
        if (currentExp >= maxExp)
        {
            // Handle level up with overflow animation
            float overflow = currentExp - maxExp;
            await expBar.AnimateOverflow(overflow);

            // Update for next level
            expBar.UpdateMaxValue(newExpRequirement);
        }
        else
        {
            expBar.UpdateValue(currentExp);
        }
    }
}
```

### Resource Bar (Drives, Energy, etc.)

```csharp
public class ResourceBarController : MonoBehaviour
{
    [SerializeField] private FillBarAnimator resourceBar;

    private void Start()
    {
        resourceBar.Initialize(maxValue: 100f, startingValue: 50f);
    }

    public void OnResourceChanged(float newAmount)
    {
        resourceBar.UpdateValue(newAmount);
    }
}
```

### Progress Bar (0-1 percentage)

```csharp
public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private FillBarAnimator progressBar;

    public void UpdateProgress(float percentage)
    {
        // Directly set fill amount (0-1 range)
        progressBar.SetFillAmount(percentage);
    }
}
```

---

## Integration with Existing Systems

### With PlayerService (HP)

```csharp
// In your HealthService or PlayerUI
public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private FillBarAnimator healthBar;
    private IHealthService healthService;

    private void Start()
    {
        healthService = GameManager.Instance.Player.Health;

        // Initialize
        healthBar.Initialize(healthService.MaxHealth, healthService.Health);

        // Subscribe to events
        healthService.OnHealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(float newHealth)
    {
        healthBar.UpdateValue(newHealth);
    }

    private void OnDestroy()
    {
        if (healthService != null)
            healthService.OnHealthChanged -= OnHealthChanged;
    }
}
```

### With ExpService

```csharp
public class PlayerExpUI : MonoBehaviour
{
    [SerializeField] private FillBarAnimator expBar;
    private IExpService expService;

    private void Start()
    {
        expService = GameManager.Instance.Player.Exp;

        expBar.Initialize(expService.ExpToNextLevel, expService.CurrentExp);

        expService.OnExpChanged += OnExpChanged;
        expService.OnLevelUp += OnLevelUp;
    }

    private void OnExpChanged(float newExp)
    {
        expBar.UpdateValue(newExp);
    }

    private async void OnLevelUp(int newLevel)
    {
        // Animate overflow if needed
        float overflow = expService.CurrentExp;
        await expBar.AnimateOverflow(overflow);

        // Update max for new level
        expBar.UpdateMaxValue(expService.ExpToNextLevel);
    }
}
```

---

## API Reference

### Initialization
- `Initialize(float maxValue, float startingValue = 0f)` - Set up the bar with max and starting values

### Update Methods
- `UpdateValue(float newValue, bool instant = false)` - Update current value
- `UpdateValues(float newValue, float newMaxValue, bool instant = false)` - Update both current and max
- `UpdateMaxValue(float newMaxValue, bool instant = false)` - Update max only
- `SetFillAmount(float fillAmount, bool instant = false)` - Set fill directly (0-1)

### Special Effects
- `AnimateOverflow(float overfillValue)` - Smooth overflow animation (async)
- `Flash(Color flashColor, float flashDuration = 0.2f)` - Flash the bar

### Getters
- `GetFillAmount()` - Returns current fill (0-1)
- `GetCurrentValue()` - Returns current value
- `GetMaxValue()` - Returns max value

---

## Tips

1. **Color Gradients**: Use Unity's Gradient editor to create health bars that change from green → yellow → red
2. **Instant Updates**: Pass `instant: true` to skip animation (useful for initialization)
3. **Unscaled Time**: Enable for UI that should animate during pause screens
4. **Punch Effect**: Great for feedback when resource changes occur
5. **Flash Effect**: Perfect for damage indication or critical states

---

## Requirements

- DOTween (Free or Pro)
- UniTask (`com.cysharp.unitask`)
- Unity UI (built-in)
