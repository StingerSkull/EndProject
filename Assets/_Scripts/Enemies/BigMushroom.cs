using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

public class BigMushroom : MonoBehaviour
{
    public Animator animator;
    public Transform spriteTransform;
    public Rigidbody2D rb2;

    [Space]
    //[Header("SPEED VALUES")]
    [Range(1, 10)]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float velocity = 0f;
    public int maxLife = 5;
    public int currentLife;

    public UnityEvent enemyHurt;
    public UnityEvent enemyDeath;

    [SerializeField] Vector2 ledgeCheckOffset = new(1f, 1f);
    [SerializeField] float ledgeCheckDistance = 1f;
    [SerializeField] LayerMask ledgeCheckLayer;
    public bool isLedge;

    public bool facingRight;

    public float pushForceX = 1f;
    public float pushForceY = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        facingRight = transform.right.x < 0;
        currentLife = maxLife;
        isLedge = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckLedgeAndWall();
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
        if (!facingRight && isLedge)
        {
            velocity = -movementSpeed;
        }
        else if (!facingRight && !isLedge)
        {
            velocity = movementSpeed;
        }
        else if (facingRight && isLedge)
        {
            velocity = movementSpeed;
        }
        else if(facingRight && !isLedge)
        {
            velocity = -movementSpeed;
        }
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

    #region Ledge
    void CheckLedgeAndWall()
    {
        //CHECH THE LEDGE
        Vector2 _ledgeCheckOrigin = (Vector2)transform.position - (Vector2)transform.right * ledgeCheckOffset.x + (Vector2)transform.up * ledgeCheckOffset.y;
        RaycastHit2D _hit = Physics2D.Raycast(_ledgeCheckOrigin, Vector2.down, ledgeCheckDistance, ledgeCheckLayer);

        isLedge = _hit;

    }
    #endregion

    #region Collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            HurtPlayer(collision);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            HurtPlayer(collision);
        }
    }

    #endregion

    void HurtPlayer(Collision2D collision)
    {
        GameObject player = collision.gameObject;
        if (!player.GetComponent<PlayerDamage>().InHurtCoolDown())
        {
            player.GetComponent<PlayerDamage>().PlayerEnemyDmg(1);
            player.GetComponent<Movement2D>().currentHorizontalSpeed = -collision.GetContact(0).normal.x * pushForceX;
            player.GetComponent<Movement2D>().currentVerticalSpeed = -collision.GetContact(0).normal.y * pushForceY;
        }
    }

    public void EnemyDmg(int dmg)
    {
            currentLife -= dmg;
            if (currentLife > 0)
            {
                animator.SetTrigger("Hurt");
                enemyHurt.Invoke();
            }
            else
            {
                enemyDeath.Invoke();
            }

    }

    private void OnDrawGizmos()
    {

        if (isLedge)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Vector2 _ledgeCheckOrigin = (Vector2)transform.position - (Vector2)transform.right * ledgeCheckOffset.x + (Vector2)transform.up * ledgeCheckOffset.y;


        Gizmos.DrawRay(_ledgeCheckOrigin, Vector2.down * ledgeCheckDistance);


    }
}
