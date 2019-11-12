using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Mission;

public class RedHatDISSOLVE : RedHatFSMState
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

        _time = 0;

        GetComponent<RedHatDEAD>().StopAllCoroutines();
        GameLib.DissoveActive(_manager.materialList, false);

        useGravity = true;
        _manager.CC.detectCollisions = true;

        
        MonsterPoolManager._Instance._RedHat.ItemReturnPool(gameObject, MonsterType.RedHat);
    }

    protected override void Update()
    {
        base.Update();

        _time += Time.deltaTime;

        if (_time >= 3f)
        {            
            _manager.SetState(RedHatState.POPUP);        
            return;
        }
    }
}