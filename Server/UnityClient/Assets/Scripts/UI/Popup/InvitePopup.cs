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

    public void Init(S_InvitePacket packet)
    {
        this.roomId = packet.roomId;
        inviteInfo.text = $"{packet.sendUserNickName}이 초대를 보냈습니다.";
    }

    void EnterRoom()
    {
        
        if (!isSend)
        {
            C_EnterRoom packet = new C_EnterRoom()
            {
                roomId = roomId,
            };
            NetworkManager.Instance.Send(packet.Write());
            isSend = true;
        }
        
    }
}
