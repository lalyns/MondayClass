using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defence : Mission
{
    

    public float _LimitTime = 30f;
    public float _CurrentLeftTime;

    public ProtectedTarget _ProtectedTarget;
    public int _ProtectedTargetHP;

    protected override void Awake()
    {
        base.Awake();
        _CurrentLeftTime = _LimitTime;
    }

    private void Update()
    {
        if (!_IsMissionStart || _IsMissionClear) return;

        _CurrentLeftTime -= Time.deltaTime;


        if (CheckForClear())
        {
            MissionClear();
            _IsMissionClear = true;
        }
    }

    private bool CheckForClear()
    {
        bool isClear = false;

        if (_CurrentLeftTime <= 0f)
        {
            if(_ProtectedTarget.hp > 0)
            {
                isClear = true;
            }
        }

        return isClear;
    }

    public override void MissionClear()
    {
        base.MissionClear();

        ObjectManager.ReturnPoolAllMonster();

        GameObject[] balls = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }

    }

    public override void MissionEnd()
    {
        base.MissionEnd();

        _ProtectedTarget.hp = 100;
        _CurrentLeftTime = _LimitTime;
    }
}
