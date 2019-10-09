using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisPATTERNA : RirisFSMState
{
    public GameObject _PatternAReadyEffect;
    public GameObject _PatternAAttackEffect;

    public bool SetJumpState = false;

    public float targetSetDelay = 1.3f;
    public float stompDelay = 1.5f;

    float stompCount = 0;

    public bool PatternEnd = false;

    Transform playerTransform;
    Vector3 targetPos;

    public ObjectPool bulletPool;
    public Transform bulletPos;
    public Transform[] positionB;

    public override void BeginState()
    {
        base.BeginState();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _manager._Weapon.gameObject.SetActive(true);
        _manager._Weapon.transform.position = this.transform.position;
        _manager._Weapon.transform.rotation = this.transform.rotation;

        SetJumpState = false;
        PatternEnd = false;
        useGravity = false;

        stompCount = 0;

        if (_manager._Phase >= 1)
        {
            _manager.Anim.Play("PatternC");
        }

    }

    public override void EndState()
    {
        base.EndState();

        _manager._Weapon.gameObject.SetActive(false);
        _manager.Anim.SetBool("Stomp", false);
        _manager._WeaponAnimator.SetBool("Stomp", false);
        _PatternAAttackEffect.SetActive(false);
        SetJumpState = false;
        PatternEnd = false;
        useGravity = true;

        stompCount = 0;

    }

    protected override void Update()
    {
        base.Update();

        if (SetJumpState) {
            stompCount += Time.deltaTime;

            if (stompCount < targetSetDelay)
            {
                targetPos = playerTransform.position;
            }
            _PatternAReadyEffect.SetActive(true);
            _PatternAReadyEffect.transform.position = targetPos;
        }


        if (stompCount > stompDelay) {
            Stomp();
            SetJumpState = false;
            stompCount = 0;
        }

        if(PatternEnd)
        {
            _manager.SetState(RirisState.PATTERNEND);
        }

    }

    public override void Start()
    {
        bulletPool = EffectPoolManager._Instance._BossBulletPool;
    }

    void BulletPattern()
    {
        transform.LookAt(_manager.PlayerCapsule.transform);
        foreach (Transform t in positionB)
        {
            bulletPool.ItemSetActive(t, false);
        }
    }

    public IEnumerator AddBullet()
    {
        bulletPos.position = _manager.Pevis.transform.position;

        for (int i = 0; i < 4; i++)
        {
            BulletPattern();
            yield return new WaitForSeconds(0.6f);
        }
    }

    public void Stomp()
    {
        _PatternAReadyEffect.SetActive(false);

        if(_manager._Phase < 1)
            transform.position = targetPos;

        _manager._Weapon.position = targetPos;

        _PatternAAttackEffect.SetActive(true);

        ParticleSystem[] particleSystems = _PatternAAttackEffect.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem p in particleSystems)
        {
            p.Play();
        }

        _manager.Anim.SetBool("Stomp", true);
        _manager._WeaponAnimator.SetBool("Stomp", true);
    }

    public void AttackCheck()
    {

        float damage = _manager.Stat.damageCoefiiecient[0] * 0.01f *
            (_manager.Stat.Str + _manager.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
            - PlayerFSMManager.Instance.Stat.Defense;

        var hitTarget = GameLib.SimpleDamageProcess(transform, _manager.Stat.AttackRange, "Player", _manager.Stat, damage);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
