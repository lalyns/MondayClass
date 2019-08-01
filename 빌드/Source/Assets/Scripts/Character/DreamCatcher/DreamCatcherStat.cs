using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCatcherStat : CharacterStat
{
    public MonsterData monsterData;

    public GameObject _AttackEffect;

    public Material _NormalMat;
    public Material _BeforeAttackMat;
    public Material _AttackMat;

    public float _AttackRange = 10f;
}
