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
        _PopupEffect.SetActive(true);
        _PopupEffect.GetComponentInChildren<ParticleSystem>().Play();
        _PopupEffect.GetComponent<Animator>().Play("Ani");
    }

    public override void EndState()
    {
        _curTime = 0.0f;
        base.EndState();
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
}
