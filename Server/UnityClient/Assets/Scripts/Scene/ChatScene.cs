using System;
using UnityEngine;

public class ChatScene : MonoBehaviour
{
    public void Awake()
    {
        ResourcesManager.Instance.Init();       
        NetworkManager.Instance.Init();
        ChatManager.Instance.Init();
        UIManager.Instance.Init();
    }

    public void Start()
    {
        UIManager.Instance.Push(UIType.UIPopup_Lobby);
    }
}
