using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

[RequireComponent(typeof(RirisFSMManager))]
public class RirisFSMState : MonoBehaviour
{
    protected RirisFSMManager _manager;
    protected float _Skill1Time;

    private void Awake()
    {
        _manager = GetComponent<RirisFSMManager>();
    }

    public virtual void BeginState()
    {
        
    }

    public virtual void EndState()
    {

    }

    protected virtual void Update()
    {

    }
    
    protected virtual void FixedUpdate()
    {

    }

}
