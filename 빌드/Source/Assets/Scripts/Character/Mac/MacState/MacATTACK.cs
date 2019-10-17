﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacATTACK : MacFSMState
{
    public int _AttackTimes = 0;

    public bool isLookAt = true;

    public override void BeginState()
    {
        base.BeginState();
        _manager.agent.destination = this.transform.position;
        _manager.agent.acceleration = 0.0f;
        _manager.agent.isStopped = true;

        if(_AttackTimes == 3)
        {
            _manager.SetState(MacState.SKILL);
        }
        
        _AttackTimes++;
        isLookAt = true;
        //Debug.Log(string.Format("공격횟수 : {0}", _AttackTimes));
    }

    public override void EndState()
    {
        base.EndState();
        _manager.agent.acceleration = 0.5f;
    }

    protected override void Update()
    {
        if(isLookAt) transform.LookAt(_manager._PriorityTarget.transform);

        if (GameLib.DistanceToCharacter(_manager.CC, _manager._PriorityTarget) > _manager.Stat.statData._AttackRange)
        {
            _manager.SetState(MacState.CHASE);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
