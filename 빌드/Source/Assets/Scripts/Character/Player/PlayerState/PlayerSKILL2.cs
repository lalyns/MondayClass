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
    bool isBox;
    public override void BeginState()
    {
        base.BeginState();
        isBox = false;

        _manager.attackType = AttackType.SkILL2;

    }

    public override void EndState()
    {
        base.EndState();
        //_manager.isSkill2 = false;
        _manager.isAttackOne = false;
        _manager.isAttackTwo = false;
        _manager.isAttackThree = false;

        _manager.isSkill2CTime = true;
    }

    void Update()
    {
        _manager.isCantMove = _time <= 0.9f ? true : false;

        _time += Time.deltaTime;

        if (_time >= 0.1f && !isBox)
        {
            //Instantiate(_manager.Skill2_Start, _manager.Skill2_Parent.transform.position, _manager.Skill2_Start.transform.rotation);
            _manager.Skill2_Start.SetActive(true);
            isBox = true;
        }

        if (_time >= 1f)
        {
            _time = 0;
            if (_manager.OnMove())
                _manager.SetState(PlayerState.RUN); 
            else
                _manager.SetState(PlayerState.IDLE);
            return;
        }
    }
}
