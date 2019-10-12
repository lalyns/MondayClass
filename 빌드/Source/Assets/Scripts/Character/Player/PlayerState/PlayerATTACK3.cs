using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATTACK3 : FSMState
{
    public float _time = 0;
    bool isAttackOne;
    public override void BeginState()
    {
        base.BeginState();
        _manager.isHardAttack = true;
        isAttackOne = false;
        _manager._Sound.sfx.PlayPlayerSFX(this.gameObject, _manager._Sound.sfx.attackSFX);
        _manager.attackType = AttackType.ATTACK3;

        var voice = _manager._Sound.voice;
        voice.PlayPlayerVoice(this.gameObject, voice.swingHardVoice);


    }

    public override void EndState()
    {
        base.EndState();
        _manager.isAttackOne = false;
        _manager.isAttackTwo = false;
        _manager.isAttackThree = false;
        _manager.isHardAttack = false;
        _time = 0;
        _manager.isCantMove = false;
    }

    private void Update()
    {
        _manager.isCantMove = _time <= _manager._attack3Time - 0.1f ? true : false;

        _time += Time.deltaTime;
        if (_manager.isAttackThree)
        {
            if ((Input.GetMouseButtonDown(0) && !isAttackOne) && _time >= _manager._attack3Time)
            {
                isAttackOne = true;
            }
            if (isAttackOne)
            {
                if (_time >= _manager._attack3Time)
                {
                    _manager.SetState(PlayerState.ATTACK1);
                    _time = 0;
                    return;
                }
            }
            if (!isAttackOne && _manager._h == 0 && _manager._v == 0)
            {
                if (_time >= _manager._attack3Time)
                {
                    _manager.SetState(PlayerState.IDLE);
                    _manager.isAttackOne = false;
                    _manager.isAttackTwo = false;
                    _manager.isAttackThree = false;
                    _time = 0;
                    return;
                }
            }
            if (!isAttackOne && (_manager._h != 0 || _manager._v != 0))
            {
                if (_time >= 0.5f)
                {
                    _manager.SetState(PlayerState.RUN);
                    _manager.isAttackOne = false;
                    _manager.isAttackTwo = false;
                    _manager.isAttackThree = false;
                    _time = 0;
                    return;
                }
            }
        }
    }
}