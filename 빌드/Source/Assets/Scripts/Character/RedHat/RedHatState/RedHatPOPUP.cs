using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatPOPUP : RedHatFSMState
{
    float _PopUpTime = 2.0f;
    float _curTime = 0.0f;

    public GameObject _PopupEffect;

    public override void BeginState()
    {
        base.BeginState();

        _manager.Anim.Play("PopUp");
        _manager.Stat.SetHp(_manager.Stat.MaxHp);

        _PopupEffect.SetActive(true);
        _PopupEffect.GetComponentInChildren<ParticleSystem>().Play();
        _PopupEffect.GetComponent<Animator>().Play("Ani");

        TargetPrioritySet();
    }

    public override void EndState()
    {
        base.EndState();

        _curTime = 0.0f;
        _manager.isDead = false;
    }

    protected override void Update()
    {
        base.Update();

        _curTime += Time.deltaTime;

        if (_curTime > _PopUpTime)
        {
            _manager.SetState(RedHatState.CHASE);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void TargetPrioritySet()
    {
        if (MissionManager.Instance.CurrentMissionType == MissionType.Defence)
        {

            Collider[] allTarget = Physics.OverlapSphere(this.transform.position, _manager._DetectingRange);

            foreach (Collider target in allTarget)
            {
                if (target.tag == "Player")
                {
                    _manager._PriorityTarget = PlayerFSMManager.
                        Instance.GetComponentInChildren<Animator>()
                        .GetComponent<Collider>();
                    break;
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
