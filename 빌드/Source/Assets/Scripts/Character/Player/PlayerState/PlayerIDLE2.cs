using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIDLE2 : FSMState
{
    float _time = 0;
    float _idle12 = 6.66f;
    float _idle3 = 7.5f;

    public override void BeginState()
    {
        base.BeginState();
        _manager.isIDLE = true;
    }

    public override void EndState()
    {
        base.EndState();
        _manager.isSpecialIDLE = false;
        _manager.isIDLE = false;
        _time = 0;
    }

    private void Update()
    {
        if (_manager.OnMove() && !_manager.isSpecial)
        {
            _manager.SetState(PlayerState.RUN);
            return;
        }
        _time += Time.deltaTime;
        if(_manager.CurrentIdle == 3)
        {
            if (_time >= 7.5f)
                _manager.SetState(PlayerState.IDLE);
        }
        else
        {
            if (_time >= 6.66f)
                _manager.SetState(PlayerState.IDLE);
        }
    }
}
