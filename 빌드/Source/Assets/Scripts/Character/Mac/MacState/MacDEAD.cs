using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class MacDEAD : MacFSMState
{
    bool Dead = false;
    float time = 0;

    public override void BeginState()
    {
        base.BeginState();

        Dead = false;
        time = 0;

        GameLib.DissoveActive(_manager.materialList, true);
        StartCoroutine(GameLib.Dissolving(_manager.materialList));

        useGravity = false;
        _manager.CC.detectCollisions = false;
    }

    public override void EndState()
    {
        base.EndState();

        GameLib.DissoveActive(_manager.materialList, false);

        time = 0;

        Dead = true;

        useGravity = true;
        _manager.CC.detectCollisions = true;

        MonsterPoolManager._Instance._Mac.ItemReturnPool(gameObject, "monster");

        if (MissionManager.Instance.CurrentMissionType == MissionType.Annihilation)
            UserInterface.Instance.GoalEffectPlay();
    }

    protected override void Update()
    {
        base.Update();

        time += 0.45f * Time.deltaTime;

        if (time > 0.7 && !Dead)
        {
            Dead = true;
        }

        if (Dead)
        {
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
