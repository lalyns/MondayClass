using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AK.Wwise.Event _FootStep = new AK.Wwise.Event();
    public void PlayFootStepSFX() {
        //_FootStep.Post(gameObject);
    }

    public AK.Wwise.Event _Dash = new AK.Wwise.Event();
    public void PlayDashSFX() { _Dash.Post(gameObject); }

    public AK.Wwise.Event _Skill1 = new AK.Wwise.Event();
    public void PlaySkill1SFX() { _Skill1.Post(gameObject); }

    public AK.Wwise.Event _Attack = new AK.Wwise.Event();
    public void PlayAttackSFX() { _Attack.Post(gameObject); }
}
