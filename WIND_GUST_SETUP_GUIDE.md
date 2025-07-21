# 💨 Wind Gust Skill - Complete Implementation

## 🎯 **Wind Gust Overview**

The Wind Gust skill creates a horizontal burst of air that pushes enemies backward and slightly upward. Perfect for:
- ✅ Knocking opponents off platforms
- ✅ Interrupting enemy attacks
- ✅ Creating space in close combat
- ✅ Environmental interactions

### **Key Features:**
- **No Damage**: Pure knockback effect
- **Short Range**: Close-range tactical skill
- **Strong Knockback**: Powerful horizontal push + slight upward lift
- **Instant Effect**: Quick burst, can't be cancelled
- **Visual Feedback**: Wind effect shows the gust direction

## 🎛️ **Wind Gust Settings**

### **Wind Gust Settings:**
- **Gust Duration**: `0.3f` - How long the wind effect lasts
- **Gust Range**: `1.5f` - Short range (close combat)
- **Gust Width**: `2f` - Height of wind area
- **Knockback Force**: `500f` - Strong horizontal push
- **Upward Force**: `200f` - Slight upward lift
- **Wind Lifetime**: `0.5f` - Visual effect duration

### **Positioning:**
- **Wind Origin Offset**: `(0.2f, 0f)` - Wind starts slightly in front of player

### **Visual Effects:**
- **Wind Gust Prefab**: Assign your wind visual prefab
- **Affected Layers**: Target player layers
- **Debug Mode**: Enable for testing
- **Show Range In Scene**: Visual range indicator

## 🎮 **Unity Setup Instructions**

### **Step 1: Create Wind Gust Skill Asset**
1. Right-click in Project → **Create → TowerGame → Skill → WindGust**
2. Name it `WindGustSkill`
3. Configure settings in Inspector

### **Step 2: Create Wind Visual Prefab**
1. Create empty GameObject named `WindGustEffect`
2. Add visual components:

#### **Option A: Sprite-Based Wind**
```
WindGustEffect
├── SpriteRenderer (wind texture/sprite)
├── WindGustVisual script (for animation)
└── Optional: Animator for wind animation
```

#### **Option B: Particle System Wind**
```
WindGustEffect
├── Particle System
│   ├── Shape: Box (matches gust area)
│   ├── Velocity: Forward direction
│   ├── Size over Lifetime: Growing
│   └── Color over Lifetime: Fade out
└── WindGustVisual script (optional)
```

#### **Option C: Multiple Wind Lines**
```
WindGustEffect
├── WindLine1 (SpriteRenderer)
├── WindLine2 (SpriteRenderer)
├── WindLine3 (SpriteRenderer)
└── WindGustVisual script
```

3. Save as prefab
4. Assign to WindGustSkill asset

### **Step 3: Configure Players**
1. Add **SkillExecutor** component to players
2. In SkillExecutor:
   - **Skills**: Add WindGustSkill asset
   - **Input Keys**: Set key for wind gust (e.g., KeyCode.C)

### **Step 4: Set Up Layers**
1. Ensure players are on correct layers
2. Set **Affected Layers** in WindGustSkill to target enemy players

## 🎨 **Visual Prefab Ideas**

### **Simple Wind Lines:**
- 3-5 horizontal white/cyan lines
- Animate them moving forward and fading
- Use **WindGustVisual** script for movement

### **Particle Wind:**
- Use Unity Particle System
- **Shape**: Box matching gust area
- **Color**: Cyan → White → Transparent
- **Velocity**: Forward burst then settle

### **Animated Sprite:**
- Create wind texture with flowing lines
- Animate with sprite sheets or Animator
- Scale to match gust range

## 🔧 **Balancing Guidelines**

### **Close Combat Fighter:**
```csharp
gustRange = 1f;         // Very short
knockbackForce = 400f;  // Medium push
gustDuration = 0.2f;    // Quick burst
```

### **Tactical Controller:**
```csharp
gustRange = 2f;         // Medium range
knockbackForce = 600f;  // Strong push
gustDuration = 0.4f;    // Longer control
```

### **Platform Fighter:**
```csharp
gustRange = 1.5f;       // Standard range
knockbackForce = 500f;  // Platform-clearing force
upwardForce = 300f;     // Higher lift for platforms
```

## 🎯 **Advanced Features**

### **Environmental Interactions:**
- Push objects with Rigidbody2D
- Affect projectiles
- Activate wind-sensitive traps

### **Combo Potential:**
- Use after dash to create space
- Combine with teleport for positioning
- Counter fire stream attacks

### **Platform-Specific:**
- Higher upward force near platform edges
- Reduced effect on grounded players
- Enhanced effect on airborne targets

## 🐛 **Troubleshooting**

### **Wind Not Pushing:**
- ✅ Check if targets have Rigidbody2D
- ✅ Verify LayerMask includes target players
- ✅ Increase knockback force values

### **Visual Not Appearing:**
- ✅ Assign wind prefab to skill asset
- ✅ Check prefab has visual components
- ✅ Verify wind lifetime > 0

### **Range Issues:**
- ✅ Enable Debug Mode and Show Range In Scene
- ✅ Check gizmos in Scene view
- ✅ Adjust gust range and width values

## 🎮 **Input Integration**

### **Recommended Key Bindings:**
- **Player 1**: `KeyCode.C` (Wind Gust)
- **Player 2**: `KeyCode.Period` (Wind Gust)

### **SkillExecutor Setup:**
```csharp
// In SkillExecutor Inspector:
Skills[2] = WindGustSkill asset
InputKeys[2] = KeyCode.C
```

## 📊 **Comparison with Other Skills**

| Skill | Range | Damage | Knockback | Duration |
|-------|-------|--------|-----------|----------|
| **Wind Gust** | Short | None | High | Instant |
| **Fire Stream** | Medium | High | Medium | 3s |
| **Dash** | Self | None | None | 0.2s |
| **Teleport** | Long | None | None | Instant |

The Wind Gust skill is now **complete and ready** for Unity implementation! 💨⚡
