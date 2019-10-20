using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

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
        
        _manager.Skill2_Test.SetActive(false);
        _manager.Skill2_Test2.SetActive(false);
        _manager.isSkill2End = false;
        isEnd = false;
        UserInterface.SetPlayerUserInterface(false);
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
            GameStatus.currentGameState = CurrentGameState.Dead;
            isEnd = true;
        }

    }

    public void DeadSupport()
    {

    }
}
