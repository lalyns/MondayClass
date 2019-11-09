﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Mission;
using MC.Sound;

public class RirisPHASE : RirisFSMState
{
    public GameObject phaseEffect;

    public override void BeginState()
    {
        base.BeginState();

        //StartCoroutine(MCSoundManager.AmbFadeOut(1f, 30f));
        //StartCoroutine(MCSoundManager.BGMFadeOut(1f, 30f));
        BossDirector.Instance.PlayPhaseChangeCine();
        phaseEffect.SetActive(true);
    }

    public override void EndState()
    {
        base.EndState();

        //StartCoroutine(MCSoundManager.AmbFadeIn(1f, 30f));
        //StartCoroutine(MCSoundManager.BGMFadeIn(1f, 30f));
        var sound = _manager.sound.ririsSFX;
        sound.PlayRirisSFX(this.gameObject, sound.ririsPowerUpLoop);
    }

    float _time = 0;
    protected override void Update()
    {
        base.Update();

        _time += Time.deltaTime;
        if (_time >= 5f)
        {
            _time = 0;
            _manager.SetState(RirisState.PATTERNEND);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
