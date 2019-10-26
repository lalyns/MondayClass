using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacDISSOLVE : MacFSMState
{
    float _time = 0;
    public override void BeginState()
    {
        base.BeginState();

        var sound = _manager._Sound.monsterSFX;
        sound.PlayMonsterSFX(_manager.gameObject, sound.monsterDisAppear);

    }

    public override void EndState()
    {
        base.EndState();

        useGravity = true;
        _manager.CC.detectCollisions = true;

        MonsterPoolManager._Instance._Mac.ItemReturnPool(gameObject, MonsterType.Mac);


        _time = 0;
    }

    protected override void Update()
    {
        base.Update();

        _time += Time.deltaTime;

        if (_time >= 3f)
        {
            _manager.SetState(MacState.POPUP);

            return;
        }
    }
}
