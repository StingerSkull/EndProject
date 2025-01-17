using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireBallSpell : MonoBehaviour, ISpell
{
    public GameObject prefabFireball;
    public Transform caster;
    public SpellData fireBallData;

    public float chrono;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        caster = transform.parent.Find("Caster");
        
        chrono = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(chrono > 0)
        {
            chrono -= Time.deltaTime;
        }
    }

    public void Cast()
    {
        if (chrono <=0)
        {
            Instantiate(prefabFireball, caster.position, caster.rotation);
            chrono = fireBallData.cooldown;
        }
    }

    public bool IsOnCooldown()
    {
        return chrono > 0;
    }
}
