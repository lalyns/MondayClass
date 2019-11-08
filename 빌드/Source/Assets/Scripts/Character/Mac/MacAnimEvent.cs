using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacAnimEvent : MonoBehaviour
{
    MacFSMManager mac;
    MacFSMManager Mac {
        get {
            if(mac == null)
            {
                mac = this.GetComponentInParent<MacFSMManager>();
            }

            return mac;
        }
    }

    public MacHIT _hitCp;
    public MacDEAD _deadCp;

    public Transform bulletLuancher;
    public Transform skillLuancher;

    public void PopupOver()
    {
        Mac.SetState(MacState.CHASE);
    }

    public void AttackOver()
    {
        Mac.SetState(MacState.RUNAWAY);
    }


    public void CastAttack()
    {
        var sound = Mac.sound.monsterSFX;
        sound.PlayMonsterSFX(this.gameObject, sound.macSmallBall);
    }

    public void CastingAttack()
    {
        MacATTACK attack = GetComponentInParent<MacATTACK>();
        attack.isLookAt = false;

        MonsterEffects.Instance.macBulletPool.ItemSetActive(
            bulletLuancher, 
            Mac.CC,
            Mac.priorityTarget);

    }

    public void CastSkillInit()
    {
        var voice = Mac.sound.monsterVoice;
        voice.PlayMonsterVoice(this.gameObject, voice.macBigBallVoice);
    }

    public void CastingSkill()
    {
        MacSKILL skill = GetComponentInParent<MacSKILL>();
        skill.isLookAt = false;

        var sound = Mac.sound.monsterSFX;
        sound.PlayMonsterSFX(this.gameObject, sound.macBigBall);


        MonsterEffects.Instance.macSkillPool.ItemSetActive(skillLuancher,
            Mac.CC,
            Mac.priorityTarget);
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
