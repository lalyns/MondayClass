using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempMissionProgress : MonoBehaviour
{
    
    public static TempMissionProgress _Instance;
    public void Start()
    {
        if(_Instance == null)
        {
            _Instance = GetComponent<TempMissionProgress>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // 미션 표시창 구성
    public Text MissionName;

    public Image _missionType;
    public Sprite _anihilation;
    public Sprite _survive;
    public Sprite _defence;

    public Image _goalIcon;
    public Sprite _mob;
    public Sprite _star;
    public Sprite _pillarHP;

    public Text _Time;
    public Text _Remain;

    private void Update()
    {
        switch(MissionManager.Instance.CurrentMissionType)
        {
            case MissionManager.MissionType.Annihilation:
                SetMission1();
                break;
            case MissionManager.MissionType.Defence:
                SetMission3();
                break;
            case MissionManager.MissionType.Survival:
                SetMission2();
                break;
        }
    }

    public static void SetMission1()
    {
        _Instance.MissionName.text = "섬멸 미션";
        _Instance._missionType.sprite = _Instance._anihilation;
        _Instance._goalIcon.sprite = _Instance._mob;

        float curTime = GameStatus._Instance._LimitTime;
        int min = (int)(curTime / 60f);
        int sec = (int)(curTime % 60f);

        if (sec >= 10)
        {
            _Instance._Time.text = min + "'" + sec + "''";
        }
        else
        {
            _Instance._Time.text = min + "'0" + sec + "''";
        }

        _Instance._Remain.text = GameStatus._Instance.ActivedMonsterList.Count + "마리";

    }

    public static void SetMission2()
    {
        _Instance.MissionName.text = "생존 미션";
        _Instance._missionType.sprite = _Instance._survive;
        _Instance._goalIcon.sprite = _Instance._star;

        float curTime = GameStatus._Instance._LimitTime;
        int min = (int)(curTime / 60f);
        int sec = (int)(curTime % 60f);

        if (sec >= 10)
        {
            _Instance._Time.text = min + "'" + sec + "''";
        }
        else
        {
            _Instance._Time.text = min + "'0" + sec + "''";
        }

        _Instance._Remain.text = GameManager.Instance.curScore + " 개 / 5 개";
    }

    private void SetMission3()
    {
        _Instance.MissionName.text = "방어 미션";
        _Instance._missionType.sprite = _Instance._defence;
        _Instance._goalIcon.sprite = _Instance._pillarHP;

        float curTime = GameStatus._Instance._LimitTime;
        int min = (int)(curTime / 60f);
        int sec = (int)(curTime % 60f);

        if (sec >= 10)
        {
            _Instance._Time.text = min + "'" + sec + "''";
        }
        else
        {
            _Instance._Time.text = min + "'0" + sec + "''";
        }

        MissionC mission = MissionManager.Instance.CurrentMission as MissionC;

        _Instance._Remain.text = mission.protectedTarget.hp +
            " / " + mission._ProtectedTargetHP;
    }
}
