using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempMissionProgress : MonoBehaviour
{
    //public GameObject Mission1;
    //public TempDungeon dungeon1;
    ////public GameObject Mission1tab;
    //public GameObject Mission2;
    //public TempDungeon dungeon2;
    ////public GameObject Mission2tab;

    //public Image MissionIcon;
    //public Image RemainIcon;

    //public Sprite mission1;
    //public Sprite mission2;

    //public Sprite remain1;
    //public Sprite remain2;

    //public Text MissionType;
    //public string MissionType1;
    //public string MissionType2;

    //public Text _Time;
    //public Text _Remain;

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
    public Image _missionType;
    public Sprite _anihilation;
    public Sprite _survive;

    public Image _goalIcon;
    public Sprite _mob;
    public Sprite _star;

    public Text _Time;
    public Text _Remain;

    private void Update()
    {
        if (GameManager.stageLevel == 1) {
            SetMission1();

        }

        if (GameManager.stageLevel == 2) {
            SetMission2();

        }
    }

    public static void SetMission1()
    {
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

        _Instance._Remain.text = GameManager._Instance.curScore + " 개 / 5 개";
    }
}
