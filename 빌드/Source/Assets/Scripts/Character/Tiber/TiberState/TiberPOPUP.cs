using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Mission;
using MC.Sound;

public class TiberPOPUP : TiberFSMState
{
    float _PopUpTime = 2.0f;
    float _curTime = 0.0f;

    public GameObject _PopupEffect;

    public override void BeginState()
    {
        base.BeginState();

        _manager.Anim.Play("PopUP");
        _manager.Stat.SetHp(_manager.Stat.MaxHp);

        _PopupEffect.SetActive(true);
        _PopupEffect.GetComponentInChildren<ParticleSystem>().Play();
        _PopupEffect.GetComponentInChildren<Animator>().Play("PopUpEffect");

        TargetPrioritySet();
        _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
        _manager.mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        _manager.agent.speed = 4f;
        _manager.agent.angularSpeed = 360;
    }
    private void Start()
    {
        GetComponentInChildren<TiberHitCollider>().capsule.enabled = true;
    }
    public override void EndState()
    {
        base.EndState();

        _curTime = 0.0f;
    }

    public void PopupReset()
    {
        _manager.isDead = false;
        GameLib.DissoveActive(_manager.materialList, false);
        StartCoroutine(GameLib.BlinkOff(_manager.materialList));
        GetComponentInChildren<TiberHitCollider>().capsule.enabled = true;

        if (MCSoundManager.SoundCall >= MCSoundManager.SoundSkill3Break)
        {
            var sound = _manager.sound.monsterSFX;
            sound.PlayMonsterSFX(_manager.gameObject, sound.monsterAppear);
            MCSoundManager.SoundCall = 0f;
        }

    }

    protected override void Update()
    {
        base.Update();

        _curTime += Time.deltaTime;

        if (_curTime > _PopUpTime)
        {
            _manager.SetState(TiberState.CHASE);
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

            Collider[] allTarget = Physics.OverlapSphere(this.transform.position, _manager.detectingRange);

            foreach (Collider target in allTarget)
            {
                if (target.tag == "Player")
                {
                    _manager.priorityTarget = PlayerFSMManager.
                        Instance.GetComponentInChildren<Animator>()
                        .GetComponent<Collider>();
                    break;
                }
                else
                {
                    MissionC mission = MissionManager.Instance.CurrentMission as MissionC;
                    _manager.priorityTarget = mission.protectedTarget.Collider;
                }
            }
        }
        else
        {
            _manager.priorityTarget = PlayerFSMManager.
                Instance.GetComponentInChildren<Animator>()
                .GetComponent<Collider>();
        }
    }
}
