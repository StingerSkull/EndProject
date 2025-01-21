using UnityEngine;

public interface ISpell
{
    void Cast();
    bool IsOnCooldown();

    SoundData GetSound();
}
