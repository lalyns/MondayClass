using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATTACK : FSMState
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

    }

    public void AttackCheck()
    {
        var hitTarget = GameLib.SimpleDamageProcess(transform, _manager.Stat.AttackRange,
            "Monster", _manager.Stat);

        if (hitTarget != null) _manager._lastAttack = hitTarget;
    }
}
