using UnityEngine;
using UnityHFSM;

public class BossHugeMushroomMoveState<T> : StateBase<BossHugeMushroom.BossHugeMushroomStates>
{
    private BossHugeMushroom _boss;

    public BossHugeMushroomMoveState(BossHugeMushroom boss) : base(needsExitTime: false, isGhostState: false)
    {
        _boss = boss;
    }
    public override void Init() { }

    public override void OnEnter()
    {
        _boss.rdmSkill = Random.Range(1, 3);
        _boss.currentMovementSpeed = _boss.movementSpeed;
    }

    public override void OnLogic()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnExitRequest() { }
}
