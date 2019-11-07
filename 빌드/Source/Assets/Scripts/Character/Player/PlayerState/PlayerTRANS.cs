using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTRANS : FSMState
{
    public float _time = 0;

    public override void BeginState()
    {
        base.BeginState();
        _manager.isCantMove = true;

        _manager.Skill2_Test.SetActive(false);
        _manager.Skill2_Test2.SetActive(false);
        _manager.isSkill2End = false;
        _manager.Stat.SetHp(_manager.Stat.MaxHp);
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;
    }

    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= 5f)
        {
            _manager.SetState(PlayerState.TRANS2);
            return;
        }
    
    }
}
