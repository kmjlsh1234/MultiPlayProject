using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScene : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject popup;
    [SerializeField] private TMP_InputField nickNameField;
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

        popup.SetActive(false);
    }

    public void Start()
    {
        button.onClick.AddListener(() => popup.SetActive(true));
        enterButton.onClick.AddListener(() => Enter());
    }

    void Enter()
    {
        if(string.IsNullOrEmpty(nickNameField.text))
        {
            nickNameField.text = string.Empty;
            Debug.Log("NickName Empty!");
        }
        else
        {
            C_PlayerInfoPacket packet = new C_PlayerInfoPacket()
            {
                nickName = nickNameField.text,
            };
            NetworkManager.Instance.Send(packet.Write());
        }
    }
}
