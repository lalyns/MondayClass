using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiberPOPUP : TiberFSMState
{
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
        _manager.SetState(TiberState.CHASE);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
