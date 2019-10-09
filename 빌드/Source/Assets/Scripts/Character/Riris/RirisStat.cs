using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisStat : CharacterStat
{
    public MonsterData monsterData;

    public float _AttackRange = 2f;

    public float _BulletDamage;
    public float _BulletSpeed = 10f;
    public float _BulletLifeTime;

    public float[] damageCoefiiecient;

    public float addStrPerRound;
    public float addDefPerRound;

    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData._MaxHp;
        _hp = _maxHp;
    }
}
