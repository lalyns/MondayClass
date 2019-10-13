using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Mission;

public class TiberDEAD : TiberFSMState
{
    public override void BeginState()
    {
        base.BeginState();

        //if (_manager.dashEffect != null)
        //{
        //    EffectPoolManager._Instance._TiberSkillRange.ItemReturnPool(_manager.dashEffect);
        //    _manager.dashEffect = null;
        //}

        //GameLib.DissoveActive(_manager.materialList, true);
        //StartCoroutine(GameLib.Dissolving(_manager.materialList));
        GameLib.DissoveActive(_manager.materialList, true);
        StartCoroutine(GameLib.Dissolving(_manager.materialList));

        useGravity = false;
        _manager.CC.detectCollisions = false;
        _manager._MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    public override void EndState()
    {
        base.EndState();

        GameLib.DissoveActive(_manager.materialList, false);
        StartCoroutine(GameLib.BlinkOff(_manager.materialList));

        //GameLib.DissoveActive(_manager.materialList, false);
        useGravity = true;
        _manager.CC.detectCollisions = true;

        if (MissionManager.Instance.CurrentMissionType == MissionType.Annihilation)
        {
            UserInterface.Instance.GoalEffectPlay();
            MissionA a = MissionManager.Instance.CurrentMission as MissionA;
            a.Invoke("MonsterCheck", 5f);
        }

        MonsterPoolManager._Instance._Tiber.ItemReturnPool(gameObject, MonsterType.Tiber);

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void DeadSupport()
    {
        _manager.SetState(TiberState.POPUP);
        MonsterPoolManager._Instance._Mac.ItemReturnPool(gameObject, MonsterType.Tiber);

    }
    public void DeadHelper()
    {
        Invoke("DeadSupport", 3f);
        Debug.Log("Dead Call");

    }
}
