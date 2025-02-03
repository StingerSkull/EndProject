using Unity.VisualScripting;
using UnityEngine;

public class BossHugeMushroom_Champi : MonoBehaviour
{
    public AnimationClip animClip;
    public float pushForceY = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, animClip.length);
    }

    #region Collider
    private void OnTriggerEnter2D(Collider2D collider)
    {
        HurtPlayer(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        HurtPlayer(collider);
    }

    #endregion

    void HurtPlayer(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GameObject player = collider.transform.parent.gameObject;
            if (!player.GetComponent<PlayerDamage>().InHurtCoolDown())
            {
                Debug.Log("aie");
                player.GetComponent<PlayerDamage>().PlayerEnemyDmg(1);
                player.GetComponent<Movement2D>().currentVerticalSpeed = pushForceY;
            }
        }
    }
}
