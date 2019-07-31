﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterATTACK : MonsterFSMState
{
    public float _AttackBeforeTime = 0.8f;
    public float _AttackTime = 3.0f;
    public float _AfterAttackTime = 1.0f;
    public float _Time = 0.0f;

    bool _CreateBall = false;
    bool _SetBall = false;
    Transform bullet;

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
        _Time += Time.deltaTime;

        if (GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) < _manager.Stat._AttackRange)
        {
            if (_Time > _AttackTime)
            {
                _manager._MR.material = _manager.Stat._BeforeAttackMat;


                if (!_CreateBall)
                {
                    bullet = Instantiate(_manager.Stat._AttackEffect,
                    _manager._AttackTransform.position,
                    Quaternion.identity).transform;
                    _CreateBall = true;
                }


                if (_Time > _AttackTime + _AttackBeforeTime)
                {
                    _manager._MR.material = _manager.Stat._AttackMat;


                    if (!_SetBall)
                    {

                        bullet.GetComponent<Bullet>().dir = GameLib.DirectionToCharacter(_manager.CC, _manager.PlayerCapsule);
                        bullet.GetComponent<Bullet>()._Move = true;
                        _SetBall = true;
                    }

                }

                if(_Time> _AttackTime + _AttackBeforeTime + _AfterAttackTime)
                {

                    _Time = 0.0f;
                    _CreateBall = false;
                    _SetBall = false;
                }
            }

        }
        else
        {
            _manager.SetState(MonsterState.CHASE);
        }

    }
}
