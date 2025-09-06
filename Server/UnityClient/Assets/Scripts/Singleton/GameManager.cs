using Google.Protobuf.Protocol;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{

    private GameObject enemy;
    public Dictionary<int, PlayerController> players = new Dictionary<int, PlayerController>();
    public Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();
    
    public override void Init()
    {
        base.Init();
        LoadingSceneManager.Instance.OnLoadingCompleted += GameStart;

        enemy = ResourcesManager.Instance.getPrefabObj("Enemy");
    }

    void GameStart()
    {

    }

    public void SpawnEnemy(S_Spawnenemy packet)
    {
        GameObject go = Instantiate(enemy);
        Enemy target= go.GetComponent<Enemy>();
        target.transform.position = new Vector3(packet.PosX, packet.PosY, packet.PosZ);
        enemies.Add(packet.ObjectId, target);
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
                Debug.Log($"Enemy {info.ObjectId} : [{info.Pos.PosX}, {info.Pos.PosY}, {info.Pos.PosZ}]");
            }
        }
        
    }
}
