using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCLEAR : FSMState
{
    [SerializeField]
    float _time = 0;
    bool isOne = false;
    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();
        _time = 0;
        isOne = false;
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
        if (_time >= 4.1f)
        {
            _time = 0;
            _manager.SetState(PlayerState.IDLE);
            return;
        }
    }
}
