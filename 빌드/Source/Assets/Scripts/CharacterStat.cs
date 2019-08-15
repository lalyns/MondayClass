using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    protected float _str = 10.0f;
    public float Str { get { return _str; } }

    protected float _maxHp = 100.0f;
    public float MaxHp { get { return _maxHp; } }

    protected float _hp = 100.0f;
    public float Hp { get { return _hp; } }

    protected float _moveSpeed = 3.0f;
    public float MoveSpeed { get { return _moveSpeed; } }

    protected float _turnSpeed = 540.0f;
    public float TurnSpeed { get { return _turnSpeed; } }

    protected float _attackRange = 2.0f;
    public float AttackRange { get { return _attackRange; } }
    
    [HideInInspector]
    public CharacterStat lastHitBy = null;
    [SerializeField]
    public StatData statData;

    protected virtual void Awake()
    {
        //_turnSpeed = statData.TurnSpeed;
    }

    public void TakeDamage(CharacterStat from, float damage)
    {
        _hp = Mathf.Clamp(_hp - damage, 0, _maxHp);
        Debug.Log(string.Format("Name: {0}, HP: {1}", transform.name, Hp));

        if(_hp <= 0)
        {
            if (lastHitBy == null)
                lastHitBy = from;

            GetComponent<FSMManager>().SetDeadState();
            from.GetComponent<FSMManager>().NotifyTargetKilled();
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

    public static void ProcessDamage(CharacterStat from, CharacterStat to, int damage)
    {
        float finalDamage = damage;
        to.TakeDamage(from, finalDamage);
    }
}
