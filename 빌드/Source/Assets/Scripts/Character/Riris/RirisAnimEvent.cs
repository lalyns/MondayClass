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

    public void PatternAStompEnd()
    {
        RirisPATTERNA pattern = _manager.CurrentStateComponent as RirisPATTERNA;

        pattern.StompEnd = true;
        
    }

    public void PatternAEnd()
    {
        RirisPATTERNA pattern = _manager.CurrentStateComponent as RirisPATTERNA;

        pattern.PatternEnd = true;

    }

    public void PatterBDashEffectUp()
    {
        RirisPATTERNB pattern = _manager.CurrentStateComponent as RirisPATTERNB;

        pattern.AttackReadyEnd();
    }
}
