using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacPOPUP : MacFSMState
{
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
        base.EndState();
    }

    private void Update()
    {
        _manager.SetState(MacState.CHASE);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
