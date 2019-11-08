﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiberCHASE : TiberFSMState
{
    bool isSpread = false;
    float time;
    Vector3 playerTrans;

    public override void BeginState()
    {

        base.BeginState();
        _manager.agent.isStopped = true;
        _manager.agent.acceleration = 1f;
        playerTrans = new Vector3(_manager.PlayerCapsule.transform.position.x, transform.position.y, _manager.PlayerCapsule.transform.position.z);
        transform.LookAt(playerTrans);
        
    }
    private void Start()
    {
        GetComponentInChildren<TiberHitCollider>().capsule.enabled = true;
    }
    public override void EndState()
    {
        _manager.agent.isStopped = true;

        isSpread = false;
        time = 0;
        base.EndState();
    }

    protected override void Update()
    {
        base.Update();

        playerTrans = new Vector3(_manager.PlayerCapsule.transform.position.x, transform.position.y, _manager.PlayerCapsule.transform.position.z);
        transform.LookAt(playerTrans);
        time += Time.deltaTime;
        if (time >= 2f)
        {
            if (!_manager.isAttack1)
            {
                _manager.isAttack1 = true;
                _manager.SetState(TiberState.ATTACK1);
                return;
            }
            if (_manager.isAttack1)
            {
                _manager.isAttack1 = false;
                _manager.SetState(TiberState.ATTACK3);
            } 
        }
        else
        {
            _manager.agent.destination = playerTrans;
            if (_manager.agent.remainingDistance >= 1.5f) {
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

            //_manager.CC.Move(moveDir * _manager.Stat.statData._MoveSpeed * Time.deltaTime);
        }
    }


    private Vector3 DecideSpreadDirection()
    {
        Vector3 correctDir;

        correctDir = UnityEngine.Random.Range(1, 100) % 2 == 0 ? transform.right : -transform.right;
        correctDir += transform.forward;

        return correctDir;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
