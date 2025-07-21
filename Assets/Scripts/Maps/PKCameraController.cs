using UnityEngine;

public class PKCameraController : MonoBehaviour
{
    [Header("Player Targets")]
    public Transform player1;
    public Transform player2;

    [Header("Camera Settings")]
    public float smoothSpeed = 5f; // tốc độ mượt khi di chuyển
    public float minSize = 6f;     // camera nhỏ nhất (zoom in)
    public float maxSize = 14f;    // camera lớn nhất (zoom out)
    public float zoomLimiter = 10f; // điều chỉnh độ nhạy zoom

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("PKCameraController: Không tìm thấy Camera trên GameObject.");
        }

        if (CharacterSelectionData.Instance != null)
        {
            if (player1 == null && CharacterSelectionData.Instance.player1Instance != null)
            {
                player1 = CharacterSelectionData.Instance.player1Instance.transform;
            }

            if (player2 == null && CharacterSelectionData.Instance.player2Instance != null)
            {
                player2 = CharacterSelectionData.Instance.player2Instance.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null)
            return;

        MoveCamera();
        ZoomCamera();
    }

    void MoveCamera()
    {

            Vector3 centerPoint = GetCenterPoint();
            Debug.Log("CenterPoint: " + centerPoint);

            Vector3 newPosition = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * smoothSpeed);


    }

    void ZoomCamera()
    {
        float distance = Vector2.Distance(player1.position, player2.position);
        float targetSize = Mathf.Clamp(minSize + (distance / zoomLimiter), minSize, maxSize);
        cam.orthographicSize = minSize;
    }

    Vector3 GetCenterPoint()
    {
        return (player1.position + player2.position) / 2f;
    }
}
