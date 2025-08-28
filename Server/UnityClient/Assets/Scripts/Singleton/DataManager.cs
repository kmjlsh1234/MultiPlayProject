using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DataManager : SingletonBase<DataManager>
{
    public Action<Dictionary<int, S_Roominfo>> RoomListRecvHandler;

    public Dictionary<int, S_Roominfo> rooms = new Dictionary<int, S_Roominfo>();

    public override void Init()
    {
        
    }

    public void OnRoomListRecvCompleted(S_Roomlist list)
    {
        rooms.Clear();
        foreach(S_Roominfo roomInfo in list.RoomList)
        {
            rooms.Add(roomInfo.RoomId, roomInfo);
        }

        RoomListRecvHandler.Invoke(rooms);
    }
}
