using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MC.Mission;

public class RirisPOPUP : RirisFSMState
{
    float popUpTime = 2.0f;
    float curTime = 0.0f;

    public override void BeginState()
    {
        base.BeginState();
        _manager._Weapon.gameObject.SetActive(false);
    }

    public override void EndState()
    {
        curTime = 0.0f;
        GameStatus.Instance.ActivedMonsterList.Add(this.gameObject);
        base.EndState();
    }

    protected override void Update()
    {
        base.Update();

        if (!MissionBoss._Instance.MissionOperate) return;

        curTime += Time.deltaTime;

        if (curTime > popUpTime)
        {
            _manager.SetState(RirisState.DIALOG);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
