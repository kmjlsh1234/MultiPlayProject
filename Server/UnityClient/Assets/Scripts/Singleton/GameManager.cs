using Google.Protobuf.Protocol;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{

    private GameObject enemy;
    public Dictionary<int, PlayerController> players = new Dictionary<int, PlayerController>();
    public Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();

    public List<SyncroAreaController> controllers = new List<SyncroAreaController>();
    public float syncroTickInterval = 1f;
    public override void Init()
    {
        base.Init();
        LoadingSceneManager.Instance.OnLoadingCompleted += GameStart;
        enemy = ResourcesManager.Instance.getPrefabObj("Enemy");
    }

    public void GameStart()
    {
        PlayerManager.Instance.GeneratePlayer();
        StartCoroutine(SyncroStart());
    }

    public void SpawnEnemy(S_Spawnenemy packet)
    {
        GameObject go = Instantiate(enemy);
        Enemy target= go.GetComponent<Enemy>();
        target.objectId = packet.ObjectInfo.ObjectId;
        target.transform.position = new Vector3(packet.ObjectInfo.Pos.PosX, packet.ObjectInfo.Pos.PosY, packet.ObjectInfo.Pos.PosZ);
        PlayerController controller = null;
        PlayerManager.Instance.playerList.TryGetValue(packet.ObjectInfo.TargetId, out controller);
        
        target.Init(controller);
        enemies.Add(packet.ObjectInfo.ObjectId, target);
    }

    public void LerpEnemyPos(S_Enemymove packet)
    {
        foreach(Objectinfo info in packet.Enemies)
        {
            Enemy enemy = null;
            enemies.TryGetValue(info.ObjectId, out enemy);
            if (enemy != null)
            {
                enemy.transform.position = new Vector3(info.Pos.PosX, info.Pos.PosY, info.Pos.PosZ);
                PlayerController controller = null;
                PlayerManager.Instance.playerList.TryGetValue(info.TargetId, out controller);
                if(controller != null)
                {
                    enemy.targetPlayer = controller;
                }
            }
        }
    }

    IEnumerator SyncroStart()
    {
        yield return new WaitForSeconds(syncroTickInterval);
        Dictionary<int, Enemy> enemiesInArea = new Dictionary<int, Enemy>();
        foreach(SyncroAreaController area in controllers)
        {
            Dictionary<int, Enemy> dic = area.CheckEnemyInArea();

            foreach(Enemy enemy in dic.Values)
            {
                if (!enemiesInArea.ContainsKey(enemy.objectId))
                {
                    enemiesInArea.Add(enemy.objectId, enemy);
                }
            }
        }
        
        if (enemiesInArea.Count > 0)
        {
            SendEnemyListToServer(enemiesInArea);
        }
        StartCoroutine(SyncroStart());
    }

    private void SendEnemyListToServer(Dictionary<int, Enemy> enemiesInArea)
    {
        C_Enemymove packet = new C_Enemymove();
        foreach (Enemy enemy in enemiesInArea.Values)
        {
            Objectinfo info = new Objectinfo()
            {
                ObjectId = enemy.objectId,
                Pos = new Positioninfo()
                {
                    PosX = enemy.transform.position.x,
                    PosY = enemy.transform.position.y,
                    PosZ = enemy.transform.position.z,
                },
                RotY = enemy.transform.eulerAngles.y,
                TargetId = enemy.targetPlayer.playerId,
            };
            packet.Enemies.Add(info);
        }

        // 서버로 전송
        Debug.Log($"Enemies In Area Count : {packet.Enemies.Count} / Packet Size : {packet.CalculateSize()}");
        NetworkManager.Instance.Send(packet);

    }
}
