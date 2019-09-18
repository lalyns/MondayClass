using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithPATTERNA : LilithFSMState
{
    float _Time1 = 0;
    float _Delay1 = 1.5f;
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

    Vector3 pos;

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

        _manager.Anim.SetBool("Stomp", false);
        _manager.WeaponAnimator.SetBool("Stomp", false);

        _Delay1Ready = false;
        _Attack = false;
        _AttackEnd = false;
    }

    protected override void Update()
    {
        base.Update();

        
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
