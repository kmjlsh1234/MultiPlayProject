using Google.Protobuf.Protocol;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScene : MonoBehaviour
{
    [SerializeField] private Button enterButton;

    public void Awake()
    {
        ResourcesManager.Instance.Init();
        NetworkManager.Instance.Init();
        UIManager.Instance.Init();
        PlayerManager.Instance.Init();
        LoadingSceneManager.Instance.Init();
        GameManager.Instance.Init();
        HttpManager.Instance.Init();
    }

    public void Start()
    {
        enterButton.onClick.AddListener(() => Enter());
    }

    void Enter()
    {
        C_Connect packet = new C_Connect() 
        { 
            NickName = Guid.NewGuid().ToString() 
        };

        NetworkManager.Instance.uuid = packet.NickName;
        NetworkManager.Instance.Send(packet);
        
    }
}
