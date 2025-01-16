using UnityEngine;

public class Casting : MonoBehaviour
{
    public GameObject prefabFirstSpell;
    public GameObject prefabSecondSpell;
    public GameObject sprite;

    private ISpell firstSpell;
    private ISpell secondSpell;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firstSpell = Instantiate(prefabFirstSpell, sprite.transform).GetComponent<ISpell>();
        secondSpell = Instantiate(prefabSecondSpell, sprite.transform).GetComponent<ISpell>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstSpell.Cast();
        }
        if (Input.GetMouseButtonDown(1))
        {
            secondSpell.Cast();
        }
    }
}
