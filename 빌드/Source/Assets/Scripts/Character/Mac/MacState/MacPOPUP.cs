﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Mission;
using MC.Sound;

public class MacPOPUP : MacFSMState
{
    public override void BeginState()
    {
        base.BeginState();

        _manager.Anim.Play("PopUp");
        _manager.Stat.SetHp(_manager.Stat.MaxHp);


        EffectPlay();
        SetTargetPriority();
        _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
        _manager.mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        _manager.agent.speed = 3.5f;
        _manager.agent.angularSpeed = 60;
    }
    
    public void PopupReset()
    {
        _manager.isDead = false;
        GameLib.DissoveActive(_manager.materialList, false);
        StartCoroutine(GameLib.BlinkOff(_manager.materialList));
        GetComponentInChildren<MacHitCollider>().capsule.enabled = true;

        if (MCSoundManager.SoundCall >= MCSoundManager.SoundSkill3Break)
        {
            var sound = _manager.sound.monsterSFX;
            sound.PlayMonsterSFX(_manager.gameObject, sound.monsterAppear);
            MCSoundManager.SoundCall = 0f;
        }

    }

    private void Start()
    {
        GetComponentInChildren<MacHitCollider>().capsule.enabled = true;
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void EffectPlay()
    {
        _manager.popupEffect.SetActive(true);
        _manager.popupEffect.GetComponentInChildren<ParticleSystem>().Play();
        _manager.popupEffect.GetComponentInChildren<Animator>().Play("PopUpEffect");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }



    private void SetTargetPriority()
    {
        if(GameStatus.currentGameState == CurrentGameState.EDITOR)
        {
            _manager.priorityTarget = PlayerFSMManager.Instance.Anim.GetComponent<Collider>();
            return;
        }

        if (GameStatus.currentGameState == CurrentGameState.Tutorial)
        {
            _manager.priorityTarget = PlayerFSMManager.Instance.Anim.GetComponent<Collider>();
            return;
        }

        if (MissionManager.Instance.CurrentMissionType == MissionType.Defence)
        {
            MissionC mission = MissionManager.Instance.CurrentMission as MissionC;
            _manager.priorityTarget = mission.protectedTarget.Collider;
        }
        else
        {
            _manager.priorityTarget = PlayerFSMManager.
                Instance.GetComponentInChildren<Animator>()
                .GetComponent<Collider>();
        }
    }
}
