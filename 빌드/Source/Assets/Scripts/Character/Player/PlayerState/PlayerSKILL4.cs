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
        _manager.TimeLine2.SetActive(false);
    }


    private void Update()
    {
        _manager.isCantMove = _time <= 17.1f ? true : false;

        _time += Time.deltaTime;
       
        if (_time >= 17.2f)
        {
            _manager.SetState(PlayerState.IDLE);
            return;
        }

    }
}
