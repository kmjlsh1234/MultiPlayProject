using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public int roomId;
    public string roomName;
    [SerializeField] private TMP_Text roomText;
    [SerializeField] private Button enterButton;

    private void Start()
    {
        enterButton.onClick.AddListener(() => EnterRoom());
    }

    public void Init(int roomId, string roomName)
    {
        this.roomId = roomId;
        this.roomName = roomName;
        roomText.text = $"RoomId : {roomId}\nRoomName : {roomName}";  
    }

    void EnterRoom()
    {
        C_EnterRoom packet = new C_EnterRoom()
        {
            roomId = roomId,
        };
        NetworkManager.Instance.Send(packet.Write());
    }
}
