using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : SingletonBase<ChatManager>
{
    public List<Player> playerList = new List<Player>();
    public Queue<Chat> pendingQueue = new Queue<Chat>();

    public Action<Chat> OnChatRecved;
    public Action<Player> OnPlayerAdd;
    public override void Init()
    {

    }

    public void AddPlayer(Player player)
    {
        playerList.Add(player);
        OnPlayerAdd.Invoke(player);
    }

    public void RemovePlayer(Player player)
    {
        playerList.Remove(player);

    }

    public void RecevMessage(Chat chat)
    {
        
        pendingQueue.Enqueue(chat);

        OnChatRecved.Invoke(chat);
    }
}
