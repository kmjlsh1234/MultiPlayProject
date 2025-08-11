using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UniRx;
using System.Collections.Generic;

public class ChatPopup : UIBase
{
    [SerializeField] private Transform playerListRoot;
    [SerializeField] private Transform chatListRoot;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;

    [SerializeField] private GameObject playerItem;
    [SerializeField] private GameObject chatMessage;

    public void Awake()
    {
        sendButton.onClick.AddListener(() => SendMessage());
        ChatManager.Instance.ObserveEveryValueChanged(x => x.playerList).Subscribe((x) => UpdatePlayerList(x));
        ChatManager.Instance.ObserveEveryValueChanged(x => x.chatStack).Subscribe((x) =>
        {
            if(x.Count == 0)
            {
                return;
            }
            UpdateChatList(x.Peek());
        });
    }

    void SendMessage()
    {
        C_Chat packet = new C_Chat();
        packet.message = inputField.text;
        NetworkManager.Instance.Send(packet.Write());

        inputField.text = string.Empty;
    }

    void UpdatePlayerList(List<Player> list)
    {
        Debug.Log("updatePlayerList");
        if(playerItem == null)
        {
            playerItem = ResourcesManager.Instance.getUIObj("PlayerItem");
        }
        
        foreach (Player player in list){
            Debug.Log($"updatePlayerList : {player.playerId}");
            GameObject go = Instantiate(playerItem);
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.SetParent(playerListRoot);

            PlayerItem item = go.GetComponent<PlayerItem>();
            item.Init(false, player.playerId);
        }
    }

    void UpdateChatList(Chat chat)
    {
        if (chatMessage == null)
        {
            chatMessage = ResourcesManager.Instance.getUIObj("ChatMessage");
        }
        GameObject go = Instantiate(chatMessage);
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.transform.SetParent(chatListRoot);

        ChatMessage message = go.GetComponent<ChatMessage>();
        message.Init(chat);

    }
}
