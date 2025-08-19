using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;   // ���� �÷��̾�
    public Vector3 offset = new Vector3(0, 25, 0);
    public float smoothSpeed = 0.125f;
    
    public void Init(Transform player)
    {
        this.player = player;
        this.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // �÷��̾� ��ġ + ���������� �̵�
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

        }
    }
}
