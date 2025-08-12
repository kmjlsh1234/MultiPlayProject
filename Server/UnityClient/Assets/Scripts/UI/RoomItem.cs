using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;
    
    public void Init(string name)
    {
        roomName.text = name;
    }
}
