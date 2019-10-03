using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSKILL2 : FSMState
{
    public float SKill2_installRange;
    public float Skill2_installSpeed;
    public float Skill2_UseTIme;
    public float Skill2_EffectRange;
    public float SKill2_CollTime;

    float _time;
    bool isBox, isStartDamage;
    public override void BeginState()
    {
        base.BeginState();
        isBox = false;
        isStartDamage = false;
        _manager.attackType = AttackType.SKILL2;

    }

    public override void EndState()
    {
        base.EndState();
        //_manager.isSkill2 = false;
        _manager.isAttackOne = false;
        _manager.isAttackTwo = false;
        _manager.isAttackThree = false;

        _manager.isSkill2CTime = true;
        _manager.isSkill2End = false;
    }

    void Update()
    {
        _manager.isCantMove = _time <= 0.9f ? true : false;

        _time += Time.deltaTime;

        if (_time >= 0.1f && !isBox)
        {
            _manager.Skill2_Test.SetActive(false);
            _manager.Skill2_Start.SetActive(true);
            isBox = true;
        }
        

        if (_time >= 1f && !isStartDamage)
        {
            _time = 0;
            isStartDamage = true;
            if (_manager.OnMove())
                _manager.SetState(PlayerState.RUN); 
            else
                _manager.SetState(PlayerState.IDLE);
            return;
        }
    }
}
