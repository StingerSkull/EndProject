using UnityEngine;

public class BossHugeMushroom_Champi : MonoBehaviour
{
    public float pushForceX = 1f;
    public float pushForceY = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    #region Collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HurtPlayer(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HurtPlayer(collision);
    }

    #endregion

    void HurtPlayer(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            if (!player.GetComponent<PlayerDamage>().InHurtCoolDown())
            {
                player.GetComponent<PlayerDamage>().PlayerEnemyDmg(1);
                player.GetComponent<Movement2D>().currentHorizontalSpeed = -collision.GetContact(0).normal.x * pushForceX;
                player.GetComponent<Movement2D>().currentVerticalSpeed = -collision.GetContact(0).normal.y * pushForceY;
            }
        }
    }
}
