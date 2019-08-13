using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TiberFSMManager))]
public class TiberFSMState : MonoBehaviour
{
    protected TiberFSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<TiberFSMManager>();
    }

    public virtual void BeginState()
    {

    }

    public virtual void EndState()
    {

    }

    private void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {

        Vector3 gravity = Vector3.zero;
        gravity.y = Physics.gravity.y * Time.deltaTime;

        _manager.CC.Move(gravity);
    }

}
