using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    public bool isPlayer;

    [SerializeField] protected float _str = 10.0f;
    public float Str { get { return _str; } }

    [SerializeField] protected float _int = 10.0f;
    public float Int { get { return _int; } }
    //[SerializeField] protected bool[] _KnockBackFlag = new bool[7];
    //public bool[] KnockBackFlag => _KnockBackFlag;

    //[SerializeField] protected int[] _KnockBackDuration = new int[7];
    //public int[] KnockBackDuration => _KnockBackDuration;

    //[SerializeField] protected float[] _KnockBackPower = new float[7];
    //public float[] KnockBackPower => _KnockBackPower;

    //[SerializeField] protected float[] _KnockBackDelay = new float[7];
    //public float[] KnockBackDelay => _KnockBackDelay;
    [SerializeField] protected float defense = 10f;
    public float Defense => defense;

    [SerializeField] protected float _maxHp = 1000.0f;
    public float MaxHp { get { return _maxHp; } }

    public float _hp = 1000.0f;
    public float Hp { get { return _hp; } }
    public void SetHp(float hp)
    {
        _hp = hp;
    }

    protected float _moveSpeed = 3.0f;
    public float MoveSpeed { get { return _moveSpeed; } }

    protected float _turnSpeed = 540.0f;
    public float TurnSpeed { get { return _turnSpeed; } }

    protected float _attackRange = 1.0f;
    public float AttackRange { get { return _attackRange; } }
          
       



    [HideInInspector]
    public CharacterStat lastHitBy = null;

    [SerializeField]
    public StatData statData;

    protected virtual void Awake()
    {

    }

    public void TakeDamage(CharacterStat from, float damage)
    {
        _hp = Mathf.Clamp(_hp - (damage - this.Defense), 0, _maxHp);

        if (from.isPlayer)
        {
            var playerStat = from as PlayerStat;
            
            if (PlayerFSMManager.Instance.isNormal)
                PlayerFSMManager.Instance.SpecialGauge += playerStat.feverGaugeGetValue;
        }

    }

    private static float CalcDamage(CharacterStat from, CharacterStat to)
    {
        return from.Str;
    }

    public static void ProcessDamage(CharacterStat from, CharacterStat to)
    {
        float finalDamage = CalcDamage(from, to);
        to.TakeDamage(from, finalDamage);
    }

    public static void ProcessDamage(CharacterStat from, CharacterStat to, float damage)
    {
        to.TakeDamage(from, damage);
    }
}
