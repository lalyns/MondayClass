using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisPATTERNA : RirisFSMState
{
    public GameObject _PatternAReadyEffect;

    public bool SetJumpState = false;

    public float targetSetDelay = 1.3f;
    public float stompDelay = 1.5f;

    float stompCount = 0;

    public bool StompEnd = false;

    Transform playerTransform;
    Vector3 targetPos;

    public override void BeginState()
    {
        base.BeginState();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _manager._Weapon.gameObject.SetActive(true);
        _manager._Weapon.transform.position = this.transform.position;
        _manager._Weapon.transform.rotation = this.transform.rotation;
    }

    public override void EndState()
    {
        base.EndState();

        _manager._Weapon.gameObject.SetActive(false);
        _manager.Anim.SetBool("Stomp", false);
        _manager._WeaponAnimator.SetBool("Stomp", false);
        SetJumpState = false;
        StompEnd = false;

    }

    protected override void Update()
    {
        base.Update();

        if (SetJumpState) {
            stompCount += Time.deltaTime;

            if (stompCount < targetSetDelay)
            {
                targetPos = playerTransform.position;
            }
            _PatternAReadyEffect.SetActive(true);
            _PatternAReadyEffect.transform.position = targetPos;
        }


        if (stompCount > stompDelay) {
            Stomp();
            SetJumpState = false;
            stompCount = 0;
        }

        if(StompEnd)
        {
            _manager.SetState(RirisState.PATTERNEND);
        }
    }

    public void Stomp()
    {
        _PatternAReadyEffect.SetActive(false);

        transform.position = targetPos;
        _manager._Weapon.position = targetPos;

        _manager.Anim.SetBool("Stomp", true);
        _manager._WeaponAnimator.SetBool("Stomp", true);
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
