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
    public Dictionary<int, Matchplayerinfo> players = new Dictionary<int, Matchplayerinfo>();

    //EventHandler
    public Action<Dictionary<int, Matchplayerinfo>> InitRoomHandler;
    public Action<Matchplayerinfo> AddPlayerHandler;
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
        foreach(Matchplayerinfo playerInfo in packet.Players)
        {
            players.Add(playerInfo.SessionId, playerInfo);
        }
    }

    public void AddPlayer(Matchplayerinfo playerInfo)
    {
        players.Add(playerInfo.SessionId, playerInfo);
        AddPlayerHandler.Invoke(playerInfo);
    }

    public void RemovePlayer(int playerId)
    {
        Matchplayerinfo playerInfo = null;
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
