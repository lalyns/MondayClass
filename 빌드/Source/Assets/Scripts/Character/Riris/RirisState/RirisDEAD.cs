using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Mission;

public class RirisDEAD : RirisFSMState
{
    public override void BeginState()
    {
        base.BeginState();

        BossDirector.Instance.PlayDeadCine();
    }

    public override void EndState()
    {
        base.EndState();


    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void DeadHelper()
    {
    }
}
