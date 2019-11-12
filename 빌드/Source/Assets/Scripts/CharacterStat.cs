using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class CharacterStat : MonoBehaviour
{
    public bool isPlayer;

    [SerializeField] protected float _str = 10.0f;
    public float Str { get { return _str; } }
    public float SpecialStr { get { return _str + 10; } }
    //[SerializeField] protected bool[] _KnockBackFlag = new bool[7];
    //public bool[] KnockBackFlag => _KnockBackFlag;

    //[SerializeField] protected int[] _KnockBackDuration = new int[7];
    //public int[] KnockBackDuration => _KnockBackDuration;

    [SerializeField] protected float _KnockBackPower = 2f;
    public float KnockBackPower => _KnockBackPower;

    //[SerializeField] protected float[] _KnockBackDelay = new float[7];
    //public float[] KnockBackDelay => _KnockBackDelay;
    [SerializeField] protected float defense = 10f;
    public float Defense => defense;
    

    [SerializeField] protected float _maxHp = 1000.0f;
    public float MaxHp { get { return _maxHp; } }

    public float _hp = 1000.0f;
    public float Hp { get { return _hp; } }
    public virtual void SetHp(float hp)
    {
        _hp = hp;
    }
    public virtual void SetMaxHP(float maxhp)
    {
        _maxHp = maxhp;
    }
    [SerializeField] protected float _moveSpeed = 3.0f;
    public float MoveSpeed { get { return _moveSpeed; } }

    [SerializeField] protected float _turnSpeed = 540.0f;
    public float TurnSpeed { get { return _turnSpeed; } }

    [SerializeField] protected float _attackRange = 1.0f;
    public float AttackRange { get { return _attackRange; } }

    [HideInInspector]
    public CharacterStat lastHitBy = null;

    public MonsterType monsterType;

    [SerializeField]
    public StatData statData;


    protected virtual void Awake()
    {

    }

    public void TakeDamage(CharacterStat from, float damage)
    {
        if(damage >= Defense)
            _hp = Mathf.Clamp(_hp - (damage - this.Defense), 0, _maxHp);

        if (from.isPlayer)
        {
            var playerStat = from as PlayerStat;

            if (PlayerFSMManager.Instance.isNormal && !PlayerFSMManager.Instance.isSkill3 && !PlayerFSMManager.Instance.isSkill2AttackTime)
            {
                PlayerFSMManager.Instance.SpecialGauge += playerStat.feverGaugeGetValue;
            }
            if (PlayerFSMManager.Instance.isNormal && PlayerFSMManager.Instance.isSkill2AttackTime && !PlayerFSMManager.Instance.isSkill3)
            {
                PlayerFSMManager.Instance.SpecialGauge += playerStat.skill2GaugeGetValue;
            }
            if (PlayerFSMManager.Instance.isNormal && PlayerFSMManager.Instance.isSkill3)
            {
                PlayerFSMManager.Instance.SpecialGauge += playerStat.skill3GaugeGetValue;
            }
        }

    }


    public static void ProcessDamage(CharacterStat from, CharacterStat to, float damage)
    {

        to.TakeDamage(from, damage);

        if (from.isPlayer)
        {
            from.lastHitBy = to;

            if (from.lastHitBy._hp <= 0)
            {
                from.lastHitBy = null;
            }
        }
    }
}
