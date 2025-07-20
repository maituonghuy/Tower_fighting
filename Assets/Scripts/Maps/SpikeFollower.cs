using UnityEngine;

public class SpikeFollower : MonoBehaviour
{
    public Transform cameraAnchor;
    public Vector3 offset;

    void Update()
    {
        if (cameraAnchor != null)
            transform.position = cameraAnchor.position + offset;
    }
}
