using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GameManager : SingletonBase<GameManager>
{
    public bool isMaster { get; set; }

    private GameObject enemy;
    public Dictionary<int, Objectinfo> players = new Dictionary<int, Objectinfo>();
    public Dictionary<int, PlayerController> playerControllers = new Dictionary<int, PlayerController>();
    public Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();

    public List<SyncroAreaController> controllers = new List<SyncroAreaController>();
    public float syncroTickInterval = 1f;
    public override void Init()
    {
        base.Init();
        enemy = ResourcesManager.Instance.getPrefabObj("Enemy");
    }

    public void GameStart(S_Gameroominfo packet)
    {
        isMaster = packet.RoomInfo.MasterId.Equals(NetworkManager.Instance.sessionId);
        GeneratePlayer(packet);

        if (isMaster)
        {
            StartCoroutine(SyncroStart());
        }
    }

    #region Player
    public void GeneratePlayer(S_Gameroominfo packet)
    {
        foreach (Objectinfo info in packet.RoomInfo.Players)
        {
            players.Add(info.ObjectId, info);
        }

        foreach (Objectinfo playerInfo in players.Values)
        {
            GameObject go = ResourcesManager.Instance.getPrefabObj("Player");
            if (go != null)
            {
                GameObject player = Instantiate(go, Vector3.zero, Quaternion.identity);
                
                if (playerInfo.ObjectId == NetworkManager.Instance.sessionId)
                {
                    PlayerController p = player.AddComponent<MyPlayerController>();
                    p.gameObject.tag = "Player";
                    p.gameObject.name = $"MyPlayer";
                    p.playerId = playerInfo.ObjectId;
                    playerControllers.Add(p.playerId, p);

                }
                else
                {
                    PlayerController p = player.AddComponent<PlayerController>();
                    p.playerId = playerInfo.ObjectId;
                    p.gameObject.name = $"Player_{playerInfo.ObjectId}";
                    playerControllers.Add(p.playerId, p);
                }


            }
            else
            {
                Debug.LogError("Player Prefab is Null");
            }
        }

    }

    public void RemovePlayer(int objectId)
    {
        if (playerControllers.TryGetValue(objectId, out PlayerController controller))
        {
            players.Remove(objectId);
            Destroy(controller.gameObject);
        }
    }

    public void PlayerMove(S_Move packet)
    {
        if(playerControllers.TryGetValue(packet.SessionId, out PlayerController player))
        {
            if (!NetworkManager.Instance.sessionId.Equals(packet.SessionId))
            {
                player.OnMovePacket(packet);
            }
        }
    }
    #endregion

    public void SpawnEnemy(S_Spawnenemy packet)
    {
        GameObject go = Instantiate(enemy);
        Enemy target= go.GetComponent<Enemy>();
        target.objectId = packet.ObjectInfo.ObjectId;

        target.transform.position = new Vector3(packet.ObjectInfo.Pos.PosX, packet.ObjectInfo.Pos.PosY, packet.ObjectInfo.Pos.PosZ);
        

        if(GameManager.Instance.playerControllers.TryGetValue(packet.ObjectInfo.TargetId, out PlayerController controller))
        {
            target.Init(controller);
            enemies.Add(packet.ObjectInfo.ObjectId, target);
        }
        
    }

    public void LerpEnemyPos(S_Enemymove packet)
    {
        foreach(Objectinfo info in packet.Enemies)
        {
            if(enemies.TryGetValue(info.ObjectId, out Enemy enemy))
            {
                Vector3 newPos = new Vector3(info.Pos.PosX, info.Pos.PosY, info.Pos.PosZ);
                float distance = Vector3.Distance(enemy.transform.position, newPos);

                if (distance > 0.5f)
                {
                    // 부드럽게 보간
                    enemy.transform.position = newPos;
                }

                if (GameManager.Instance.playerControllers.TryGetValue(info.TargetId, out PlayerController controller))
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
