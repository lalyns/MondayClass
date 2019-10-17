﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField]
    InputHandler input;
    PlayerFSMManager player;
    public TrailRenderer Normal_trail;
    public TrailRenderer Special_trail;
    public ParticleSystem particle;
    bool isNormal;
    PlayerSKILL2 skill2;
    PlayerHIT2 hit2;
    public CapsuleCollider WeaponCapsule;

    public FollowCam cam;
    private void Start()
    {
        //input = InputHandler.instance;
        player = PlayerFSMManager.Instance;
        skill2 = player.GetComponent<PlayerSKILL2>();
        hit2 = player.GetComponent<PlayerHIT2>();
        cam = player.followCam;
    }

    void hitCheck()
    {
        if (null != player)
        {
            //input.AttackCheck();
            player.AttackCheck();
            
            if (isNormal)
                Normal_trail.gameObject.SetActive(true);
            if (!isNormal)
                Special_trail.gameObject.SetActive(true);
        }
    }
    void hitCancel()
    {
        if (null != player)
        {
            player.AttackCancel();
            
            if (isNormal)
                Normal_trail.gameObject.SetActive(false);
            if (!isNormal)
                Special_trail.gameObject.SetActive(false);
        }
    }
    void skill3Check()
    {
        player.Skill3Attack();
    }
    void skill3Cancel()
    {
        player.Skill3Cancel();
    }
    public void PlayParticle()
    {
        var main = particle.main;
        
        main.startLifetime = 1;
        particle.Play();
    }

    void FootStepSound()
    {
        player._Sound.sfx.PlayPlayerSFX(player.gameObject, player._Sound.sfx.footstepSFX);
    }

    void Skill3Finish()
    {
        var voice = player._Sound.voice;
        voice.PlayPlayerVoice(this.gameObject, voice.skill3FinishVoice);
    }

    void SpecialCast()
    {
        var voice = player._Sound.voice;
        voice.PlayPlayerVoice(this.gameObject, voice.specialCastVoice);
    }

    void SpecialEnd()
    {

        var voice = player._Sound.voice;
        voice.PlayPlayerVoice(this.gameObject, voice.specialFinishVoice);
    }

    void Skill3CastSFX()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.skill3CastSFX);
    }

    void Skill3LoopSFX()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.skill3LoopSFX);
    }

    void Skill3FinishSFX()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.skill3FinishSFX);
    }

    void Skill3ImpactSFX()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.skill3HitSFX);
    }

    void SpecialJumpSFX()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.specialJumpSFX);
    }
    void SpecialSpinSFX()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.specialSpinSFX);
    }
    void SpecialGripSFX()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.specialGripSFX);
    }
    void SpecialHeartSFX()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.specialHeartSFX);
    }
    void SpecialSwingSFX()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.specialSwingSFX);
    }
    void SpecialVioletBeamSFX()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.specialVioletBeamSFX);
    }

    public void StopParticle()
    {

        Debug.Log("끝");

        //particle.Stop();
        //particle.Clear();
    }

    void Skill2End()
    {
        skill2.isEnd = true;
    }
    void Hit2End()
    {
        hit2.isEnd = true;
    }
    float realTime;
    float processTime = 0;
    float countDown = 0;
    float countTime = 1;

    private void Update()
    {
        isNormal = player.isNormal;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.tag == "Monster")
    //        BreakTime();
    //}

  
}

