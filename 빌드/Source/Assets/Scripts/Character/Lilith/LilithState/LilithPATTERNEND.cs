﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithPATTERNEND : LilithFSMState
{
    float _Time1 = 0;
    float _Delay = 2f;

    bool pattern = false;

    public LilithState[] _Pattern;
    public int _PrevPhase = 0;
    [HideInInspector] public int _Turn = 0;

    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        _Time1 = 0;
        _Turn++;
        base.EndState();
    }

    protected override void Update()
    {
        base.Update();

        PhaseCheck();

        _Time1 += Time.deltaTime;

        if (_Time1 > _Delay)
        {
            if (_Turn > _Pattern.Length)
            {
                _Turn = 0;
            }

            LilithState nextState = _Pattern[_Turn];
            Debug.Log(nextState.ToString());
            _manager.SetState(nextState);

            _Time1 = 0;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void PhaseCheck()
    {
        float hpRatio = _manager.Stat.Hp / _manager.Stat.MaxHp;

        _PrevPhase = _manager._Phase;

        if (hpRatio >= _manager._PhaseThreshold[0])
        {
            _manager._Phase = 0;
        }
        else if(hpRatio >= _manager._PhaseThreshold[1])
        {
            _manager._Phase = 1;
        }
        else if(hpRatio >= _manager._PhaseThreshold[2])
        {
            _manager._Phase = 2;
        }
        else
        {
            _manager._Phase = 3;
        }

        if(_PrevPhase != _manager._Phase)
        {
            _Turn = 0;
        }
    }
}
