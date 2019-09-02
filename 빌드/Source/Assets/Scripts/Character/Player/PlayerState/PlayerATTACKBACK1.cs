using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATTACKBACK1 : FSMState
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

        if (!_manager.OnMove())//_manager._h == 0 && _manager._v == 0)
        {
            if (_time >= _manager._attackBack1)
            {
                _manager.SetState(PlayerState.IDLE);
                _manager.isAttackOne = false;

                _time = 0;
                return;
            }
        }

        if (_manager.OnMove())//_manager._h != 0 || _manager._v != 0)
        {
            _manager.SetState(PlayerState.RUN);
            _manager.isAttackOne = false;

            _time = 0;
            return;
        }
    }
}
