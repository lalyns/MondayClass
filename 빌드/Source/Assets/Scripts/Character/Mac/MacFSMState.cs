using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MacFSMManager))]
public class MacFSMState : MonoBehaviour
{
    protected MacFSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<MacFSMManager>();
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
