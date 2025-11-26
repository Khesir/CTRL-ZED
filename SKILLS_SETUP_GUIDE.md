# Detection & Bypass Skills - Setup Guide

This guide will walk you through setting up the Detection and Bypass skills in Unity.

---

## Part 1: Setup MinimapManager (Required for Detection Skill)

### Step 1: Add MinimapManager to Gameplay Scene

1. **Open the Gameplay scene**
   - In Unity, navigate to `Assets/Scenes/Gameplay/`
   - Double-click `Gameplay.unity` to open it

2. **Create MinimapManager GameObject**
   - In the Hierarchy window, right-click in empty space
   - Select `Create Empty`
   - Name it: `MinimapManager`

3. **Add the MinimapManager component**
   - With the MinimapManager GameObject selected
   - In the Inspector, click `Add Component`
   - Search for: `MinimapManager`
   - Select it to add

4. **Configure MinimapManager Settings**
   In the Inspector, you should see:

   ```
   Minimap Settings:
   ☐ Show Enemies By Default   <- MUST BE UNCHECKED (false)
   Minimap Icon Name: MInimap Icon
   Minimap Layer: 14
   ☑ Disable Game Object        <- MUST BE CHECKED (true)
   ```

5. **Save the scene**
   - File → Save Scene (or Ctrl+S)

---

## Part 2: Create Detection Skill

### Step 1: Create Detection Prefab

1. **Create empty GameObject in Hierarchy**
   - Right-click in Hierarchy → Create Empty
   - Name it: `Detection`

2. **Add Detection script component**
   - Select the Detection GameObject
   - In Inspector, click `Add Component`
   - Search for: `Detection`
   - Select it to add

3. **Configure Detection settings** (in Inspector)
   ```
   Detection Settings:
   Detection Duration: 5
   ```

4. **Add AudioSource (optional, for sound effects)**
   - Click `Add Component` → Audio → Audio Source
   - Check "Play On Awake" OFF
   - Assign an audio clip if you have one

