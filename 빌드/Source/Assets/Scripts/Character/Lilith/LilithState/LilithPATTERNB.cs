﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithPATTERNB : LilithFSMState
{
    float _Time1 = 0;
    bool _IsTele = false;

    public Transform _MapCenter;

    public GameObject PatternBReadyEffect;
    public GameObject PatternBAttackEffect;
    bool _IsAttackReady = false;
    public float _AttackReadyTime = 1f;

    float _Time2 = 0;
    float _AttackEndTime = 2;



    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();

        _IsAttackReady = false;
        _IsTele = false;
        _Time1 = 0;
        _Time2 = 0;
        PatternBReadyEffect.SetActive(false);
        PatternBAttackEffect.SetActive(false);

    }

    protected override void Update()
    {
        base.Update();
        
        if(!_IsTele)
        {
            _manager.TelePortToPos(_MapCenter.position);
            _IsTele = false;
        }

        _Time1 += Time.deltaTime;

        if(_Time1 < _AttackReadyTime)
        {
            if (!_IsAttackReady)
            {
                PatternBReadyEffect.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
                PatternBAttackEffect.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
                PatternBReadyEffect.SetActive(true);
            }
        }

        else
        {
            _Time2 += Time.deltaTime;
            _IsAttackReady = true;

            PatternBReadyEffect.SetActive(false);
            PatternBAttackEffect.SetActive(true);
        }

        if(_Time2 > _AttackEndTime)
        {
            _manager.SetState(LilithState.PATTERNEND);
        }

    }

    public void AttackCheck()
    {
        var hitTarget = GameLib.SimpleDamageProcess(transform,
            _manager.Stat.AttackRange,
            "Player", _manager.Stat);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
