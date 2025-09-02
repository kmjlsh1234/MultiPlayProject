using Google.Protobuf.Protocol;
using UniRx;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerId;

    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;
    public CharacterController controller;

    private Vector3 targetPos;
    private Quaternion targetRot;

    private Vector3 startPos;
    private Quaternion startRot;

    public float tickInterval = 0.2f; // 서버 패킷 주기
    private float elapsed = 0f;

    private void Update()
    {
        // 보간 이동
        //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
        if (elapsed < tickInterval)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / tickInterval);

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
        }
    }

    public virtual void OnMovePacket(S_Move packet)
    {
        //targetPos = new Vector3(packet.PosX, packet.PosY, packet.PosZ);
        //targetRot = Quaternion.Euler(0, packet.RotY, 0);
        startPos = transform.position;
        targetPos = new Vector3(packet.PosX, packet.PosY, packet.PosZ);

        startRot = transform.rotation;
        targetRot = Quaternion.Euler(0, packet.RotY, 0);

        elapsed = 0f; // 보간 시작
    }
}
