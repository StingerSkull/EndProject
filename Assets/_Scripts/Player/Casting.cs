using UnityEngine;

public class Casting : MonoBehaviour
{
    public GameObject prefabFirstSpell;
    public GameObject prefabSecondSpell;
    public GameObject sprite;
    public Animator animator;
    public Movement2D movement;

    public bool canCast = true;
    public bool animCast1 = false;
    public bool animCast2 = false;

    public bool animCast = false;

    public ISpell firstSpell;
    public ISpell secondSpell;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = sprite.GetComponent<Animator>();
        movement = GetComponent<Movement2D>();

        if (prefabFirstSpell != null)
        {
            firstSpell = Instantiate(prefabFirstSpell, sprite.transform).GetComponent<ISpell>();
        }

        if (prefabSecondSpell != null)
        {
            secondSpell = Instantiate(prefabSecondSpell, sprite.transform).GetComponent<ISpell>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (canCast)
        {
            if (firstSpell != null)
            {
                if (Input.GetMouseButtonDown(0) && !firstSpell.IsOnCooldown() && !animCast1)
                {
                    animCast = false;
                    animator.SetTrigger("Cast");
                    animCast1 = true;
                }

                if (animCast1 && animCast)
                {
                    firstSpell.Cast();
                    ResetAnimCast(ref animCast1);
                }
            }
            
            if (secondSpell != null)
            {
                if (Input.GetMouseButtonDown(1) && !secondSpell.IsOnCooldown() && !animCast2)
                {
                    animCast = false;
                    animator.SetTrigger("Cast");
                    animCast2 = true;
                }

                if (animCast2 && animCast)
                {
                    secondSpell.Cast();
                    ResetAnimCast(ref animCast2);
                }
            }
        }
        else
        {
            if (animCast)
            {
                animator.ResetTrigger("Cast");
                animCast = false;
            }
            animCast1 = false;
            animCast2 = false;
        }
    }

    public void SetAnimCast()
    {
        animCast = true;
    }

    public void ResetAnimCast(ref bool animCastSpell)
    {
        animCast = false;
        animCastSpell = false;
    }
}
