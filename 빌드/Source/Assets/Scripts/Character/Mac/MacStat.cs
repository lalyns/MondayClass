﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacStat : CharacterStat
{
    public MonsterData monsterData;

    public GameObject _AttackEffect;
    public GameObject _SkillEffect;

    public Material _NormalMat;
    public Material _BeforeAttackMat;
    public Material _AttackMat;

    public float _AttackRange = 10f;
}
