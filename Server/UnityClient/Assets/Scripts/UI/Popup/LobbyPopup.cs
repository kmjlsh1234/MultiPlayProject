using Google.Protobuf.Protocol;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPopup : UIBase
{
    [SerializeField] private TMP_Text playerInfo;

    [Header("Button")]
    [SerializeField] private Button createOrJoinRoomButton;
    [SerializeField] private Button createRoomButton;

    void Start()
    {
        createRoomButton.onClick.AddListener(() => CreateRoom());
        createOrJoinRoomButton.onClick.AddListener(() => CreateOrJoinRoom());
        playerInfo.text = $"sessionId : {NetworkManager.Instance.sessionId}";
    }

    void CreateRoom()
    {
        C_Createroom packet = new C_Createroom();
        NetworkManager.Instance.Send(packet);
    }

    void CreateOrJoinRoom()
    {
        C_Createorjoinroom packet = new C_Createorjoinroom();
        NetworkManager.Instance.Send(packet);        
    }  
}
