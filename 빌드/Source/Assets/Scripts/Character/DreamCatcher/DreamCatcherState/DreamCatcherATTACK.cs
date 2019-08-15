﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCatcherATTACK : DreamCatcherFSMState
{

    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();
    }

    protected override void Update()
    {
        base.Update();

        DahsCheck();

        if (GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) < _manager.Stat.statData._AttackRange)
        {
            
        }
        else
        {
            _manager.SetState(DreamCatcherState.CHASE);
        }

    }

    public void AttackCheck()
    {
        var hitTarget = GameLib.SimpleDamageProcess(transform,
            _manager.Stat.AttackRange,
            "Player", _manager.Stat);

        if (hitTarget != null) _manager._lastAttack = hitTarget;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
