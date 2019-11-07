﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiberAnimEvent : MonoBehaviour
{
    public TiberATTACK1 _attackCp1;
    public TiberATTACK2 _attackCp2;
    public TiberHIT _hitCp;
    public TiberDEAD _deadCp;
    public TiberWeapon tiberWeapon;

    public TiberFSMManager _manager => GetComponentInParent<TiberFSMManager>();

    private void Awake()
    {
        _attackCp1 = GetComponentInParent<TiberATTACK1>();
        _attackCp2 = GetComponentInParent<TiberATTACK2>();
    }

    public SphereCollider _WeaponCapsule;
    void Start()
    {
        _WeaponCapsule.gameObject.SetActive(false);
    }

    void OnWeaponTrigger()
    {
        _WeaponCapsule.gameObject.SetActive(true);
        tiberWeapon._Dameged = false;
    }

    void DisableWeaponTrigger()
    {
        _WeaponCapsule.gameObject.SetActive(false);
    }

    void Attack2End()
    {
        _attackCp2.isEnd = true;
    }

    void SpinStart()
    {
        var sound = _manager.sound.monsterSFX;
        sound.PlayMonsterSFX(gameObject, sound.tiberSpinInit);

    }

    void SpinSound()
    {
        var sound = _manager.sound.monsterSFX;
        sound.PlayMonsterSFX(gameObject, sound.tiberSpin);
    }

    void SpinVoice()
    {
        var voice = _manager.sound.monsterVoice;
        voice.PlayMonsterVoice(gameObject, voice.tiberSpinVoice);

    }

    void JumpSound()
    {
        var sound = _manager.sound.monsterSFX;
        sound.PlayMonsterSFX(gameObject, sound.tiberStamp);

    }

    void JumpVoice()
    {
        var voice = _manager.sound.monsterVoice;
        voice.PlayMonsterVoice(gameObject, voice.tiberStompVoice);
    }

    void DropSound()
    {
        var sound = _manager.sound.monsterSFX;
        sound.PlayMonsterSFX(gameObject, sound.tiberStampDrop);

    }

    void BoomSound()
    {

        var sound = _manager.sound.monsterSFX;
        sound.PlayMonsterSFX(gameObject, sound.tiberStampBoom);
    }

    void HitCheck1()
    {
        //if (null != _attackCp2)
        //_attackCp2.AttackCheck();
    }

    public void HitEnd()
    {
        _hitCp.HitEnd();
    }

    public void NotifyDead()
    {
        _deadCp.DeadHelper();
    }
    public void EndCheck()
    {
        _attackCp1.isEnd = true;
    }
}
