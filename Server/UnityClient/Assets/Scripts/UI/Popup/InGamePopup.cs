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
        StartCoroutine(UpdateTimer());
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
