using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTRANS2 : FSMState
{
    public float _time = 0;

    public override void BeginState()
    {
        base.BeginState();
        _manager._h = 0;
        _manager._v = 0;
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;
    }

    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= 0.66f)
        {            
            _manager.SetState(PlayerState.IDLE);
            return;
        }
    }
}
