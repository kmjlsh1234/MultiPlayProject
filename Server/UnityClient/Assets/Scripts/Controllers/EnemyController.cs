using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int objectId;
    public float moveSpeed = 2f;      // 이동 속도
    public float searchInterval = 0.5f; // 몇 초마다 타겟 갱신할지

    public PlayerController targetPlayer; // 현재 따라가는 대상
    private Rigidbody rigid;

    public EnemyMoveMode moveMode = EnemyMoveMode.Local; //Local, Sync

    public void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Spawn시 서버에서 가까운 플레이어를 계산에서 패킷으로 보내면 Init에서 받음
    /// </summary>
    /// <param name="controller"></param>
    public void Init(PlayerController controller)
    {
        targetPlayer = controller;

        if (GameManager.Instance.isMaster)
        {
            StartCoroutine(FindClosestPlayer());
        }
    }

    private void FixedUpdate()
    {
        if(targetPlayer == null) { return; }

        if(moveMode == EnemyMoveMode.Local)
        {
            //TODO : 로컬 기반 이동(RigidBody이용) & targetPlayer를 바라보게 Y축 회전
        }
        else if(moveMode == EnemyMoveMode.Sync)
        {
            //TODO : 서버에서 받은 좌표로 이동(RigidBody이용) & targetPlayer를 바라보게 Y축 회전
        }
    }

    IEnumerator FindClosestPlayer()
    {
        yield return new WaitForSeconds(searchInterval);
        Dictionary<int, PlayerController> dic = GameManager.Instance.playerControllers;

        float minDist = float.MaxValue;
        PlayerController closest = null;

        foreach (var kv in dic)
        {
            PlayerController p = kv.Value;
            if (p == null) continue;

            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = p;
            }
        }

        targetPlayer = closest;
        StartCoroutine(FindClosestPlayer());
    }
}
