using Google.Protobuf.Protocol;
using UnityEngine;

public class MyPlayerController : PlayerController
{
    private float tickInterval = 0.2f;
    private float tickTimer = 0f;
    private Vector3 lastMoveDir = Vector3.zero;

    protected override void Start()
    {
        base.Start();
        //controller = GetComponent<CharacterController>();

        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.Init(this.transform);
    }

    private void Update()
    {
        bool isIdle = moveDir.sqrMagnitude <= 0.001f;
        anim.SetBool("IsIdle", isIdle);
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        moveDir = new Vector3(h, 0, v);

        // 로컬 이동
        if (moveDir.sqrMagnitude > 0.001f)
        {
            // 위치 이동 (물리 기반)
            Vector3 newPos = rigid.position + moveDir.normalized * moveSpeed * Time.deltaTime;
            rigid.MovePosition(newPos);

            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            rigid.MoveRotation(Quaternion.Slerp(rigid.rotation, targetRotation, rotateSpeed * Time.deltaTime));

            //Charactercontroller 기반
            /*
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            // 방향 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            */
        }

        tickTimer += Time.deltaTime;

        // 입력이 있을 때만 주기적으로 패킷 전송
        if (tickTimer >= tickInterval && moveDir.sqrMagnitude > 0.001f)
        {
            SendMovePacket(CreatureState.Move);
            lastMoveDir = moveDir;
            tickTimer = 0f;
        }

        // 이동이 끝났다면 마지막 패킷 전송
        if (lastMoveDir.sqrMagnitude > 0 && moveDir.sqrMagnitude <= 0.001f)
        {
            SendMovePacket(CreatureState.Idle);
            lastMoveDir = Vector3.zero; // 마지막 패킷 보낸 후 초기화
            tickTimer = 0f;
        }
    }

    void SendMovePacket(CreatureState state)
    {
        C_Move packet = new C_Move()
        {
            PosX = transform.position.x,
            PosY = transform.position.y,
            PosZ = transform.position.z,
            RotY = transform.eulerAngles.y,
            State = state
        };
        NetworkManager.Instance.Send(packet);
    }
}
