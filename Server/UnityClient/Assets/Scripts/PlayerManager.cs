using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonBase<PlayerManager>
{
    public Dictionary<int, PlayerData> playerDataList = new Dictionary<int, PlayerData>();
    public Dictionary<int, Player> playerList = new Dictionary<int, Player>();

    public void GeneratePlayer()
    {
        playerDataList = ChatManager.Instance.playerDic;
        foreach (KeyValuePair<int, PlayerData> pair in playerDataList)
        {
            GameObject go = ResourcesManager.Instance.getPrefabObj("Player");
            if (go != null)
            {
                GameObject player = Instantiate(go, Vector3.zero, Quaternion.identity);
                
                if(pair.Key == NetworkManager.Instance.sessionId)
                {
                    Debug.Log($"My id : {pair.Key}");
                    Player p = player.AddComponent<MyPlayer>();
                    p.playerId = pair.Key;
                    playerList.Add(pair.Key, p);
                }
                else
                {
                    Player p = player.AddComponent<Player>();
                    p.playerId = pair.Key;
                    playerList.Add(pair.Key, p);
                }
                
               
            }
            else
            {
                Debug.LogError("Player Prefab is Null");
            }
        }
    }

    public void OnPacketRecv(S_BroadCast_MovePacket packet)
    {
        Player player= null;
        playerList.TryGetValue(packet.playerId, out player);
        if(player != null)
        {
            if (packet.playerId == NetworkManager.Instance.sessionId) return;
            player.RecvPacket(packet);
        }
    }
}
