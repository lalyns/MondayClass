using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterFSMManager))]
public class MonsterFSMState : MonoBehaviour
{
    protected MonsterFSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<MonsterFSMManager>();    
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
