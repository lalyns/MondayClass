using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATTACK1 : FSMState
{
    public float _time = 0;

    public override void BeginState()
    {
        base.BeginState();
        _manager._Sound.PlayAttackSFX();
        _manager.attackType = AttackType.ATTACK1;
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;

    }

    private void Update()
    {
        _time += Time.deltaTime;

        _manager.isCantMove = _time <= _manager._attack1Time-0.2f ? true : false;
        
        if (Input.GetMouseButtonDown(0) && !_manager.isAttackTwo)
        {
            _manager.isAttackTwo = true;
        }
        if (_manager.isAttackTwo)
        {
            if (_time >= _manager._attack1Time)
            {
                _manager.SetState(PlayerState.ATTACK2);
                _time = 0;

                return;
            }
        }
        if (!_manager.isAttackTwo)
        {
            // 만약 플레이어가 키를 누르고 있지 않으면 공격 회수 애니 재생 후 그것마저 끝나면 IDLE로 보내짐.

            if (_time >= _manager._attack1Time)
            {
                if (!_manager.OnMove())//_manager._h == 0 && _manager._v == 0)
                {
                    _manager.SetState(PlayerState.ATTACKBACK1);
                    _time = 0;
                    return;
                }
            }
            // 만약 플레이어가 키를 누르고 있으면 공격 애니가 끝나자마자 바로 누르고 있는 값의 방향으로 RUN

            if (_time >= _manager._attack1Time)
            {
                if (_manager.OnMove())//_manager._h !=0 || _manager._v != 0)
                {
                    _manager.SetState(PlayerState.RUN);
                    _manager.isAttackOne = false;

                    _time = 0;
                    return;
                }
            }
        }
    }
}
