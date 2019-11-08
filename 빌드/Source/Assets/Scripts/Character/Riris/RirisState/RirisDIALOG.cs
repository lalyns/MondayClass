using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Mission;
using MC.Sound;

public class RirisDIALOG : RirisFSMState
{
    public override void BeginState()
    {
        base.BeginState();


        GameStatus.SetCurrentGameState(CurrentGameState.Dialog);

        var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();
        UserInterface.DialogSetActive(true);
        UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[7], () =>
        {
            _manager.SetState(RirisState.PATTERNEND);
            GameManager.Instance.CharacterControl = true;
        });
    }

    public override void EndState()
    {
        base.EndState();


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
    }
}
