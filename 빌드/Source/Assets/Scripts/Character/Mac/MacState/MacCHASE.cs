using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacCHASE : MacFSMState
{
    bool isSpread = false;
    Vector3 playerPos;

    public override void BeginState()
    {

        _manager.agent.acceleration = 0.5f;
        base.BeginState();
    }

    public override void EndState()
    {
        _manager.agent.isStopped = true;

        isSpread = false;

        base.EndState();
    }

    protected override void Update()
    {
        base.Update();

        this.transform.localRotation = Quaternion.RotateTowards(this.transform.rotation,
            Quaternion.LookRotation(PlayerFSMManager.GetLookTargetPos(transform) - transform.position,
            Vector3.up), 2f * Time.deltaTime);

        playerPos = new Vector3(_manager.PlayerCapsule.transform.position.x, transform.position.y, _manager.PlayerCapsule.transform.position.z);

        if (_manager.agent.remainingDistance < _manager.Stat.AttackRange)
        {
            _manager.SetState(MacState.ATTACK);
            _manager.agent.isStopped = true;
        }        
        else
        {
            _manager.agent.destination = playerPos;
            _manager.agent.isStopped = false;
            _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
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
