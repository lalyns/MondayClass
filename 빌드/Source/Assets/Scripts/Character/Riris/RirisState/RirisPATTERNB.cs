using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisPATTERNB : RirisFSMState
{
    bool _IsTele = false;

    public GameObject PatternBReadyEffect;
    public GameObject PatternBAttackEffect;
    bool _IsAttackReady = false;
    public bool isEnd = false;

    public ObjectPool bulletPool;
    public Transform bulletPos;
    public Transform[] positionB;

    void BulletPattern()
    {
        _manager.Anim.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager.Anim.transform));
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

    public override void BeginState()
    {
        base.BeginState();

        _manager._Weapon.gameObject.SetActive(true);

        var pos = MissionManager.Instance.CurrentMission.Grid.center.position;

        transform.position = pos;
        _manager._Weapon.position = pos;

        transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager.Anim.transform));
        _manager._Weapon.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager._Weapon.transform));

        useGravity = false;

        if (_manager._Phase >= 1)
        {
            var randPos = UnityEngine.Random.Range(0, MissionManager.Instance.CurrentMission.Grid.mapPositions.Count);
            _manager.transform.position = MissionManager.Instance.CurrentMission.Grid.mapPositions[randPos];
            _manager.Anim.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager.Anim.transform));

            _manager.Anim.Play("PatternC");
        }

        _manager._WeaponAnimator.Play("Weapon_Skill2_A");

    }

    public override void EndState()
    {
        base.EndState();

        _IsAttackReady = false;
        _IsTele = false;
        isEnd = false;
        PatternBReadyEffect.SetActive(false);
        PatternBAttackEffect.SetActive(false);

        _manager._Weapon.gameObject.SetActive(false);
        useGravity = true;
    }

    protected override void Update()
    {
        base.Update();

        if (_IsAttackReady)
             PatternBAttackEffect.transform.position = _manager._WeaponCenter.transform.position;

        if (isEnd)
        {
            _manager.SetState(RirisState.PATTERNEND);
            isEnd = false;
        }
    }

    public void AttackCheck()
    {
        float damage = _manager.Stat.damageCoefiiecient[0] * 0.01f *
               (_manager.Stat.Str + _manager.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
               - PlayerFSMManager.Instance.Stat.Defense;

        var hitTarget = GameLib.SimpleDamageProcess(transform, _manager.Stat.AttackRange, "Player", _manager.Stat, damage);
    }

    public override void Start()
    {
        bulletPool = BossEffects.Instance.bullet;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void AttackReadyEnd()
    {
        _IsAttackReady = true;
        PatternBAttackEffect.SetActive(true);
    }
}
