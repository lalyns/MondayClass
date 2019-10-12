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
    private void Start()    
    {
        //input = InputHandler.instance;
        player = PlayerFSMManager.Instance;
        skill2 = player.GetComponent<PlayerSKILL2>();
        hit2 = player.GetComponent<PlayerHIT2>();
    }

    void hitCheck()
    {
        if (null != player)
        {
            //input.AttackCheck();
            player.AttackCheck();
            try
            {
                if (isNormal)
                    Normal_trail.gameObject.SetActive(true);
                if (!isNormal)
                    Special_trail.gameObject.SetActive(true);
            }
            catch
            {

            }
        }
    }
    void hitCancel()
    {
        if (null != player)
        {
            //input.AttackCancel();
            player.AttackCancel();
            try
            {
                if (isNormal)
                    Normal_trail.gameObject.SetActive(false);
                if (!isNormal)
                    Special_trail.gameObject.SetActive(false);
            }
            catch
            {

            }
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
        //Debug.Log("시작");

        var main = particle.main;
        try
        {
            main.startLifetime = 1;
            particle.Play();
        }
        catch
        {

        }
    }

    void footstepsound()
    {
        try
        {
            player._Sound.sfx.PlayPlayerSFX(player.gameObject, player._Sound.sfx.footstepSFX);
        }
        catch
        {

        }
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
