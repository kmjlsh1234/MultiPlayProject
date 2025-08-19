using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPopup : UIBase
{
    [SerializeField] private Transform roomListRoot;
    [SerializeField] private GameObject roomItem;
    [SerializeField] private GameObject createRoomPopup;

    [Header("Button")]
    [SerializeField] private Button createOrJoinRoomButton;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button popupOpenButton;
    [SerializeField] private Button popupCloseButton;
    [SerializeField] private Button createRoomButton;

    [Header("CreateRoomPopup")]
    [SerializeField] private TMP_InputField roomNameField;
    
    Dictionary<int, RoomItem> roomDic = new Dictionary<int, RoomItem>();


    private void Awake()
    {
        createRoomPopup.gameObject.SetActive(false);
        ChatManager.Instance.S_RoomInfo_Handler += (() => createRoomPopup.SetActive(false));
    }

    void Start()
    {
        popupOpenButton.onClick.AddListener(() => createRoomPopup.gameObject.SetActive(true));
        createOrJoinRoomButton.onClick.AddListener(() => CreateOrJoinRoom());
        popupCloseButton.onClick.AddListener(() => ResetPopup());
        createRoomButton.onClick.AddListener(() => CreateRoom());
        refreshButton.onClick.AddListener(() => RefreshRoomList());

        DataManager.Instance.RoomListRecvHandler += UpdateRoomList;

        RefreshRoomList();
    }

    void UpdateRoomList(Dictionary<int, RoomData> dic)
    {
        if (roomItem == null)
        {
            roomItem = ResourcesManager.Instance.getUIObj("RoomItem");
        }
        List<int> removeKeys = new List<int>();

        foreach (KeyValuePair<int, RoomItem> pair in roomDic)
        {
            if (!dic.ContainsKey(pair.Key))
            {
                GameObject go = pair.Value.gameObject;
                removeKeys.Add(pair.Key);
                Destroy(go);
            }

        }
        
        foreach (int key in removeKeys)
        {
            roomDic.Remove(key);
        }

        foreach (KeyValuePair<int, RoomData> pair in dic)
        {
            if (roomDic.ContainsKey(pair.Key))
            {
                continue;
            }
            GameObject go = Instantiate(roomItem);
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.SetParent(roomListRoot);

            RoomItem item = go.GetComponent<RoomItem>();
            item.Init(pair.Value.roomId, pair.Value.roomName, pair.Value.playerCount);

            roomDic.Add(pair.Value.roomId, item);
        }
    }

    void ResetPopup()
    {
        roomNameField.text = string.Empty;
        createRoomPopup.gameObject.SetActive(false);
    }

    void CreateRoom()
    {
        C_CreateRoom packet = new C_CreateRoom() { roomName = roomNameField.text};
        NetworkManager.Instance.Send(packet.Write());
        roomNameField.text = string.Empty;
    }

    void CreateOrJoinRoom()
    {
        C_CreateOrJoinRoom packet = new C_CreateOrJoinRoom();
        NetworkManager.Instance.Send(packet.Write());
    }

    void RefreshRoomList()
    {
        C_RoomList packet = new C_RoomList();
        NetworkManager.Instance.Send(packet.Write());
    }
}
