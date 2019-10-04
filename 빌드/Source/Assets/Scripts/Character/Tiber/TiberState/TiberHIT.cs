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

        //GetComponentInChildren<TiberAnimEvent>()._WeaponCapsule.gameObject.SetActive(false);

        //StartCoroutine(GameLib.Blinking(_manager.materialList, Color.white));
    }

    public override void EndState()
    {
        base.EndState();

        hitEnd = false;

        _manager.CurrentAttackType = AttackType.NONE;
    }

    protected override void Update()
    {
        base.Update();

        if (!hitEnd)
        {
        }

        if (hitEnd)
            _manager.SetState(TiberState.CHASE);
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
