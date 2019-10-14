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
        _manager.Skill1Return(_manager.Skill1_Effects, _manager.Skill1_Special_Effects, _manager.isNormal);
        _manager.Skill1Return(_manager.Skill1_Shoots, _manager.Skill1_Special_Shoots, _manager.isNormal);
        _manager.Skill1PositionSet(_manager.Skill1_Effects, _manager.Skill1_Shoots, _manager.Skill1_Special_Shoots, _manager.isNormal);
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
