using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatData : ScriptableObject
{
    public float _MoveSpeed;
    public float _AttackSpeed;
    public float _MaxHp;
    public float _Deffece;

    public float _Offence;
    public float _CriticalProbability;
    public float _CriticalScale = 1.5f;

    public bool _AttackType; // True = Range, False = Melee

    public float _SkillCoolTime1;
    public float _SkillCoolTime2;

    public float _DashSpeed;
    public float _AttackRange;

    // if AttackType is melee, using this datas.
    public float _AttackCapsule_Center;
    public float _AttackCapsule_Radius;
    public float _AttackCapsule_Height;
}
