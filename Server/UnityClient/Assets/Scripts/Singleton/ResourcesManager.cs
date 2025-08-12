using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourcesManager : SingletonBase<ResourcesManager>
{
    Dictionary<string, GameObject> uiObjDic = new Dictionary<string, GameObject>();
    public override void Init()
    {
        LoadUI();

    }

    public void LoadUI()
    {
        GameObject[] array = Resources.LoadAll<GameObject>("UI/"); 
        foreach (GameObject go in array)
        {
            uiObjDic.Add(go.name, go);
        }
    }

    public GameObject getUIObj(string name)
    {
        GameObject obj = null;
        if(uiObjDic.TryGetValue(name, out obj))
        {
            return obj;
        }
        return null;
    }
}
