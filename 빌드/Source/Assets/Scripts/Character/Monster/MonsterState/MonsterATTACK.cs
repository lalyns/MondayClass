using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterATTACK : MonsterFSMState
{
    public float _AttackTime = 3.0f;
    public float _Time = 0.0f;

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
            if (_Time > _AttackTime)
            {
                Transform bullet = Instantiate(_manager.Stat._AttackEffect,
                    _manager._AttackTransform.position,
                    Quaternion.identity).transform;

                Debug.Log(_manager.transform.name + " : " + bullet.name);

                bullet.GetComponent<Bullet>().dir = GameLib.DirectionToCharacter(_manager.CC, _manager.PlayerCapsule);
                bullet.GetComponent<Bullet>()._Move = true;

                _Time = 0.0f;
            }

        }
        else
        {
            _manager.SetState(MonsterState.CHASE);
        }

    }
}
