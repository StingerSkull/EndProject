using UnityEngine;
using UnityHFSM;

public class BossHugeMushroomDeadState<T> : StateBase<BossHugeMushroom.BossHugeMushroomStates>
{
    private BossHugeMushroom _boss;

    public BossHugeMushroomDeadState(BossHugeMushroom boss) : base(needsExitTime: false, isGhostState: false)
    {
        _boss = boss;
    }
}
