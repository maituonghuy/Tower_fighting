using UnityEngine;

public class ClimbCameraController : MonoBehaviour
{
    public float moveAmount = 2f;
    public float interval = 3f;

    public Transform checkPointTop; 
    public float stopY = 85f;       

    private float timer;
    private bool hasReachedTop = false;

    void Update()
    {
        if (checkPointTop == null)
            return;

        if (!hasReachedTop && checkPointTop.position.y >= stopY)
        {
            hasReachedTop = true;
        }

        if (hasReachedTop)
            return;

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            transform.position += new Vector3(0f, moveAmount, 0f);
            timer = 0f;
        }
    }
}
