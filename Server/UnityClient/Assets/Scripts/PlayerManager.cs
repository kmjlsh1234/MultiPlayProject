using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonBase<PlayerManager>
{
    //public Dictionary<int, Matchplayerinfo> playerDataList = new Dictionary<int, Matchplayerinfo>();
    public Dictionary<int, PlayerController> playerList = new Dictionary<int, PlayerController>();

    public void GeneratePlayer()
    {
        /*
        foreach (Matchplayerinfo playerInfo in RoomManager.Instance.players.Values)
        {
            GameObject go = ResourcesManager.Instance.getPrefabObj("Player");
            if (go != null)
            {
                GameObject player = Instantiate(go, Vector3.zero, Quaternion.identity);

                if(playerInfo.SessionId == NetworkManager.Instance.sessionId)
                {
                    PlayerController p = player.AddComponent<MyPlayerController>();
                    p.gameObject.tag = "Player";
                    p.playerId = playerInfo.SessionId;
                    playerList.Add(p.playerId, p);
                    
                }
                else
                {
                    PlayerController p = player.AddComponent<PlayerController>();
                    p.playerId = playerInfo.SessionId;
                    playerList.Add(p.playerId, p);
                }

               
            }
            else
            {
                Debug.LogError("Player Prefab is Null");
            }
        }
        */
    }
    
    public void OnPacketRecv(S_Move packet)
    {
        /*
        PlayerController player= null;
        playerList.TryGetValue(packet.SessionId, out player);
        if(player != null && !NetworkManager.Instance.sessionId.Equals(packet.ObjectId))
        {
            player.OnMovePacket(packet);
        }
        */
    }
}
