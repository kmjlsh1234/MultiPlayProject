using TMPro;
using UnityEngine;
using UnityEngine.UI;


public struct Chat
{
    public int playerId;
    public string message;
}

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Image chatBubble;
    [SerializeField] private int playerId;

    public void Init(Chat chat)
    {
        this.playerId = chat.playerId;
        messageText.text = chat.message;
    }
}
