using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Sound;

public class PlayerSKILL4 : FSMState
{
    public float _time = 0;

    public override void BeginState()
    {
        base.BeginState();

        _manager.attackType = AttackType.SKILL4;

        // 시작해볼까?
        var voice = _manager._Sound.voice;
        voice.PlayPlayerVoice(_manager.gameObject, voice.skill4CastVoice);

        GameStatus.SetCurrentGameState(CurrentGameState.Product);

        StartCoroutine(MCSoundManager.BGMFadeOut(0.7f));
        StartCoroutine(MCSoundManager.AmbFadeOut(0.7f));

        UserInterface.SetAllUserInterface(false);
        _manager.isCanUltimate = false;
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;

        StartCoroutine(MCSoundManager.BGMFadeIn(0.7f));
        StartCoroutine(MCSoundManager.AmbFadeIn(0.7f));
        GameStatus.SetCurrentGameState(CurrentGameState.Start);

        UserInterface.SetAllUserInterface(true);
        _manager.TimeLine2.SetActive(false);
        _manager.isCantMove = false;
        _manager.isAttackOne = false;
        _manager.isAttackTwo = false;
        _manager.isAttackThree = false;
        _manager.isSkill4CTime = true;
        _manager.isSkill4 = false;        
    }


    private void Update()
    {
        _manager.isCantMove = _time <= 17.1f ? true : false;

        _time += Time.deltaTime;
       
        if (_time >= 17.2f)
        {
            _manager.SetState(PlayerState.IDLE);
            return;
        }

    }
}
