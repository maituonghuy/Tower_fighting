using UnityEngine;

public class FireStreamVisual : MonoBehaviour
{
    [Header("Fire Animation Settings")]
    [SerializeField] private float flickerSpeed = 10f;
    [SerializeField] private float intensityVariation = 0.3f;
    [SerializeField] private Color fireColorStart = new Color(1f, 0.5f, 0f, 0.8f); // Orange
    [SerializeField] private Color fireColorEnd = new Color(1f, 0f, 0f, 0.6f); // Red
    [SerializeField] private bool enableParticles = true;
    
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particles;
    private Vector3 originalScale;
    private float baseIntensity = 1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particles = GetComponent<ParticleSystem>();
        originalScale = transform.localScale;
        
        // Set initial fire color
        if (spriteRenderer != null)
        {
            spriteRenderer.color = fireColorStart;
        }

        // Configure particle system if present
        ConfigureParticles();
    }

    void Update()
    {
        // Animate fire flickering
        AnimateFlicker();
        
        // Animate color transitions
        AnimateColor();
    }

    private void AnimateFlicker()
    {
        // Create flickering effect by varying scale
        float flicker = 1f + Mathf.Sin(Time.time * flickerSpeed) * intensityVariation;
        transform.localScale = new Vector3(
            originalScale.x, 
            originalScale.y * flicker, 
            originalScale.z
        );
    }

    private void AnimateColor()
    {
        if (spriteRenderer == null) return;

        // Animate between fire colors
        float t = (Mathf.Sin(Time.time * flickerSpeed * 0.5f) + 1f) / 2f;
        spriteRenderer.color = Color.Lerp(fireColorStart, fireColorEnd, t);
    }

    private void ConfigureParticles()
    {
        if (particles == null || !enableParticles) return;

        var main = particles.main;
        main.startColor = fireColorStart;
        main.startLifetime = 0.5f;
        main.startSpeed = 3f;
        main.maxParticles = 100;

        var emission = particles.emission;
        emission.rateOverTime = 50f;

        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(originalScale.x, originalScale.y * 0.5f, 0.1f);

        var velocityOverLifetime = particles.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(2f, 4f);

        var colorOverLifetime = particles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(fireColorStart, 0.0f), new GradientColorKey(fireColorEnd, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        colorOverLifetime.color = gradient;
    }

    public void SetFireIntensity(float intensity)
    {
        baseIntensity = intensity;
        if (particles != null)
        {
            var emission = particles.emission;
            emission.rateOverTime = 50f * intensity;
        }
    }

    public void SetFireDirection(float direction)
    {
        // Update particle direction based on fire stream direction
        if (particles != null)
        {
            var velocityOverLifetime = particles.velocityOverLifetime;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(direction * 2f, direction * 4f);
        }
    }
}
