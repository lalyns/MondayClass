    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    public float[] skillCTime = new float[4];

    public float[] dmgCoefficient = new float[7];

    public float feverGaugeGetValue;
    public float transDuration;

    protected override void Awake()
    {
        base.Awake();

        SetStatValue();
    }

    public void SetStatValue()
    {
        SetHp(_maxHp);

    }
}
