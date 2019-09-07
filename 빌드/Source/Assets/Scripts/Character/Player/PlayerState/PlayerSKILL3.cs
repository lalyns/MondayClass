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
        isAttack = false;
    }
    bool isAttack;
    private void Update()
    {
        //1.7초동안 못움직임.
        _manager.isCantMove = _time <= 1.7f ? true : false;

        _time += Time.deltaTime;


        if (isAttack)
            _manager.Skill3Attack();
        else
            _manager.Skill3Cancle();


        if ((_time >= 1.2f && _time < 1.4f) 
            || (_time >= 1.7f && _time < 1.9f) 
            || (_time >= 2.2f && _time < 2.4f) 
            || (_time >= 2.7f && _time < 2.9f) 
            || (_time >= 3.2f && _time < 3.4f) 
            || (_time >= 3.7f && _time < 3.9f))
        {
            isAttack = true;
        }

        else
            isAttack = false;

        if (_time >= 1.7f)
        {
            if (_manager.OnMove())
            {
                _manager.SetState(PlayerState.RUN);
            }
        }
        if (_time >= 4.2f)
        {
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
