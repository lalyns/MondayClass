using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacATTACK : MacFSMState
{
    public float _AttackBeforeTime = 0.8f;
    public float _AttackTime = 3.0f;
    public float _AfterAttackTime = 1.0f;
    public float _Time = 0.0f;

    bool _CreateBall = false;
    bool _SetBall = false;
    public Transform bullet;

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

        if (GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) < _manager.Stat.statData._AttackRange)
        {
            if (_Time > _AttackTime)
            {
                _manager._MR.material = _manager.Stat._BeforeAttackMat;


                if (!_CreateBall)
                {
                    transform.LookAt(_manager.PlayerCapsule.transform);
                    //bullet = Instantiate(_manager.Stat._AttackEffect,
                    //_manager._AttackTransform.position,
                    //Quaternion.identity).transform;
                    //bullet.transform.parent = this.transform;
                    _CreateBall = true;
                }


                if (_Time > _AttackTime + _AttackBeforeTime)
                {
                    _manager._MR.material = _manager.Stat._AttackMat;


                    if (!_SetBall)
                    {
                        try
                        {
                            //    bullet.GetComponent<Bullet>().LookAtTarget(_manager.PlayerCapsule.transform);
                            //    bullet.GetComponent<Bullet>().dir = GameLib.DirectionToCharacter(_manager.CC, _manager.PlayerCapsule);
                            //    bullet.GetComponent<Bullet>()._Move = true;
                        }
                        catch
                        {

                        }
                        _SetBall = true;
                    }

                }

                if(_Time> _AttackTime + _AttackBeforeTime + _AfterAttackTime)
                {
                    _Time = 0.0f;
                    _CreateBall = false;
                    _SetBall = false;

                    _manager.SetState(MacState.RUNAWAY);
                }
            }

        }
        else
        {
            _manager.SetState(MacState.CHASE);
        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
