using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiberHIT : TiberFSMState
{
    float time = 2.0f;
    float curTime = 0.0f;

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

        curTime += Time.deltaTime;

        if (curTime > time)
        {
            _manager.SetState(TiberState.CHASE);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
