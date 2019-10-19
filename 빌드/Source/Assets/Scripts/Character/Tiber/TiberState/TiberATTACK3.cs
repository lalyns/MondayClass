using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
public class TiberATTACK3 : TiberFSMState
{
    public float _time;
    bool _IsSpread = false;

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
        _time = 0;
    }
    protected override void Update()
    {
        base.Update();
        playerTrans = new Vector3(_manager.PlayerCapsule.transform.position.x, transform.position.y, _manager.PlayerCapsule.transform.position.z);

        _time += Time.deltaTime;

        if (_time >= 7.1f)
        {
            _manager.SetState(TiberState.CHASE);
            _time = 0;
            return;
        }
        if( _time < 6 && _time >= 1)
        {
            _manager.agent.destination = playerTrans;

            if (_manager.agent.remainingDistance >= 3f) {
                _manager.agent.isStopped = false;
            } else {
                _manager.agent.isStopped = true;
            }

            //Vector3 playerTrans = new Vector3(_manager.PlayerCapsule.transform.position.x, transform.position.y, _manager.PlayerCapsule.transform.position.z);

            //_manager.CC.transform.LookAt(playerTrans);


            //Vector3 moveDir = (playerTrans
            //    - _manager.CC.transform.position).normalized;

            //moveDir.y = 0;

            //if ((_manager.CC.collisionFlags & CollisionFlags.Sides) != 0)
            //{
            //    Vector3 correctDir = Vector3.zero;
            //    if (!_IsSpread)
            //    {
            //        correctDir = DecideSpreadDirection();
            //        _IsSpread = true;
            //    }

            //    moveDir += correctDir;
            //}

            //_manager.CC.Move(moveDir * _manager.Stat.statData._MoveSpeed * 1.3f * Time.deltaTime);
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
