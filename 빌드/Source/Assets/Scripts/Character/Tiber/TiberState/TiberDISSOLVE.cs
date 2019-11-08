using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Mission;

public class TiberDISSOLVE : TiberFSMState
{
    float _time = 0;
    public DamageDisplay display;

    public override void BeginState()
    {
        base.BeginState();

        var sound = _manager.sound.monsterSFX;
        sound.PlayMonsterSFX(_manager.gameObject, sound.monsterDisAppear);

        for (int i = 0; i < display.texts.Length; i++)
            display.texts[i].gameObject.SetActive(false);
    }

    public override void EndState()
    {
        base.EndState();

        useGravity = true;
        _manager.CC.detectCollisions = true;
        _time = 0;
        
        GameLib.DissoveActive(_manager.materialList, false);
        MonsterPoolManager._Instance._Tiber.ItemReturnPool(gameObject, MonsterType.Tiber);
    }

    protected override void Update()
    {
        base.Update();

        if (_time >= 3f)
        {
            
            _manager.SetState(TiberState.POPUP);            
            return;
        }
    }
}
