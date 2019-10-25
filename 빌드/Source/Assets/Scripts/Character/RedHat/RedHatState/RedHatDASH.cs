using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatDASH : RedHatFSMState
{
    float _DashTime = 1f;
    float _DashReadyTime = 0.5f;
    float _DashAfterDelay = 1.1f;
    float _Time = 0.0f;

    bool _IsDrawDashRoute = false;

    bool _IsDash = false;
    Vector3 _TargetPos = Vector3.zero;
    Vector3 dashEndPos = Vector3.zero;

    public override void BeginState()
    {
        _TargetPos = _manager._PriorityTarget.transform.position;

        _manager.agent.velocity = Vector3.zero;
        _manager.agent.destination = this.transform.position;
        _manager.agent.acceleration = 5.0f;

        _manager.CC.detectCollisions = false;
        try
        {
            transform.LookAt(_manager._PriorityTarget.transform);

            dashEndPos = this.transform.position;
            dashEndPos += transform.forward * _manager.Stat.statData._DashRange;
            dashEndPos.y = this.transform.position.y;

            //_manager.dashEffect.SetActive(true);
            //_manager.dashEffect.GetComponent<UIAttackRange>().SetTarget(_manager._PriorityTarget);

            _manager.dashEffect1.SetActive(true);
            ParticleSystem effect1 = _manager.dashEffect1.GetComponentInChildren<ParticleSystem>();
            effect1.Play();


        }
        catch
        {
            Debug.Log("대쉬 이펙트 버그");
        }
        // 대쉬 시간 조정

        base.BeginState();
    }

    public override void EndState()
    {
        _Time = 0.0f;
        Vector3 _TargetPos = Vector3.zero;

        //_manager.dashEffect = null;
        //_manager.dashEffect.GetComponent<UIAttackRange>().EffectEnd();

        _manager.CC.detectCollisions = true; 
        _manager.isNotChangeState = false;

        _manager.agent.velocity = Vector3.zero;
        _manager.agent.destination = this.transform.position;
        _manager.agent.acceleration = 5.0f;

        _manager.dashEffect1.SetActive(false);
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

            }
        }

        else if(_Time < _DashReadyTime + _DashTime)
        {
            _manager.isNotChangeState = true;
            transform.position = Vector3.MoveTowards(this.transform.position, dashEndPos, 
                _manager.Stat.statData._DashSpeed * Time.deltaTime);

        }

        else if(_Time < _DashReadyTime + _DashTime + _DashAfterDelay)
        {
            _manager.CC.detectCollisions = true;
        }

        else if(_Time > _DashReadyTime + _DashTime + _DashAfterDelay)
        {
            if (GameLib.DistanceToCharacter(_manager.CC, _manager._PriorityTarget) <= _manager.Stat.AttackRange)
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
