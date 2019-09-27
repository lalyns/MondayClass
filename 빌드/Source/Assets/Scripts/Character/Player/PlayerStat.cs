using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    public float feverGaugeGetValue;
    public float transDuration;

    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData._MaxHp;
        _hp = _maxHp;
        _attackRange = statData._AttackRange;
        _damage = statData._Damage;
    }
}
