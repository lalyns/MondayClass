﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacSKILL : MacFSMState
{
    public bool isLookAt = true;

    public override void BeginState()
    {
        base.BeginState();

        isLookAt = true;
    }

    public override void EndState()
    {
        base.EndState();

        GetComponent<MacATTACK>().attackTimes = 0;
    }

    protected override void Update()
    {
        base.Update();

        if (isLookAt)
        {
            transform.LookAt(_manager.priorityTarget.transform);
        }

        if (GameLib.DistanceToCharacter(_manager.CC, _manager.priorityTarget) > _manager.Stat.statData._AttackRange)
        {
            _manager.SetState(MacState.CHASE);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
