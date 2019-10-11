﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Mission;

public class RedHatDEAD : RedHatFSMState
{

    public override void BeginState()
    {
        base.BeginState();

        if (_manager.dashEffect != null)
        {
            _manager.dashEffect.SetActive(false);
            _manager.dashEffect = null;
        }

        GameLib.DissoveActive(_manager.materialList, true);
        StartCoroutine(GameLib.Dissolving(_manager.materialList));

        useGravity = false;
        _manager.CC.detectCollisions = false;
    }

    public override void EndState()
    {
        base.EndState();

        GameLib.DissoveActive(_manager.materialList, false);
        StartCoroutine(GameLib.BlinkOff(_manager.materialList));
        useGravity = true;
        _manager.CC.detectCollisions = true;
        
        if (MissionManager.Instance.CurrentMissionType == MissionType.Annihilation)
        {
            UserInterface.Instance.GoalEffectPlay();
            MissionA a = MissionManager.Instance.CurrentMission as MissionA;
            a.Invoke("MonsterCheck", 5f);
        }

        MonsterPoolManager._Instance._RedHat.ItemReturnPool(gameObject, MonsterType.RedHat);

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void DeadHelper()
    {
        Debug.Log("Dead Call");
        _manager.SetState(RedHatState.POPUP);
    }
}
