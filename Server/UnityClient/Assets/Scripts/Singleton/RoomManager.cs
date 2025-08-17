using NUnit.Framework;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RoomManager : SingletonBase<RoomManager>
{
    public Dictionary<int, RoomData> roomDic = new Dictionary<int, RoomData>();
    public Action<Dictionary<int, RoomData>> OnRoomListRecvCompleted;

    public void OnRoomListRecv(Dictionary<int, RoomData> dic)
    {
        this.roomDic = dic;
        OnRoomListRecvCompleted?.Invoke(dic);
    }
}
