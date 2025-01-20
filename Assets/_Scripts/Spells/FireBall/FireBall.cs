using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public Rigidbody2D rb2;
    public CapsuleCollider2D capsuleCollider;
    public SpriteRenderer spriteRenderer;
    public float projectileSpeed;
    public float timeDespawn;
    public GameObject explosion;
    public AnimationClip explosionClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2.linearVelocity = transform.right * projectileSpeed;
        StartCoroutine(TimeBeforeExplose());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.transform.parent.gameObject);
            GameObject newEffectEnemy = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(newEffectEnemy, explosionClip.length);
            Destroy(gameObject);
        }
        else
        {
            GameObject newEffect = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(newEffect, explosionClip.length);
            Destroy(gameObject);
        }

    }

    public IEnumerator TimeBeforeExplose()
    {
        yield return new WaitForSeconds(timeDespawn);
        if (!gameObject.IsDestroyed())
        {
            GameObject newEffect = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(newEffect, explosionClip.length);
            Destroy(gameObject);
        }
    }

}
