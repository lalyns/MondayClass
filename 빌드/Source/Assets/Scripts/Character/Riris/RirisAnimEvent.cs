using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisAnimEvent : MonoBehaviour
{
    public RirisFSMManager _manager;

    public bool isWeapon;

    public void Start()
    {
        if(!isWeapon)
            _manager = GetComponentInParent<RirisFSMManager>();
            
    }

    public void PatternAJumpEnd()
    {
        RirisPATTERNA pattern = _manager.CurrentStateComponent as RirisPATTERNA;

        pattern.SetJumpState = true;
    }

    public void PatternAEnd()
    {
        Debug.Log("End Call");

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
}
