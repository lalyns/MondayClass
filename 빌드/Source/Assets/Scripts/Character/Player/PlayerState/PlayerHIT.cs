﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHIT : FSMState
{
    float _time;
    public override void BeginState()
    {
        base.BeginState();
        var color = new Color(1, 0.3725f, 0.3725f);

        var voice = _manager._Sound.voice;

        voice.PlayPlayerVoice(this.gameObject, voice.damagedVoice);
    }

    public override void EndState()
    {
        base.EndState();

        _time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= 0.83f)
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
}
