using System;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
    public LobbyPopup LobbyPopup;
    public ChatPopup ChatPopup;

    
    public void Start()
    {
        ChatPopup.gameObject.SetActive(false);
        LobbyPopup.gameObject.SetActive(true);
    }

    public void EnterRoom()
    {
        LobbyPopup.gameObject.SetActive(false);
        ChatPopup.gameObject.SetActive(true);
    }
}
