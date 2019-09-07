using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSKILL2 : FSMState
{
    public float SKill2_installRange;
    public float Skill2_installSpeed;
    public float Skill2_UseTIme;
    public float Skill2_EffectRange;
    public float SKill2_CollTime;

    float _time;
    public override void BeginState()
    {
        base.BeginState();
        
    }

    public override void EndState()
    {
        base.EndState();
    }

    void Update()
    {
        _manager.isCantMove = _time <= 0.9f ? true : false;

        _time += Time.deltaTime;

        if (_time >= 0.5f)
        {
            _manager.Skill2_Start.SetActive(true);

        }

        if (_time >= 1f)
        {
            _time = 0;
            _manager.SetState(PlayerState.IDLE);
            return;
        }
    }
}
