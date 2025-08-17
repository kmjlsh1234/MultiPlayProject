using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UniRx;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ChatPopup : UIBase
{
    [SerializeField] private Transform playerListRoot;
    [SerializeField] private Transform chatListRoot;
    [SerializeField] private TMP_Text roomInfoText;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button enterButton;

    [SerializeField] private GameObject playerItem;
    [SerializeField] private GameObject chatMessage;

    public Dictionary<int, PlayerItem> playerDic = new Dictionary<int, PlayerItem>();

    [SerializeField] private Sprite[] readySprites;
    public void Awake()
    {
        sendButton.onClick.AddListener(() => SendMessage());
        backButton.onClick.AddListener(() => Back());
        enterButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Playground");
            UIManager.Instance.Clear();
        });
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
            item.Init(pair.Value.isSelf, pair.Value.sessionId);

            playerDic.Add(pair.Value.sessionId, item);
        }
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
        item.Init(playerData.isSelf, playerData.sessionId);

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
}
