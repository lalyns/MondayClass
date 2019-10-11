﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisAnimEvent : MonoBehaviour
{
    public RirisFSMManager _manager;

    public bool isWeapon;

    public Collider patternA;
    public Collider patternB;

    public void Start()
    {
        if(!isWeapon)
            _manager = GetComponentInParent<RirisFSMManager>();

    }

    void OnPatternATrigger()
    {
        patternA.enabled = true;
    }

    void DisablePatternATrigger()
    {
        patternA.enabled = false;
    }

    void OnPatternBTrigger()
    {
        patternB.enabled = true;
    }

    void DisablePatternBTrigger()
    {
        patternB.enabled = false;
    }

    public void PatternAJumpEnd()
    {
        RirisPATTERNA pattern = _manager.CurrentStateComponent as RirisPATTERNA;

        pattern.SetJumpState = true;

        pattern.targetPos = pattern.playerTransform.position;

        pattern._PatternAReadyEffect.SetActive(true);
        pattern._PatternAReadyEffect.transform.position = pattern.targetPos;
    }

    public void PatternAEnd()
    {
        //Debug.Log("End Call");

        RirisPATTERNA pattern = _manager.CurrentStateComponent as RirisPATTERNA;

        pattern.PatternEnd = true;

    }

    public void AddBulletPattern()
    {
        if(_manager.CurrentState == RirisState.PATTERNA)
        {
            RirisPATTERNA pattern = _manager.CurrentStateComponent as RirisPATTERNA;
            pattern.StartCoroutine(pattern.AddBullet()); 
        }
        else if (_manager.CurrentState == RirisState.PATTERNB)
        {
            RirisPATTERNB pattern = _manager.CurrentStateComponent as RirisPATTERNB;
            pattern.StartCoroutine(pattern.AddBullet());
        }
        else if (_manager.CurrentState == RirisState.PATTERNC)
        {
            RirisPATTERNC pattern = _manager.CurrentStateComponent as RirisPATTERNC;
            pattern.StartCoroutine(pattern.FireBullet());
        }
    }

    public void PatterBDashEffectUp()
    {
        RirisPATTERNB pattern = _manager.CurrentStateComponent as RirisPATTERNB;

        pattern.AttackReadyEnd();
    }

    public void PatterBEnd()
    {
        RirisPATTERNB pattern = _manager.CurrentStateComponent as RirisPATTERNB;

        pattern.isEnd = true;
    }

    public void SetOff()
    {
        BossEffects.Instance.tornaedo.ItemReturnPool(this.gameObject);
    }
}
