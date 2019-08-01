using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacPOPUP : MacFSMState
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
        _manager.SetState(MonsterState.CHASE);
    }
}
