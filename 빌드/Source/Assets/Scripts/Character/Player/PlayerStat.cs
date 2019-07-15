using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData.PlayerMaxHp;
        _hp = _maxHp;
        _attackRange = statData.PlayerAttackRange;
        _moveSpeed = statData.PlayerMoveSpeed;
        _str = statData.PlayerStr;
    }
}
