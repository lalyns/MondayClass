using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisAnimEvent : MonoBehaviour
{
    public RirisFSMManager _manager;

    public void Start()
    {
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
}
