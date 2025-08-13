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

    public Dictionary<int, PlayerItem> playerDic = new Dictionary<int, PlayerItem>();


    public void Awake()
    {
        sendButton.onClick.AddListener(() => SendMessage());
        backButton.onClick.AddListener(() => Back());   
        
        ChatManager.Instance.OnChatRecved += UpdateChatList;
        ChatManager.Instance.OnPlayerAdd += AddPlayer;
        ChatManager.Instance.OnPlayerRemove += RemovePlayer;
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
        UIManager.Instance.Pop();
    }

    void PlayerListInitialize()
    {
        Dictionary<int, Player> dic= ChatManager.Instance.playerDic;

        if(playerItem == null)
        {
            playerItem = ResourcesManager.Instance.getUIObj("PlayerItem");
        }

        foreach (KeyValuePair<int, Player> pair in dic)
        {
            GameObject go = Instantiate(playerItem);
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.SetParent(playerListRoot);

            PlayerItem item = go.GetComponent<PlayerItem>();
            item.Init(pair.Value.isSelf, pair.Value.playerId);

            playerDic.Add(pair.Value.playerId, item);
        }
    }

    void AddPlayer(Player player)
    {
        if(player.playerId == NetworkManager.Instance.sessionId)
        {
            return;
        }

        if (playerDic.ContainsKey(player.playerId))
        {
            Debug.Log($"Already Enter Room : {player.playerId}");
            return;
        }

        Debug.Log($"ChatPopup.AddPlayer : {player.playerId}");

        GameObject go = Instantiate(playerItem);
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.transform.SetParent(playerListRoot);

        PlayerItem item = go.GetComponent<PlayerItem>();
        item.Init(player.isSelf, player.playerId);

        playerDic.Add(player.playerId, item);
    }

    void RemovePlayer(int playerId)
    {
        PlayerItem item = null;
        if(playerDic.TryGetValue(playerId, out item))
        {
            Debug.Log("Player Remove");

            if (item != null) // Unity 오브젝트 null 체크
            {
                Destroy(item.gameObject);
            }
            playerDic.Remove(playerId);
            
        }

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
