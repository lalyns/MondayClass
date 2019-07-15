using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerFSMManager))]
public class FSMState : MonoBehaviour
{
    protected PlayerFSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<PlayerFSMManager>();    
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
}
