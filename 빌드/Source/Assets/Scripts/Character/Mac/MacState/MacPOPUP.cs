using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacPOPUP : MacFSMState
{
    public override void BeginState()
    {
        base.BeginState();

        _manager.Anim.Play("PopUp");
        _manager.Stat.SetHp(_manager.Stat.MaxHp);

        EffectPlay();

        TargetPrioritySet();
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void EffectPlay()
    {
        _manager._PopupEffect.SetActive(true);
        _manager._PopupEffect.GetComponentInChildren<ParticleSystem>().Play();
        _manager._PopupEffect.GetComponent<Animator>().Play("Ani");
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void TargetPrioritySet()
    {
        if (MissionManager.Instance.CurrentMissionType == MissionManager.MissionType.Defence)
        {

            Collider[] allTarget = Physics.OverlapSphere(this.transform.position, _manager._DetectingRange);

            foreach (Collider target in allTarget)
            {
                if (target.tag == "Player")
                {
                    _manager._PriorityTarget = PlayerFSMManager.
                        Instance.GetComponentInChildren<Animator>()
                        .GetComponent<Collider>();
                }
                else
                {
                    MissionC mission = MissionManager.Instance.CurrentMission as MissionC;
                    _manager._PriorityTarget = mission.protectedTarget.Collider;
                }
            }
        }
        else
        {
            _manager._PriorityTarget = PlayerFSMManager.
                Instance.GetComponentInChildren<Animator>()
                .GetComponent<Collider>();
        }
    }
}
