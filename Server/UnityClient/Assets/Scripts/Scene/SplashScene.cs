using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScene : MonoBehaviour
{
    [SerializeField] private Button button;

    public void Awake()
    {
        ResourcesManager.Instance.Init();
        RoomManager.Instance.Init();
        NetworkManager.Instance.Init();
        ChatManager.Instance.Init();
        UIManager.Instance.Init();
        PlayerManager.Instance.Init();
        
    }

    public void Start()
    {
        button.onClick.AddListener(() => SceneManager.LoadScene("LobbyScene"));
    }
}
