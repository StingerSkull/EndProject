using UnityEngine;

public interface ISpell
{
    void Cast();
    float OnCooldown();

    SpellData GetSpellData();
    SoundData GetSound();
}
