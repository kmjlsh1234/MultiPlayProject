using ServerCore;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    UIPopup_Lobby,
    UIPopup_Chat,
    UIPopup_Error,
}

public class UIManager : SingletonBase<UIManager>
{
    Dictionary<UIType, GameObject> uiDic = new Dictionary<UIType, GameObject>();
    Stack<GameObject> uiStack = new Stack<GameObject>();
    int canvasOrder = 0;
    string uiPath = "UIPopup/"; 

    public override void Init()
    {
        GameObject[] array = Resources.LoadAll<GameObject>(uiPath);

        foreach (GameObject go in array)
        {
            UIBase ui = go.GetComponent<UIBase>();
            uiDic.Add(ui.uiType, go);
        }
    }

    public void Push(UIType type, IPacket pkt = null)
    {
        GameObject go = null;

        if (type == UIType.UIPopup_Error)
        {
            S_ErrorCode packet = pkt as S_ErrorCode;
            if (uiDic.TryGetValue(type, out go))
            {
                go.name = go.name.Replace("(Clone)", "");
                GameObject ui = Instantiate(go, Vector3.zero, Quaternion.identity, transform);
                ui.GetComponentInChildren<Canvas>().sortingOrder = canvasOrder++;
                UIBase uiBase = ui.GetComponent<UIBase>();
                uiStack.Push(ui);
                ErrorPopup errorPopup = uiBase as ErrorPopup;
                errorPopup.Init(packet);
            }
        }
        else
        {
            if (uiDic.TryGetValue(type, out go))
            {
                go.name = go.name.Replace("(Clone)", "");
                GameObject ui = Instantiate(go, Vector3.zero, Quaternion.identity, transform);
                ui.GetComponentInChildren<Canvas>().sortingOrder = canvasOrder++;
                UIBase uiBase = ui.GetComponent<UIBase>();
                uiStack.Push(ui);
                uiBase.Init();
            }
        }


        
        
        //예외 처리
    }

    public void Clear()
    {
        while(uiStack.Count > 0)
        {
            GameObject go = uiStack.Pop();
            canvasOrder--;
            Destroy(go);
        }
    }
    public void Pop()
    {
        Debug.Log("Pop");
        GameObject go = uiStack.Pop();
        canvasOrder--;
        Destroy(go);
    }
}
