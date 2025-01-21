using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement2D : MonoBehaviour
{
    public GUIStyle mainHeaderStyle = new();
    Animator animator;
    Casting casting;
    [SerializeField] Transform spriteTransform;
    [SerializeField] Rigidbody2D rb2;
    [SerializeField] CapsuleCollider2D capsuleCollider;
    [SerializeField] AnimationClip climbAnim;
    [SerializeField] GameObject runDust;
    [SerializeField] GameObject jumpDustPrefab;
    Vector2 input;
    [HideInInspector] public float currentHorizontalSpeed;
    [HideInInspector] public float currentVerticalSpeed;
    [HideInInspector] public PlayerStates currentState;

    [Space]
    //[Header("SPEED VALUES")]
    [Range(1, 10)]
    [SerializeField] float movementSpeed = 5f;
    [Range(0.02f, 1)]
    [SerializeField] float speedUpDuration = 0.1f;
    [Range(0.02f, 1)]
    [SerializeField] float speedDownDuration = 0.06f;
    [Range(0.02f, 1)]
    [SerializeField] float stopDuration = 0.15f;

    //[Header("DASH")]
    [SerializeField] bool Dash;
    [SerializeField] KeyCode dashButton = KeyCode.LeftShift;
    [SerializeField] bool cancelDashOnWallHit;
    //[Header("-Dash Values")]
    [Range(1f, 10f)]
    [SerializeField] float dashDistance = 3f;
    [Range(0.1f, 10)]
    [SerializeField] float dashDuration = 0.3f;
    [Range(0f, 1)]
    [SerializeField] float dashStopEffect = 0.5f;

    //[Header("-Dash Settings")]
    [SerializeField] bool resetDashOnGround;
    [SerializeField] bool resetDashOnWall;
    [SerializeField] bool airDash;
    //[Header("-Dash Values")]
    [Range(1f, 10f)]
    [SerializeField] float airDashDistance = 3f;
    [Range(0.1f, 10)]
    [SerializeField] float airDashDuration = 0.3f;
    [Range(0f, 1)]
    [SerializeField] float airDashStopEffect = 0.5f;

    [SerializeField] bool dashCancelsGravity;
    [SerializeField] bool verticalDash;
    [SerializeField] bool horizontalDash;
    [SerializeField] Vector2 dashColliderScale;
    [SerializeField] Vector2 dashColliderOffset;
    [Space]
    [SerializeField] float dashCooldown = 0.5f;
    [SerializeField] float dashCoolTimer;
    [SerializeField] float dashToleranceTimer;

    [SerializeField] bool canDash;
    [SerializeField] bool isAirDashing = false;
    Vector2 defaultColliderOffset;
    Vector2 defaulColliderSize;


    //DASH INFO
    [HideInInspector] public bool isDashing;
    float dashSpeed;
    [SerializeField] float dashingTimer;

    [Space]
    //[Header("WALL JUMP")]
    public bool WallJump;
    [SerializeField] Vector2 wallJumpVelocity;
    [Range(0.02f, 1f)]
    [SerializeField] float wallJumpDecelerationFactor = 0.3f;
    [Range(0.01f, 10f)]
    [SerializeField] float wallSlideSpeed = 0.5f;
    [SerializeField] bool isSlidingOnWall;
    [SerializeField] bool variableJumpHeightOnWallJump;

    [SerializeField] float inputDelay = 0.2f;
    [SerializeField] float inputDelayTimer;


    [Space]
    //[Header("----Jumping Values----")]
    [Range(0.5f, 10f)]
    [SerializeField] float jumpHight = 1.5f;
    [Range(0.5f, 50f)]
    [SerializeField] float jumpUpAcceleration = 2.5f;
    [Range(0.5f, 50f)]
    [SerializeField] float jumpDownAcceleration = 4f;
    [Range(0.1f, 50f)]
    [SerializeField] float fallSpeedClamp = 50;
    [Range(1f, 20f)]
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float jumpVelocity;
    [SerializeField] float fallClamp;
    [SerializeField] float jumpUpDuration;
    [SerializeField] bool isNormalJumped;
    [SerializeField] bool isWallJumped;
    [SerializeField] bool doubleJump;
    [SerializeField] int numberJumps;
    [SerializeField] int jumpCounter;

    [Space]
    //[Header("----Jump Adjustments----")]
    [Range(0f, 1f)]
    [SerializeField] float coyoteTime = 0.15f;
    [Range(0f, 1f)]
    [SerializeField] float jumpBuffer = 0.1f;
    [Range(0f, 1f)]
    [SerializeField] float onAirControl = 1f;

    [Range(0f, 1f)]
    [SerializeField] float variableJumpHeightDuration = 0.75f;
    [Range(0f, 1f)]
    [SerializeField] float jumpReleaseEffect = 0.5f;
    [SerializeField] KeyCode jumpButton = KeyCode.Space;
    bool isHoldingJumpButton;
    float jumpHoldTimer;
    bool isForcingJump;

    //[Header("Ledge Climb")]
    [SerializeField] bool LedgeGrab;
    [SerializeField] bool autoClimbLedge;
    [SerializeField] KeyCode climbButton = KeyCode.W;
    [SerializeField] bool canWallJumpWhileClimbing;
    [SerializeField] float ledgeCheckOffset = 1f;
    [SerializeField] float ledgeCheckDistance = 1f;
    [SerializeField] LayerMask ledgeCheckLayer;
    public bool isLedge;
    Vector2 ledgePosition;
    [SerializeField] Vector2 ledgeClimbPosOffset;
    public bool isClimbingLedge;
    [SerializeField] float ledgeClimbDuration;
    [SerializeField] float ledgeClimbTimer;

    [Space]
    //[Header("----GroundCheck----")]
    [SerializeField] Vector2 groundCheckCenter;
    [SerializeField] float groundCheckRayDistance = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckCircleRadius = 0.5f;

    [Space]
    //[Header("----CeilCheck----")]
    [SerializeField] Vector2 ceilCheckCenter;
    [SerializeField] float ceilCheckRayDistance = 0.1f;
    [SerializeField] LayerMask ceilLayer;
    [SerializeField] float ceilCheckCircleRadius = 0.5f;

    [Space]
    //[Header("----WallCheck----")]
    [SerializeField] float wallCheckRayDistance;
    [SerializeField] LayerMask wallCheckLayer;

    [Space]
    //[Header("JUPM DEBUG")]
    [SerializeField] float jumpToleranceTimer;
    [SerializeField] float fallToleranceTimer;
    public bool isGrounded;
    [SerializeField] bool onCeil;
    public bool canJump;

    [Space]

    //[Header("*****DEBUG*****")]
    public bool leftWallHit;
    public bool rightWallHit;
    public bool hitWall;
    public bool isJumped;//to check if the player is on air because of jumping or falling
    public bool isPressedJumpButton;
    public bool isPressedDashButton;

    float onAirControlMultiplier;



    private void Reset()
    {
        rb2 = GetComponent<Rigidbody2D>();
        rb2.freezeRotation = true;
        rb2.gravityScale = 0;
        rb2.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb2.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        if (transform.childCount == 0)
        {
            GameObject _sprite = new();
            _sprite.name = "Sprite";
            _sprite.transform.SetParent(transform);
            _sprite.transform.localPosition = Vector3.zero;
            spriteTransform = _sprite.transform;
            _sprite.AddComponent<SpriteRenderer>();

        }
        GetAutoValueForCeilCheck();
        GetAutoValueForGroundCheck();
    }

    public enum PlayerStates
    {
        None,
        Grounded,
        Jumping,
        Falling
    }

    void SwitchState()
    {
        PlayerStates newState;
        if (isGrounded)
        {
            newState = PlayerStates.Grounded;
        }
        else
        {

            if (currentVerticalSpeed >= 0)
            {
                newState = PlayerStates.Jumping;
            }
            else
            {
                newState = PlayerStates.Falling;
            }
        }
        if (newState != currentState)
        {
            currentState = newState;
        }
    }

    private void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        defaulColliderSize = capsuleCollider.size;
        defaultColliderOffset = capsuleCollider.offset;
        animator = transform.GetChild(0).GetComponent<Animator>();
        casting = GetComponent<Casting>();
        fallClamp = fallSpeedClamp;
        ledgeClimbDuration = climbAnim.length;
        gravity = -Physics2D.gravity.y;
        runDust.SetActive(false);
    }
    private void Update()
    {
        HandlePlatformerMovement();
        CanCast();
    }

    private void FixedUpdate()
    {
        UpdatePlatformerSpeed();
        MovePlayer();
        LedgeClimbCountdown();

    }

    void HandlePlatformerMovement()
    {
        CheckSideWall();
        CheckCeil();
        CheckGround();
        CheckLedge();
        GetPlatformerInput();
        DelayInputOnWall();
        TimerDash();
        FlipThePlayer();
        CountDownJumpTolerance();
        CountDownDashTolerance();
        SwitchState();
        DashCooldownCounter();
        
    }

    #region Input
    void GetPlatformerInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpButton))
        {
            PressJumpButton();

        }
        if (Input.GetKeyUp(jumpButton))
        {
            isHoldingJumpButton = false;
        }
        if (Input.GetKeyDown(dashButton))
        {
            DashPressed();

        }

        DoDash();
        Jump();
    }
    #endregion

    #region Dash
    void TimerDash()
    {
        if (isDashing)
        {
            dashingTimer -= Time.deltaTime;
            if (dashingTimer <= 0 && ( (isGrounded && !onCeil) || !isGrounded) )
            {
                CancelDash();

            }
        }
    }
    void DashCooldownCounter()
    {
        if (dashCoolTimer > 0)
        {
            dashCoolTimer -= Time.deltaTime;
        }
    }

    void CountDownDashTolerance()
    {
        if (isPressedDashButton)
        {
            dashToleranceTimer -= Time.deltaTime;
            if (dashToleranceTimer <= 0)
            {
                isPressedDashButton = false;
            }
        }
    }

    void CancelDash()
    {
        
        if (isDashing)
        {
            isDashing = false;
            
            if (!isAirDashing)
            {
                dashCoolTimer = dashCooldown;
                currentHorizontalSpeed *= dashStopEffect;
                currentVerticalSpeed *= dashStopEffect;
            }
            else
            {
                dashCoolTimer = 0f;
                currentHorizontalSpeed *= airDashStopEffect;
                currentVerticalSpeed *= airDashStopEffect;
            }
            isAirDashing = false;
            capsuleCollider.offset = defaultColliderOffset;
            capsuleCollider.size = defaulColliderSize;
        }
    }

    void DashPressed()
    {

        if (!isPressedDashButton && Dash)
        {
            isPressedDashButton = true;
            dashToleranceTimer = jumpBuffer;
        }
    }

    void DoDash() { 

        if ((canDash && isPressedDashButton) && !isDashing && Dash && ((horizontalDash && input.x != 0) || (verticalDash && input.y != 0)) && dashCoolTimer <= 0f)
        {

            if (!isGrounded)
            {
                canDash = false;
            }
            isAirDashing = !isGrounded && airDash;
            isDashing = true;
            isPressedDashButton = false;

            if (!isAirDashing)
            {
                dashSpeed = (dashDistance) / dashDuration;
                dashingTimer = dashDuration;
            }
            else
            {
                dashSpeed = (airDashDistance) / airDashDuration;
                dashingTimer = airDashDuration;
            }

            Vector2 _dir = Vector2.zero;

            if (horizontalDash && !verticalDash)
            {
                _dir.x = input.x;
                currentHorizontalSpeed = dashSpeed * _dir.x;
                currentVerticalSpeed = 0f;


            }
            else if (verticalDash && !horizontalDash)
            {
                _dir.y = input.y;

                currentHorizontalSpeed = dashSpeed * _dir.x;
                currentVerticalSpeed = dashSpeed * _dir.y;
            }
            else if (horizontalDash && verticalDash)
            {
                _dir = input.normalized;

                currentHorizontalSpeed = dashSpeed * _dir.x;
                currentVerticalSpeed = dashSpeed * _dir.y;
            }

            capsuleCollider.offset = dashColliderOffset;
            capsuleCollider.size = dashColliderScale;


        }
    }
    #endregion

    #region Ledge
    void ClimbLedge()
    {
        //transform.GetChild(0).GetComponent<Animator>().Play("ProtoLedgeClimb");
        
        isClimbingLedge = true;
        animator.SetBool("ClimbLedge", isClimbingLedge);
        ledgeClimbTimer = ledgeClimbDuration;

    }
    void LedgeClimbCountdown()
    {
        if (isClimbingLedge)
        {
            ledgeClimbTimer -= Time.deltaTime;
            if (ledgeClimbTimer <= 0f)
            {
                UpdateLedgeClimbPosition();
            }
        }
    }
    public void UpdateLedgeClimbPosition()
    {
        isClimbingLedge = false;
        transform.position = spriteTransform.position;
        Vector2 _posOffset = new (spriteTransform.right.x * ledgeClimbPosOffset.x,
            spriteTransform.up.y * ledgeClimbPosOffset.y);
        transform.position = (Vector2)transform.position + _posOffset;

    }

    void CheckLedge()
    {
        if (LedgeGrab) 
        { 
            //CHECH THE LEDGE
            Vector2 _ledgeCheckOrigin = new(transform.position.x,transform.position.y + ledgeCheckOffset);
            RaycastHit2D _HitLedge = Physics2D.Raycast(_ledgeCheckOrigin, spriteTransform.right, ledgeCheckDistance, ledgeCheckLayer);
            bool _canGrabLedge = !_HitLedge && hitWall;


            if ( _canGrabLedge)
            {
                //GET LEDGE POSITION
                Vector2 _ledgeGroundCheckOrigin = _ledgeCheckOrigin + (Vector2)(spriteTransform.right * ledgeCheckDistance);
                RaycastHit2D _hit = Physics2D.Raycast(_ledgeGroundCheckOrigin, Vector2.down, ledgeCheckDistance, ledgeCheckLayer);

                if (_hit)
                {
                
                    if (!isLedge)
                    {
                        ledgePosition = (Vector2)transform.position - new Vector2(0f, _hit.distance - 0.1f);
                        isLedge = true;
                        currentVerticalSpeed = 0f;
                        fallClamp = 0f;
                        transform.position= ledgePosition;
                        //rb2.MovePosition(ledgePosition);
                        if (autoClimbLedge)
                        {
                            ClimbLedge();

                        }
                    }
                    if (Input.GetKeyDown(climbButton) && !autoClimbLedge)
                    {
                        ClimbLedge();
                    }

                }
            
            }
        
            else if (isLedge && !_canGrabLedge)
            {
                isLedge = false;
                isClimbingLedge = false;
                fallClamp = fallSpeedClamp;
            }

        }
    }
    #endregion

    #region Ceiling
    void CheckCeil()
    {
        RaycastHit2D hit2D = Physics2D.CircleCast((Vector2)transform.position + ceilCheckCenter,ceilCheckCircleRadius,transform.up,ceilCheckRayDistance,ceilLayer);

        if (hit2D )
        {
            if (!onCeil)
            {
                onCeil = true;
                currentVerticalSpeed = 0;
            }
        }
        else
        {
            onCeil = false;
        }
    }

    public void GetAutoValueForCeilCheck()
    {
        if (capsuleCollider == null)
        {
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        }
#if UNITY_EDITOR
        Undo.RecordObject(this, "Set Value For Ceil Check");
#endif
        ceilCheckCenter = capsuleCollider.offset;
        ceilCheckCircleRadius = capsuleCollider.size.x / 2f;
        ceilCheckRayDistance = (capsuleCollider.size.y / 2f) - (ceilCheckCircleRadius) + 0.02f;
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
    #endregion

    #region Wall
    void CheckSideWall()
    {
        rightWallHit = Physics2D.Raycast(transform.position,transform.right,wallCheckRayDistance,wallCheckLayer);
        leftWallHit = Physics2D.Raycast(transform.position, -transform.right, wallCheckRayDistance, wallCheckLayer);

        if (rightWallHit || leftWallHit)
        {
            if (!hitWall)
            {
                if (resetDashOnWall)
                {
                    canDash = true;
                }
                hitWall = true;
                currentHorizontalSpeed = 0;
                
            }

            if (WallJump && !isGrounded)
            {
                SlideOnWall(true);
                
            }

            if (cancelDashOnWallHit && isDashing)
            {
                CancelDash();
            }
        }
        else
        {
            if (WallJump)
            {
                SlideOnWall(false);
                
                //fallSpeedClamp /= wallSlideSpeedEffect;
            }
            hitWall = false;
        }

    }

    void SlideOnWall(bool _sliding)
    {
        if (!isSlidingOnWall && _sliding)
        {
            isSlidingOnWall = true;
            if (doubleJump)
            {
                canJump = true;
                jumpCounter = numberJumps;
            }
            isNormalJumped = false;
            isWallJumped = false;
            fallClamp = wallSlideSpeed;
        }
        else if (isSlidingOnWall && !_sliding)
        {
            isSlidingOnWall = false;
            fallClamp = fallSpeedClamp;
        }
    }
    void DelayInputOnWall()
    {
        if (isSlidingOnWall)
        {
            inputDelayTimer = (input.x == 0) ? inputDelay : inputDelayTimer - Time.deltaTime;
            if (inputDelayTimer > 0f) input.x = 0;
        }

    }
    #endregion

    #region Ground
    void CheckGround()
    {
        RaycastHit2D hit2D = Physics2D.CircleCast((Vector2)transform.position + groundCheckCenter, groundCheckCircleRadius, -transform.up, groundCheckRayDistance, groundLayer);
        if (hit2D)
        {
            
            if (!isGrounded)
            {
                
                isGrounded = true;
                isWallJumped = false;
                isNormalJumped = false;
                fallClamp = fallSpeedClamp;
                canJump = true;
                if (doubleJump)
                {
                    jumpCounter = numberJumps;
                }
                canDash = resetDashOnGround;
                onAirControlMultiplier = 1;
                SlideOnWall(false);

                if (currentVerticalSpeed <= 0)//to check if the player if grounded while falling
                {
                    currentVerticalSpeed = 0f;
                    isJumped = false;
                }
            }

            if (resetDashOnGround)
            {
                canDash = true;
            }

        }
        else
        {
            if (isGrounded)
            {
                onAirControlMultiplier = onAirControl;
                isGrounded = false;
                

                if (!isJumped)
                {
                    fallToleranceTimer = coyoteTime;
                    if (!dashCancelsGravity)
                    {
                        CancelDash();
                    }
                }
            }
            

        }

        if (!isGrounded)
        {

            
            if (fallToleranceTimer <= 0)
            {
                if (!airDash)
                {
                    canDash = false;
                }
                if (!doubleJump)
                {
                    canJump = false;
                }
                else if (jumpCounter == numberJumps && !isSlidingOnWall)
                {
                    jumpCounter--;
                }
            }
            else
            {
                fallToleranceTimer -= Time.deltaTime;
            }
        }
    }

    public void GetAutoValueForGroundCheck()
    {
        if (capsuleCollider == null)
        {
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        }
#if UNITY_EDITOR
        Undo.RecordObject(this, "Set Value For Ground Check");
#endif
        groundCheckCenter = capsuleCollider.offset;
        groundCheckCircleRadius = capsuleCollider.size.x / 2f;
        groundCheckRayDistance= (capsuleCollider.size.y / 2f) - (groundCheckCircleRadius) + 0.02f;
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
    #endregion

    #region Jump
    void PressJumpButton()
    {
        //if (!isDashing)
        //{
        
        isHoldingJumpButton = true;
            isPressedJumpButton = true;
            isForcingJump = true;
            jumpHoldTimer = variableJumpHeightDuration * jumpUpDuration;
            jumpToleranceTimer = jumpBuffer;
        //}
    }
    void Jump()
    {
        if (isForcingJump)
        {
            jumpHoldTimer -= Time.deltaTime;
            if (jumpHoldTimer <= 0)
            {
                isForcingJump = false;
            }
            if (!isHoldingJumpButton && isForcingJump && ((variableJumpHeightOnWallJump && isWallJumped) || isNormalJumped))
            {
                currentVerticalSpeed *= jumpReleaseEffect;
                isForcingJump = false;
            }

        }

        if (canJump && isPressedJumpButton && !isSlidingOnWall)
        {
            CancelDash();
            jumpVelocity = Mathf.Sqrt(2 * jumpUpAcceleration * jumpHight * gravity);
            jumpUpDuration = jumpVelocity / (jumpUpAcceleration * gravity);

            isJumped = true;
            if (doubleJump && jumpCounter > 0)
            {
                jumpCounter--;
            }
            else 
            {
                canJump = false;
            }
            isPressedJumpButton = false;
            currentVerticalSpeed = jumpVelocity;
            isNormalJumped = true;
            JumpParticle();
        }
        else if (isPressedJumpButton && isSlidingOnWall && ((!canWallJumpWhileClimbing && !isClimbingLedge)|| canWallJumpWhileClimbing ))
        {
            CancelDash();
            jumpVelocity = Mathf.Sqrt(2 * jumpUpAcceleration * jumpHight * gravity);
            isJumped = true;
            if (!doubleJump)
            {
                canJump = false;
            }
            isWallJumped = true;
            isPressedJumpButton = false;
            onAirControlMultiplier = wallJumpDecelerationFactor;
            currentVerticalSpeed = jumpVelocity * wallJumpVelocity.y;
            if (leftWallHit)
            {
                currentHorizontalSpeed = jumpVelocity * wallJumpVelocity.x;
            }
            else if (rightWallHit)
            {
                currentHorizontalSpeed = -jumpVelocity * wallJumpVelocity.x;
            }
            JumpParticle();
        }
    }

    void JumpParticle()
    {
        GameObject jumpDust = Instantiate(jumpDustPrefab, transform.position, Quaternion.identity);
        jumpDust.GetComponent<ParticleSystem>().Play();
        Destroy(jumpDust, jumpDust.GetComponent<ParticleSystem>().main.duration);
    }

    void CountDownJumpTolerance()
    {
        if (isPressedJumpButton)
        {
            jumpToleranceTimer -= Time.deltaTime;
            if (jumpToleranceTimer <= 0)
            {
                isPressedJumpButton = false;
            }
        }
    }
    #endregion

    #region Move
    void UpdatePlatformerSpeed()
    {
        
        if ((casting.animCast1 || casting.animCast2) && isGrounded)
        {
            currentHorizontalSpeed = 0;
            currentVerticalSpeed = 0;
            return;
        }

        if (!isDashing) 
        {
            if (input.x != 0)
            {
                if (input.x * currentHorizontalSpeed >= 0)
                {
                    if ((input.x > 0 && !rightWallHit) || (input.x < 0 && !leftWallHit))
                    {
                        currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, input.x * movementSpeed, (Time.deltaTime / speedUpDuration) * onAirControlMultiplier * movementSpeed);// /(xDist)
                    }
                }
                else
                {
                    currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, (Time.deltaTime / speedDownDuration) * onAirControlMultiplier * movementSpeed);
                }
            }
            else
            {
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, (Time.deltaTime / stopDuration) * onAirControlMultiplier * movementSpeed);
            }
        }
        

        if (!isGrounded && (!isDashing || (!dashCancelsGravity && isDashing && !isAirDashing)))
        {
            if (currentVerticalSpeed >= 0)
            {
                currentVerticalSpeed -= jumpUpAcceleration * Time.fixedDeltaTime * gravity;
            }
            else if (currentVerticalSpeed < 0)
            {
                currentVerticalSpeed -= jumpDownAcceleration * Time.fixedDeltaTime * gravity;
            }
            currentVerticalSpeed = Mathf.Clamp(currentVerticalSpeed,-fallClamp, 100f);
        }
        else if (isGrounded)
        {
            if (currentVerticalSpeed < 0)
            {
                currentVerticalSpeed = Mathf.MoveTowards(currentVerticalSpeed, 0, jumpDownAcceleration * jumpVelocity * 3 * Time.fixedDeltaTime);
            }
        }
    }

    void FlipThePlayer()
    {
        Vector3 _playerRot = spriteTransform.localEulerAngles;
        
        if (!isSlidingOnWall && currentHorizontalSpeed > 0 || (WallJump && isSlidingOnWall && rightWallHit))
        {
            _playerRot.y = 0f;
            spriteTransform.localEulerAngles = _playerRot;
            runDust.GetComponent<ParticleSystemRenderer>().flip = Vector3.zero;
        }
        else if (!isSlidingOnWall && currentHorizontalSpeed < 0 || (WallJump && isSlidingOnWall && leftWallHit))
        {
            _playerRot.y = 180f;
            spriteTransform.localEulerAngles = _playerRot;
            runDust.GetComponent<ParticleSystemRenderer>().flip = Vector3.right;
        }
    }

    void MovePlayer()
    {

        runDust.SetActive((isGrounded || isDashing) && Mathf.Abs(currentHorizontalSpeed) > 0);

        rb2.linearVelocity = new Vector2(currentHorizontalSpeed,currentVerticalSpeed); 
    }
    #endregion

    #region Cast
    void CanCast()
    {
        casting.canCast = !isDashing && !isSlidingOnWall && !isLedge && !isClimbingLedge;
    }

    #endregion
    public void GetColliderSize()
    {
        dashColliderScale = capsuleCollider.size;
        dashColliderOffset = capsuleCollider.offset;
        print("Values Saved");
    }

    private void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckCenter - (Vector2)transform.up * groundCheckRayDistance, groundCheckCircleRadius);

        if (onCeil)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere((Vector2)transform.position + ceilCheckCenter + (Vector2)transform.up * ceilCheckRayDistance, ceilCheckCircleRadius);


        if (rightWallHit)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawRay(transform.position, transform.right * wallCheckRayDistance);

        if (leftWallHit)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawRay(transform.position, -transform.right * wallCheckRayDistance);
        Vector2 _ledgePos;
        if (isLedge)
        {
            Gizmos.color = Color.green;
            
        }
        else
        {
            Gizmos.color = Color.red;
        }

        _ledgePos = new Vector2(transform.position.x, transform.position.y + ledgeCheckOffset);
        Gizmos.DrawRay(_ledgePos, spriteTransform.right * ledgeCheckDistance);
        Vector2 _ledgeGroundCheckCenter = _ledgePos + (Vector2)(spriteTransform.right * ledgeCheckDistance);

        Gizmos.DrawRay(_ledgeGroundCheckCenter, Vector2.down * ledgeCheckDistance);

        Vector2 _posOffset = new(spriteTransform.right.x * ledgeClimbPosOffset.x,
            spriteTransform.up.y * ledgeClimbPosOffset.y);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere((Vector2)transform.position + _posOffset, 0.05f);


    }
}
