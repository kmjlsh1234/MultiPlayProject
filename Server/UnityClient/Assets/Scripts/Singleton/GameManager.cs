using Google.Protobuf.Protocol;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{

    private GameObject enemy;
    public List<Enemy> enemyDic = new List<Enemy>();
    
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
        enemyDic.Add(target);
    }
}
