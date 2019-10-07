using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSKILL4 : FSMState
{
    public float _time = 0;

    public override void BeginState()
    {
        base.BeginState();

        _manager.attackType = AttackType.SKILL4;


    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;
        _manager.isCantMove = false;
        _manager.isAttackOne = false;
        _manager.isAttackTwo = false;
        _manager.isAttackThree = false;
        
        _manager.isSkill4CTime = true;
        _manager.isSkill4 = false;
    }


    private void Update()
    {
        _manager.isCantMove = _time <= 15.3f ? true : false;

        _time += Time.deltaTime;
       
        if (_time >= 15.5f)
        {
            _manager.SetState(PlayerState.IDLE);
            return;
        }

    }
}
