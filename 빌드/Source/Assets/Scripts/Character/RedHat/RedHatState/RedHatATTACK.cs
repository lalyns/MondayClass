using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class RedHatATTACK : RedHatFSMState
{
    public float _time;
    public CapsuleCollider _WeaponCapsule;
    public override void BeginState()
    {
        base.BeginState();
        _manager.agent.velocity = Vector3.zero;
        _manager.agent.destination = this.transform.position;
        _manager.agent.acceleration = 0.0f;
    }

    public override void EndState()
    {
        base.EndState();
        _WeaponCapsule.gameObject.SetActive(false);
        _time = 0;
    }

    protected override void Update()
    {
        base.Update();

        DahsCheck();
        _time += Time.deltaTime;

        if (_time >= 1f)
        {
            _manager.SetState(RedHatState.CHASE);
            _time = 0;
            return;
        }

    }
    
    //public void AttackCheck()
    //{
    //    var hitTarget = GameLib.SimpleDamageProcess(transform,
    //        _manager.Stat.AttackRange,
    //        "Player", _manager.Stat, MonsterType.RedHat);

    //    Invoke("AttackSupport", 0.5f);

    //    //if (hitTarget != null) _manager._lastAttack = hitTarget;
    //}

    public void AttackSupport()
    {
        UserInterface.Instance.UIPlayer.hpBar.HitBackFun();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
