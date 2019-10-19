using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacHIT : MacFSMState
{
    bool knockBack = true;
    float knockBackDuration = 1.5f;
    float knockBackPower = 3.0f;
    float knockBackDelay = 0.3f;

    float _Count = 0;
    
    bool hitEnd = false;

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

        StartCoroutine(GameLib.Blinking(_manager.materialList, Color.white));
        
    }

    public override void EndState()
    {
        base.EndState();

        _Count = 0;
        hitEnd = false;

        _manager.CurrentAttackType = AttackType.NONE;
        _manager.isChange = false;

        StopAllCoroutines();
    }

    protected override void Update()
    {
        base.Update();

        if (hitEnd && !PlayerFSMManager.Instance.isSpecial && !PlayerFSMManager.Instance.isSkill4)
            _manager.SetState(MacState.CHASE);
        if (PlayerFSMManager.Instance.isSkill4)
        {
            PlayerStat playerStat = PlayerFSMManager.Instance.Stat;
            _manager.Stat.TakeDamage(playerStat, 1);
        }
        if (_manager.Stat.Hp <= 0)
            _manager.SetDeadState();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    public void HitEnd()
    {
        hitEnd = true;
    }
}
