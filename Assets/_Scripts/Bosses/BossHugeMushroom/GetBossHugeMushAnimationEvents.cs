using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class GetBossHugeMushAnimationEvents : MonoBehaviour
{
    public BossHugeMushroom boss;

    public UnityEvent meleeAttack;
    public UnityEvent endMeleeAttack;
    public UnityEvent mushroomAttack;
    public UnityEvent endMushroomAttack;
    public UnityEvent spawn;
    public UnityEvent endSpawn;

    public void MeleeAttack()
    {
        meleeAttack.Invoke();
    }
    public void EndMeleeAttack()
    {
        endMeleeAttack.Invoke();
    }
    public void MushroomAttack()
    {
        mushroomAttack.Invoke();
        boss.animMushroomAttack = true;
    }
    public void EndMushroomAttack()
    {
        boss.endAnimMushroomAttack = true;
        boss.pauseChrono = boss.pauseTimer;
        boss.canFlip = true;
        endMushroomAttack.Invoke();
    }
    public void Spawn()
    {
        spawn.Invoke();
        boss.animSpawn = true;
    }
    public void EndSpawn()
    {
        boss.endAnimSpawn = true;
        boss.pauseChrono = boss.pauseTimer;
        boss.canFlip = true;
        endSpawn.Invoke();
    }

    public void Hurt()
    {
        boss.hurt = true;        
    }

    public void EndHurt()
    {
        boss.endHurt = true;
        boss.canFlip = true;
    }

}
