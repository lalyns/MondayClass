using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSKILL3 : FSMState
{
    public float _time = 0;

    public override void BeginState()
    {
        base.BeginState();
        _manager.Skill3_Start.SetActive(true);
        _manager.Skill3_End.SetActive(false);
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;
        _manager.isSkill3 = false;
        _manager.Skill3_Start.SetActive(false);
    }

    private void Update()
    {
        //1.2초동안 못움직임.
        _manager.isCantMove = _time <= 1.7f ? true : false;

        _time += Time.deltaTime;

        
       
        if (_time >= 1.2f)
        {
            _manager.Anim.SetInteger("CurrentState", 9);

         
        }
        if (_time >= 1.7f)
        {
            if (_manager.OnMove())
            {
                _manager.SetState(PlayerState.RUN);
            }
        }
        if(_time>= 4.0f)
        {
            _manager.Anim.SetInteger("CurrentState", 10);
            _manager.Skill3_End.SetActive(true);
            if (_manager.OnMove())
            {
                _manager.SetState(PlayerState.RUN);
            }
        }
        if (_time >= 4.8f)
        {
            _manager.SetState(PlayerState.IDLE);
            return;
        }

    }
}
