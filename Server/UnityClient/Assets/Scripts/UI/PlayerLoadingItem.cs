using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoadingItem : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text playerInfoText;
    private int sessionId;
    private int nickName;

    public void Awake()
    {
        
    }

    public void Init(PlayerData playerData)
    {
        slider.value = 0;
        progressText.text = "0%";
        playerInfoText.text = $"Session Id : {playerData.sessionId}\nNickName : {playerData.nickName}";
    }

}
