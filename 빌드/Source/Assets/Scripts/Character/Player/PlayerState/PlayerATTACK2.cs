using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATTACK2 : FSMState
{
    public float _time = 0;

    public override void BeginState()
    {
        base.BeginState();
        _manager._Sound.sfx.PlayPlayerSFX(this.gameObject, _manager._Sound.sfx.attackSFX);
        _manager.attackType = AttackType.ATTACK2;

        var voice = _manager._Sound.voice;
        if (_manager.isNormal)
        {
            voice.PlayPlayerVoice(this.gameObject, voice.swingNormalVoice);
        }
        else
        {
            voice.PlayPlayerVoice(this.gameObject, voice.swingSpeicialVoice);
        }
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;
        _manager.isCantMove = false;
    }

    private void Update()
    {
        _manager.isCantMove = _time <= _manager._attack2Time - 0.1f ? true : false;
        _time += Time.deltaTime;

        if (_manager.isAttackTwo)
        {
            if (Input.GetMouseButtonDown(0) && !_manager.isAttackThree)
            {
                _manager.isAttackThree = true;
            }
            if (_manager.isAttackThree)
            {
                if (_time >= _manager._attack2Time)
                {
                    _manager.SetState(PlayerState.ATTACK3);
                    _time = 0;
                    return;
                }
            }
            if (!_manager.isAttackThree)
            {

                if (_time >= _manager._attack2Time)
                {
                    if (!_manager.OnMove())
                    {
                        _manager.SetState(PlayerState.ATTACKBACK2);

                        _time = 0;
                        return;
                    }
                }


                if (_time >= _manager._attack2Time)
                {
                    if (_manager.OnMove())
                    {
                        _manager.SetState(PlayerState.RUN);
                        _manager.isAttackOne = false;
                        _manager.isAttackTwo = false;
                        _time = 0;
                        return;
                    }
                }
            }
        }
    }
}