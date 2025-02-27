using UnityEngine;
using UnityEngine.Events;

public class EnemyDamage : MonoBehaviour, IDamage
{
    public Animator animator;

    public int maxLife = 5;
    public int currentLife;

    public float hurtCooldown = 0.5f;
    public float hurtTimer = 0f;

    public UnityEvent enemyHurt;
    public UnityEvent enemyDeath;

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

    public void DealDmg(int dmg)
    {
        if (hurtTimer <= 0f)
        {
            currentLife -= dmg;
            if (currentLife > 0)
            {
                animator.SetTrigger("Hurt");
                enemyHurt.Invoke();
            }
            else
            {
                animator.SetTrigger("Death");
                enemyDeath.Invoke();
            }
            hurtTimer = hurtCooldown;
        }
    }

    public bool InHurtCoolDown()
    {
        return hurtTimer > 0f;
    }
}
