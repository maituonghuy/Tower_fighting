using UnityEngine;

public class ClimbCameraController : MonoBehaviour
{
    public float moveAmount = 2f;
    public float interval = 3f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            transform.position += new Vector3(0f, moveAmount, 0f);
            timer = 0f;
        }
    }
}
