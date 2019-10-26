using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiberHIT : TiberFSMState
{
    bool knockBack = true;
    int knockBackDuration = 1;
    float knockBackPower = 3.0f;
    float knockBackDelay = 0.3f;

    public bool hitEnd = false;

    public bool isBelowHalf = false;

    Vector3 knockBackTargetPos = Vector3.zero;

    public override void BeginState()
    {
        base.BeginState();

        knockBack = _manager.KnockBackFlag;
        knockBackDuration = _manager.KnockBackDuration;
        knockBackPower = _manager.KnockBackPower;
        knockBackDelay = _manager.KnockBackDelay;

        Vector3 direction = (_manager.PlayerCapsule.transform.forward).normalized;
        direction.y = 0;
        knockBackTargetPos = direction + this.transform.position;

        if(_manager.Stat.Hp/_manager.Stat.MaxHp < 0.5f && !isBelowHalf)
        {
            isBelowHalf = true;
            var voice = _manager._Sound.monsterVoice;
            voice.PlayMonsterVoice(gameObject, voice.tiberDamageVoice);
        }

        //GetComponentInChildren<TiberAnimEvent>()._WeaponCapsule.gameObject.SetActive(false);
        _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
        //StartCoroutine(GameLib.Blinking(_manager.materialList, Color.white));

        _manager.agent.acceleration = 0;
        _manager.agent.velocity = Vector3.zero;
    }

    public override void EndState()
    {
        base.EndState();

        hitEnd = false;

        _manager.CurrentAttackType = AttackType.NONE;
        _manager.isChange = false;
        _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
    }

    protected override void Update()
    {
        base.Update();

        if (!hitEnd)
        {
        }

        if (hitEnd && !PlayerFSMManager.Instance.isSpecial && !PlayerFSMManager.Instance.isSkill4)
            _manager.SetState(TiberState.CHASE);
        if (PlayerFSMManager.Instance.isSkill4)
        {
            PlayerStat playerStat = PlayerFSMManager.Instance.Stat;
            if (!PlayerFSMManager.Instance.isCantMove && !isHit)
            {
                _manager.Stat.TakeDamage(playerStat, playerStat.dmgCoefficient[6]);
                isHit = true;
            }
        }
        if (_manager.Stat.Hp <= 0)
            _manager.SetDeadState();
    }
    bool isHit = false;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void HitEnd()
    {
        hitEnd = true;
    }

}
