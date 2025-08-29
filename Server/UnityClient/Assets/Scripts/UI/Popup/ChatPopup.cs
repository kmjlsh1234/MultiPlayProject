using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Protocol;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChatPopup : UIBase
{
    [SerializeField] private Transform playerListRoot;
    [SerializeField] private Transform chatListRoot;
    [SerializeField] private TMP_Text roomIdText;
    [SerializeField] private TMP_Text masterIdText;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button enterButton;
    [SerializeField] private Button popupOpenButton;
    [SerializeField] private TMP_Text readyText;

    [SerializeField] private GameObject playerItem;
    [SerializeField] private GameObject chatMessage;

    public Dictionary<int, PlayerItem> playerDic = new Dictionary<int, PlayerItem>();

    [SerializeField] private Sprite[] readySprites;

    private bool isReady = false;

    [Header("InvitePopup")]
    [SerializeField] private GameObject invitePopup;
    [SerializeField] private TMP_InputField inviteNickNameField;
    [SerializeField] private Button inviteButton;
    [SerializeField] private Button popupCloseButton;

    public void Awake()
    {

        invitePopup.SetActive(false);

        RegisterButton();
        RegisterEventHandler();

        ChatManager.Instance.OnChatRecved += UpdateChatList;
        
    }

    private void Start()
    {
        if (playerItem == null)
        {
            playerItem = ResourcesManager.Instance.getUIObj("PlayerItem");
        }

        UpdateRoomId(ChatManager.Instance.roomId);
        UpdateMaster(ChatManager.Instance.masterId);

        Dictionary<int, PlayerInfo> players = ChatManager.Instance.players;
        InitPlayerInfo(players);
    }

    void RegisterButton()
    {
        sendButton.onClick.AddListener(() => SendMessage());
        backButton.onClick.AddListener(() => Back());
        enterButton.onClick.AddListener(() => Enter());
        popupOpenButton.onClick.AddListener(() => invitePopup.SetActive(true));
        inviteButton.onClick.AddListener(() => Invite());
        popupCloseButton.onClick.AddListener(() => invitePopup.SetActive(false));
    }

    void RegisterEventHandler()
    {
        ChatManager.Instance.AddPlayerHandler += AddPlayer;
        ChatManager.Instance.RemovePlayerHandler += RemovePlayer;
        ChatManager.Instance.UpdateMasterHandler += UpdateMaster;
        ChatManager.Instance.UpdateReadyHandler += UpdateReady;
    }

    void SendMessage()
    {
        C_Chat packet = new C_Chat();
        packet.Message = inputField.text;
        NetworkManager.Instance.Send(packet);

        inputField.text = string.Empty;
    }

    void Enter()
    {
        C_Ready packet = new C_Ready()
        {
            IsReady = !isReady,
        };

        NetworkManager.Instance.Send(packet);
    }

    void Back()
    {
        C_Exitroom packet = new C_Exitroom();
        NetworkManager.Instance.Send(packet);
        UIManager.Instance.Pop();
    }

    void InitPlayerInfo(Dictionary<int, PlayerInfo> players)
    {
        foreach (PlayerInfo playerInfo in players.Values)
        {
            GameObject go = Instantiate(playerItem);
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.SetParent(playerListRoot);

            PlayerItem item = go.GetComponent<PlayerItem>();
            item.Init(playerInfo);

            playerDic.Add(playerInfo.SessionId, item);
        }
    }

    void AddPlayer(PlayerInfo playerInfo)
    {
        if(playerInfo.SessionId == NetworkManager.Instance.sessionId)
        {
            return;
        }

        if (playerDic.ContainsKey(playerInfo.SessionId))
        {
            Debug.Log($"Already Enter Room : {playerInfo.SessionId}");
            return;
        }

        Debug.Log($"ChatPopup.AddPlayer : {playerInfo.SessionId}");

        GameObject go = Instantiate(playerItem);
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.transform.SetParent(playerListRoot);

        PlayerItem item = go.GetComponent<PlayerItem>();
        item.Init(playerInfo);

        playerDic.Add(playerInfo.SessionId, item);
    }

    void RemovePlayer(int playerId)
    {
        PlayerItem item = null;
        if(playerDic.TryGetValue(playerId, out item))
        {
            Debug.Log("Player Remove");

            if (item != null) 
            {
                Destroy(item.gameObject);
            }
            playerDic.Remove(playerId);
        }
    }
    
    void UpdateRoomId(int roomId)
    {
        roomIdText.text = $"Room Id : {roomId}";
    }

    void UpdateMaster(int masterId)
    {
        masterIdText.text = $"Master Id : {masterId}";
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

    void UpdateReady(int sessionId, bool isReady)
    {
        PlayerItem playerItem = null;
        playerDic.TryGetValue(sessionId, out playerItem);
        if (playerItem != null)
        {
            if(sessionId == NetworkManager.Instance.sessionId)
            {
                enterButton.GetComponent<Image>().sprite = isReady ? readySprites[1] : readySprites[0];
                readyText.text = isReady ? "Ready" : "Not Ready";
            }
            
            playerItem.ChangeColor(isReady);
        }

        if(sessionId == NetworkManager.Instance.sessionId)
        {
            this.isReady = isReady;
        }
    }

    void Invite()
    {
        if (string.IsNullOrEmpty(inviteNickNameField.text))
        {
            Debug.LogError("Empty");
        }
        else
        {
            C_Invite packet = new C_Invite()
            {
                SessionId = int.Parse(inviteNickNameField.text),
            };

            NetworkManager.Instance.Send(packet);
            inviteNickNameField.text = string.Empty;
            invitePopup.SetActive(false);
        }
    }
}
