using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LilithFSMManager))]
public class LilithFSMState : MonoBehaviour
{
    protected LilithFSMManager _manager;
    protected float _Skill1Time;

    private void Awake()
    {
        _manager = GetComponent<LilithFSMManager>();
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
        Vector3 gravity = Vector3.zero;
        gravity.y = Physics.gravity.y * Time.deltaTime;

        _manager.CC.Move(gravity);
    }

}
