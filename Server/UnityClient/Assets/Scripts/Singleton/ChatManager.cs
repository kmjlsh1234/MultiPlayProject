using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : SingletonBase<ChatManager>
{
    public Dictionary<int, PlayerData> playerDic = new Dictionary<int, PlayerData>();
    public bool isMaster = false;
    public Action<Chat> OnChatRecved;
    public Action<PlayerData> OnPlayerAdd;
    public Action<int> OnPlayerRemove;
    public Action S_PlayerList_Handler;

    public override void Init()
    {

    }

    public void OnPlayerListRecv(Dictionary<int, PlayerData> dic)
    {
        playerDic = dic;
        foreach(KeyValuePair<int, PlayerData> pair in dic)
        {
            if (pair.Value.isMaster)
            {
                isMaster = true;
            }
        }
        S_PlayerList_Handler.Invoke();
    }

    public void AddPlayer(PlayerData playerData)
    {
        Debug.Log("ChatManager.AddPlayer");
        playerDic.Add(playerData.sessionId, playerData);
        OnPlayerAdd.Invoke(playerData);
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
}
