using Google.Protobuf.Protocol;
using System.Collections;
using TMPro;
using UnityEngine;

public class InGamePopup : UIBase
{
    [SerializeField] private TMP_Text timerText;

    int timer = 0;
    private bool isRunning = false;
    void Start()
    {
        LoadingSceneManager.Instance.OnLoadingCompleted += (() => StartCoroutine(UpdateTimer()));
        C_Loadingcomplete packet = new C_Loadingcomplete();
        NetworkManager.Instance.Send(packet);
    }

    private IEnumerator UpdateTimer()
    {
        isRunning = true;
        while (true)
        {
            timerText.text = timer.ToString(); // 0, 1, 2, 3 ...
            yield return new WaitForSeconds(1f);
            timer++;
        }
    }

}
