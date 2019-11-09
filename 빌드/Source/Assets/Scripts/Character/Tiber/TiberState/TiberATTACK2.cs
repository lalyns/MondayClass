using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
public class TiberATTACK2 : TiberFSMState
{
    public bool isEnd = false;

    public override void BeginState()
    {
        base.BeginState();        
        
        this.transform.position = _manager.Attack1Effect.transform.position;
        _manager.Attack2Effect.SetActive(true);
        _manager.Attack2Effect.transform.position = this.transform.position;
        _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
        _manager.Anim.Play("TB01_Anim_Attack1_002");
    }

    public override void EndState()
    {
        base.EndState();
        _manager.Attack2Effect.SetActive(false);
        isEnd = false;
        _manager.CC.detectCollisions = true;
        _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
    }

    protected override void Update()
    {
        base.Update();

        if (isEnd)
        {
            _manager.SetState(TiberState.CHASE);
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

    public void AttackCheck()
    {

    }
}
