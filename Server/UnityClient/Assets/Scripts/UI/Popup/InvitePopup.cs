using Google.Protobuf.Protocol;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvitePopup : UIBase
{
    [SerializeField] private TMP_Text inviteInfo;
    [SerializeField] private Button enterRoomButton;
    [SerializeField] private Button closeButton;

    private int roomId;
    bool isSend = false;

    private void Start()
    {
        enterRoomButton.onClick.AddListener(() => EnterRoom());
        closeButton.onClick.AddListener(() => UIManager.Instance.Pop());
    }

    public void Init(S_Invite packet)
    {
        this.roomId = packet.RoomId;
        inviteInfo.text = $"{packet.NickName}이 초대를 보냈습니다.";
    }

    void EnterRoom()
    {
        
        if (!isSend)
        {
            C_Entermatchroom packet = new C_Entermatchroom()
            {
                RoomId = roomId,
            };
            NetworkManager.Instance.Send(packet);
            isSend = true;
        }
        
    }
}
