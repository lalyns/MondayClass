using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHIT2 : FSMState
{
    float _time;
    public bool isEnd;
    public override void BeginState()
    {
        base.BeginState();
        var color = new Color(1, 0.3725f, 0.3725f);

        var voice = _manager._Sound.voice;

        voice.PlayPlayerVoice(this.gameObject, voice.damagedVoice);
        _manager.isAttackOne = false;
        _manager.isAttackTwo = false;
        _manager.isAttackThree = false;
    }

    public override void EndState()
    {
        base.EndState();
        isEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        _manager.isCantMove = _time <= 1.3f ? true : false;
        if (isEnd)
        {
            if (!_manager.OnMove())
            {
                _manager.SetState(PlayerState.IDLE);
                return;
            }
            if (_manager.OnMove())
            {
                _manager.SetState(PlayerState.RUN);
                return;
            }
        }
        //if (_manager.OnMove())
        //{
        //    _manager.SetState(PlayerState.RUN);
        //    return;
        //}
    }
}
