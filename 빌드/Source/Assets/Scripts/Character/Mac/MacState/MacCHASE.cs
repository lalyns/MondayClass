using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacCHASE : MacFSMState
{
    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        if(GameLib.DistanceToCharacter(_manager.CC,_manager.PlayerCapsule) < _manager.Stat._AttackRange)
        {
            _manager.SetState(MacState.ATTACK);
        }

        else
        {
            _manager._MR.material = _manager.Stat._NormalMat;
            _manager.CC.transform.LookAt(_manager.PlayerCapsule.transform);
            _manager.CC.transform.position = Vector3.Lerp(_manager.CC.transform.position,
                _manager.PlayerCapsule.transform.position, 0.3f * Time.deltaTime);
        }
    }
}
