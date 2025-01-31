using UnityEditor.PackageManager;
using UnityEngine;
using UnityHFSM;

public class BossHugeMushroomIdleState<T> : StateBase<BossHugeMushroom.BossHugeMushroomStates>
{
    private BossHugeMushroom _boss;

    public BossHugeMushroomIdleState(BossHugeMushroom boss) : base(needsExitTime: false, isGhostState: false)
    {
        _boss = boss;
    }

    public override void Init() { }

    public override void OnEnter()
    {
        _boss.currentMovementSpeed = 0f;
    }

    public override void OnLogic()
    {
    }

    public override void OnExit()
    {
        
    }

    public override void OnExitRequest() { }

    public void OnFixedUpdate()
    {

    }
}
