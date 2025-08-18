using NUnit.Framework;
using System;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class ChatManager : SingletonBase<ChatManager>
{
    public Dictionary<int, PlayerData> playerDic = new Dictionary<int, PlayerData>();
    public bool isMaster = false;

    public Action<Chat> OnChatRecved;
    public Action<PlayerData> OnPlayerAdd;
    public Action<int> OnPlayerRemove;
    public Action S_RoomInfo_Handler;
    public Action<RoomData> S_ChangeRoomInfo_Handler;
    public Action<int, bool> S_BroadCast_ReadyPacketHandler;
    public RoomData roomData;

    public override void Init()
    {

    }

    public void OnPlayerListRecv(RoomData roomData, Dictionary<int, PlayerData> dic)
    {
        this.roomData = roomData;

        playerDic = dic;
        foreach(KeyValuePair<int, PlayerData> pair in dic)
        {
            if (pair.Value.isMaster)
            {
                isMaster = true;
            }
        }
        S_RoomInfo_Handler.Invoke();
    }

    public void AddPlayer(PlayerData playerData)
    {
        Debug.Log("ChatManager.AddPlayer");
        playerDic.Add(playerData.sessionId, playerData);
        OnPlayerAdd.Invoke(playerData);
    }

    public void ChangeRoomInfo(RoomData roomData)
    {
        this.roomData = roomData;
        S_ChangeRoomInfo_Handler.Invoke(roomData);
    }

    public void RemovePlayer(int playerId)
    {
        playerDic.Remove(playerId);
        OnPlayerRemove.Invoke(playerId);
    }

    public void RecevMessage(Chat chat)
    {
        OnChatRecved.Invoke(chat);
    }

    public void OnBroadCastReadyPacketRecv(int sessionId, bool isReady)
    {
        S_BroadCast_ReadyPacketHandler.Invoke(sessionId, isReady);
    }
}
