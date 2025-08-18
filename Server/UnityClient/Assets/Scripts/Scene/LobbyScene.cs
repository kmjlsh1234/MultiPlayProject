using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    
    void Start()
    {
        UIManager.Instance.Push(UIType.UIPopup_Lobby);
    }
}
