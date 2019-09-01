using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiberATTACK : TiberFSMState
{
    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        if (GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) > _manager.Stat._AttackRange)
        {
            _manager.SetState(TiberState.CHASE);
        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
