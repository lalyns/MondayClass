using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHIT : FSMState
{
    float _time;
    public override void BeginState()
    {
        base.BeginState();
        StartCoroutine(GameLib.Blinking(_manager.materialList));
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= 0.83f)
        {
            _manager.SetState(PlayerState.IDLE);
            return;
        }
    }
}
