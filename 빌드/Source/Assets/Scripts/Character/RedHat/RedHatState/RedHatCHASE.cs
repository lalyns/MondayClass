using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatCHASE : RedHatFSMState
{
    bool _IsSpread = false;
    Vector3 playerTrans;
    public override void BeginState()
    {
        _manager.agent.velocity = Vector3.zero;
        _manager.agent.destination = this.transform.position;
        _manager.agent.acceleration = 5.0f;

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

        DahsCheck();

        this.transform.localRotation = Quaternion.RotateTowards(this.transform.rotation,
            Quaternion.LookRotation(PlayerFSMManager.GetLookTargetPos(transform) - transform.position,
            Vector3.up), 2f * Time.deltaTime);

        if (GameLib.DistanceToCharacter(_manager.CC,_manager.priorityTarget) < _manager.Stat.AttackRange)
        {
            _manager.SetState(RedHatState.ATTACK);
        }

        else
        {
            _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));

            _manager.agent.destination = playerTrans;

            if (_manager.agent.remainingDistance >= 1.5f) {
                _manager.agent.isStopped = false;
            } else {
                _manager.agent.isStopped = true;
            }
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
