using NUnit.Framework;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RoomManager : SingletonBase<RoomManager>
{
    public Dictionary<int, Room> roomDic = new Dictionary<int, Room>();
    public Action<Dictionary<int, Room>> OnRoomListRecvCompleted;

    public void OnRoomListRecv(Dictionary<int, Room> dic)
    {
        this.roomDic = dic;
        OnRoomListRecvCompleted.Invoke(dic);
    }
}
