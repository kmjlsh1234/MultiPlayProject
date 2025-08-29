using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonBase<PlayerManager>
{
    public Dictionary<int, Playerinfo> playerDataList = new Dictionary<int, Playerinfo>();
    public Dictionary<int, Player> playerList = new Dictionary<int, Player>();
    public List<Playerinfo> list = new List<Playerinfo>();
    public void GeneratePlayer()
    {
        foreach (Playerinfo playerInfo in ChatManager.Instance.players.Values)
        {
            GameObject go = ResourcesManager.Instance.getPrefabObj("Player");
            if (go != null)
            {
                GameObject player = Instantiate(go, Vector3.zero, Quaternion.identity);
                /*
                if(pair.Key == NetworkManager.Instance.sessionId)
                {
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
                */
               
            }
            else
            {
                Debug.LogError("Player Prefab is Null");
            }
        }
    }

    public void OnPacketRecv(S_Move packet)
    {
        /*
        Player player= null;
        playerList.TryGetValue(packet.PlayerId, out player);
        if(player != null)
        {
            if (packet.PlayerId == NetworkManager.Instance.sessionId) return;
            player.RecvPacket(packet);
        }
        */
    }
}
