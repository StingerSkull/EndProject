using UnityEngine;
using UnityHFSM;

public class BossHugeMushroomDeadState<T> : StateBase<BossHugeMushroom.BossHugeMushroomStates>
{
    private BossHugeMushroom _boss;

    public BossHugeMushroomDeadState(BossHugeMushroom boss) : base(needsExitTime: false, isGhostState: false)
    {
        _boss = boss;
    }
    public override void Init() { }

    public override void OnEnter()
    {
    }

    public override void OnLogic()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnExitRequest() { }
}
