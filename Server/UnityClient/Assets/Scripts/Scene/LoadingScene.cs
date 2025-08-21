using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AsyncOperation = UnityEngine.AsyncOperation;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text progressText;

    [SerializeField] private Transform contentTransform;


    public void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {

        AsyncOperation op = SceneManager.LoadSceneAsync(LoadingSceneManager.Instance.sceneType.ToString());
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                slider.value = op.progress;
                progressText.text = $"{(int)(op.progress * 100)}%";
            }
            else
            {
                // 90% 이후는 씬 activation 대기
                timer += Time.unscaledDeltaTime;
                slider.value = Mathf.Lerp(slider.value, 1f, timer);
                progressText.text = $"{(int)(slider.value * 100)}%";

                if (slider.value >= 0.99f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}


