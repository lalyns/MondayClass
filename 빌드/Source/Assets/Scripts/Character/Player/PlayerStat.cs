    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    public float[] skillCTime = new float[4];

    public float[] dmgCoefficient = new float[7];

    public float feverGaugeGetValue;
    public float transDuration;

    public int perStr = 5;
        
    public void RewardSkill1Damage(int value)
    {
        dmgCoefficient[3] += value;
    }
    public void RewardSkill2Damage(int value)
    {
        dmgCoefficient[4] += value;
    }
    public void RewardSkill3Damage(int value)
    {
        dmgCoefficient[5] += value;
    }

    public float GetStr()
    {
        return Str + (perStr * GameSetting.rewardAbillity.strLevel);
    }

    public void RewardStr(int value)
    {
        _str += value;
    }
    public void RewardDefense(int value)
    {
        defense += value;
    }
    public void RewardHP(int value)
    {
        _maxHp += value;
    }

    public void StrSet(int value)
    {
        _str = value;
    }

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
