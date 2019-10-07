using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIDLE : FSMState
{
    float _time = 0;
    public override void BeginState()
    {
        base.BeginState();
        _manager.isIDLE = true;
        _manager.isCantMove = false;
    }

    public override void EndState()
    {
        base.EndState();
        _manager.isIDLE = false;
    }

    private void Update()
    {
        if (_manager.OnMove() && !_manager.isSpecial)
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