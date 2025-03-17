using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

public class GoblinMelee : MonoBehaviour
{
    public Animator animator;
    public Transform spriteTransform;
    public Rigidbody2D rb2;

    [Space]
    //[Header("SPEED VALUES")]
    [Range(1, 10)]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float velocity = 0f;

    [Header("Player detection")]
    public GameObject player;
    public LayerMask collisionLayer;
    public float detectRange = 4f;

    public bool facingRight;

    public float pushForceX = 1f;
    public float pushForceY = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdatePlatformerSpeed();
        Move();
        Flip();
    }

    #region Move
    void UpdatePlatformerSpeed()
    {
        if (CheckDetectRange())
        {
            Vector2 angleVector = (player.transform.position - transform.position);

            velocity = angleVector.normalized.x * movementSpeed;
        }
        else
        {
            velocity = 0;
        }
    }

    void ResetMovement()
    {
        velocity = 0;
    }

    void Flip()
    {
        Vector3 _enemyRot = transform.localEulerAngles;

        if (velocity > 0)
        {
            facingRight = true;
            _enemyRot.y = 180f;
            transform.localEulerAngles = _enemyRot;
        }
        else if (velocity < 0)
        {
            facingRight = false;
            _enemyRot.y = 0f;
            transform.localEulerAngles = _enemyRot;
        }
    }

    void Move()
    {
        animator.SetFloat("MovementSpeed", Mathf.Abs(rb2.linearVelocityX));
        rb2.linearVelocityX = velocity;
    }
    #endregion


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
                player.GetComponent<PlayerDamage>().DealDmg(1);
                player.GetComponent<Movement2D>().currentHorizontalSpeed = -collision.GetContact(0).normal.x * pushForceX;
                player.GetComponent<Movement2D>().currentVerticalSpeed = -collision.GetContact(0).normal.y * pushForceY;
            }
        }
    }

    public bool CheckDetectRange()
    {
        bool detected = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, detectRange, collisionLayer);
        if (hit)
        {
            detected = hit.transform.CompareTag("Player");
        }
        return detected;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        if (player != null)
        {
            Gizmos.color = Color.red;
            if (CheckDetectRange())
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawRay(transform.position, (player.transform.position - transform.position).normalized * detectRange);

        }
    }
}
