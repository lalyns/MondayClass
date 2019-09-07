using System.Collections;
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

        EffectPoolManager._Instance._MacBulletPool.ItemSetActive(
            bulletLuancher, 
            FSMManager.CC,
            FSMManager.PlayerCapsule);

    }

    public void CastingSkill()
    {
        EffectPoolManager._Instance._MacSkillPool.ItemSetActive(skillLuancher,
            this.GetComponentInParent<MacFSMManager>().CC,
            this.GetComponentInParent<MacFSMManager>().PlayerCapsule);
    }
}
