using UnityEngine;
using UnityHFSM;

public class BossHugeMushroomSpawnState<T> : StateBase<BossHugeMushroom.BossHugeMushroomStates>
{
    private BossHugeMushroom _boss;

    public BossHugeMushroomSpawnState(BossHugeMushroom boss) : base(needsExitTime: false, isGhostState: false)
    {
        _boss = boss;
    }
    public override void Init() { }

    public override void OnEnter()
    {
        _boss.currentMovementSpeed = 0f;
        _boss.canFlip = false;
        _boss.animator.SetTrigger("Spawn");
    }

    public override void OnLogic()
    {
        if (_boss.animSpawn)
        {
            GameObject.Instantiate(_boss.prefabBig, _boss.bigSpawner.position, _boss.bigSpawner.rotation);
            _boss.animSpawn = false;
        }
    }

    public override void OnExit()
    {
        _boss.endAnimSpawn = false;
    }

    public override void OnExitRequest() { }
}
