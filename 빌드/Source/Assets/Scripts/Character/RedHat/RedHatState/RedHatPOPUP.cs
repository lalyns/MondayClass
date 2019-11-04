using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Mission;
using MC.Sound;

public class RedHatPOPUP : RedHatFSMState
{
    public GameObject _PopupEffect;

    public override void BeginState()
    {
        base.BeginState();

        _manager.Anim.Play("PopUp");
        _manager.Stat.SetHp(_manager.Stat.MaxHp);


        _PopupEffect.SetActive(true);
        _PopupEffect.GetComponentInChildren<ParticleSystem>().Play();
        _PopupEffect.GetComponentInChildren<Animator>().Play("PopUpEffect");

        TargetPrioritySet();
        _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));

        _manager.mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        _manager.agent.speed = 4;
        _manager.agent.angularSpeed = 120;
    }

    public void PopupReset()
    {
        _manager.isDead = false;
        GameLib.DissoveActive(_manager.materialList, false);
        StartCoroutine(GameLib.BlinkOff(_manager.materialList));
        GetComponentInChildren<RedHatHitCollider>().capsule.enabled = true;

        if (MCSoundManager.SoundCall >= MCSoundManager.SoundSkill3Break)
        {
            var sound = _manager.sound.monsterSFX;
            sound.PlayMonsterSFX(_manager.gameObject, sound.monsterAppear);
            MCSoundManager.SoundCall = 0f;
        }

    }

    private void Start()
    {
        GetComponentInChildren<RedHatHitCollider>().capsule.enabled = true;
    }
    public override void EndState()
    {
        base.EndState();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void TargetPrioritySet()
    {
        if (GameStatus.currentGameState == CurrentGameState.EDITOR)
        {
            _manager.priorityTarget = PlayerFSMManager.Instance.Anim.GetComponent<Collider>();
            return;
        }

        if (GameStatus.currentGameState == CurrentGameState.Tutorial)
        {
            _manager.priorityTarget = PlayerFSMManager.Instance.Anim.GetComponent<Collider>();
            return;
        }

        _manager.priorityTarget = PlayerFSMManager.
            Instance.GetComponentInChildren<Animator>()
            .GetComponent<Collider>();
    }
}
