﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatATTACK : RedHatFSMState
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

        if (GameLib.DistanceToCharacter(_manager.CC, _manager._PriorityTarget) < _manager.Stat.statData._AttackRange)
        {
            
        }
        else
        {
            _manager.SetState(RedHatState.CHASE);
        }

    }

    public void AttackCheck()
    {
        var hitTarget = GameLib.SimpleDamageProcess(transform,
            _manager.Stat.AttackRange,
            "Player", _manager.Stat);

        Invoke("AttackSupport", 0.5f);

        //if (hitTarget != null) _manager._lastAttack = hitTarget;
    }

    public void AttackSupport()
    {
        UserInterface.Instance.PCUI.PlayerHpBar.HitBackFun();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
