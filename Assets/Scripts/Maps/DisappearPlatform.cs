using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class DisappearPlatform : MonoBehaviour
{
    public float visibleTime = 2f;
    public float invisibleTime = 2f;
    public float fadeDuration = 0.5f;

    private SpriteRenderer sr;
    private Collider2D col;
    private Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        originalColor = sr.color;

        StartCoroutine(FadeCycle());
    }

    IEnumerator FadeCycle()
    {
        while (true)
        {
            // Giai đoạn hiện platform
            sr.color = originalColor;
            sr.enabled = true;
            col.enabled = true;

            yield return new WaitForSeconds(visibleTime - fadeDuration);

            // Fade dần
            float t = 0f;
            while (t < fadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                t += Time.deltaTime;
                yield return null;
            }

            // Tắt hoàn toàn
            sr.enabled = false;
            col.enabled = false; // 🔥 Câu này là thứ khiến player rơi
            yield return new WaitForSeconds(invisibleTime);
        }
    }

}
