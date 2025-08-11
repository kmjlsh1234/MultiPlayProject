using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private Image chatBubble;
    [SerializeField] private TMP_Text name;

    public void Init(bool isSelf, int name)
    {
        chatBubble.color = isSelf ? Color.yellow : Color.white;
        this.name.text = name.ToString();
    }
}
