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

        // Kiểm tra xem camera có gắn tag MainCamera chưa (nếu dùng Camera.main ở nơi khác)
        if (cam == null)
        {
            Debug.LogError("AnchorCameraController: Không tìm thấy Camera trên GameObject.");
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
        Vector3 newPosition = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * smoothSpeed);
    }

    void ZoomCamera()
    {
        float distance = Vector2.Distance(player1.position, player2.position);
        float newSize = Mathf.Clamp(distance / zoomLimiter, minSize, maxSize);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newSize, Time.deltaTime * smoothSpeed);
    }

    Vector3 GetCenterPoint()
    {
        return (player1.position + player2.position) / 2f;
    }
}
