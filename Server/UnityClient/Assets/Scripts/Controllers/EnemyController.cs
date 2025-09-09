using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int objectId { get; set; }
    public PlayerController targetPlayer; // 현재 따라가는 대상
    private Rigidbody rigid;
}
