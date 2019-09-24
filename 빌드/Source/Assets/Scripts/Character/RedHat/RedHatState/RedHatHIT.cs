using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatHIT : RedHatFSMState
{
    bool knockBack = true;
    int knockBackDuration = 1;
    float knockBackPower = 3.0f;
    float knockBackDelay = 0.3f;

    float _Count = 0;
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
    }
    public override void EndState()
    {
        base.EndState();
        _Count = 0;
        hitEnd = false;
    }

    protected override void Update()
    {
        base.Update();

        if (hitEnd)
            _manager.SetState(RedHatState.CHASE);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        

        if(knockBack)
            StartCoroutine(KnockBack(knockBackDuration, knockBackPower));
    }

    public IEnumerator KnockBack(int dur, float power)
    {
        Vector3 direction = _manager.PlayerCapsule.transform.forward.normalized;
        direction.y = 0;

        for (int time = 0; time < dur; time++)
        {

            transform.position = knockBackTargetPos + direction * (power);

            yield return new WaitForSeconds(0.1f);
        }

        knockBack = false;
    }
}
