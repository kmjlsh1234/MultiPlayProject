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
    [SerializeField] private Button backButton;

    [SerializeField] private GameObject playerItem;
    [SerializeField] private GameObject chatMessage;

    public void Awake()
    {
        sendButton.onClick.AddListener(() => SendMessage());
        backButton.onClick.AddListener(() => Back());   
        
        ChatManager.Instance.OnChatRecved += UpdateChatList;
        ChatManager.Instance.OnPlayerAdd += AddPlayer;
    }

    private void Start()
    {
        PlayerListInitialize();

    }

    void SendMessage()
    {
        C_Chat packet = new C_Chat();
        packet.message = inputField.text;
        NetworkManager.Instance.Send(packet.Write());

        inputField.text = string.Empty;
    }

    void Back()
    {
        C_ExitRoom packet = new C_ExitRoom();
        NetworkManager.Instance.Send(packet.Write());
    }

    void PlayerListInitialize()
    {

        List<Player> list = ChatManager.Instance.playerList;

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
            item.Init(player.isSelf, player.playerId);
        }
    }

    void AddPlayer(Player player)
    {
        if(player.playerId == NetworkManager.Instance.sessionId)
        {
            return;
        }

        GameObject go = Instantiate(playerItem);
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.transform.SetParent(playerListRoot);

        PlayerItem item = go.GetComponent<PlayerItem>();
        item.Init(player.isSelf, player.playerId);
    }

    void UpdateChatList(Chat chat)
    {
        Debug.Log("UpdateChatList");
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
