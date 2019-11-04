using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class RedHatATTACK : RedHatFSMState
{
    public float time;

    public override void BeginState()
    {
        base.BeginState();

        _manager.agent.velocity = Vector3.zero;
        _manager.agent.destination = this.transform.position;
        _manager.agent.acceleration = 0.0f;
    }

    public override void EndState()
    {
        base.EndState();
        time = 0;
    }

    protected override void Update()
    {
        base.Update();

        DahsCheck();
        time += Time.deltaTime;

        if (time >= 1f)
        {
            _manager.SetState(RedHatState.CHASE);
            time = 0;
            return;
        }
    }

    public void AttackSupport()
    {
        UserInterface.Instance.UIPlayer.hpBar.HitBackFun();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
