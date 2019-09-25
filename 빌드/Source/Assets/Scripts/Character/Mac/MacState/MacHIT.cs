﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacHIT : MacFSMState
{
    bool knockBack = true;
    float knockBackDuration = 1.5f;
    float knockBackPower = 3.0f;

    float knockBackDelay = 0.3f;

    float _Count = 0;
    
    bool blink = false;

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

        StartCoroutine(Blink());
    }

    public override void EndState()
    {
        base.EndState();

        _Count = 0;
        hitEnd = false;

        foreach (Material mat in _manager.Mats)
        {
            mat.SetFloat("_Hittrigger", 0);
        }

    }

    protected override void Update()
    {
        base.Update();

        if (hitEnd)
        {
            _manager.SetState(MacState.CHASE);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(knockBack)
            StartCoroutine(KnockBack(knockBackDuration, knockBackPower));
    }

    public IEnumerator Blink()
    {
        int i = 0;
        while (i++ < 4)
        {
            float BV = blink ? 0 : 1;

            foreach (Material mat in _manager.Mats)
            {
                mat.SetFloat("_Hittrigger", BV);
            }

            blink = !blink;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator KnockBack(float dur, float power)
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

    public void HitEnd()
    {
        hitEnd = true;
    }
}
