using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UniRx;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ChatPopup : UIBase
{
    [SerializeField] private Transform playerListRoot;
    [SerializeField] private Transform chatListRoot;
    [SerializeField] private TMP_Text roomInfoText;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button enterButton;
    [SerializeField] private Button popupOpenButton;
    [SerializeField] private Button inviteButton;

    [SerializeField] private GameObject invitePopup;

    [SerializeField] private GameObject playerItem;
    [SerializeField] private GameObject chatMessage;

    public Dictionary<int, PlayerItem> playerDic = new Dictionary<int, PlayerItem>();

    [SerializeField] private Sprite[] readySprites;

    private bool isReady = false;

    public void Awake()
    {
        sendButton.onClick.AddListener(() => SendMessage());
        backButton.onClick.AddListener(() => Back());
        enterButton.onClick.AddListener(() => Enter());
        ChatManager.Instance.OnChatRecved += UpdateChatList;
        ChatManager.Instance.OnPlayerAdd += AddPlayer;
        ChatManager.Instance.OnPlayerRemove += RemovePlayer;
        ChatManager.Instance.S_ChangeRoomInfo_Handler += UpdateRoomInfo;
        ChatManager.Instance.S_BroadCast_ReadyPacketHandler += UpdateReadyState;
    }

    private void Start()
    {
        PlayerListInitialize();

        UpdateRoomInfo(ChatManager.Instance.roomData);
    }

    void SendMessage()
    {
        C_Chat packet = new C_Chat();
        packet.message = inputField.text;
        NetworkManager.Instance.Send(packet.Write());

        inputField.text = string.Empty;
    }

    void Enter()
    {
        C_ReadyPacket packet = new C_ReadyPacket()
        {
            isReady = !isReady,
        };

        NetworkManager.Instance.Send(packet.Write());
    }

    void Back()
    {
        C_ExitRoom packet = new C_ExitRoom();
        NetworkManager.Instance.Send(packet.Write());
        UIManager.Instance.Pop();
    }

    void PlayerListInitialize()
    {
        Dictionary<int, PlayerData> dic= ChatManager.Instance.playerDic;

        if(playerItem == null)
        {
            playerItem = ResourcesManager.Instance.getUIObj("PlayerItem");
        }

        foreach (KeyValuePair<int, PlayerData> pair in dic)
        {
            GameObject go = Instantiate(playerItem);
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.SetParent(playerListRoot);

            PlayerItem item = go.GetComponent<PlayerItem>();
            item.Init(pair.Value.isSelf, pair.Value.sessionId, pair.Value.nickName);

            playerDic.Add(pair.Value.sessionId, item);
        }
    }

    void UpdateRoomInfo(RoomData roomData)
    {
        roomInfoText.text = $"RoomId : {roomData.roomId} / RoomName : {roomData.roomName} / MasterId : {roomData.masterId}";
    }

    void AddPlayer(PlayerData playerData)
    {
        if(playerData.sessionId == NetworkManager.Instance.sessionId)
        {
            return;
        }

        if (playerDic.ContainsKey(playerData.sessionId))
        {
            Debug.Log($"Already Enter Room : {playerData.sessionId}");
            return;
        }

        Debug.Log($"ChatPopup.AddPlayer : {playerData.sessionId}");

        GameObject go = Instantiate(playerItem);
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.transform.SetParent(playerListRoot);

        PlayerItem item = go.GetComponent<PlayerItem>();
        item.Init(playerData.isSelf, playerData.sessionId, playerData.nickName);

        playerDic.Add(playerData.sessionId, item);
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

    void UpdateReadyState(int sessionId, bool isReady)
    {
        PlayerItem playerItem = null;
        playerDic.TryGetValue(sessionId, out playerItem);
        if (playerItem != null)
        {
            playerItem.ChangeColor(isReady);
        }

        if(sessionId == NetworkManager.Instance.sessionId)
        {
            this.isReady = isReady;
        }
    }
}
