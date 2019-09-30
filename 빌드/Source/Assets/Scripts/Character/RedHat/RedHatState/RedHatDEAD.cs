using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class RedHatDEAD : RedHatFSMState
{
    bool Dead = false;
    float time = 0;

    public override void BeginState()
    {
        base.BeginState();

        time = 0;
        Dead = false;

        if (_manager.dashEffect != null)
        {
            EffectPoolManager._Instance._RedHatSkillRange.ItemReturnPool(_manager.dashEffect);
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
        useGravity = true;
        _manager.CC.detectCollisions = true;
        
        MonsterPoolManager._Instance._RedHat.ItemReturnPool(gameObject, "monster");
        time = 0;
        Dead = false;
        if(MissionManager.Instance.CurrentMissionType == MissionType.Annihilation)
            UserInterface.Instance.GoalEffectPlay();
    }

    protected override void Update()
    {
        base.Update();

        time += 0.45f * Time.deltaTime;

        if(time > 0.7 && !Dead)
        {
            Dead = true;
        }

        if (Dead) {
            EndState();
            Dead = false;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void DeadHelper()
    {
        Dead = true;
    }
}
