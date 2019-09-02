﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATTACKBACK2 : FSMState
{
    public float _time = 0;

    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();
    }

    void Update()
    {
        _time += Time.deltaTime;

        if (!_manager.OnMove())
        {
            if(_time >= _manager._attackBack2)
            {
                _manager.SetState(PlayerState.IDLE);
                _time = 0;
                _manager.isAttackOne = false;
                _manager.isAttackTwo = false;
                return;
            }
        }
        if (_manager.OnMove())
        {
            _manager.SetState(PlayerState.RUN);
            _time = 0;
            _manager.isAttackOne = false;
            _manager.isAttackTwo = false;
            return;
        }
    }
}
