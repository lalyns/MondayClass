using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacRUNAWAY : MacFSMState
{
    public bool _SetTarget = false;
    Vector3 TargetPos;

    public override void BeginState()
    {
        base.BeginState();
        TargetPos = Vector3.zero;
        SetTarget();
    }

    public override void EndState()
    {
        base.EndState();
        _SetTarget = false;
    }

    private void Update()
    {
        if (!_SetTarget) return;
        
        if(Vector3.Distance(this.transform.position, TargetPos) > 1f)
        {
            transform.position = Vector3.Lerp(this.transform.position, TargetPos, 0.5f * Time.deltaTime);
        }

        else
        {
            _manager.SetState(MacState.CHASE);
        }
    }

    public void SetTarget()
    {
        TargetPos = transform.position - transform.forward * 5f;
        _SetTarget = true;
    }

}
