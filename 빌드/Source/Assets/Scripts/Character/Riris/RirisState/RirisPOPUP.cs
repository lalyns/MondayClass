using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisPOPUP : RirisFSMState
{
    float _PopUpTime = 2.0f;
    float _curTime = 0.0f;

    public override void BeginState()
    {
        base.BeginState();
        _manager._Weapon.gameObject.SetActive(false);
    }

    public override void EndState()
    {
        _curTime = 0.0f;
        GameStatus.Instance.ActivedMonsterList.Add(this.gameObject);
        base.EndState();
    }

    protected override void Update()
    {
        base.Update();

        if (!MissionBoss._Instance.MissionOperate) return;

        _curTime += Time.deltaTime;

        if (_curTime > _PopUpTime)
        {
            _manager.SetState(RirisState.PATTERNEND);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
