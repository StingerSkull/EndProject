using UnityEngine;
using UnityHFSM;

public class BossHugeMushroomHurtState<T> : StateBase<BossHugeMushroom.BossHugeMushroomStates>
{
    private BossHugeMushroom _boss;

    public BossHugeMushroomHurtState(BossHugeMushroom boss) : base(needsExitTime: false, isGhostState: false)
    {
        _boss = boss;
    }
    public override void Init() { }

    public override void OnEnter()
    {
        _boss.currentMovementSpeed = 0f;
        _boss.canFlip = false;
        _boss.hurt = false;
    }

    public override void OnLogic()
    {
    }

    public override void OnExit()
    {
        _boss.endHurt = false;
    }

    public override void OnExitRequest() { }
}