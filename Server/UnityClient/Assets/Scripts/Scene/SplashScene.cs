using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScene : MonoBehaviour
{
    [SerializeField] private Button button;

    public void Awake()
    {
        ResourcesManager.Instance.Init();
        DataManager.Instance.Init();
        NetworkManager.Instance.Init();
        ChatManager.Instance.Init();
        UIManager.Instance.Init();
        PlayerManager.Instance.Init();
        LoadingSceneManager.Instance.Init();
    }

    public void Start()
    {
        button.onClick.AddListener(() => SceneManager.LoadScene("LobbyScene"));
    }
}
