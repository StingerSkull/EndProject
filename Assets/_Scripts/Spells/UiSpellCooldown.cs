using UnityEngine;
using UnityEngine.UI;

public class UiSpellCooldown : MonoBehaviour
{
    public Image spellCD1;
    public Image spellCD2;
    public Casting casting;

    private float maxCdSpell1;
    private float maxCdSpell2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        casting.OnSpellsInitialized += InitializeCooldowns;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(casting.firstSpell.OnCooldown());
        spellCD1.fillAmount = 1-(casting.firstSpell.OnCooldown() / maxCdSpell1);
        Color color1 = spellCD1.color;
        color1.a =  Mathf.Pow(1 - (casting.firstSpell.OnCooldown() / maxCdSpell1),3);
        spellCD1.color = color1;

        // spellCD2.fillAmount = casting.secondSpell.OnCooldown() / maxCdSpell2; 
    }

    void InitializeCooldowns()
    {
        maxCdSpell1 = casting.firstSpell?.GetSpellData().cooldown ?? 0;
        maxCdSpell2 = casting.secondSpell?.GetSpellData().cooldown ?? 0;

        Debug.Log(maxCdSpell1);
    }
}
