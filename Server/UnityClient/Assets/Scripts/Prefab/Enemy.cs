using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;      // 이동 속도
    public float searchInterval = 0.5f; // 몇 초마다 타겟 갱신할지
    private float lastSearchTime = 0f;
    Dictionary<int, Player> playerDic;

    private Player targetPlayer; // 현재 따라가는 대상
    private Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerDic = PlayerManager.Instance.playerList;

        //가장 가까운 적 찾아 이동
        FindClosestPlayer();
    }
    private void FixedUpdate()
    {
        // 주기적으로 가장 가까운 플레이어 찾기
        if (Time.time - lastSearchTime > searchInterval)
        {
            FindClosestPlayer();
            lastSearchTime = Time.time;
        }

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

    private void FindClosestPlayer()
    {
        Dictionary<int, Player> dic = PlayerManager.Instance.playerList;

        float minDist = float.MaxValue;
        Player closest = null;

        foreach (var kv in dic)
        {
            Player p = kv.Value;
            if (p == null) continue;

            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = p;
            }
        }

        targetPlayer = closest;
    }
}
