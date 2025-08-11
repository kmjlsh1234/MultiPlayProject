using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : SingletonBase<ChatManager>
{
    public List<Player> playerList = new List<Player>();
    public Stack<Chat> chatStack = new Stack<Chat>();

    public override void Init()
    {

    }

    public void AddPlayer(Player player)
    {
        playerList.Add(player);
    }

    public void RecevMessage(Chat chat)
    {
        chatStack.Push(chat);
    }
}
