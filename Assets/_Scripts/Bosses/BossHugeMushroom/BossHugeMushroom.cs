using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityHFSM;

public class BossHugeMushroom : MonoBehaviour
{
    public StateMachine<BossHugeMushroomStates> fsm;
    public enum BossHugeMushroomStates
    {
        IDLE, MOVE, HOME, MELEEATTACK, MUSHROOMATTACK, SPAWN, DEAD
    }

    [Header("Boss Components")]
    public Animator animator;
    public Transform spriteTransform;
    public Rigidbody2D rb2;

    [Header("Boss stats")]
    public int maxLife = 5;
    public int currentLife;
    [Range(1, 10)]
    public float movementSpeed = 2.5f;
    public float currentMovementSpeed = 0f;
    public float velocity = 0f;
    public float pushForceX = 1f;
    public float pushForceY = 0.5f;
    public bool canFlip;
    public bool dead;

    [Header("Big Mushroom Spawn")]
    public Transform bigSpawner;
    public GameObject prefabBig;
    public bool animSpawn;
    public bool endAnimSpawn;

    [Header("Mushroom Atttack")]
    public Transform mushSpawner;
    public GameObject prefabMushrooms;
    public bool animMushroomAttack;
    public bool endAnimMushroomAttack;
    public int numMushrooms = 10;
    public float distanceBetweenMush = 0.7f;
    public float offsetRayCheckY = 0.5f;
    public LayerMask mushCheckLayer;
    public float timeBetweenMush = 0.1f;

    [Header("Player detection")]
    public GameObject player;
    public float detectRange = 10f;
    public float attackRange = 4f;

