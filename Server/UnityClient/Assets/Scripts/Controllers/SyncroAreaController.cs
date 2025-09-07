using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncroAreaController : MonoBehaviour
{
    public SphereCollider collider;

    public void Init()
    {
        if(!NetworkManager.Instance.sessionId.Equals(RoomManager.Instance.masterId))
        {
            enabled = false;
        }
        collider = GetComponent<SphereCollider>();
        GameManager.Instance.controllers.Add(this);
    }

    public Dictionary<int, Enemy> CheckEnemyInArea()
    {
        Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();
        // 영역 안 Enemy 감지
        Collider[] hits = Physics.OverlapSphere(transform.position, collider.radius);
        
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.gameObject.GetComponent<Enemy>();
                enemies.Add(enemy.objectId, enemy);
            }
        }
        return enemies;
    }

    
}
