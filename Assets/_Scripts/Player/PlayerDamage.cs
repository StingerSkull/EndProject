using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDamage : MonoBehaviour
{
    public Animator animator;
    public Casting casting;

    public int maxLife = 5;
    public int currentLife;

    public float hurtCooldown = 0.5f;
    public float hurtTimer = 0f;

    public UnityEvent playerHurt;
    public UnityEvent playerDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLife = maxLife;
        hurtTimer = 0f;
    }

    private void Update()
    {
        if (hurtTimer > 0f)
        {
            hurtTimer -= Time.deltaTime;
        }
    }

    public void PlayerEnemyDmg(int dmg)
    {
        if (hurtTimer <= 0f)
        {
            currentLife -= dmg;
            if (currentLife > 0)
            {
                animator.SetTrigger("Hurt");
                playerHurt.Invoke();
            }
            else
            {
                playerDeath.Invoke();
            }
            ResetAnimationTriggers();
            hurtTimer = hurtCooldown;
        }
    }
    
    public void PlayerDmg(int dmg)
    {
        if (hurtTimer <= 0f)
        {
            currentLife -= dmg;
            if (currentLife > 0)
            {
                animator.SetTrigger("Hurt");
                playerHurt.Invoke();
            }
            else
            {
                playerDeath.Invoke();
            }
            ResetAnimationTriggers();
            hurtTimer = hurtCooldown;
        }
    }

    public void ResetAnimationTriggers()
    {
        animator.ResetTrigger("Cast");
        casting.ResetAllAnimCast();
        animator.ResetTrigger("Jump");
    }

    public bool InHurtCoolDown()
    {
        return hurtTimer > 0f;
    }
}
