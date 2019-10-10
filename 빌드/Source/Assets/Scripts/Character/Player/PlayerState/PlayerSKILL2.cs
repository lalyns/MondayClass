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
    public bool isEnd;
    public override void BeginState()
    {
        base.BeginState();
        isBox = false;
        isStartDamage = false;
        _manager.attackType = AttackType.SKILL2;

        var voice = _manager._Sound.voice;
        voice.PlayPlayerVoice(this.gameObject, voice.skill2Voice);


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
        isEnd = false;
        _time = 0;
    }

    void Update()
    {
        _manager.isCantMove = _time <= 0.9f ? true : false;

        _time += Time.deltaTime;

        if (_time >= 0.1f && !isBox)
        {
            _manager.Skill2_Test.SetActive(false);

            if (_manager.isNormal)
            {
                _manager.Skill2_Normal.SetActive(true);
                Debug.Log("스킬2찍혀야함.");
            }
            else
            {
                _manager.Skill2_Special.SetActive(true);
                Debug.Log("스킬2찍혀야함.");
            }

            isBox = true;
        }
     
        if (isEnd && !isStartDamage)
        {
            isStartDamage = true;
            if (_manager.OnMove())
                _manager.SetState(PlayerState.RUN); 
            else
                _manager.SetState(PlayerState.IDLE);
            return;
        }
    }
}
