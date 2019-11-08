using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Sound;

public class PlayerDEAD : FSMState
{
    bool Dead = false;
    float time = 0;

    public override void BeginState()
    {
        base.BeginState();
        //GetComponent<Collider>().enabled = false;

        var voice = _manager._Sound.voice;
        voice.PlayPlayerVoice(this.gameObject, voice.dieVoice);

        GameStatus.SetCurrentGameState(CurrentGameState.Dead);

        _manager.Skill2_Test.SetActive(false);
        _manager.Skill2_Test2.SetActive(false);
        _manager.isSkill2End = false;
        _manager.Stat.lastHitBy = null;
        isEnd = false;
        UserInterface.SetPlayerUserInterface(false);
        UserInterface.SetMissionProgressUserInterface(false);

        CanvasInfo.Instance.enemyHP.SetFalse();

        _manager.Skill1Return(_manager.Skill1_Effects, _manager.Skill1_Special_Effects, _manager.isNormal);
        _manager.Skill1Return(_manager.Skill1_Shoots, _manager.Skill1_Special_Shoots, _manager.isNormal);
        _manager.Skill1PositionSet(_manager.Skill1_Effects, _manager.Skill1_Shoots, _manager.Skill1_Special_Shoots, _manager.isNormal);

        StartCoroutine(MCSoundManager.AmbFadeOut(0.4f));
        StartCoroutine(MCSoundManager.BGMFadeOut(0.4f));

        var sound = MCSoundManager.Instance.objectSound.ui;
        sound.PlaySound(_manager.gameObject, sound.dead);
    }

    public override void EndState()
    {
        base.EndState();
        UserInterface.SetPlayerUserInterface(true);
    }

    bool isEnd = false;

    private void Update()
    {
        time += Time.deltaTime;

        if(time <= 1f)
        {
            _manager.colorGrading.saturation.value -= 2f;
        }

        if (_manager.colorGrading.saturation.value <= -85f && !isEnd)
        {
            _manager.colorGrading.saturation.value = -85f;
            UserInterface.FailMissionSetActive(true);
            isEnd = true;
        }

    }

    public void DeadSupport()
    {

    }
}
