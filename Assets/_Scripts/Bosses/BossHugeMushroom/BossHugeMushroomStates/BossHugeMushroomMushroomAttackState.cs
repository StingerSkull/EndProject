using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityHFSM;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BossHugeMushroomMushroomAttackState<T> : StateBase<BossHugeMushroom.BossHugeMushroomStates>
{
    private BossHugeMushroom _boss;

    public BossHugeMushroomMushroomAttackState(BossHugeMushroom boss) : base(needsExitTime: false, isGhostState: false)
    {
        _boss = boss;
    }
    public override void Init() { }

    public override void OnEnter()
    {
        _boss.currentMovementSpeed = 0f;
        _boss.canFlip = false;
        _boss.animator.SetTrigger("MushroomAttack");
        
    }

    public override void OnLogic()
    {
        if (_boss.animMushroomAttack)
        {
            _boss.CoroutineMushrooms(_boss.mushSpawner.position, _boss.transform.right.x);
            _boss.animMushroomAttack= false;
        }
    }

    public override void OnExit()
    {
        _boss.endAnimMushroomAttack = false;
    }

    public override void OnExitRequest() { }

    
}
