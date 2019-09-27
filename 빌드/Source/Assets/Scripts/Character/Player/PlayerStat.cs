    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    public float[] skillCTime = new float[4];
    public float feverGaugeGetValue;
    public float transDuration;

    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData._MaxHp;
        _hp = _maxHp;
        _attackRange = statData._AttackRange;
        _damage = statData._Damage;
        skillCTime[0] = 10f;
        skillCTime[1] = 10f;
        skillCTime[2] = 10f;
        skillCTime[3] = 10f;
    }
}
