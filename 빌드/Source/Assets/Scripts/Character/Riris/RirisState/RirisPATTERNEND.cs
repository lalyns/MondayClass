using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RirisPhase
{
    public RirisState[] patterns;
}

public class RirisPATTERNEND : RirisFSMState
{
    private float delayCount = 0;
    public float delay = 5f;

    public RirisPhase[] ririsPhases = new RirisPhase[3];

    private int prevPhase = 0;
    private int turn = 0;

    private bool isDead = false;

    private bool isPhase3Init = false;

    public override void BeginState()
    {
        base.BeginState();

        PhaseCheck();
    }

    public override void EndState()
    {
        delayCount = 0;
        turn++;

        if (turn >= ririsPhases[_manager._Phase].patterns.Length)
        {
            turn = 0;
        }

        
        if (isPhase3Init)
        {
            isPhase3Init = false;
            BossDirector.Instance.PlayPhaseChangeCine();
        }

        base.EndState();
    }

    protected override void Update()
    {
        base.Update();


        delayCount += Time.deltaTime;

        if (!isDead && delayCount > delay)
        {
            RirisState nextState = ririsPhases[_manager._Phase].patterns[turn];
            _manager.SetState(nextState);

            delayCount = 0;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void PhaseCheck()
    {
        float hpRatio = _manager.Stat.Hp / _manager.Stat.MaxHp;
        //Debug.Log("HP 비율 : " + hpRatio);

        prevPhase = _manager._Phase;

        if (hpRatio >= _manager._PhaseThreshold[0])
        {
            _manager._Phase = 0;
        }
        else if(hpRatio >= _manager._PhaseThreshold[1])
        {
            _manager._Phase = 1;
        }
        else if(hpRatio >= _manager._PhaseThreshold[2])
        {
            _manager._Phase = 2;

            if(prevPhase != _manager._Phase)
            {
                isPhase3Init = true;
            }
        }
        else
        {
            _manager.SetState(RirisState.DEAD);
        }

        if(prevPhase != _manager._Phase)
        {
            turn = 0;
        }
    }
}
