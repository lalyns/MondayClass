﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RirisPATTERNB : RirisFSMState
{
    public GameObject PatternBReadyEffect;
    public GameObject PatternBAttackEffect;
    bool isAttackReady = false;
    public bool isEnd = false;

    public ObjectPool bulletPool;
    public Transform bulletPos;
    public Transform[] positionB;

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

        var pos = MissionManager.Instance.CurrentMission.MapGrid.center.position;

        transform.position = pos;
        _manager._Weapon.position = pos;

        //var sound = _manager.sound.ririsVoice;
        //sound.PlayRirisVoice(this.gameObject, sound.dash);
        _manager.Anim.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager.Anim.transform));
        _manager._Weapon.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager._Weapon.transform));

        PatternBReadyEffect.SetActive(true);
        PatternBReadyEffect.transform.LookAt(PlayerFSMManager.GetLookTargetPos(PatternBReadyEffect.transform));
        PatternBReadyEffect.GetComponentInChildren<Animator>().Play("Play");

        useGravity = false;

        if (_manager._Phase >= 1)
        {
            var randPos = UnityEngine.Random.Range(0, MissionManager.Instance.CurrentMission.MapGrid.mapPositions.Count);
            var posa = MissionManager.Instance.CurrentMission.MapGrid.mapPositions[randPos];

            _manager.Anim.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager.Anim.transform));

            _manager.Anim.Play("PatternC");
        }

        _manager._WeaponAnimator.Play("Weapon_Skill2_A");

    }

    public override void EndState()
    {
        base.EndState();

        isAttackReady = false;
        isEnd = false;
        PatternBReadyEffect.SetActive(false);
        PatternBAttackEffect.SetActive(false);

        _manager._Weapon.gameObject.SetActive(false);
        useGravity = true;
    }

    protected override void Update()
    {
        base.Update();

        if (isAttackReady)
             PatternBAttackEffect.transform.position = _manager._WeaponCenter.transform.position;

        if (isEnd)
        {
            _manager.SetState(RirisState.PATTERNEND);
            isEnd = false;
        }
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
        isAttackReady = true;
        PatternBAttackEffect.SetActive(true);
    }
}
