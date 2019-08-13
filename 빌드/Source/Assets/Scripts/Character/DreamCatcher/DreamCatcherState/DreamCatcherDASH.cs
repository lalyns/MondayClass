using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCatcherDASH : DreamCatcherFSMState
{
    float _DashTime = 1f;
    float _DashReadyTime = 1.5f;
    float _DashAfterDelay = 1.5f;
    float _Time = 0.0f;

    bool _IsDrawDashRoute = false;

    bool _IsDash = false;
    Vector3 _TargetPos = Vector3.zero;
    Vector3 dashEndPos = Vector3.zero;

    public override void BeginState()
    {
        _TargetPos = _manager.PlayerCapsule.transform.position;

        _manager._MR.material = _manager.Stat._DashMat;

        _manager._DashRoute.SetPosition(0, this.transform.position);
        _manager._DashRoute.SetPosition(1, this.transform.position);
        _manager._DashRoute.gameObject.SetActive(true);

        // 대쉬 시간 조정

        base.BeginState();
    }

    public override void EndState()
    {
        _Time = 0.0f;
        Vector3 _TargetPos = Vector3.zero;
        _manager._MR.material = _manager.Stat._NormalMat;
        _manager._DashRoute.gameObject.SetActive(false);
        _IsDrawDashRoute = false;
        base.EndState();
    }

    protected override void Update()
    {
        _Time += Time.deltaTime;

        float maxDistance = _DashTime * _manager.Stat.statData._DashSpeed;
        if (_Time < _DashReadyTime)
        {
            //if (!_IsDrawDashRoute)
            {
                transform.LookAt(_manager.PlayerCapsule.transform);

                _manager._DashRoute.SetPosition(0, this.transform.position);

                dashEndPos = this.transform.position;
                dashEndPos += transform.forward * _manager.Stat.statData._DashRange;
                dashEndPos.y = this.transform.position.y;

                _manager._DashRoute.SetPosition(1, dashEndPos);
                _IsDrawDashRoute = true;
            }
        }

        else if(_Time < _DashReadyTime + _DashTime)
        {
            //Vector3 targetDir = _TargetPos - this.transform.position;
            //targetDir = targetDir.normalized;
            //targetDir.y = 0;

            //_manager.CC.Move(targetDir * _manager.Stat.statData._DashSpeed * Time.deltaTime);
            //_manager.CC.detectCollisions = false;
            transform.position = Vector3.MoveTowards(this.transform.position, dashEndPos, 
                _manager.Stat.statData._DashSpeed * Time.deltaTime);

        }

        else if(_Time < _DashReadyTime + _DashTime + _DashAfterDelay)
        {
            _manager._DashRoute.gameObject.SetActive(false);
            _manager.CC.detectCollisions = true;
        }

        else if(_Time > _DashReadyTime + _DashTime + _DashAfterDelay)
        {
            if (GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) <= _manager.Stat.AttackRange)
            {
                _manager.SetState(DreamCatcherState.ATTACK);
            }
            else
            {
                _manager.SetState(DreamCatcherState.CHASE);
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
