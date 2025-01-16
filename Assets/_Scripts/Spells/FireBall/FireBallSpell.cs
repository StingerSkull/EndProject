using UnityEngine;

public class FireBallSpell : MonoBehaviour, ISpell
{
    public GameObject prefabFireball;
    public GameObject caster;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        caster = GameObject.Find("Caster");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Cast()
    {
        Instantiate(prefabFireball, caster.transform.position, caster.transform.rotation);
    }
}
