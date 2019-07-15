using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIDLE : FSMState
{
    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        if (_manager.OnMove())
            _manager.SetState(PlayerState.RUN);
    }
}