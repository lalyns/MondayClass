using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCatcherATTACK : DreamCatcherFSMState
{
    public float _AttackBeforeTime = 0.8f;
    public float _AttackTime = 3.0f;
    public float _AfterAttackTime = 1.0f;
    public float _Time = 0.0f;

    bool _CreateBall = false;
    bool _SetBall = false;
    public Transform bullet;

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
        _Time += Time.deltaTime;

        if (GameLib.DistanceToCharacter(_manager.CC, _manager.PlayerCapsule) < _manager.Stat._AttackRange)
        {
            

        }
        else
        {
            _manager.SetState(DreamCatcherState.CHASE);
        }

    }
}
