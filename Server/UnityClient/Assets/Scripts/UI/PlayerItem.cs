using Google.Protobuf.Protocol;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private GameObject isSelfObj;  
    [SerializeField] private Image frame;
    [SerializeField] private TMP_Text playerInfoText;

    public void Init(Playerinfo playerInfo)
    {
        isSelfObj.gameObject.SetActive(playerInfo.SessionId.Equals(NetworkManager.Instance.sessionId));

        playerInfoText.text = $"Session Id : {playerInfo.SessionId}\nNickName : {playerInfo.NickName}";
        frame.color = playerInfo.IsReady ? Color.green : Color.white;
    }

    public void ChangeColor(bool isReady)
    {
        frame.color = isReady ? Color.green : Color.white;
    }
}
