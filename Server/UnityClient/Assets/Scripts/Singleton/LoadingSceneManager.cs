using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    ChatScene,
    Playground,
    LoadingScene,
}

public class LoadingSceneManager : SingletonBase<LoadingSceneManager>
{
    public SceneType sceneType;

    public void LoadScene(SceneType type)
    {
        sceneType = type;
        SceneManager.LoadScene(SceneType.LoadingScene.ToString());
    }
}
