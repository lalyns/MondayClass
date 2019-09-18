using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithPATTERNA : LilithFSMState
{
    float _Time1 = 0;
    float _Delay1 = 1f;
    bool _Delay1Ready = false;


    float _Time2 = 0;
    float _Delay2 = 2f;

    bool _Attack = false;
    bool _AttackEnd = false;

    public float _Time3 = 0;
    float _Delay3 = 2;

    public GameObject _PatternAReadyEffect;

    Transform playerTransform;
    Vector3 targetPos;

    public override void BeginState()
    {
        base.BeginState();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _manager.Weapon.gameObject.SetActive(true);
        _manager.Weapon.transform.position = this.transform.position;
        _manager.WeaponAnimator.SetInteger("CurrentState" ,1);
    }

    public override void EndState()
    {
        base.EndState();

        _Time1 = 0;
        _Time2 = 0;
        _Time3 = 0;

        _Delay1Ready = false;
        _Attack = false;
        _AttackEnd = false;
    }

    protected override void Update()
    {
        base.Update();

        _Time1 += Time.deltaTime;

        if (_Time1 < _Delay1)
        {

        }
        else
        {
            if (!_Delay1Ready)
            {
                _Delay1Ready = true;
                this.transform.position = this.transform.position + new Vector3(0, 5f, 0);
            }
        }

        if (_Delay1Ready)
        {
            _Time2 += Time.deltaTime;

            if (_Time2 < _Delay2)
            {
                _PatternAReadyEffect.SetActive(true);
                Vector3 pos = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.transform.position.z);
                _PatternAReadyEffect.transform.position = playerTransform.position;
                this.transform.position = pos;
            }

            if(_Time2 > _Delay2)
            {
                if (!_Attack)
                {
                    targetPos = playerTransform.position;
                }

                _PatternAReadyEffect.SetActive(false);
                _Attack = true;

            }
        }

        if (_Attack)
        {
            _Time3 += Time.deltaTime;

            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, 1f * Time.deltaTime);

            if (_Time3 > _Delay3)
            {
                _AttackEnd = true;
            }

        }

        if (_AttackEnd)
        {
            _manager.SetState(LilithState.PATTERNEND);
        }
    }

    public void AttackCheck()
    {
        var hitTarget = GameLib.SimpleDamageProcess(transform,
            _manager.Stat.AttackRange,
            "Player", _manager.Stat);

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
