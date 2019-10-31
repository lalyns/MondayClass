using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Mission;
using MC.Sound;

public class RirisDEAD : RirisFSMState
{
    public override void BeginState()
    {
        base.BeginState();

        UserInterface.SetPlayerUserInterface(false);

        StartCoroutine(MCSoundManager.BGMFadeOut(1f));
        MCSoundManager.StopBGM();
        BossDirector.Instance.PlayDeadCine();
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
