using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiberCHASE : TiberFSMState
{
    bool _IsSpread = false;

    float _time;

    public override void BeginState()
    {

        base.BeginState();
    }

    public override void EndState()
    {
        _IsSpread = false;
        _time = 0;
        base.EndState();
    }

    protected override void Update()
    {
        base.Update();
        _time += Time.deltaTime;
        if (_time >= 2f)
        {
           // if (GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) < _manager.Stat._AttackRange)
           // {
                _manager.SetState(TiberState.ATTACK1);
            //}
        }
        else
        {
            Vector3 playerTrans = new Vector3(_manager.PlayerCapsule.transform.position.x, transform.position.y, _manager.PlayerCapsule.transform.position.z);

            _manager.CC.transform.LookAt(playerTrans);

            
            Vector3 moveDir = (playerTrans
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
