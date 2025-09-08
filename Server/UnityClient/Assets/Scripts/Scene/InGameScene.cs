using Google.Protobuf.Protocol;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : MonoBehaviour
{
    public void Awake()
    {
        UIManager.Instance.Push(UIType.UIPopup_InGame);
        
        C_Loadingcomplete packet = new C_Loadingcomplete();
        NetworkManager.Instance.Send(packet);
    }
}
