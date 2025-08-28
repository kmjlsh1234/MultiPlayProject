using Google.Protobuf.Protocol;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScene : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject popup;
    [SerializeField] private Button enterButton;

    public void Awake()
    {
        ResourcesManager.Instance.Init();
        DataManager.Instance.Init();
        NetworkManager.Instance.Init();
        ChatManager.Instance.Init();
        UIManager.Instance.Init();
        PlayerManager.Instance.Init();
        LoadingSceneManager.Instance.Init();
        GameManager.Instance.Init();
        HttpManager.Instance.Init();
        popup.SetActive(false);
    }

    public void Start()
    {
        button.onClick.AddListener(() => popup.SetActive(true));
        enterButton.onClick.AddListener(() => Enter());
    }

    void Enter()
    {
        string uuid = Guid.NewGuid().ToString();
        C_Playerinfo packet = new C_Playerinfo()
        {
            NickName = uuid,
        };
        NetworkManager.Instance.Send(packet);
    }
}
