using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacSKILL : MacFSMState
{
    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();

        GetComponent<MacATTACK>()._AttackTimes = 0;
    }

    private void Update()
    {
        transform.LookAt(_manager.PlayerCapsule.transform);

        if (GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) > _manager.Stat.statData._AttackRange)
        {
            _manager.SetState(MacState.CHASE);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
