using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIDLE : FSMState
{
    float _time = 0;
    public override void BeginState()
    {
        MCSoundManager.SetSound();

        base.BeginState();
        _manager.isIDLE = true;
    }

    public override void EndState()
    {
        base.EndState();
        _manager.isIDLE = false;
    }

    private void Update()
    {
        if (_manager.OnMove())
        {
            _manager.SetState(PlayerState.RUN);
            return;
        }
        //_time += Time.deltaTime;

        //if(_time>= 1.25)
        //{
        //    _manager.TimeLine.SetActive(false);
        //    _time = 0;
        //    return;
        //}
    }
}