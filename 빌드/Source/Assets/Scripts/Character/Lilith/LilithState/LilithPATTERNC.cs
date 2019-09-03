using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 불릿패턴
public class LilithPATTERNC : LilithFSMState
{
    float _CurrentTime = 0;

    int pattern = 0;

    bool type = false;

    public ObjectPool bulletPool;
    public Transform[] positionA;
    public Transform[] positionB;

    private void Start()
    {
        bulletPool = EffectPoolManager._Instance._BossBulletPool;
    }

    void BulletPatternA()
    {
        transform.LookAt(_manager.PlayerCapsule.transform);
        foreach (Transform t in positionA) {
            bulletPool.ItemSetActive(t, true);
        }
    }

    void BulletPatternB()
    {
        transform.LookAt(_manager.PlayerCapsule.transform);
        foreach (Transform t in positionB)
        {
            bulletPool.ItemSetActive(t, false);
        }
    }

    public override void BeginState()
    {
        base.BeginState();
    }

    protected override void Update()
    {
        _CurrentTime += Time.deltaTime;

        if(_CurrentTime > 5.0f)
        {
            _CurrentTime = 0;

            if (!type)
            {
                BulletPatternA();
                type = !type;
            }
            else
            {
                StartCoroutine("PatternB");
                type = !type;
            }
        }
    }

    IEnumerator PatternB()
    {
        for (int i = 0; i < 4; i++)
        {
            BulletPatternB();
            yield return new WaitForSeconds(0.6f);
        }
    }
}
