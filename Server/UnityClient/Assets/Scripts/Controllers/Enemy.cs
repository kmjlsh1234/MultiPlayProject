using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int objectId;
    public float moveSpeed = 2f;      // 이동 속도
    public float searchInterval = 0.5f; // 몇 초마다 타겟 갱신할지

    public PlayerController targetPlayer; // 현재 따라가는 대상
    private Rigidbody rb;

    // 패킷 보간용
    private Coroutine lerpCoroutine;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //가장 가까운 적 찾아 이동
    }

    public void Init(PlayerController target)
    {
        this.targetPlayer = target;
        if (GameManager.Instance.isMaster)
        {
            StartCoroutine(FindClosestPlayer());
        }
    }

    private void FixedUpdate()
    {
        // 타겟이 있으면 그쪽으로 이동
        if (targetPlayer != null)
        {
            Vector3 dir = (targetPlayer.transform.position - transform.position).normalized;

            // Rigidbody 이동 (속도 설정 방식)
            rb.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);

            // 회전 (부드럽게)
            if (dir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * 5f));
            }
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
