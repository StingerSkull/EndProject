using UnityEngine;

public class FireBall : MonoBehaviour
{
    public Rigidbody2D rb2;
    public CapsuleCollider2D capsuleCollider;
    public SpriteRenderer spriteRenderer;
    public float projectileSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2.linearVelocity = transform.right * projectileSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
