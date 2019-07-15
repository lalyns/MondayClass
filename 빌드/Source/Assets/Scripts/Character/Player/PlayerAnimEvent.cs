using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    PlayerFSMManager _manager;
    PlayerATTACK _attackCp;
    private void Awake()
    {
        _manager = transform.root.GetComponent<PlayerFSMManager>();
        _attackCp = _manager.GetComponent<PlayerATTACK>();
    }

    void HitCheck()
    {
        if(null != _attackCp)
        {
            _attackCp.AttackCheck();
        }
    }

}
