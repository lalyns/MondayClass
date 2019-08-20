﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithPOPUP : LilithFSMState
{
    float _PopUpTime = 2.0f;
    float _curTime = 0.0f;

    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        _curTime = 0.0f;
        base.EndState();
    }

    protected override void Update()
    {
        base.Update();

        _curTime += Time.deltaTime;

        if (_curTime > _PopUpTime)
        {
            _manager.SetState(LilithState.PATTERNEND);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
