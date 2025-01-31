using UnityEngine;
using UnityHFSM;

public class BossHugeMushroom : MonoBehaviour
{
    public StateMachine<BossHugeMushroomStates> fsm;
    public enum BossHugeMushroomStates
    {
        IDLE, MOVE, HOME, MELEEATTACK, MUSHROOMATTACK, SPAWN, DEAD
    }

    public Animator animator;
    public Transform spriteTransform;
    public Rigidbody2D rb2;

    public Transform mushSpawner;
    public GameObject prefabMushrooms;

    public GameObject player;
    public float detectRange = 10f;
    public float attackRange = 4f;

    public float pauseTimer = 2f;
    public float pauseChrono = 0f;

    public int rdmSkill = 0;

    [Space]
    //[Header("SPEED VALUES")]
    [Range(1, 10)]
    public float movementSpeed = 5f;
    public float currentMovementSpeed = 5f;
    public float velocity = 0f;

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
        facingRight = false;

        fsm = new StateMachine<BossHugeMushroomStates>();

        #region States

        fsm.AddState(BossHugeMushroomStates.IDLE, new BossHugeMushroomIdleState<BossHugeMushroomStates>(this));
        fsm.AddState(BossHugeMushroomStates.MOVE, new BossHugeMushroomMoveState<BossHugeMushroomStates>(this));
        fsm.AddState(BossHugeMushroomStates.HOME, new BossHugeMushroomHomeState<BossHugeMushroomStates>(this));
        fsm.AddState(BossHugeMushroomStates.MELEEATTACK, new BossHugeMushroomMeleeAttackState<BossHugeMushroomStates>(this));
        fsm.AddState(BossHugeMushroomStates.MUSHROOMATTACK, new BossHugeMushroomMushroomAttackState<BossHugeMushroomStates>(this));
        fsm.AddState(BossHugeMushroomStates.SPAWN, new BossHugeMushroomSpawnState<BossHugeMushroomStates>(this));
        fsm.AddState(BossHugeMushroomStates.DEAD, new BossHugeMushroomDeadState<BossHugeMushroomStates>(this));
        #endregion

        fsm.SetStartState(BossHugeMushroomStates.IDLE);
        
        #region StateTransition
        #region IDLE
        fsm.AddTransition(BossHugeMushroomStates.IDLE, BossHugeMushroomStates.MOVE,
            transition => Vector2.Distance(transform.position, player.transform.position) < detectRange
            && Vector2.Distance(transform.position, player.transform.position) > attackRange
            && pauseTimer <=0);
        fsm.AddTransition(BossHugeMushroomStates.IDLE, BossHugeMushroomStates.MELEEATTACK,
            transition => Vector2.Distance(transform.position, player.transform.position) < attackRange
            && rdmSkill == 0
            && pauseTimer <= 0);
        fsm.AddTransition(BossHugeMushroomStates.IDLE, BossHugeMushroomStates.MUSHROOMATTACK,
            transition => Vector2.Distance(transform.position, player.transform.position) < attackRange
            && rdmSkill == 1
            && pauseTimer <= 0);
        fsm.AddTransition(BossHugeMushroomStates.IDLE, BossHugeMushroomStates.SPAWN,
            transition => Vector2.Distance(transform.position, player.transform.position) < attackRange
            && rdmSkill == 2
            && pauseTimer <= 0);
        #endregion

        #region MOVE
        fsm.AddTransition(BossHugeMushroomStates.MOVE, BossHugeMushroomStates.IDLE,
            transition => Vector2.Distance(transform.position, player.transform.position) > detectRange);
        fsm.AddTransition(BossHugeMushroomStates.MOVE, BossHugeMushroomStates.MELEEATTACK,
            transition => false);
        fsm.AddTransition(BossHugeMushroomStates.MOVE, BossHugeMushroomStates.MUSHROOMATTACK,
            transition => false);
        fsm.AddTransition(BossHugeMushroomStates.MOVE, BossHugeMushroomStates.SPAWN,
            transition => false);
        #endregion

        #region MELEEATTACK
        fsm.AddTransition(BossHugeMushroomStates.MELEEATTACK, BossHugeMushroomStates.IDLE,
            transition => false);
        fsm.AddTransition(BossHugeMushroomStates.MELEEATTACK, BossHugeMushroomStates.MOVE,
            transition => false);

        #endregion

        #region MUSHROOMATTACK
        fsm.AddTransition(BossHugeMushroomStates.MUSHROOMATTACK, BossHugeMushroomStates.IDLE,
            transition => false);
        fsm.AddTransition(BossHugeMushroomStates.MUSHROOMATTACK, BossHugeMushroomStates.MOVE,
            transition => false);

        #endregion
        #region SPAWN
        fsm.AddTransition(BossHugeMushroomStates.SPAWN, BossHugeMushroomStates.IDLE,
            transition => false);
        fsm.AddTransition(BossHugeMushroomStates.SPAWN, BossHugeMushroomStates.MOVE,
            transition => false);

        #endregion

        #region ALL
        fsm.AddTransitionFromAny(BossHugeMushroomStates.DEAD,
            transition => false);

        #endregion
        #endregion
        
        fsm.Init();
    }

    // Update is called once per frame
    void Update()
    {
        CheckLedgeAndWall();
        fsm.OnLogic();
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
        if (facingRight && isLedge)
        {
            velocity = -movementSpeed;
        }
        else if (facingRight && !isLedge)
        {
            velocity = movementSpeed;
        }
        else if (!facingRight && isLedge)
        {
            velocity = movementSpeed;
        }
        else if (!facingRight && !isLedge)
        {
            velocity = -movementSpeed;
        }
    }

    void Flip()
    {
        Vector3 _enemyRot = transform.localEulerAngles;

        if (velocity > 0)
        {
            facingRight = false;
            _enemyRot.y = 180f;
            transform.localEulerAngles = _enemyRot;
        }
        else if (velocity < 0)
        {
            facingRight = true;
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