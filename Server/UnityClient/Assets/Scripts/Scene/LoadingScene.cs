using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text progressText;

    [SerializeField] private Transform contentTransform;

    public AsyncOperation op;

    private void Awake()
    {
        LoadingSceneManager.Instance.OnLoadingCompleted += MoveScene;
    }

    public void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {

        op = SceneManager.LoadSceneAsync(LoadingSceneManager.Instance.sceneType.ToString());
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
                    SendPacket();
                    yield break;
                }
            }
        }
        yield break;
    }

    void SendPacket()
    {
        C_LoadingCompletePacket packet = new C_LoadingCompletePacket();
        NetworkManager.Instance.Send(packet.Write());
    }

    void MoveScene()
    {
        op.allowSceneActivation = true;
    }
}


