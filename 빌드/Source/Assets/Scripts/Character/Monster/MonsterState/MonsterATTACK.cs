using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterATTACK : MonsterFSMState
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
        if(GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) < _manager.Stat._AttackRange)
            Debug.Log("공격");

        else
        {
            _manager.SetState(MonsterState.CHASE);
        }

    }
}
