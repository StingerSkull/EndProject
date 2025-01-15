using UnityEngine;

public class AnimationController : MonoBehaviour
{

    Movement2D movement;
    [SerializeField] Animator animator;
    private void Awake()
    {
        movement = GetComponent<Movement2D>();
    }
    void Update()
    {
        animator.SetBool("Dashing", movement.isDashing);
        animator.SetBool("WallJump", movement.WallJump);
        animator.SetFloat("JumpSpeed", movement.currentVerticalSpeed);
        animator.SetFloat("MovementSpeed", Mathf.Abs(movement.currentHorizontalSpeed));
        animator.SetBool("Grounded",movement.isGrounded);
        animator.SetBool("OnWallHit",movement.hitWall);
        animator.SetBool("OnLedge",movement.isLedge);
        animator.SetBool("ClimbLedge", movement.isClimbingLedge);
        
    }
}
