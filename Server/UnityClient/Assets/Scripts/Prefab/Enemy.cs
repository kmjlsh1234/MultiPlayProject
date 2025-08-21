using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;      // �̵� �ӵ�
    public float searchInterval = 0.5f; // �� �ʸ��� Ÿ�� ��������
    private float lastSearchTime = 0f;
    Dictionary<int, Player> playerDic;

    private Player targetPlayer; // ���� ���󰡴� ���
    private Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerDic = PlayerManager.Instance.playerList;

        //���� ����� �� ã�� �̵�
        FindClosestPlayer();
    }
    private void FixedUpdate()
    {
        // �ֱ������� ���� ����� �÷��̾� ã��
        if (Time.time - lastSearchTime > searchInterval)
        {
            FindClosestPlayer();
            lastSearchTime = Time.time;
        }

        // Ÿ���� ������ �������� �̵�
        if (targetPlayer != null)
        {
            Vector3 dir = (targetPlayer.transform.position - transform.position).normalized;

            // Rigidbody �̵� (�ӵ� ���� ���)
            rb.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);

            // ȸ�� (�ε巴��)
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
