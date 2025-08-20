using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private GameObject isSelfObj;  
    [SerializeField] private Image frame;
    [SerializeField] private TMP_Text playerInfo;

    public void Init(PlayerData playerData)
    {
        isSelfObj.gameObject.SetActive(playerData.isSelf);

        playerInfo.text = $"Session Id : {playerData.sessionId}\nNickName : {playerData.nickName}";
        frame.color = playerData.isReady ? Color.green : Color.white;
    }

    public void ChangeColor(bool isReady)
    {
        frame.color = isReady ? Color.green : Color.white;
    }
}
