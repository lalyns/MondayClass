    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    public float[] skillCTime = new float[4];

    public float[] dmgCoefficient = new float[7];

    public float feverGaugeGetValue;
    public float skill2GaugeGetValue;
    public float skill3GaugeGetValue;

    public float transDuration;

    public int perStr = 5;
    public int perDef = 3;
    public int perHP = 150;
    public int perSkill1 = 40;
    public int perSkill2 = 25;
    public int perSkill3 = 10;
    public int perSkill3Speed = 10;
    public int perSkill1Bounce = 1;

    public float GetStr()
    {
        return _str + (perStr * GameSetting.rewardAbillity.strLevel);
    }
    public void SetStr()
    {
        _str = GetStr();
    }

    public float GetDfs()
    {
        return defense + (perDef * GameSetting.rewardAbillity.defLevel);
    }
    public void SetDfs()
    {
        defense = GetDfs();
    }

    public float GetHP()
    {
        return _maxHp + (perHP * GameSetting.rewardAbillity.hpLevel);
    }


    public float GetSkill1Damage()
    {
        return dmgCoefficient[3] + (perSkill1 * GameSetting.rewardAbillity.skill1DMGLevel);
    }
    public void SetSkill1Damage()
    {
        dmgCoefficient[3] = GetSkill1Damage();
    }

    public float GetSkill2Damage()
    {
        return dmgCoefficient[4] + (perSkill2 * GameSetting.rewardAbillity.skill2DMGLevel);
    }
    public void SetSkil2Damage()
    {
        dmgCoefficient[4] = GetSkill2Damage();
    }


    public float GetSkill3Damage()
    {
        return dmgCoefficient[5] + (perSkill3 * GameSetting.rewardAbillity.skill3DMGLevel);
    }
    public void SetSkill3Damage()
    {
        dmgCoefficient[5] = GetSkill3Damage();
    }

    public float GetSkill3Speed()
    {
        return PlayerFSMManager.Instance.Skill3MouseSpeed + (perSkill3Speed * GameSetting.rewardAbillity.skill3TurnLevel);
    }
    public void SetSkill3Speed()
    {
        PlayerFSMManager.Instance.Skill3MouseSpeed = GetSkill3Speed();
    }

    public int GetSkill1Bounce()
    {
        return PlayerFSMManager.Instance.Skill1BounceCount + (perSkill1Bounce * GameSetting.rewardAbillity.skill1BounceLevel);
    }
    public void SetSkill1Bounce()
    {
        PlayerFSMManager.Instance.Skill1BounceCount = GetSkill1Bounce();
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
        SetHp(GetHP());
        SetMaxHP(GetHP());
        SetDfs();
        SetStr();
        SetSkil2Damage();
        SetSkill1Bounce();
        SetSkill1Damage();
        SetSkill3Damage();
        SetSkill3Speed();








    }

    //public void RewardSkill1Damage(int value)
    //{
    //    dmgCoefficient[3] += value;
    //}
    //public void RewardSkill2Damage(int value)
    //{
    //    dmgCoefficient[4] += value;
    //}
    //public void RewardSkill3Damage(int value)
    //{
    //    dmgCoefficient[5] += value;
    //}

    //public void RewardStr(int value)
    //{
    //    _str += value;
    //}
    //public void RewardDefense(int value)
    //{
    //    defense += value;
    //}
    //public void RewardHP(int value)
    //{
    //    _maxHp += value;
    //}
}
