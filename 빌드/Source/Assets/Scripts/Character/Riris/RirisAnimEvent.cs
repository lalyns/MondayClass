using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisAnimEvent : MonoBehaviour
{
    public RirisFSMManager riris;

    public bool isWeapon;

    public Collider patternA;
    public Collider patternB;

    public void Start()
    {
        if(!isWeapon)
            riris = GetComponentInParent<RirisFSMManager>();

    }

    void OnPatternATrigger()
    {
        patternA.enabled = true;
    }

    void DisablePatternATrigger()
    {
        patternA.enabled = false;
    }

    void OnPatternBTrigger()
    {
        patternB.enabled = true;
    }

    void DisablePatternBTrigger()
    {
        patternB.enabled = false;
    }

    public void PatternAJumpEnd()
    {
        RirisPATTERNA pattern = riris.GetComponent<RirisPATTERNA>();

        pattern.SetJumpState = true;

        pattern.targetPos = pattern.playerTransform.position;

        pattern._PatternAReadyEffect.SetActive(true);
        pattern._PatternAReadyEffect.transform.position = pattern.targetPos;
    }

    public void PatternAEnd()
    {
        //Debug.Log("End Call");

        RirisPATTERNA pattern = riris.GetComponent<RirisPATTERNA>();

        pattern.PatternEnd = true;

    }

    public void DashSound()
    {
        var sound = riris.sound.ririsSFX;
        sound.PlayRirisSFX(riris.gameObject, sound.dashLoopSFX);
        sound.PlayRirisSFX(riris.gameObject, sound.dashSFX);
    }

    public void DashVoice()
    {
        var voice = riris.sound.ririsVoice;
        voice.PlayRirisVoice(riris.gameObject, voice.dash);
    }

    public void JumpVoice()
    {
        var voice = riris.sound.ririsVoice;
        voice.PlayRirisVoice(riris.gameObject, voice.jump);
    }

    public void PatternAJumpSound()
    {
        var sound = riris.sound.ririsSFX;
        sound.PlayRirisSFX(riris.gameObject, sound.jumpSFX);
    }

    public void DropSound()
    {
        var sound = riris.sound.ririsSFX;
        sound.PlayRirisSFX(riris.gameObject, sound.dropSFX);

    }

    public void StompSound()
    {
        var sound = riris.sound.ririsSFX;
        sound.PlayRirisSFX(riris.gameObject, sound.stompSFX);

    }

    public void DropVoice()
    {
        var voice = riris.sound.ririsVoice;
        voice.PlayRirisVoice(riris.gameObject, voice.stomp);
    }

    public void AddBulletPattern()
    {
        if(riris.CurrentState == RirisState.PATTERNA)
        {
            RirisPATTERNA pattern = riris.CurrentStateComponent as RirisPATTERNA;
            pattern.StartCoroutine(pattern.AddBullet()); 
        }
        else if (riris.CurrentState == RirisState.PATTERNB)
        {
            RirisPATTERNB pattern = riris.CurrentStateComponent as RirisPATTERNB;
            pattern.StartCoroutine(pattern.AddBullet());
        }
        else if (riris.CurrentState == RirisState.PATTERNC)
        {
            RirisPATTERNC pattern = riris.CurrentStateComponent as RirisPATTERNC;

            pattern.bulletPos.position = riris.Pevis.transform.position;
            pattern.StartCoroutine(pattern.FireBullet());
        }
    }

    public void PatterBDashEffectUp()
    {
        RirisPATTERNB pattern = riris.CurrentStateComponent as RirisPATTERNB;

        pattern.AttackReadyEnd();
    }

    public void PatterBEnd()
    {
        RirisPATTERNB pattern = riris.CurrentStateComponent as RirisPATTERNB;

        pattern.isEnd = true;
    }

    public void Teleport()
    {
        var randPos = UnityEngine.Random.Range(0, MissionManager.Instance.CurrentMission.MapGrid.mapPositions.Count);
        var pos = MissionManager.Instance.CurrentMission.MapGrid.mapPositions[randPos];
        riris.transform.position = pos;
        riris.GetComponent<RirisPATTERNEND>().NextState();
        Instantiate(riris.missingEndEffect, riris.Pevis.position, Quaternion.identity);

        var sfx = riris.sound.ririsSFX;
        var voice = riris.sound.ririsVoice;
        sfx.PlayRirisSFX(gameObject, sfx.teleportSFX);
        voice.PlayRirisVoice(gameObject, voice.smile);
    }

    public void UltimateEnd()
    {
        riris.GetComponent<RirisULTIMATE>().PatternEnd();
    }

    public void BatSwarmVoice()
    {
        var voice = riris.sound.ririsVoice;
        voice.PlayRirisVoice(riris.gameObject, voice.batswarm);
    }

    public void BatSwarmCast()
    {
        var sound = riris.sound.ririsSFX;
        sound.PlayRirisSFX(riris.gameObject, sound.batSwarmLoopSFX);
    }

    public void BoxTriggerOn()
    {
        GetComponentInParent<Collider>().enabled = true;
    }

    public void BoxTriggerOff()
    {
        GetComponentInParent<Collider>().enabled = false;
    }


    public void SetOff()
    {
        GetComponent<Animator>().Play("Idle");
        BossEffects.Instance.tornaedo.ItemReturnPool(GetComponentInParent<BossEffectTornaedo>().gameObject);
    }
}
