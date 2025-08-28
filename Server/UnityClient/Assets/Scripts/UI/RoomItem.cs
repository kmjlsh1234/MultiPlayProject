using Google.Protobuf.Protocol;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public int roomId;

    [SerializeField] private TMP_Text roomText;
    [SerializeField] private Button enterButton;

    private void Start()
    {
        enterButton.onClick.AddListener(() => EnterRoom());
    }

    public void Init(int roomId)
    {
        this.roomId = roomId;
        roomText.text = $"RoomId : {roomId}";  
    }

    void EnterRoom()
    {
        C_Enterroom packet = new C_Enterroom()
        {
            RoomId = roomId,
        };
        NetworkManager.Instance.Send(packet);
    }
}
