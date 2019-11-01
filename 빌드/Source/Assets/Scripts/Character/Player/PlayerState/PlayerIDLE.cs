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
        _manager.CurrentIdle = Random.Range((int)1, (int)4);
    }

    public override void EndState()
    {
        base.EndState();
        _manager.isIDLE = false;
        
        _time = 0;
    }

    private void Update()
    {
        if (_manager.OnMove() && !_manager.isSpecial)
        {
            _manager.SetState(PlayerState.RUN);
            return;
        }

        _time += Time.deltaTime;

        if(_time >= 3f)
        {
            _manager.SetState(PlayerState.IDLE2);
            _manager.Anim.SetFloat("CurrentIdle", (int)_manager.CurrentIdle);
            _manager.isSpecialIDLE = true;
            return;
        }
        
        //if(_time>= 1.25)
        //{
        //    _manager.TimeLine.SetActive(false);
        //    _time = 0;
        //    return;
        //}
    }
}