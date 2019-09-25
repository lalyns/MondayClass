﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacAnimEvent : MonoBehaviour
{
    MacFSMManager _Manager;
    MacFSMManager FSMManager {
        get {
            if(_Manager == null)
            {
                _Manager = this.GetComponentInParent<MacFSMManager>();
            }

            return _Manager;
        }
    }

    public MacHIT _hitCp;
    public MacDEAD _deadCp;

    public Transform bulletLuancher;
    public Transform skillLuancher;

    public void PopupOver()
    {
        FSMManager.SetState(MacState.CHASE);
    }

    public void AttackOver()
    {
        FSMManager.SetState(MacState.RUNAWAY);
    }

    public void CastingAttack()
    {
        MacATTACK attack = _Manager.CurrentStateComponent as MacATTACK;
        try
        {
            attack.isLookAt = false;
        }
        catch
        {

        }
        EffectPoolManager._Instance._MacBulletPool.ItemSetActive(
            bulletLuancher, 
            FSMManager.CC,
            FSMManager._PriorityTarget);

    }

    public void CastingSkill()
    {
        MacSKILL skill = _Manager.CurrentStateComponent as MacSKILL;
        try
        {
            skill.isLookAt = false;
        }
        catch
        {

        }
        EffectPoolManager._Instance._MacSkillPool.ItemSetActive(skillLuancher,
            FSMManager.CC,
            FSMManager._PriorityTarget);
    }

    public void HitEnd()
    {
        _hitCp.HitEnd();
    }

    public void NotifyDead()
    {
        _deadCp.DeadHelper();
    }
}
