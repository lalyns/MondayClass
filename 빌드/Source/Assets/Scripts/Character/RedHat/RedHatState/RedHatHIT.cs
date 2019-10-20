using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatHIT : RedHatFSMState
{
    bool knockBack = true;
    int knockBackDuration = 1;
    float knockBackPower = 3.0f;
    float knockBackDelay = 0.3f;

    public bool hitEnd = false;

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

        GetComponentInChildren<RedHatAnimEvent>()._WeaponCapsule.gameObject.SetActive(false);

        if(_manager.CurrentAttackType != AttackType.SKILL2)
            StartCoroutine(GameLib.Blinking(_manager.materialList, Color.white));

        _manager.agent.acceleration = 0;
        _manager.agent.velocity = Vector3.zero;
    }

    public override void EndState()
    {
        base.EndState();

        hitEnd = false;

        StopAllCoroutines();

        _manager.CurrentAttackType = AttackType.NONE;
        _manager.isChange = false;

        
    }

    protected override void Update()
    {
        base.Update();

        if (!hitEnd)
        {
        }

        if (hitEnd && !PlayerFSMManager.Instance.isSpecial && !PlayerFSMManager.Instance.isSkill4)
            _manager.SetState(RedHatState.CHASE);
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
