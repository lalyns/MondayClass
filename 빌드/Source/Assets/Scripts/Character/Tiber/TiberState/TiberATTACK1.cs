using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
public class TiberATTACK1 : TiberFSMState
{
    [HideInInspector]
    public bool isEnd;
    public override void BeginState()
    {
        base.BeginState();
        isEnd = false;
        _manager.CC.detectCollisions = false;

        _manager.agent.velocity = Vector3.zero;
        _manager.agent.destination = this.transform.position;
        _manager.agent.acceleration = 0.0f;

        _manager.Attack1Effect.SetActive(true);

        _manager.Attack1Effect.transform.position = new Vector3(_manager.PlayerCapsule.transform.position.x, _manager.PlayerCapsule.transform.position.y + 0.3f, _manager.PlayerCapsule.transform.position.z);

        _manager.Attack1Effect.transform.localRotation = Quaternion.identity;
    }

    public override void EndState()
    {
        base.EndState();
        _manager.Attack1Effect.SetActive(false);        
        
        isEnd = false;
    }

    protected override void Update()
    {
        base.Update();

        if (isEnd)
        {
            _manager.SetState(TiberState.ATTACK2);
            return;
        }
       
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
