using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Mission;

public class MacPOPUP : MacFSMState
{
    public override void BeginState()
    {
        base.BeginState();

        _manager.Anim.Play("PopUp");
        _manager.Stat.SetHp(_manager.Stat.MaxHp);

        EffectPlay();
        TargetPrioritySet();
        _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
        _manager._MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        _manager.agent.speed = 3.5f;
        _manager.agent.angularSpeed = 60;
    }
    
    public void PopupReset()
    {
        _manager.isDead = false;
        GameLib.DissoveActive(_manager.materialList, false);
        StartCoroutine(GameLib.BlinkOff(_manager.materialList));
        GetComponentInChildren<MacHitCollider>().capsule.enabled = true;
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
        _manager._PopupEffect.SetActive(true);
        _manager._PopupEffect.GetComponentInChildren<ParticleSystem>().Play();
        _manager._PopupEffect.GetComponentInChildren<Animator>().Play("PopUpEffect");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }



    private void TargetPrioritySet()
    {
        if(GameStatus.currentGameState == CurrentGameState.EDITOR)
        {
            _manager._PriorityTarget = PlayerFSMManager.Instance.Anim.GetComponent<Collider>();
            return;
        }

        if (GameStatus.currentGameState == CurrentGameState.Tutorial)
        {
            _manager._PriorityTarget = PlayerFSMManager.Instance.Anim.GetComponent<Collider>();
            return;
        }

        if (MissionManager.Instance.CurrentMissionType == MissionType.Defence)
        {
            MissionC mission = MissionManager.Instance.CurrentMission as MissionC;
            _manager._PriorityTarget = mission.protectedTarget.Collider;
        }
        else
        {
            _manager._PriorityTarget = PlayerFSMManager.
                Instance.GetComponentInChildren<Animator>()
                .GetComponent<Collider>();
        }
    }
}
