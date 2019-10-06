using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
public class TiberATTACK3 : TiberFSMState
{
    public float _time;
    bool _IsSpread = false;
    public override void BeginState()
    {
        base.BeginState();

        _manager.Attack3Effect.SetActive(true);
    }

    public override void EndState()
    {
        base.EndState();

        _manager.Attack3Effect.SetActive(false);
        _time = 0;
    }
    protected override void Update()
    {
        base.Update();

        _time += Time.deltaTime;

        if (_time >= 7.2f)
        {
            _manager.SetState(TiberState.CHASE);
            _time = 0;
            return;
        }
        else
        {
            Vector3 playerTrans = new Vector3(_manager.PlayerCapsule.transform.position.x, transform.position.y, _manager.PlayerCapsule.transform.position.z);

            _manager.CC.transform.LookAt(playerTrans);


            Vector3 moveDir = (playerTrans
                - _manager.CC.transform.position).normalized;

            moveDir.y = 0;

            if ((_manager.CC.collisionFlags & CollisionFlags.Sides) != 0)
            {
                Vector3 correctDir = Vector3.zero;
                if (!_IsSpread)
                {
                    correctDir = DecideSpreadDirection();
                    _IsSpread = true;
                }

                moveDir += correctDir;
            }

            _manager.CC.Move(moveDir * _manager.Stat.statData._MoveSpeed * 1.3f * Time.deltaTime);
        }
    }
    public void AttackSupport()
    {
        UserInterface.Instance.UIPlayer.hpBar.HitBackFun();
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

    public void AttackCheck()
    {

    }
}
