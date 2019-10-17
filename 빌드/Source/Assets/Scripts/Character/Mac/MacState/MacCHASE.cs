using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacCHASE : MacFSMState
{
    bool _IsSpread = false;
    Vector3 playerTrans;
    public override void BeginState()
    {

        base.BeginState();
    }

    public override void EndState()
    {
        _manager.agent.isStopped = true;

        _IsSpread = false;

        base.EndState();
    }

    protected override void Update()
    {
        base.Update();

        playerTrans = new Vector3(_manager.PlayerCapsule.transform.position.x, transform.position.y, _manager.PlayerCapsule.transform.position.z);

        if (GameLib.DistanceToCharacter(_manager.CC, _manager._PriorityTarget) < _manager.Stat.AttackRange)
        {
            _manager.SetState(MacState.ATTACK);
        }        
        else
        {
            _manager.agent.destination = playerTrans;
            _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
            if (_manager.agent.remainingDistance >= 1.5f) {
                _manager.agent.isStopped = false;
            } else {
                _manager.agent.isStopped = true;
            }
            //_manager.CC.transform.LookAt(_manager._PriorityTarget.transform);

            //Vector3 moveDir = (_manager._PriorityTarget.transform.position
            //    - _manager.CC.transform.position).normalized;

            //moveDir.y = 0;

            //if ((_manager.CC.collisionFlags & CollisionFlags.Sides) != 0)
            //{
            //    Vector3 correctDir = Vector3.zero;
            //    if (!_IsSpread)
            //    {
            //        correctDir = DecideSpreadDirection();
            //        _IsSpread = true;
            //    }

            //    moveDir += correctDir;
            //}

            //_manager.CC.Move(moveDir * _manager.Stat.statData._MoveSpeed * Time.deltaTime);

        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private Vector3 DecideSpreadDirection()
    {
        Vector3 correctDir;

        correctDir = UnityEngine.Random.Range(1, 100) % 2 == 0 ? transform.right : -transform.right;
        correctDir += transform.forward;

        return correctDir;
    }

}
