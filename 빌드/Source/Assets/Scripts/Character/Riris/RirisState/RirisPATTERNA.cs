using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisPATTERNA : RirisFSMState
{
    public GameObject _PatternAReadyEffect;
    public GameObject _PatternAAttackEffect;

    public bool SetJumpState = false;

    public float targetSetDelay = 1.3f;
    public float stompDelay = 0.5f;

    float stompCount = 0;

    public bool PatternEnd = false;

    public Transform playerTransform;
    public Vector3 targetPos;

    public ObjectPool bulletPool;
    public Transform bulletPos;
    public Transform[] positionB;

    public override void BeginState()
    {
        base.BeginState();

        playerTransform = PlayerFSMManager.Instance.Anim.transform;
        _manager._Weapon.gameObject.SetActive(true);
        _manager._Weapon.transform.position = this.transform.position;
        _manager._Weapon.transform.rotation = this.transform.rotation;


        SetJumpState = false;
        PatternEnd = false;
        useGravity = false;

        var sound = _manager.sound.ririsVoice;
        sound.PlayRirisVoice(this.gameObject, sound.stomp);

        _manager.Anim.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager.Anim.transform));
        _manager._Weapon.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager._Weapon.transform));

        stompCount = 0;

        if (_manager._Phase >= 1)
        {
            

            var randPos = UnityEngine.Random.Range(0, MissionManager.Instance.CurrentMission.MapGrid.mapPositions.Count);
            var pos = MissionManager.Instance.CurrentMission.MapGrid.mapPositions[randPos];

            _manager.Anim.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager.Anim.transform));

            _manager.Anim.Play("PatternC");
        }
        
        
        _manager._WeaponAnimator.Play("Weapon_Skill1_Jump");

    }

    public override void EndState()
    {
        base.EndState();

        _manager.Anim.SetBool("Stomp", false);
        _manager._WeaponAnimator.SetBool("Stomp", false);
        _PatternAAttackEffect.SetActive(false);
        SetJumpState = false;
        PatternEnd = false;
        useGravity = true;
        stompCount = 0;
        _manager._Weapon.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (SetJumpState) {
            stompCount += Time.deltaTime;
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
        bulletPool = BossEffects.Instance.bullet;
    }

    void BulletPattern()
    {
        _manager.Anim.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager.Anim.transform));
        foreach (Transform t in positionB)
        {
            GameObject bullet = bulletPool.ItemSetActive(t.position);
            bullet.GetComponent<RirisBullet>().SetBullet(bulletPos.position, false);
        }
    }

    public IEnumerator AddBullet()
    {
        var sound = _manager.sound.ririsVoice;
        sound.PlayRirisVoice(this.gameObject, sound.batswarm1);
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


        if (_manager._Phase < 1)
            transform.position = targetPos;

        _manager._Weapon.position = targetPos;
        
        _PatternAAttackEffect.SetActive(true);
        _PatternAAttackEffect.transform.position = targetPos;

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
