using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public Rigidbody2D rb2;
    public float projectileSpeed;
    public float timeDespawn;
    public GameObject prefabExplosion;
    public AnimationClip explosionClip;
    public SoundData explosionSounds;
    private float maxSoundLength = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2.linearVelocity = transform.right * projectileSpeed;
        StartCoroutine(TimeBeforeExplose());
        maxSoundLength = 0f;
        foreach(AudioClip audioClip in explosionSounds.sounds)
        {
            maxSoundLength = Mathf.Max(maxSoundLength, audioClip.length);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.transform.parent.gameObject);
            GameObject explosionEffectEnemy = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
            Destroy(explosionEffectEnemy, Mathf.Max(maxSoundLength, explosionClip.length));
            Destroy(gameObject);
        }
        else
        {
            GameObject explosionEffect = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
            Destroy(explosionEffect, Mathf.Max(maxSoundLength, explosionClip.length));
            Destroy(gameObject);
        }

    }

    public IEnumerator TimeBeforeExplose()
    {
        yield return new WaitForSeconds(timeDespawn);
        if (!gameObject.IsDestroyed())
        {
            GameObject newEffect = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
            Destroy(newEffect, Mathf.Max(maxSoundLength, explosionClip.length));
            Destroy(gameObject);
        }
    }

}
