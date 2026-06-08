using UnityEngine;

public class SandstormFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float yOffset = 10f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    void LateUpdate()
    {
        if (player == null) return;

        transform.position = new Vector3(
            player.position.x,
            player.position.y + yOffset,
            player.position.z
        );
    }
}