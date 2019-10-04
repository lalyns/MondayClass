using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

[RequireComponent(typeof(RirisFSMManager))]
public class RirisFSMState : MonoBehaviour
{
    protected RirisFSMManager _manager;
    protected float _Skill1Time;

    protected bool useGravity = true;

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


    public virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }
    
    protected virtual void FixedUpdate()
    {
        if (useGravity)
            Gravity();
    }

    public void Gravity()
    {

        Vector3 gravity = Vector3.zero;
        gravity.y = Physics.gravity.y * Time.deltaTime;

        _manager.CC.Move(gravity);
    }

}
