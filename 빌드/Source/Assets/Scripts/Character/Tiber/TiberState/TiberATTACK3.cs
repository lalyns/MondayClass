using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
public class TiberATTACK3 : TiberFSMState
{
    public float time;
    bool isSpread = false;
    Vector3 playerTrans;

    public override void BeginState()
    {
        base.BeginState();

        _manager.Attack3Effect.SetActive(true);


        _manager.agent.velocity = Vector3.zero;
        _manager.agent.destination = this.transform.position;
        _manager.agent.acceleration = 4f;
    }

    public override void EndState()
    {
        base.EndState();

        _manager.agent.velocity = Vector3.zero;
        _manager.agent.destination = this.transform.position;
        _manager.agent.acceleration = 4f;
        _manager.agent.isStopped = true;

        _manager.Attack3Effect.SetActive(false);
        time = 0;
    }

    protected override void Update()
    {
        base.Update();
        playerTrans = new Vector3(_manager.PlayerCapsule.transform.position.x, transform.position.y, _manager.PlayerCapsule.transform.position.z);

        time += Time.deltaTime;

        if (time >= 7.1f)
        {
            _manager.SetState(TiberState.CHASE);
            time = 0;
            return;
        }
        if( time < 6 && time >= 1)
        {
            _manager.agent.destination = playerTrans;

            if (_manager.agent.remainingDistance >= 3f) {
                _manager.agent.isStopped = false;
            } else {
                _manager.agent.isStopped = true;
            }
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

    public void AttackCheck()
    {

    }
}
