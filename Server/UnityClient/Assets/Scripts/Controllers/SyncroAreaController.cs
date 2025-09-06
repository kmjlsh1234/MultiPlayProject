using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncroAreaController : MonoBehaviour
{
    public SphereCollider collider;

    public float tickInterval = 1f; // 1초마다 검사
    private List<GameObject> enemiesInArea = new List<GameObject>();

    public void Init()
    {
        if(NetworkManager.Instance.sessionId != this.GetComponentInParent<PlayerController>().playerId)
        {
            enabled = false;
        }
        collider = GetComponent<SphereCollider>();
        StartCoroutine(CheckEnemiesCoroutine());
    }

    private IEnumerator CheckEnemiesCoroutine()
    {
        while (true)
        {
            // 영역 안 Enemy 감지
            Collider[] hits = Physics.OverlapSphere(transform.position, collider.radius);
            enemiesInArea.Clear();

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    enemiesInArea.Add(hit.gameObject);
                }
            }
            if(enemiesInArea.Count > 0)
            {
                // 서버에 전송할 Enemy 리스트 준비
                SendEnemyListToServer(enemiesInArea);
            }
            

            yield return new WaitForSeconds(tickInterval);
        }
    }

    private void SendEnemyListToServer(List<GameObject> enemies)
    {
        C_Enemymove packet = new C_Enemymove();
        foreach (var enemy in enemies)
        {
            Objectinfo info = new Objectinfo()
            {
                Pos = new Positioninfo()
                {
                    PosX = enemy.transform.position.x,
                    PosY = enemy.transform.position.y,
                    PosZ = enemy.transform.position.z,
                },
                RotY = enemy.transform.eulerAngles.y,
                TargetId = enemy.GetComponent<Enemy>().targetPlayer.playerId,
            };
            packet.Enemies.Add(info);
        }

        // 서버로 전송
        NetworkManager.Instance.Send(packet);
        
    }
}
