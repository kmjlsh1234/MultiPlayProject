using Google.Protobuf.Protocol;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class ChatManager : SingletonBase<ChatManager>
{
    public bool isMaster = false;

    public Action<Chat> OnChatRecved;
    public Action<PlayerInfo> OnPlayerAdd;
    public Action<int> OnPlayerRemove;
    public Action<S_Roominfo> S_RoomInfo_Handler;
    public Action<S_Roominfo> S_ChangeRoomInfo_Handler;
    public Action<int, bool> S_BroadCast_ReadyPacketHandler;
    public S_Roominfo roomInfo;

    public override void Init()
    {

    }

    public void OnPlayerListRecv(S_Roominfo packet)
    {
        this.roomInfo = roomInfo;
        S_RoomInfo_Handler.Invoke(packet);
    }

    public void AddPlayer(PlayerInfo playerInfo)
    {
        Debug.Log("ChatManager.AddPlayer");
        roomInfo.Players.Add(playerInfo);
        OnPlayerAdd.Invoke(playerInfo);
    }

    public void ChangeRoomInfo(S_Changeroominfo changeInfo)
    {
        roomInfo.MasterId = changeInfo.MasterId;
        S_ChangeRoomInfo_Handler.Invoke(roomInfo);
    }

    public void RemovePlayer(int playerId)
    {
        PlayerInfo playerInfo = roomInfo.Players[playerId];
        roomInfo.Players.Remove(playerInfo);
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
