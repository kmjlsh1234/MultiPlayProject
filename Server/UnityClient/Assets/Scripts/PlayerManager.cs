using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonBase<PlayerManager>
{
    public Dictionary<int, Player> playerList = new Dictionary<int, Player>();
    

    public void GeneratePlayer()
    {
        foreach (KeyValuePair<int, Player> pair in playerList)
        {
            GameObject go = ResourcesManager.Instance.getPrefabObj("Player");
            if (go != null)
            {
                GameObject player = Instantiate(go, Vector3.zero, Quaternion.identity);
                if(pair.Key == NetworkManager.Instance.sessionId)
                {
                    player.AddComponent<MyPlayer>();
                }
                else
                {
                    player.AddComponent<Player>();
                }
            }
            else
            {
                Debug.LogError("Player Prefab is Null");
            }
        }
    }
}
