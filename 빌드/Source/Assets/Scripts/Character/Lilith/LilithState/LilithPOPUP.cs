using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithPOPUP : LilithFSMState
{
    float _PopUpTime = 2.0f;
    float _curTime = 0.0f;

    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        _curTime = 0.0f;
        GameStatus._Instance.ActivedMonsterList.Add(this.gameObject);
        base.EndState();
    }

    protected override void Update()
    {
        base.Update();

        if (!TempMissionBoss._Instance.missionTrigger.isStart) return;

        _curTime += Time.deltaTime;

        if (_curTime > _PopUpTime)
        {
            _manager.SetState(LilithState.PATTERNEND);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
