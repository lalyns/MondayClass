using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisPATTERNB : RirisFSMState
{
    float _Time1 = 0;
    bool _IsTele = false;

    public Transform _MapCenter;

    public GameObject PatternBReadyEffect;
    public GameObject PatternBAttackEffect;
    bool _IsAttackReady = false;
    public float _AttackReadyTime = 1f;

    float _Time2 = 0;
    float _AttackEndTime = 5f;



    public override void BeginState()
    {
        base.BeginState();
        //transform.LookAt(PlayerFSMManager.instance.transform);
        //_manager._Weapon.transform.LookAt(PlayerFSMManager.instance.transform);
        _manager._Weapon.gameObject.SetActive(true);

        transform.position = _MapCenter.position;
        _manager._Weapon.position = _MapCenter.position;
    }

    public override void EndState()
    {
        base.EndState();

        _IsAttackReady = false;
        _IsTele = false;
        _Time1 = 0;
        _Time2 = 0;
        PatternBReadyEffect.SetActive(false);
        PatternBAttackEffect.SetActive(false);

        _manager._Weapon.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        _Time1 += Time.deltaTime;

        if (_IsAttackReady)
             PatternBAttackEffect.transform.position = _manager._WeaponCenter.transform.position;

        if (_Time1 > _AttackEndTime)
        {
            _manager.SetState(RirisState.PATTERNEND);
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

    public void AttackReadyEnd()
    {
        _IsAttackReady = true;
        PatternBAttackEffect.SetActive(true);
    }
}
