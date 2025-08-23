# 💨 Wind Gust Skill - Moving Projectile Implementation

## 🎯 **Wind Gust Overview**

The Wind Gust skill creates a moving projectile of wind that travels horizontally for 3 units in the player's facing direction. As it moves, it pushes enemies backward and slightly upward with strong knockback force. Perfect for:
- ✅ Knocking opponents off platforms
- ✅ Interrupting enemy attacks at range
- ✅ Creating space and repositioning enemies
- ✅ Area denial and crowd control

### **Key Features:**
- **Moving Projectile**: Wind gust travels as a projectile rather than being stationary
- **No Damage**: Pure knockback utility skill
- **Extended Range**: Travels 3 units from player position
- **Strong Knockback**: Powerful horizontal push + slight upward lift
- **Continuous Effect**: Applies knockback throughout its travel path
- **Visual Feedback**: Animated wind effect during movement

## 🎛️ **Wind Gust Settings**

### **Wind Gust Settings:**
- **Gust Duration**: `0.3f` - How long wind effect lasts at each point
- **Gust Range**: `1.5f` - Width of wind effect area
- **Gust Width**: `2f` - Height of wind effect area
- **Knockback Force**: `500f` - Strong horizontal push
- **Upward Force**: `200f` - Slight upward lift
- **Wind Lifetime**: `0.5f` - Visual effect duration

### **Movement Settings:**
- **Travel Distance**: `3f` - How far the wind projectile travels
- **Travel Speed**: `10f` - Speed of wind movement

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

### **Step 2: Create Moving Wind Visual Prefab**
1. Create empty GameObject named `WindGustProjectile`
2. Add visual components:

#### **Option A: Sprite-Based Wind**
```
WindGustProjectile
├── SpriteRenderer (wind texture/sprite)
├── WindGustVisual (animation script)
└── Transform (for movement)
```

#### **Option B: Particle-Based Wind**
```
WindGustProjectile
├── ParticleSystem (wind particles)
├── WindGustVisual (animation script)
└── Transform (for movement)
```

### **Step 3: Configure WindGustVisual Script**
Add the `WindGustVisual` component to your prefab:
- **Fade Duration**: `0.3f` - Fade-out time
- **Rotation Speed**: `720f` - Wind swirl animation
- **Scale Pulse Speed**: `2f` - Breathing effect speed
- **Scale Pulse Amount**: `0.1f` - Breathing intensity

### **Step 4: Assign to Player**
1. Select your player GameObject
2. Find the **SkillExecutor** component
3. Expand the skill slots array
4. Assign your `WindGustSkill` asset
5. Set the input key (e.g., `Q`)

### **Step 5: Layer Configuration**
1. Set **Affected Layers** to include enemy player layers
2. Ensure Physics2D layer collision is configured correctly
3. Test with different layer combinations

## 🎯 **Movement Behavior**

### **Travel Pattern:**
```
Player Position → Wind travels 3 units → End Position
    ↓               ↓                    ↓
 Wind Origin    Moving Effect      Effect Ends
```

### **Effect Application:**
- Wind gust applies knockback at each position during travel
- Continuous area coverage as it moves
- No damage, only knockback and positioning
- Strong horizontal push with slight upward lift

## 🧪 **Testing Guide**

### **Visual Testing:**
1. Enable **Debug Mode** and **Show Range In Scene**
2. Select `WindGustSkill` in Inspector
3. Observe range visualization in Scene view
4. Blue gizmos show wind area and direction

### **Gameplay Testing:**
1. Set up two players in the scene
2. Position enemy player within 3 units
3. Trigger Wind Gust (default: Q key)
4. Observe:
   - ✅ Wind visual appears and moves forward
   - ✅ Enemy gets knocked back horizontally
   - ✅ Enemy gets slight upward lift
   - ✅ No damage numbers appear
   - ✅ Skill goes on cooldown
   - ✅ Wind visual disappears after travel

### **Range Testing:**
- Test at various distances (0.5, 1, 2, 3+ units)
- Verify knockback strength consistency
- Check edge cases (corners, platforms)

## 🔧 **Troubleshooting**

### **❌ Wind Doesn't Move**
**Possible Causes:**
- Travel Speed = 0
- Travel Distance = 0
- Coroutine not starting

**Solutions:**
- Set Travel Speed > 0 (try 10f)
- Set Travel Distance > 0 (try 3f)
- Check Console for coroutine errors

### **❌ No Knockback Effect**
**Possible Causes:**
- Wrong LayerMask settings
- Missing Rigidbody2D on targets
- Insufficient knockback forces

**Solutions:**
- Verify Affected Layers includes target layers
- Add Rigidbody2D to enemy players
- Increase Knockback Force (try 500f+)

### **❌ Visual Issues**
**Possible Causes:**
- Missing Wind Gust Prefab
- Broken prefab references
- Missing WindGustVisual script

**Solutions:**
- Assign valid prefab to Wind Gust Prefab field
- Check prefab has SpriteRenderer/ParticleSystem
- Ensure WindGustVisual script is attached

### **❌ Range/Positioning Problems**
**Possible Causes:**
- Incorrect Wind Origin Offset
- Wrong facing direction detection
- Scale issues with visual

**Solutions:**
- Adjust Wind Origin Offset (try 0.2, 0)
- Test facing direction with Debug.Log
- Check visual prefab scale settings

## 🎨 **Advanced Customization**

### **Custom Knockback Patterns:**
Modify `ApplyWindKnockback` method to:
- Add distance-based force scaling
- Implement spin/rotation effects
- Create directional variations
- Add terrain interaction

### **Enhanced Visuals:**
- Add particle trail effects
- Implement camera shake on hit
- Create different wind styles per character
- Add sound effect integration

### **Gameplay Variations:**
- Adjustable travel speed per character
- Multiple wind bursts
- Charging mechanic for longer range
- Wall bouncing behavior

## 🎯 **Balance Considerations**

### **Strengths:**
- ✅ Strong utility for positioning
- ✅ Reliable crowd control
- ✅ Good range (3 units)
- ✅ Quick activation

### **Weaknesses:**
- ❌ No damage output
- ❌ Linear, predictable path
- ❌ Cooldown prevents spam
- ❌ Requires good positioning

### **Recommended Balance:**
- **Cooldown**: 3-5 seconds
- **Range**: 3 units (medium range)
- **Knockback**: Strong but not excessive
- **Upward Force**: Slight lift, not launch

## 🚀 **Integration with Other Skills**

### **Combo Potential:**
- **After Teleport**: Position then push enemies
- **Before Dash**: Create space then escape
- **With Fire Stream**: Push enemies into fire range
- **Area Denial**: Force enemies off objectives

### **Counter-Play:**
- Dash can avoid the moving wind
- Teleport can escape knockback
- Timing-based dodging
- Positioning behind cover

---

## 📝 **Implementation Checklist**

- [ ] ✅ Wind Gust skill asset created
- [ ] ✅ Moving wind visual prefab configured
- [ ] ✅ WindGustVisual script attached
- [ ] ✅ Skill assigned to player
- [ ] ✅ Layer masks configured
- [ ] ✅ Travel distance/speed set
- [ ] ✅ Knockback forces balanced
- [ ] ✅ Debug visualization working
- [ ] ✅ Input key assigned
- [ ] ✅ Testing completed

**Status: ✅ IMPLEMENTATION COMPLETE - Ready for gameplay testing!**
