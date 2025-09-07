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
        DataManager.Instance.Init();
        NetworkManager.Instance.Init();
        RoomManager.Instance.Init();
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
        string uuid = Guid.NewGuid().ToString();
        C_Connect packet = new C_Connect() { NickName =uuid };
        NetworkManager.Instance.Send(packet);
        NetworkManager.Instance.uuid = uuid;
    }
}
