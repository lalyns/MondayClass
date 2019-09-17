using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatDASH : RedHatFSMState
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
        try
        {
            _manager.dashEffect = EffectPoolManager._Instance._RedHatEffectPool.ItemSetActive(this.transform);
        }
        catch
        {

        }
        // 대쉬 시간 조정

        base.BeginState();
    }

    public override void EndState()
    {
        _Time = 0.0f;
        Vector3 _TargetPos = Vector3.zero;
        _manager._MR.material = _manager.Stat._NormalMat;

        //_manager.dashEffect = null;

        base.EndState();
    }

    protected override void Update()
    {
        _Time += Time.deltaTime;

        Debug.Log("Dash Effect name : " + _manager.dashEffect);

        float maxDistance = _DashTime * _manager.Stat.statData._DashSpeed;
        if (_Time < _DashReadyTime)
        {
            //if (!_IsDrawDashRoute)
            {
                transform.LookAt(_manager.PlayerCapsule.transform);

                dashEndPos = this.transform.position;
                dashEndPos += transform.forward * _manager.Stat.statData._DashRange;
                dashEndPos.y = this.transform.position.y;

            }
        }

        else if(_Time < _DashReadyTime + _DashTime)
        {
            
            transform.position = Vector3.MoveTowards(this.transform.position, dashEndPos, 
                _manager.Stat.statData._DashSpeed * Time.deltaTime);

        }

        else if(_Time < _DashReadyTime + _DashTime + _DashAfterDelay)
        {
            _manager.CC.detectCollisions = true;
        }

        else if(_Time > _DashReadyTime + _DashTime + _DashAfterDelay)
        {
            if (GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) <= _manager.Stat.AttackRange)
            {
                _manager.SetState(RedHatState.ATTACK);
            }
            else
            {
                _manager.SetState(RedHatState.CHASE);
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
