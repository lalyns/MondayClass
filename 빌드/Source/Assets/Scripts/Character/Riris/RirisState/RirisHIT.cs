﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisHIT : RirisFSMState
{
    public bool hitEnd = false;


    public override void BeginState()
    {
        base.BeginState();



    }
    public override void EndState()
    {
        base.EndState();

        StopAllCoroutines();
        _manager.CurrentAttackType = AttackType.NONE;
    }

    protected override void Update()
    {
        base.Update();
        if (!PlayerFSMManager.Instance.isSpecial && !PlayerFSMManager.Instance.isSkill4)
            _manager.SetState(RirisState.PATTERNEND);

        if (PlayerFSMManager.Instance.isSkill4)
        {
            PlayerStat playerStat = PlayerFSMManager.Instance.Stat;
            if (!PlayerFSMManager.Instance.isCantMove && !isHit)
            {
                _manager.Stat.TakeDamage(playerStat, playerStat.dmgCoefficient[6]);
                isHit = true;
            }
        }
        if (_manager.Stat.Hp <= 0)
            _manager.SetDeadState();
    }
    bool isHit = false;
}