    [Header("States info")]
    public Transform home;
    public float pauseTimer = 2f;
    public float pauseChrono = 0f;
    public int rdmSkill = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        rb2 = GetComponent<Rigidbody2D>();
        canFlip = true;
        currentLife = maxLife;
        fsm = new StateMachine<BossHugeMushroomStates>();
        home = transform;
        rdmSkill = 1;
        animMushroomAttack = false;
        endAnimMushroomAttack= false;


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
            transition => Vector2.Distance(transform.position, player.transform.position) <= detectRange
            && Vector2.Distance(transform.position, player.transform.position) > attackRange
            && pauseChrono <=0);
        fsm.AddTransition(BossHugeMushroomStates.IDLE, BossHugeMushroomStates.HOME,
            transition => Vector2.Distance(transform.position, player.transform.position) > detectRange
            && Vector2.Distance(transform.position, home.position) > 0.1f
            && pauseChrono <=0);
        fsm.AddTransition(BossHugeMushroomStates.IDLE, BossHugeMushroomStates.MELEEATTACK,
            transition => Vector2.Distance(transform.position, player.transform.position) <= attackRange
            && rdmSkill == 0
            && pauseChrono <= 0);
        fsm.AddTransition(BossHugeMushroomStates.IDLE, BossHugeMushroomStates.MUSHROOMATTACK,
            transition => Vector2.Distance(transform.position, player.transform.position) <= attackRange
            && rdmSkill == 1
            && pauseChrono <= 0);
        fsm.AddTransition(BossHugeMushroomStates.IDLE, BossHugeMushroomStates.SPAWN,
            transition => Vector2.Distance(transform.position, player.transform.position) <= attackRange
            && rdmSkill == 2
            && pauseChrono <= 0);
        #endregion

        #region MOVE
        fsm.AddTransition(BossHugeMushroomStates.MOVE, BossHugeMushroomStates.IDLE,
            transition => Vector2.Distance(transform.position, player.transform.position) > detectRange);
        fsm.AddTransition(BossHugeMushroomStates.MOVE, BossHugeMushroomStates.MELEEATTACK,
            transition => Vector2.Distance(transform.position, player.transform.position) <= attackRange
            && rdmSkill == 0);
        fsm.AddTransition(BossHugeMushroomStates.MOVE, BossHugeMushroomStates.MUSHROOMATTACK,
            transition => Vector2.Distance(transform.position, player.transform.position) <= attackRange
            && rdmSkill == 1);
        fsm.AddTransition(BossHugeMushroomStates.MOVE, BossHugeMushroomStates.SPAWN,
            transition => Vector2.Distance(transform.position, player.transform.position) <= attackRange
            && rdmSkill == 2);
        #endregion

        #region MELEEATTACK
        fsm.AddTransition(BossHugeMushroomStates.MELEEATTACK, BossHugeMushroomStates.IDLE,
            transition => false);
        fsm.AddTransition(BossHugeMushroomStates.MELEEATTACK, BossHugeMushroomStates.MOVE,
            transition => false);

        #endregion

        #region MUSHROOMATTACK
        fsm.AddTransition(BossHugeMushroomStates.MUSHROOMATTACK, BossHugeMushroomStates.IDLE,
            transition => endAnimMushroomAttack
            && (Vector2.Distance(transform.position, player.transform.position) > detectRange
            || Vector2.Distance(transform.position, player.transform.position) <= attackRange));
        fsm.AddTransition(BossHugeMushroomStates.MUSHROOMATTACK, BossHugeMushroomStates.MOVE,
            transition => endAnimMushroomAttack
            && Vector2.Distance(transform.position, player.transform.position) <= detectRange
            && Vector2.Distance(transform.position, player.transform.position) > attackRange
            && pauseChrono <=0 );

        #endregion

        #region SPAWN
        fsm.AddTransition(BossHugeMushroomStates.SPAWN, BossHugeMushroomStates.IDLE,
            transition => endAnimSpawn
            && (Vector2.Distance(transform.position, player.transform.position) > detectRange
            || Vector2.Distance(transform.position, player.transform.position) <= attackRange));
        fsm.AddTransition(BossHugeMushroomStates.SPAWN, BossHugeMushroomStates.MOVE,
            transition => endAnimSpawn
            && Vector2.Distance(transform.position, player.transform.position) <= detectRange
            && Vector2.Distance(transform.position, player.transform.position) > attackRange
            && pauseChrono <= 0);

        #endregion

        #region HOME
        fsm.AddTransition(BossHugeMushroomStates.HOME, BossHugeMushroomStates.IDLE,
            transition => Vector2.Distance(transform.position, home.position) <= 0.1f);
        #endregion

        #region ALL
        fsm.AddTransitionFromAny(BossHugeMushroomStates.DEAD,
            transition => dead);

        #endregion
        #endregion
        
        fsm.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseChrono > 0f)
        {
            pauseChrono -= Time.deltaTime;
        }
        Debug.Log(fsm.ActiveStateName);
        fsm.OnLogic();
    }

    private void FixedUpdate()
    {
        UpdatePlatformerSpeed();
        Move();
    }

    #region Move
    void UpdatePlatformerSpeed()
    {
        Vector2 angleVector = (player.transform.position - transform.position);
        velocity = angleVector.normalized.x * currentMovementSpeed;


        if (canFlip)
        {
            Vector3 _enemyRot = transform.localEulerAngles;
            if (angleVector.normalized.x > 0)
            {
                _enemyRot.y = 180f;
                transform.localEulerAngles = _enemyRot;
            }
            else if (angleVector.normalized.x < 0)
            {
                _enemyRot.y = 0f;
                transform.localEulerAngles = _enemyRot;
            }
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
                player.GetComponent<PlayerDamage>().PlayerEnemyDmg(1);
                player.GetComponent<Movement2D>().currentHorizontalSpeed = -collision.GetContact(0).normal.x * pushForceX;
                player.GetComponent<Movement2D>().currentVerticalSpeed = -collision.GetContact(0).normal.y * pushForceY;
            }
        }
    }

    public void CoroutineMushrooms(Vector2 spawnerPos, float rotation)
    {
        StartCoroutine(CreateMushrooms(spawnerPos, rotation));
    }

    public IEnumerator CreateMushrooms(Vector2 spawnerPos, float rotation)
    {
        int maxNumMushrooms = CheckMaxNumberMushrooms();
        for (int i = 0; i < maxNumMushrooms; i++)
        {
            Instantiate(prefabMushrooms, new Vector2(spawnerPos.x - rotation * i * distanceBetweenMush, spawnerPos.y), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenMush);
        }
    }

    public int CheckMaxNumberMushrooms()
    {
        Vector2 rayOrigin = new(transform.position.x, transform.position.y - offsetRayCheckY);
        RaycastHit2D _hit = Physics2D.Raycast(rayOrigin, -transform.right, numMushrooms * distanceBetweenMush, mushCheckLayer);

        return _hit ? (int)(Mathf.Abs(_hit.point.x - transform.position.x) / distanceBetweenMush) : numMushrooms;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.magenta;
        Vector2 rayOrigin = new(transform.position.x, transform.position.y - offsetRayCheckY);
        Gizmos.DrawRay(rayOrigin, -transform.right * numMushrooms * distanceBetweenMush);
    }
}