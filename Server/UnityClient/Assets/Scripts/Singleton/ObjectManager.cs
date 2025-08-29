using Google.Protobuf.Protocol;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SingletonBase<ObjectManager>
{
    Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> enemys = new Dictionary<int, GameObject>();
    public MyPlayerController myPlayer { get; set; }

    public void Add(int id, GameObject go)
    {

    }

    public void Add(Playerinfo info, bool myPlayer)
    {
        if (myPlayer)
        {
            
        }
        else
        {

        }
    }
}
