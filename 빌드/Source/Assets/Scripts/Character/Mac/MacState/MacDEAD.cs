using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Mission;

public class MacDEAD : MacFSMState
{
    public override void BeginState()
    {
        base.BeginState();

        GameLib.DissoveActive(_manager.materialList, true);
        StartCoroutine(GameLib.Dissolving(_manager.materialList));
        StartCoroutine(GameLib.BlinkOff(_manager.materialList));

        var voice = _manager._Sound.monsterVoice;
        voice.PlayMonsterVoice(this.gameObject, voice.macDieVoice);

        useGravity = false;
        _manager.CC.detectCollisions = false;
        _manager._MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }
    private void Start()
    {
    }
    public override void EndState()
    {
        base.EndState();

        useGravity = true;
        _manager.CC.detectCollisions = true;

        GameStatus.Instance.RemoveActivedMonsterList(gameObject);

        _manager.agent.speed = 0;
        _manager.agent.angularSpeed = 0;

        if(GameStatus.currentGameState != CurrentGameState.EDITOR)
            if (MissionManager.Instance.CurrentMissionType == MissionType.Annihilation)
            {
                UserInterface.Instance.GoalEffectPlay();
                MissionA a = MissionManager.Instance.CurrentMission as MissionA;
                a.Invoke("MonsterCheck", 5f);
            }
    }

    public void DeadHelper()
    {
        _manager.SetState(MacState.DISSOLVE);
    }

    protected override void Update()
    {
        base.Update();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

   
}
