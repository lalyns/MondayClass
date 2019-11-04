using System.Collections;
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
        StartCoroutine(GameLib.BlinkOff(_manager.materialList));
        GameLib.DissoveActive(_manager.materialList, true);
        StartCoroutine(GameLib.Dissolving(_manager.materialList));

        var voice = _manager.sound.monsterVoice;
        voice.PlayMonsterVoice(this.gameObject, voice.redhatDeadVoice);

        useGravity = false;
        _manager.CC.detectCollisions = false;
        _manager.mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _manager.agent.speed = 0;
        _manager.agent.angularSpeed = 0;
    }
    private void Start()
    {
        GetComponentInChildren<RedHatHitCollider>().capsule.enabled = false;
    }
    public override void EndState()
    {
        base.EndState();

        
        if (MissionManager.Instance.CurrentMissionType == MissionType.Annihilation)
        {
            UserInterface.Instance.GoalEffectPlay();
            MissionA a = MissionManager.Instance.CurrentMission as MissionA;
            a.Invoke("MonsterCheck", 5f);
        }

        GameStatus.Instance.RemoveActivedMonsterList(gameObject);

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
        _manager.SetState(RedHatState.DISSOLVE);
    }
}
