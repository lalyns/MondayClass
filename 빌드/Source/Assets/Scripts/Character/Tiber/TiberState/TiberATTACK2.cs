using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
public class TiberATTACK2 : TiberFSMState
{
    public float _time;

    public override void BeginState()
    {
        base.BeginState();
        this.transform.position = _manager.Attack1Effect.transform.position;
    }

    public override void EndState()
    {
        base.EndState();

        _time = 0;
    }
    protected override void Update()
    {
        base.Update();

        _time += Time.deltaTime;

        if (_time >= 1f)
        {
            _manager.SetState(TiberState.CHASE);
            _time = 0;
            return;
        }
        //if (GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) > _manager.Stat._AttackRange)
        //{
        //    _manager.SetState(TiberState.CHASE);
        //}
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
