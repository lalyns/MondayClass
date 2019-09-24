using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatHIT : RedHatFSMState
{
    bool knockBack = true;
    public float knockBackDuration = 1.5f;
    public float knockBackPower = 3.0f;
    public float knockBackDelay = 0.3f;

    float _Count = 0;

    Vector3 knockBackTargetPos = Vector3.zero;
    public override void BeginState()
    {
        base.BeginState();

        knockBack = true;
        knockBackTargetPos = transform.position +
            _manager.PlayerCapsule.transform.forward * knockBackPower;
        knockBackTargetPos.y = this.transform.position.y;
    }

    public override void EndState()
    {
        base.EndState();
        _Count = 0;
    }

    protected override void Update()
    {
        base.Update();


        if (!knockBack)
        {
            _Count += Time.deltaTime;

            if(_Count>=knockBackDelay)
                _manager.SetState(RedHatState.CHASE);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        StartCoroutine(KnockBack(knockBackDuration, knockBackPower));
    }

    public IEnumerator KnockBack(float dur, float power)
    {
        float timer = 0.0f;
        Vector3 direction = _manager.PlayerCapsule.transform.forward.normalized;

        while (timer <= dur)
        {
            timer += Time.deltaTime;

            //transform.position = Vector3.Lerp(this.transform.position, knockBackTargetPos, 0.15f * Time.deltaTime);
            transform.position += direction * Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        knockBack = false;

    }
}
