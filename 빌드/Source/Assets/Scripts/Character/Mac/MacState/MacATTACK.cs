using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacATTACK : MacFSMState
{

    public int attackTimes = 0;
    public bool isLookAt = true;

    public override void BeginState()
    {
        base.BeginState();
        _manager.agent.velocity = Vector3.zero;
        _manager.agent.destination = this.transform.position;
        _manager.agent.acceleration = 0.0f;
        _manager.agent.isStopped = true;

        if(attackTimes == 3)
        {
            _manager.SetState(MacState.SKILL);
        }
        
        isLookAt = true;
        //Debug.Log(string.Format("공격횟수 : {0}", _AttackTimes));
    }

    public override void EndState()
    {
        base.EndState();
        _manager.agent.acceleration = 0.5f;
        attackTimes++;
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
