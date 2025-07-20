# 🔥 Fire Stream Skill - Unity Setup Guide

## 📋 Fire Stream Skill Overview

The **Fire Stream Skill** is fully self-contained and provides:
- ✅ Continuous damage beam in facing direction
- ✅ Knockback effect on hit targets
- ✅ Dash restriction while active (player can still move)
- ✅ Configurable damage, range, duration, and visual effects
- ✅ LayerMask targeting for specific player types
- ✅ Debug logging for troubleshooting

## 🎯 Unity Editor Setup Steps

### **Step 1: Create Fire Stream Skill Asset**
1. In Project window, right-click in your `Assets/` folder
2. Select **Create → TowerGame → Skill → FireStream**
3. Name it `FireStreamSkill` and place it in `Assets/Skills/` folder
4. Configure the following settings in Inspector:

#### **Fire Stream Settings:**
- **Stream Duration**: `3f` (how long the fire stream lasts)
- **Stream Range**: `4f` (3-4 player widths forward)
- **Stream Width**: `1.5f` (height of the damage area)
- **Damage Per Tick**: `5f` (damage dealt every tick)
- **Tick Interval**: `0.2f` (time between damage ticks)
- **Knockback Force**: `200f` (force applied to hit targets)

#### **Visual Effects:**
- **Fire Stream Prefab**: [Assign your fire visual prefab]
- **Affected Layers**: Set to target player layers (e.g., "Player1" and "Player2")
- **Debug Mode**: ✅ Enable for testing

### **Step 2: Create Fire Stream Visual Prefab**
1. Create an empty GameObject in scene, name it `FireStreamVisual`
2. Add visual components:
   - **SpriteRenderer** with fire texture/sprite
   - **OR Particle System** for fire effects
   - **OR Animator** for animated fire
3. Scale it to roughly `1x1x1` (will be scaled by skill)
4. Drag to Project to create prefab
5. Delete from scene
6. Assign this prefab to the Fire Stream Skill asset

### **Step 3: Set Up Player LayerMasks**
1. Go to **Edit → Project Settings → Tags and Layers**
2. Create layers:
   - Layer 8: `Player1`
   - Layer 9: `Player2`
3. Assign your player GameObjects to these layers
4. In Fire Stream Skill asset, set **Affected Layers** to include both player layers

### **Step 4: Configure Player GameObjects**
Each player needs these components:
- ✅ **PlayerController** (already have)
- ✅ **SkillExecutor** component
- ✅ **Collider2D** (for hit detection)
- ✅ **Rigidbody2D** (for knockback)

#### **Set up SkillExecutor:**
1. Select Player GameObject
2. Add **SkillExecutor** component
3. In Inspector, configure:
   - **Skills**: Add your FireStreamSkill asset
   - **Input Keys**: Set key for fire stream (e.g., KeyCode.X for Player1)

### **Step 5: Test Fire Stream**
1. Play the scene
2. Press the configured key to activate fire stream
3. Check Console for debug messages
4. Verify:
   - Fire visual appears in front of player
   - Other players take damage when in range
   - Knockback effect works
   - Dash is disabled during fire stream
   - Player can still move while firing

## 🔧 Advanced Configuration

### **Adjusting Fire Stream Range/Area**
- **Stream Range**: Distance forward from player
- **Stream Width**: Height/thickness of damage area
- The damage area is a box centered in front of the player

### **Visual Prefab Tips**
- Use **Particle System** for best fire effects
- Set particle **Start Lifetime** to match **Stream Duration**
- Use **Shape → Box** to match stream dimensions
- Add **Light** component for glow effect

### **LayerMask Configuration**
```csharp
// Example: Target only Player1 and Player2
// In Inspector, check only Player1 and Player2 layers
affectedLayers = (1 << 8) | (1 << 9); // Layers 8 and 9
```

## 🐛 Troubleshooting

### **Fire Stream Not Appearing**
- ✅ Check if Fire Stream Prefab is assigned
- ✅ Verify prefab has visual components (SpriteRenderer/ParticleSystem)
- ✅ Check if prefab scale is appropriate

### **No Damage Being Dealt**
- ✅ Verify LayerMask includes target players
- ✅ Check if players have Collider2D components
- ✅ Ensure players are on correct layers
- ✅ Enable Debug Mode and check Console logs

### **Knockback Not Working**
- ✅ Verify players have Rigidbody2D components
- ✅ Check if Rigidbody2D is not kinematic
- ✅ Adjust Knockback Force value

### **Dash Still Works During Fire Stream**
- ✅ Verify PlayerController has SetCanDash() method
- ✅ Check if CanDash field exists in PlayerController
- ✅ Ensure UseDashSkill() checks canDash variable

## 🎮 Input Integration Example

### **Option 1: Direct Key Binding (SkillExecutor)**
```csharp
// In SkillExecutor Inspector:
Skills[0] = FireStreamSkill asset
InputKeys[0] = KeyCode.X
```

### **Option 2: Custom Input (PlayerController)**
```csharp
// Add to PlayerController HandleInput():
if (Input.GetKeyDown(KeyCode.X)) 
{
    SkillExecutor executor = GetComponent<SkillExecutor>();
    executor.ActivateSkill(0); // Fire Stream is skill index 0
}
```

## 📊 Performance Notes
- Fire Stream uses coroutines for damage ticks
- Only one fire stream per player at a time
- Visual prefab is automatically destroyed when skill ends
- Minimal performance impact with proper LayerMask usage

## 🎯 Integration with Other Skills
- Fire Stream is fully compatible with all other skills
- Uses same SkillExecutor system as Dash, Teleport, Time-Slow Bubble
- Follows same cooldown management
- Can be easily extended with new effects

This system is now **production-ready** and fully modular!
