using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    
    void Start()
    {
        UIManager.Instance.Push(UIType.UIPopup_Lobby);
        
        //Room List 가져오기
        C_RoomList packet = new C_RoomList();
        NetworkManager.Instance.Send(packet.Write());
    }
}
