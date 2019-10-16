using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
public class TiberATTACK2 : TiberFSMState
{
    public float _time;
    public bool isEnd = false;

    public override void BeginState()
    {
        base.BeginState();        
        
        this.transform.position = _manager.Attack1Effect.transform.position;
        _manager.Attack2Effect.SetActive(true);
        _manager.Attack2Effect.transform.position = this.transform.position;

        _manager.capsule.enabled = false;

    }
    public override void EndState()
    {
        base.EndState();
        _manager.Attack2Effect.SetActive(false);
        _time = 0;
        isEnd = false;

        _manager.capsule.enabled = true;
    }
    protected override void Update()
    {
        base.Update();

        _time += Time.deltaTime;

        if (_manager.agent.remainingDistance >= 1.5f) {
            _manager.agent.isStopped = false;
        } else {
            _manager.agent.isStopped = true;
        }

        if (isEnd)
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

    public void AttackCheck()
    {

    }
}
