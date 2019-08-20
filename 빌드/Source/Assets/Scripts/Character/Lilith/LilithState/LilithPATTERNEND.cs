using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithPATTERNEND : LilithFSMState
{
    float _Time1 = 0;
    float _Delay = 2f;

    bool pattern = false;

    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        _Time1 = 0;
        base.EndState();
    }

    protected override void Update()
    {
        base.Update();

        _Time1 += Time.deltaTime;

        if (_Time1 > _Delay)
        {
            if (pattern)
            {
                pattern = !pattern;
                _manager.SetState(LilithState.PATTERNA);
            }
            else
            {
                pattern = !pattern;
                _manager.SetState(LilithState.PATTERNB);
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
