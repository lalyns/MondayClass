using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCatcherHIT : DreamCatcherFSMState
{
    float time = 0.5f;
    float curtime = 0.0f;

    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();

        curtime = 0;
    }

    protected override void Update()
    {
        base.Update();

        curtime += Time.deltaTime;

        if (curtime > time)
        {
            _manager.SetState(DreamCatcherState.CHASE);
            curtime = 0;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
