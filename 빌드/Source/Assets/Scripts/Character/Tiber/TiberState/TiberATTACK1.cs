using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
public class TiberATTACK1 : TiberFSMState
{
    public float _time;
    [Header("바로 찍길 원한다면 이걸 클릭하시오")]
    public bool isDongMin;
    public override void BeginState()
    {
        base.BeginState();
        _manager.Attack1Effect.SetActive(true);
    }

    public override void EndState()
    {
        base.EndState();
        _manager.Attack1Effect.SetActive(false);
        _time = 0;
    }
    protected override void Update()
    {
        base.Update();

        _time += Time.deltaTime;

        if (_time < 1f)
        {
            _manager.Attack1Effect.transform.position = new Vector3(_manager.PlayerCapsule.transform.position.x, _manager.PlayerCapsule.transform.position.y + 0.3f, _manager.PlayerCapsule.transform.position.z);

            _manager.Attack1Effect.transform.localRotation = Quaternion.identity;
        }
        if (_time >= 1f && isDongMin)
        {
            _manager.SetState(TiberState.ATTACK2);
            return;
        }
        if (_time >= 1.3f && !isDongMin)
        {
            _manager.SetState(TiberState.ATTACK2);
            return;
        }
        //if (GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) > _manager.Stat._AttackRange)
        //{
        //    _manager.SetState(TiberState.CHASE);
        //}
    }
    public void AttackSupport()
    {
        UserInterface.Instance.UIPlayer.hpBar.HitBackFun();
    }



    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
