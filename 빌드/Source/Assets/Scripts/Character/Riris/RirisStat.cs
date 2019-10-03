﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisStat : CharacterStat
{
    public MonsterData monsterData;

    public float _AttackRange = 2f;

    public Material _NormalMat;
    public Material _DashMat;

    public float _BulletDamage;
    public float _BulletSpeed = 10f;
    public float _BulletLifeTime;

    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData._MaxHp;
        _hp = _maxHp;
    }
}
