using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 불릿패턴
public class RirisPATTERNC : RirisFSMState
{
    bool type = false;

    public ObjectPool bulletPool;

    public Transform bulletPos;
    public Quaternion init;

    public Transform[] positionA;
    public Transform[] positionB;

    public override void Start()
    {
        bulletPool = BossEffects.Instance.bullet;
        init = bulletPos.rotation;
    }

    void BulletPatternA()
    {
        _manager.Anim.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager.Anim.transform));
        foreach (Transform t in positionA) {
            bulletPool.ItemSetActive(t, true);
        }
    }

    void BulletPatternB()
    {
        _manager.Anim.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager.Anim.transform));
        foreach (Transform t in positionB)
        {
            bulletPool.ItemSetActive(t, true);
        }
    }

    public override void BeginState()
    {
        base.BeginState();

        useGravity = false;

        _manager.transform.position = MissionManager.Instance.CurrentMission.MapGrid.center.position;

        _manager.transform.LookAt(PlayerFSMManager.GetLookTargetPos(_manager.transform));
    }

    public override void EndState()
    {
        base.EndState();

        useGravity = true;
    }

    protected override void Update()
    {

    }

    public IEnumerator FireBullet()
    {
        bulletPos.position = _manager.Pevis.transform.position;

        for (int i = 0; i < 4; i++)
        {
            BulletPatternB();

            var random = Random.Range(0, 999) % 2 == 0 ? -1f : 1f;
            bulletPos.Rotate(0, random * 25f, 0);

            yield return new WaitForSeconds(0.8f);
        }

        yield return new WaitForSeconds(1f);

        bulletPos.rotation = init;
        _manager.SetState(RirisState.PATTERNEND);
    }

}