5. **Create the prefab**
   - Drag the Detection GameObject from Hierarchy into:
     `Assets/Features/Skills/Implementations/Detection/`
   - You should see a blue cube icon appear in the folder
   - Delete the Detection GameObject from Hierarchy (it's now saved as prefab)

### Step 2: Create Detection SkillConfig (ScriptableObject)

1. **Create the config asset**
   - Navigate to `Assets/Features/Skills/Implementations/Detection/`
   - Right-click in the folder → Create → Configs → Skill
   - Name it: `DetectionSkill`

2. **Configure the DetectionSkill asset**
   - Select the DetectionSkill asset
   - In the Inspector, set:

   ```
   Skill Name: Detection
   Description: Reveals all enemies on the minimap for a few seconds
   Icon: [Drag an icon sprite here - optional]
   Cooldown: 10
   Skill Lifetime: 5
   Skill Prefab: [Drag Detection prefab here]
   Vfx Prefab: [Optional - leave empty for now]
   ```

3. **Save** (Ctrl+S)

---

## Part 3: Create Bypass Skill

### Step 1: Create Bypass Prefab

1. **Create empty GameObject in Hierarchy**
   - Right-click in Hierarchy → Create Empty
   - Name it: `Bypass`

2. **Add Bypass script component**
   - Select the Bypass GameObject
   - In Inspector, click `Add Component`
   - Search for: `Bypass`
   - Select it to add

3. **Configure Bypass settings** (in Inspector)
   ```
   Bypass Settings:
   Bypass Dash Multiplier: 3
   Collision Radius: 1.5
   ```

4. **Add AudioSource (optional, for sound effects)**
   - Click `Add Component` → Audio → Audio Source
   - Check "Play On Awake" OFF
   - Assign an audio clip if you have one

5. **Create the prefab**
   - Drag the Bypass GameObject from Hierarchy into:
     `Assets/Features/Skills/Implementations/Bypass/`
   - You should see a blue cube icon appear in the folder
   - Delete the Bypass GameObject from Hierarchy (it's now saved as prefab)

### Step 2: Create Bypass SkillConfig (ScriptableObject)

1. **Create the config asset**
   - Navigate to `Assets/Features/Skills/Implementations/Bypass/`
   - Right-click in the folder → Create → Configs → Skill
   - Name it: `BypassSkill`

2. **Configure the BypassSkill asset**
   - Select the BypassSkill asset
   - In the Inspector, set:

   ```
   Skill Name: Bypass
   Description: Dash through enemies, killing all in your path
   Icon: [Drag an icon sprite here - optional]
   Cooldown: 8
   Skill Lifetime: 0.3
   Skill Prefab: [Drag Bypass prefab here]
   Vfx Prefab: [Optional - leave empty for now]
   ```

3. **Save** (Ctrl+S)

---

## Part 4: Assign Skills to a Character

### Step 1: Find Your Character Config

1. **Navigate to character configs**
   - Go to `Assets/Features/Characters/Data/`
   - You should see several character config files (e.g., MainCharacter, TestCharacter, etc.)

2. **Select a character config to edit**
   - Click on the character config you want to give skills to

### Step 2: Assign the Skills

In the Inspector, scroll down to the **Skill** section:

```
Skill:
Skill 1: [Drag DetectionSkill here]
Skill 2: [Drag BypassSkill here]
```

Or you can:
- Click the circle/target icon next to Skill 1
- Search for "DetectionSkill"
- Select it
- Repeat for Skill 2 with BypassSkill

### Step 3: Save and Test

1. **Save** (Ctrl+S)
2. **Enter Play Mode** (press the Play button)
3. **Test the skills:**
   - **Q key** - Should activate Skill 1 (Detection)
   - **E key** - Should activate Skill 2 (Bypass)

---

## Part 5: Testing & Verification

### Test Detection Skill:

1. **Start the game**
2. **Look at the minimap** - Enemy red dots should be HIDDEN
3. **Press Q** (or E, depending on which slot)
4. **Check the minimap** - Enemy red dots should now be VISIBLE
5. **Wait 5 seconds** - Enemy red dots should HIDE again
6. **Check Unity Console** for logs:
   ```
   [MinimapManager] Initialized - Enemies hidden by default: True
   [Detection] ===== DETECTION SKILL ACTIVATED =====
   [MinimapManager] SHOWING X enemies on minimap
   ```

### Test Bypass Skill:

1. **Start the game**
2. **Move near enemies** (use WASD)
3. **Press E** (or Q, depending on which slot)
4. **Player should dash quickly** through enemies
5. **Enemies touched during dash should DIE instantly**
6. **Check Unity Console** for logs:
   ```
   [Bypass] Dash initiated!
   [Bypass] Eliminated enemy: Enemy(Clone)
   [Bypass] Dash ended. Enemies eliminated: X
   ```

---

## Troubleshooting

### Detection Skill Not Working:

**Problem: Enemies still visible on minimap**
- Check MinimapManager exists in scene (Hierarchy)
- Check "Show Enemies By Default" is UNCHECKED
- Check "Disable Game Object" is CHECKED
- Check Console for warnings about "MInimap Icon" not found
- Verify enemy prefabs have a child named exactly "MInimap Icon" (case-sensitive)

**Problem: No logs in Console**
- MinimapManager might not be in scene
- Check DetectionSkill is properly assigned to character
- Make sure you're pressing the correct key (Q or E)

### Bypass Skill Not Working:

**Problem: Dash doesn't kill enemies**
- Check Collision Radius is set (default 1.5)
- Check enemies have EnemyService component
- Look for logs like "[Bypass] Eliminated enemy"
- Enemies might be too far away - try increasing Collision Radius to 2.0 or 3.0

**Problem: Dash doesn't move player**
- Check Bypass Dash Multiplier is set (default 3)
- PlayerDash component must exist on player
- Try increasing multiplier to 5 or higher

### General Skill Issues:

**Problem: Skills don't activate at all**
- Character might not have skills equipped
- Check Character Config → Skill section is filled
- Verify SkillConfig has Skill Prefab assigned
- Make sure prefabs have the correct script components

**Problem: "Skill on cooldown" message**
- This is normal - wait for cooldown timer
- Check SkillConfig cooldown value (Detection: 10s, Bypass: 8s)

---

## Quick Reference

### File Locations:
```
MinimapManager script:     Assets/Features/Enemies/MinimapManager.cs
Detection script:          Assets/Features/Skills/Implementations/Detection/Detection.cs
Bypass script:             Assets/Features/Skills/Implementations/Bypass/Bypass.cs

Enemy prefabs:             Assets/Features/Enemies/Prefab/
Character configs:         Assets/Features/Characters/Data/
```

### Default Skill Settings:
```
Detection:
- Cooldown: 10 seconds
- Duration: 5 seconds
- Key: Q or E

Bypass:
- Cooldown: 8 seconds
- Duration: 0.3 seconds
- Dash Speed: 3x multiplier
- Collision Radius: 1.5 units
- Key: Q or E
```

---

## Next Steps (Optional)

### Add Visual Effects:
1. Create or import VFX prefabs (particles, animations)
2. Assign them to SkillConfig → Vfx Prefab field
3. VFX will automatically spawn when skill activates

### Add Sound Effects:
1. Import audio clips
2. Edit Detection/Bypass prefabs
3. Assign audio clip to AudioSource component
4. Sound will play when skill activates

### Customize Values:
- Adjust cooldowns in SkillConfig assets
- Modify dash speed in Bypass prefab settings
- Change detection duration in Detection prefab settings
- Increase collision radius for larger hitbox

---

**Setup Complete!** Both skills should now be working in your game.
