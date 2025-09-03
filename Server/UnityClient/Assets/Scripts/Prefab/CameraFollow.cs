using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;   // 따라갈 플레이어
    public Vector3 offset = new Vector3(0, 15, 0);
    public Vector3 rot = new Vector3(70f, 0f, 0f);
    public float smoothSpeed = 0.125f;
    
    public void Init(Transform player)
    {
        this.player = player;
        this.transform.rotation = Quaternion.Euler(rot);
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // 플레이어 위치 + 오프셋으로 이동
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

        }
    }
}
