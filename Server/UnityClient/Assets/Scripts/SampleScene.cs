using System;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
    public void Awake()
    {
        UIManager.Instance.Init();
        NetworkManager.Instance.Init();
        ChatManager.Instance.Init();
        ResourcesManager.Instance.Init();
    }

    public void Start()
    {
        UIManager.Instance.Push(UIType.UIPopup_Lobby);
    }
}
