using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatAnimEvent : MonoBehaviour
{
    public RedHatATTACK _attackCp;
    public RedHatHIT _hitCp;
    public RedHatDEAD _deadCp;

    RedHatFSMManager redhat;
    RedHatFSMManager RedHat {
        get {
            if (redhat == null)
            {
                redhat = this.GetComponentInParent<RedHatFSMManager>();
            }

            return redhat;
        }
    }

    private void Awake()
    {
        _attackCp = GetComponentInParent<RedHatATTACK>();
    }

    public CapsuleCollider _WeaponCapsule;

    void OnWeaponTrigger()
    {
        _WeaponCapsule.enabled = true;
    }

    void DisableWeaponTrigger()
    {
        _WeaponCapsule.enabled = false;
    }

    void AttackReadySFX()
    {
        var sound = RedHat.sound.monsterSFX;
        sound.PlayMonsterSFX(this.gameObject, sound.redhatAttackReady);
    }

    void AttackSFX()
    {
        var sound = RedHat.sound.monsterSFX;
        sound.PlayMonsterSFX(this.gameObject, sound.redhatAttack);

        var voice = RedHat.sound.monsterVoice;
        voice.PlayMonsterVoice(this.gameObject, voice.redhatAttackVoice);
    }

    void DashSFX()
    {
        var sound = RedHat.sound.monsterSFX;
        sound.PlayMonsterSFX(this.gameObject, sound.redhatDash);
    }

    public void PopupOver()
    {
        RedHat.SetState(RedHatState.CHASE);
    }

    void HitCheck()
    {
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
