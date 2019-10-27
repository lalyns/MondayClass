using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Mission;

public class RirisPHASE : RirisFSMState
{
    public override void BeginState()
    {
        base.BeginState();

        BossDirector.Instance.PlayPhaseChangeCine();
    }

    public override void EndState()
    {
        base.EndState();


    }

    float _time = 0;
    protected override void Update()
    {
        base.Update();

        _time += Time.deltaTime;
        if(_time >= 5f)
        {
            _time = 0;
            _manager.SetState(RirisState.PATTERNEND);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void DeadHelper()
    {
    }
}
