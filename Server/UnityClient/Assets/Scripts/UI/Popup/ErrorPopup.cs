using ServerCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPopup : UIBase
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text errorCodeText;
    [SerializeField] private TMP_Text errorMessageText;

    public void Start()
    {
        closeButton.onClick.AddListener(() => UIManager.Instance.Pop());
    }

    public void Init(S_ErrorCode errorCode)
    {
        errorCodeText.text = $"ErrorCode : {errorCode.code.ToString()}";
        errorMessageText.text = $"ErrorMessage : {errorCode.message}";
    }

   
}
