using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Protocol;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchRoomPopup : UIBase
{

    //RpomInfo Data
    public int roomId { get; set; }
    public int masterId { get; set; }
    private Dictionary<int, Matchplayerinfo> matchPlayers = new Dictionary<int, Matchplayerinfo>();
    private Dictionary<int, PlayerItem> playerItems = new Dictionary<int, PlayerItem>();

    [Header("Button")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button enterButton;
    [SerializeField] private Button popupOpenButton;

    [Header("TMP_Text")]
    [SerializeField] private TMP_Text roomIdText;
    [SerializeField] private TMP_Text masterIdText;
    [SerializeField] private TMP_Text readyText;

    private GameObject playerItem { get; set; }
    [SerializeField] private Transform playerListRoot;
    [SerializeField] private Sprite[] readySprites;

    private bool isReady { get; set; } = false;

    [Header("InvitePopup")]
    [SerializeField] private GameObject invitePopup;
    [SerializeField] private TMP_InputField inviteNickNameField;
    [SerializeField] private Button inviteButton;
    [SerializeField] private Button popupCloseButton;

    public void Awake()
    {
        invitePopup.SetActive(false);

        RegistButton();
        RegistEvent();
    }

   
    public void Init(S_Matchroominfo packet)
    {
        playerItem = ResourcesManager.Instance.getUIObj("PlayerItem");
        UpdateRoomInfo(packet);
    }

    void RegistButton()
    {
        backButton.onClick.AddListener(() => Back());
        enterButton.onClick.AddListener(() => OnClickReady());
        popupOpenButton.onClick.AddListener(() => invitePopup.SetActive(true));
        inviteButton.onClick.AddListener(() => Invite());
        popupCloseButton.onClick.AddListener(() => invitePopup.SetActive(false));
    }

    void RegistEvent()
    {
        PacketHandler.Event_S_MatchRoomInfo += UpdateRoomInfo;
        PacketHandler.Event_S_EnterMatchRoom += AddPlayer;
        PacketHandler.Event_S_ExitRoom += RemovePlayer;
        PacketHandler.Event_S_Ready += UpdateReady;
}

    void UpdateRoomInfo(S_Matchroominfo packet)
    {
        this.roomId = packet.RoomInfo.RoomId;
        this.masterId = packet.RoomInfo.MasterId;

        roomIdText.text = $"RoomId : {packet.RoomInfo.RoomId.ToString()}";
        masterIdText.text = $"MasterId : {packet.RoomInfo.MasterId.ToString()}";

        // 기존 플레이어 삭제
        foreach (var item in playerItems.Values)
        {
            Destroy(item.gameObject);
        }

        matchPlayers.Clear();
        playerItems.Clear();

        // 현재 플레이어 목록 생성
        foreach (Matchplayerinfo info in packet.RoomInfo.Players)
        {
            AddPlayer(info);
        }

    }

    void OnClickReady()
    {
        C_Ready packet = new C_Ready();
        packet.IsReady = !isReady;
        NetworkManager.Instance.Send(packet);
    }

    void Back()
    {
        PacketHandler.Event_S_MatchRoomInfo -= UpdateRoomInfo;
        PacketHandler.Event_S_EnterMatchRoom -= AddPlayer;
        PacketHandler.Event_S_ExitRoom -= RemovePlayer;
        PacketHandler.Event_S_Ready -= UpdateReady;

        C_Exitroom packet = new C_Exitroom();
        NetworkManager.Instance.Send(packet);
        UIManager.Instance.Pop();
    }

    void AddPlayer(Matchplayerinfo info)
    {
        if (matchPlayers.ContainsKey(info.SessionId))
        {
            Debug.Log($"Already Enter Room : {info.SessionId}");
            return;
        }

        PlayerItem item = GenerateItem();
        item.Init(info);

        matchPlayers.Add(info.SessionId, info);
        playerItems.Add(info.SessionId, item);
    }

    void RemovePlayer(S_Exitroom packet)
    {
        if(matchPlayers.TryGetValue(packet.SessionId, out Matchplayerinfo info))
        {
            matchPlayers.Remove(packet.SessionId);
        }

        if(playerItems.TryGetValue(packet.SessionId, out PlayerItem item))
        {
            Destroy(item.gameObject);
            playerItems.Remove(packet.SessionId);
        }
    }

    void UpdateReady(S_Ready packet)
    {
        Debug.Log("UpdateReady");
        if(matchPlayers.TryGetValue(packet.SessionId, out Matchplayerinfo info))
        {
            info.IsReady = packet.IsReady;
        }

        if (playerItems.TryGetValue(packet.SessionId, out PlayerItem playerItem))
        {
            if(packet.SessionId == NetworkManager.Instance.sessionId)
            {
                enterButton.GetComponent<Image>().sprite = packet.IsReady ? readySprites[1] : readySprites[0];
                readyText.text = packet.IsReady ? "Ready" : "Not Ready";
                this.isReady = packet.IsReady;
            }
            
            playerItem.ChangeColor(packet.IsReady);
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

    PlayerItem GenerateItem()
    {
        GameObject go = Instantiate(playerItem);
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.transform.SetParent(playerListRoot);

        PlayerItem item = go.GetComponent<PlayerItem>();
        return item;
    }
}
