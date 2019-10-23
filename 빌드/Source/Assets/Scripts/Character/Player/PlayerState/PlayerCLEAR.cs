using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
public class PlayerCLEAR : FSMState
{
    [SerializeField]
    float _time = 0;
    bool isOne = false;

    public GameObject CMSet;
    public override void BeginState()
    {
        base.BeginState();
        if(_manager.CurrentClear == 0)
        {
            _manager.ClearTimeLine.SetActive(true);
        }
        else
        {
            _manager.ClearTimeLine2.SetActive(true);
        }
        UserInterface.SetPlayerUserInterface(false);
        UserInterface.SetMissionProgressUserInterface(false);
        _manager.enemyHPBar.gameObject.SetActive(false);
        _manager.Skill1Return(_manager.Skill1_Effects, _manager.Skill1_Special_Effects, _manager.isNormal);
        _manager.Skill1Return(_manager.Skill1_Shoots, _manager.Skill1_Special_Shoots, _manager.isNormal);
        _manager.Skill1PositionSet(_manager.Skill1_Effects, _manager.Skill1_Shoots, _manager.Skill1_Special_Shoots, _manager.isNormal);
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;
        isOne = false;
        _manager.ClearTimeLine.SetActive(false);
        _manager.ClearTimeLine2.SetActive(false);

        UserInterface.SetPlayerUserInterface(true);
        UserInterface.SetMissionProgressUserInterface(true);
        _manager.enemyHPBar.gameObject.SetActive(true);
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (_manager.CurrentClear == 1)
        {
            if(_time >= 3.3f && !isOne)
            {
                _manager.CurrentClear = 2;
                _manager.Anim.SetFloat("CurrentClear", _manager.CurrentClear);
                isOne = true;
            }
        }      

        if (GameStatus.currentGameState == CurrentGameState.Wait)
        {
            return;
        }
    }
}
