using UnityEngine;

public class TeleportOnFall : MonoBehaviour
{
    [SerializeField] Transform teleportPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent.position = teleportPos.position;
        }
    }
}
