using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : MonoBehaviour
{
    public void Awake()
    {
        PlayerManager.Instance.GeneratePlayer();
    }

    
}
