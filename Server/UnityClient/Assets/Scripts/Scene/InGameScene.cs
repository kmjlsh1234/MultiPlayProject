using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : MonoBehaviour
{
    public void Awake()
    {
        UIManager.Instance.Push(UIType.UIPopup_InGame);
        PlayerManager.Instance.GeneratePlayer();

        
    }
}
