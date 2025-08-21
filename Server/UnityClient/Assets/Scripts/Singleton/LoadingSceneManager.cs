using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    ChatScene,
    Playground,
    LoadingScene,
    LobbyScene,
    InGameScene,
}

public class LoadingSceneManager : SingletonBase<LoadingSceneManager>
{
    public SceneType sceneType;
    public Action OnLoadingCompleted;
    public void LoadScene(SceneType type)
    {
        this.sceneType = type;
        SceneManager.LoadScene(SceneType.LoadingScene.ToString());
    }
}
