using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ChatPopup : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;

    public void Start()
    {
        sendButton.onClick.AddListener(() => SendMessage());
    }

    void SendMessage()
    {
        C_Chat packet = new C_Chat();
        packet.message = inputField.text;
        NetworkManager.Instance.Send(packet.Write());
    }
}
