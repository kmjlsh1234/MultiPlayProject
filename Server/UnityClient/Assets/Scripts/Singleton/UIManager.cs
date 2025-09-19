using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum UIType
{
    UIPopup_Lobby,
    UIPopup_Match,
    UIPopup_Error,
    UIPopup_Invite,
    UIPopup_InGame,
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

    public void Push(UIType type, IMessage pkt = null)
    {
        if (IsShow(type))
        {
            return;
        }

        GameObject go = null;

        if (type == UIType.UIPopup_Error)
        {
            S_Errorcode packet = pkt as S_Errorcode;
            if (uiDic.TryGetValue(type, out go))
            {
                GameObject ui = Instantiate(go, Vector3.zero, Quaternion.identity, transform);
                ui.GetComponentInChildren<Canvas>().sortingOrder = canvasOrder++;
                ErrorPopup errorPopup = ui.GetComponent<ErrorPopup>();
                uiStack.Push(ui);
                errorPopup.Init(packet);
                ui.name = go.name.Replace("(Clone)", "");
            }
        }
        else if(type == UIType.UIPopup_Invite)
        {
            S_Invite packet = pkt as S_Invite;
            if (uiDic.TryGetValue(type, out go))
            {
                GameObject ui = Instantiate(go, Vector3.zero, Quaternion.identity, transform);
                ui.GetComponentInChildren<Canvas>().sortingOrder = canvasOrder++;
                InvitePopup invitePopup = ui.GetComponent<InvitePopup>();
                uiStack.Push(ui);
                invitePopup.Init(packet);
                ui.name = go.name.Replace("(Clone)", "");
            }
        }
        else if(type == UIType.UIPopup_Match)
        {
            S_Matchroominfo packet = pkt as S_Matchroominfo;
            if (uiDic.TryGetValue(type, out go))
            {
                
                GameObject ui = Instantiate(go, Vector3.zero, Quaternion.identity, transform);
                ui.GetComponentInChildren<Canvas>().sortingOrder = canvasOrder++;
                MatchRoomPopup popup = ui.GetComponent<MatchRoomPopup>();
                uiStack.Push(ui);
                popup.Init(packet);
                ui.name = go.name.Replace("(Clone)", "");
            }
        }
        else
        {
            if (uiDic.TryGetValue(type, out go))
            {
                GameObject ui = Instantiate(go, Vector3.zero, Quaternion.identity, transform);
                ui.GetComponentInChildren<Canvas>().sortingOrder = canvasOrder++;
                UIBase uiBase = ui.GetComponent<UIBase>();
                uiStack.Push(ui);
                uiBase.Init();
                //ui.name = go.name.Replace("(Clone)", "");
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
        Debug.Log("Destroying: " + go.name);
        Destroy(go);
    }

    public bool IsShow(UIType type)
    {
        foreach(GameObject go in uiStack)
        {
            UIBase uiBase = go.GetComponent<UIBase>();
            if(uiBase.uiType == type)
            {
                return true;
            }
        }

        return false;
    }
}
