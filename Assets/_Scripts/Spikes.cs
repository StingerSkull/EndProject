using Unity.Cinemachine;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject player = collision.transform.parent.gameObject;
            if (!player.GetComponent<PlayerDamage>().InHurtCoolDown())
            {
               player.GetComponent<PlayerDamage>().DealDmg(1);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject player = collision.transform.parent.gameObject;
            if (!player.GetComponent<PlayerDamage>().InHurtCoolDown())
            {
                player.GetComponent<PlayerDamage>().DealDmg(1);
            }
        }
    }
}
