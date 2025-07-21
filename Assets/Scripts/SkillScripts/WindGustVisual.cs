using UnityEngine;

public class WindGustVisual : MonoBehaviour
{
    [Header("Wind Animation")]
    [SerializeField] private float animationSpeed = 2f;
    [SerializeField] private float fadeSpeed = 3f;
    [SerializeField] private Vector2 windMovement = new Vector2(1f, 0.2f);
    
    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    private float timeAlive = 0f;
    private Color startColor;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        
        if (spriteRenderer != null)
        {
            startColor = spriteRenderer.color;
        }
    }
    
    void Update()
    {
        timeAlive += Time.deltaTime;
        
        // Animate wind movement
        AnimateWindMovement();
        
        // Fade out over time
        AnimateFadeOut();
    }
    
    private void AnimateWindMovement()
    {
        // Create wind flowing effect
        float windOffset = Mathf.Sin(timeAlive * animationSpeed) * 0.1f;
        Vector3 movement = new Vector3(windMovement.x * timeAlive, windMovement.y * windOffset, 0f);
        transform.position = startPosition + movement;
    }
    
    private void AnimateFadeOut()
    {
        if (spriteRenderer != null)
        {
            // Fade alpha over time
            float alpha = Mathf.Lerp(startColor.a, 0f, timeAlive * fadeSpeed);
            Color newColor = startColor;
            newColor.a = alpha;
            spriteRenderer.color = newColor;
        }
    }
}
