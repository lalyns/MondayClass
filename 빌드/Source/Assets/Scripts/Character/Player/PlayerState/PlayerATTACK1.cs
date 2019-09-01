using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATTACK1 : FSMState
{
    public float _time = 0;
    
    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();
        _manager.isAttackOne = false;
    }
    private void Update()
    {
        _time += Time.deltaTime;

        if(Input.GetMouseButtonDown(0) && !_manager.isAttackTwo)
        {
            _manager.isAttackTwo = true;
        }
        if (_manager.isAttackTwo)
        {
            if (_time >= _manager._attack1Time)
            {
                _manager.SetState(PlayerState.ATTACK2);
                _time = 0;
                return;
            }
        }
        if (!_manager.isAttackTwo)
        {
            if (_time >= _manager._attack1Time)
            {
                _manager.SetState(PlayerState.IDLE);
                _time = 0;
                return;
            }
        }

    }
   
}
