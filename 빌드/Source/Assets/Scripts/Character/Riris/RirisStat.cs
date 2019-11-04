using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisStat : CharacterStat
{
    public float _BulletSpeed = 10f;
    public float _BulletLifeTime;

    public float[] damageCoefiiecient;

    public float addStrPerRound;
    public float addDefPerRound;

    protected override void Awake() {
        base.Awake();
        SetHp(_maxHp);
    }
}
