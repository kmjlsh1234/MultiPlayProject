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

    //RoomInfo
    public int roomId;
    public int masterId;
    public Dictionary<int, Playerinfo> players = new Dictionary<int, Playerinfo>();

    //EventHandler
    public Action<Dictionary<int, Playerinfo>> InitRoomHandler;
    public Action<Playerinfo> AddPlayerHandler;
    public Action<int> RemovePlayerHandler;
    public Action<int> UpdateMasterHandler;
    public Action<int, bool> UpdateReadyHandler;

    public Action<Chat> OnChatRecved;

    public override void Init()
    {

    }

    public void InitRoom(S_Roominfo packet)
    {
        roomId = packet.RoomId;
        masterId = packet.MasterId;

        players.Clear();
        foreach(Playerinfo playerInfo in packet.Players)
        {
            players.Add(playerInfo.SessionId, playerInfo);
        }
    }

    public void AddPlayer(Playerinfo playerInfo)
    {
        players.Add(playerInfo.SessionId, playerInfo);
        AddPlayerHandler.Invoke(playerInfo);
    }

    public void RemovePlayer(int playerId)
    {
        Playerinfo playerInfo = null;
        if(players.TryGetValue(playerId, out playerInfo))
        {
            players.Remove(playerInfo.SessionId);

        }
        RemovePlayerHandler.Invoke(playerId);
    }

    public void UpdateMaster(int masterId)
    {
        this.masterId = masterId;
        UpdateMasterHandler.Invoke(masterId);
    }

    public void RecevMessage(Chat chat)
    {
        OnChatRecved.Invoke(chat);
    }

    public void UpdateReady(int sessionId, bool isReady)
    {
        UpdateReadyHandler.Invoke(sessionId, isReady);
    }
}
