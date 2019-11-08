using System.Collections;
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
        player = PlayerFSMManager.Instance;
        skill2 = player.GetComponent<PlayerSKILL2>();
        hit2 = player.GetComponent<PlayerHIT2>();
        cam = player.followCam;
    }

    void hitCheck()
    {
        if (null != player)
        {
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


    void Sigh()
    {
        var voice = player._Sound.voice;
        voice.PlayPlayerVoice(player.gameObject, voice.sigh);
    }

    void Singing()
    {

        var voice = player._Sound.voice;
        voice.PlayPlayerVoice(player.gameObject, voice.singing);
    }

    void Humming()
    {

        var voice = player._Sound.voice;
        voice.PlayPlayerVoice(player.gameObject, voice.humming);
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

    void SpecialWink()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.specialWinkSFX);
    }

    void SpecialFireWork()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.specialFireSFX);

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

    void SKill4BGM()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.skill4BGMSFX);

    }

    void SKill4LightOn()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.skill4LightOnSFX);
    }

    void Skill4Roar()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.skill4Skill4RoarSFX);
    }

    void SKill4FireWork()
    {
        var sound = player._Sound.sfx;
        sound.PlayPlayerSFX(this.gameObject, sound.skill4FireWord);
    }

    void Skill4EndVoice()
    {
        var voice = player._Sound.voice;
        voice.PlayPlayerVoice(this.gameObject, voice.skill4FinishVoice);
    }

    public void StopParticle()
    {
        //particle.Stop();
        //particle.Clear();
    }

    void Skill2End()
    {
        player.isSkill2Dash = false;
        //skill2.isEnd = true;
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

