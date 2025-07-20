using UnityEngine;

public class BubbleVisual : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minScale = 0.9f;
    [SerializeField] private float maxScale = 1.1f;
    [SerializeField] private Color bubbleColor = new Color(0.5f, 0.8f, 1f, 0.3f);
    
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        
        // Set bubble color
        if (spriteRenderer != null)
        {
            spriteRenderer.color = bubbleColor;
        }
    }

    void Update()
    {
        // Pulsing animation
        float pulseValue = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        transform.localScale = originalScale * pulseValue;
    }

    public void SetRadius(float radius)
    {
        transform.localScale = Vector3.one * (radius * 2f);
        originalScale = transform.localScale;
    }

    public void SetColor(Color color)
    {
        bubbleColor = color;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }
}
