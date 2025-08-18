using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DataManager : SingletonBase<DataManager>
{
    private Dictionary<int, RoomData> roomDic = new Dictionary<int, RoomData>();

    public Action<Dictionary<int, RoomData>> RoomListRecvHandler;

    public override void Init()
    {
        
    }

    public void OnRoomListRecvCompleted(Dictionary<int, RoomData> dic)
    {
        roomDic.Clear();
        roomDic = dic;

        RoomListRecvHandler.Invoke(roomDic);
    }
}
