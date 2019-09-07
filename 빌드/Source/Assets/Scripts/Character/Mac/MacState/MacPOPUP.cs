using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacPOPUP : MacFSMState
{
    public override void BeginState()
    {
        base.BeginState();

        _manager.Anim.Play("PopUp");
        _manager.Stat.SetHp(_manager.Stat.MaxHp);

        EffectPlay();
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void EffectPlay()
    {
        _manager._PopupEffect.SetActive(true);
        _manager._PopupEffect.GetComponentInChildren<ParticleSystem>().Play();
        _manager._PopupEffect.GetComponent<Animator>().Play("Ani");
    }

    private void Update()
    {

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
