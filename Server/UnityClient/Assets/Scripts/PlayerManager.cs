using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Dictionary<int, Player> playerList = new Dictionary<int, Player>();

    public static PlayerManager Instance { get; } = new PlayerManager();

}
