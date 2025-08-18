using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private Image chatBubble;
    [SerializeField] private TMP_Text name;
    [SerializeField] private Image readySprite;
    [SerializeField] private TMP_Text readyText;
    public void Init(bool isSelf, int name)
    {
        chatBubble.color = isSelf ? Color.yellow : Color.white;
        this.name.text = name.ToString();
        readySprite.color = Color.red;
        readyText.text = "Not Ready";
    }

    public void ChangeColor(bool isReady)
    {
        readySprite.color = isReady ? Color.green : Color.red;
        readyText.text = isReady ? "Ready" : "Not Ready";
    }
}
