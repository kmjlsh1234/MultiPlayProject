using UnityEngine;
using UnityEngine.UI;

public class LobbyPopup : UIBase
{
    [SerializeField] private Transform roomListRoot;
    [SerializeField] private GameObject roomItem;

    [SerializeField] private Button enterButton;
    void Start()
    {
        enterButton.onClick.AddListener(() => EnterRoom());
    }

    // Update is called once per frame
    void EnterRoom()
    {
        C_EnterRoom packet = new C_EnterRoom();
        NetworkManager.Instance.Send(packet.Write());
    }
}
