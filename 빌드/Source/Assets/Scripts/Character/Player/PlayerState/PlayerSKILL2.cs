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
        _time = 0;
        isBox = false;
        isStartDamage = false;
        _manager.attackType = AttackType.SKILL2;

        var voice = _manager._Sound.voice;
        voice.PlayPlayerVoice(this.gameObject, voice.skill2Voice);

        var sfx = _manager._Sound.sfx;

        if(_manager.isNormal)
            sfx.PlayPlayerSFX(_manager.Skill2_Test, sfx.skill2SFX);
        else
            sfx.PlayPlayerSFX(_manager.Skill2_Test2, sfx.skill2SFX);
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
            _manager.Skill2_Test2.SetActive(false);
            if (_manager.isNormal)
            {
                _manager.Skill2_Normal.SetActive(true);
            }
            else
            {
                _manager.Skill2_Special.SetActive(true);
            }            
            isBox = true;
        }
        //if(_time >= 0.66f)
        //{
        //    _manager.isSkill2Dash = false;
        //}

        if (!_manager.isSkill2Dash && !isStartDamage)
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
