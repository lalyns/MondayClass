﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCatcherCHASE : DreamCatcherFSMState
{
    bool _IsSpread = false;

    public override void BeginState()
    {
        _manager._MR.material = _manager.Stat._NormalMat;
        base.BeginState();
    }

    public override void EndState()
    {
        _IsSpread = false;

        base.EndState();
    }

    protected override void Update()
    {
        base.Update();

        DahsCheck();

        if (GameLib.DistanceToCharacter(_manager.CC,_manager.PlayerCapsule) < _manager.Stat._AttackRange)
        {
            _manager.SetState(DreamCatcherState.ATTACK);
        }

        else
        {
            _manager.CC.transform.LookAt(_manager.PlayerCapsule.transform);

            Vector3 moveDir = (_manager.PlayerCapsule.transform.position
                - _manager.CC.transform.position).normalized;

            moveDir.y = 0;

            if ((_manager.CC.collisionFlags & CollisionFlags.Sides) != 0)
            {
                Vector3 correctDir = Vector3.zero;
                if (!_IsSpread)
                {
                    correctDir = DecideSpreadDirection();
                    _IsSpread = true;
                }

                moveDir += correctDir;
            }

            _manager.CC.Move(moveDir * _manager.Stat.statData._MoveSpeed * Time.deltaTime);
        }

        
    }


    private Vector3 DecideSpreadDirection()
    {
        Vector3 correctDir;

        correctDir = UnityEngine.Random.Range(1, 100) % 2 == 0 ? transform.right : -transform.right;
        correctDir += transform.forward;

        return correctDir;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
