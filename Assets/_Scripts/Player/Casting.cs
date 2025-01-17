using UnityEngine;

public class Casting : MonoBehaviour
{
    public GameObject prefabFirstSpell;
    public GameObject prefabSecondSpell;
    public GameObject sprite;
    public Animator animator;
    public float cooldownBetweenSpells;

    public bool canCast = true;
    public bool animCast1 = false;
    public bool animCast2 = false;

    public bool isCasting = false;

    private ISpell firstSpell;
    private ISpell secondSpell;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = sprite.GetComponent<Animator>();

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
                if (Input.GetMouseButtonDown(0) && !firstSpell.IsOnCooldown())
                {
                    animator.SetTrigger("Cast");
                    animCast1 = true;
                }

                if (animCast1 && isCasting)
                {
                    firstSpell.Cast();
                    isCasting = false;
                    animCast1 = false;
                }
            }
            
            if (secondSpell != null)
            {
                if (Input.GetMouseButtonDown(1) && !secondSpell.IsOnCooldown())
                {
                    animator.SetTrigger("Cast");
                    animCast2 = true;
                }

                if (animCast2 && isCasting)
                {
                    secondSpell.Cast();
                    isCasting = false;
                    animCast2 = false;
                }
            }
        }
        else
        {
            /////////
            ///HEEEEEEEEERE fix attack run animation
            isCasting = false;
            animCast1 = false;
            animCast2 = false;
        }
    }

    public void SetIsCasting()
    {
        isCasting = true;
    }
}
