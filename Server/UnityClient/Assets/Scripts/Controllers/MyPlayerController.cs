using Google.Protobuf.Protocol;
using UnityEngine;

public class MyPlayerController : PlayerController
{
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(h, 0, v);

        if (moveDir.sqrMagnitude > 0.001f)
        {
            // 이동 (CharacterController.Move)
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            // 이동 방향 바라보기 (Y축 회전만)
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    void SendMovePacket(Vector3 moveDir)
    {
        C_Input packet = new C_Input
        {
            
        };

        NetworkManager.Instance.Send(packet); // 네트워크 전송
    }
}
