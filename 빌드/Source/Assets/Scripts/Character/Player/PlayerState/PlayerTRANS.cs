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
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;
    }

    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= 3.73f)
        {
            _manager.SetState(PlayerState.TRANS2);
            return;
        }
    
    }
}
