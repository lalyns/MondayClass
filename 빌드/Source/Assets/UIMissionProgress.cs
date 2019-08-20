using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionProgress : MonoBehaviour
{
    public Text _MissionString;
    public Text _MissionType;

    public GameObject _AnnihilationUI;
    public Text _LeftMonster;
    public Text _LeftTime;

    public GameObject _SurviveUI;
    public Text _LeftProgress;
    public Text _LeftSurviveTime;

    public GameObject _DefenceUI;
    public Text _LeftProtectedTargetHP;
    public Text _LeftDefenceTime;

    public void SetMissionType(string type)
    {
        //_MissionType.text = "[ " + type + " ]";
    }

    public void SetMissionString(string str)
    {
        _MissionString.text = str;
    }

    public void SetLeftTime(float time)
    {
        int mm = (int)time / 60;
        int ss = (int)time % 60;

        _LeftTime.text = "남은 시간 : " + mm + " : " + ss;
    }

    public void SetLeftMonster(int quantity)
    {
        _LeftMonster.text = "남은   수 : " + quantity + " 마리";
    }

    public void SetProgress(int Progress, int maxProgress)
    {
        _LeftProgress.text = "진 행 도 :" + Progress + " / " + maxProgress;
    }

    public void SetLeftSurviveTime(float time)
    {
        int mm = (int)time / 60;
        int ss = (int)time % 60;

        _LeftSurviveTime.text = "남은 시간 : " + mm + " : " + ss;
    }

    public void SetProtectedTargetHP(float hp)
    {
        _LeftProtectedTargetHP.text = "남은 체력 :" + hp;
    }

    public void SetLeftDefenceTime(float time)
    {
        int mm = (int)time / 60;
        int ss = (int)time % 60;

        _LeftDefenceTime.text = "남은 시간 : " + mm + " : " + ss;
    }

}
