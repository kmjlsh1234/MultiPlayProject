using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : SingletonBase<ChatManager>
{
    public Dictionary<int, Player> playerDic = new Dictionary<int, Player>();
    public Queue<Chat> pendingQueue = new Queue<Chat>();

    public Action<Chat> OnChatRecved;
    public Action<Player> OnPlayerAdd;
    public Action<int> OnPlayerRemove;
    public Action OnPlayerListRecved;

    public override void Init()
    {

    }

    public void OnPlayerListRecv(Dictionary<int, Player> dic)
    {
        playerDic = dic;
        OnPlayerListRecved.Invoke();
    }

    public void AddPlayer(Player player)
    {
        Debug.Log("ChatManager.AddPlayer");
        playerDic.Add(player.playerId, player);
        OnPlayerAdd.Invoke(player);
    }

    public void RemovePlayer(int playerId)
    {
        playerDic.Remove(playerId);
        OnPlayerRemove.Invoke(playerId);
    }

    public void RecevMessage(Chat chat)
    {
        
        pendingQueue.Enqueue(chat);

        OnChatRecved.Invoke(chat);
    }
}
