using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    [SerializeField] protected int[] _dmg = new int[7];
    public int[] DMG => _dmg;

    [SerializeField] protected bool[] _KnockBackFlag = new bool[7];
    public bool[] KnockBackFlag => _KnockBackFlag;

    [SerializeField] protected int[] _KnockBackDuration = new int[7];
    public int[] KnockBackDuration => _KnockBackDuration;

    [SerializeField] protected float[] _KnockBackPower = new float[7];
    public float[] KnockBackPower => _KnockBackPower;

    [SerializeField] protected float[] _KnockBackDelay = new float[7];
    public float[] KnockBackDelay => _KnockBackDelay;

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
