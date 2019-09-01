using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempMissionProgress : MonoBehaviour
{
    public GameObject Mission1;
    public TempDungeon dungeon1;
    //public GameObject Mission1tab;
    public GameObject Mission2;
    public TempDungeon dungeon2;
    //public GameObject Mission2tab;

    public Image MissionIcon;
    public Image RemainIcon;

    public Sprite mission1;
    public Sprite mission2;

    public Sprite remain1;
    public Sprite remain2;

    public Text MissionType;
    public string MissionType1;
    public string MissionType2;

    public Text _Time;
    public Text _Remain;


    void Update()
    {
        if (GameManager._Instance._IsDummyScene)
        {
            if (GameManager.stageLevel == 0)
            {
                MissionIcon.sprite = mission1;
                RemainIcon.sprite = remain1;

                MissionType.text = MissionType1;

                int m = (int)(dungeon1.MissionTime / 60f);
                int s = (int)(dungeon1.MissionTime % 60f);

                _Time.text = m + " : " + s;


                int r;
                try
                {
                    r = dungeon1.Waves[dungeon1.CurWave].transform.childCount;
                }
                catch
                {
                    r = 0;
                }
                _Remain.text = "" + r;
            }

            if (GameManager.stageLevel == 1)
            {
                MissionIcon.sprite = mission2;
                RemainIcon.sprite = remain2;

                int m = (int)(dungeon2.MissionTime / 60f);
                int s = (int)(dungeon2.MissionTime % 60f);

                _Time.text = m + " : " + s;

                int r = GameManager._Instance.curScore;
                int rMax = dungeon2.clearCount;
                _Remain.text = r + " / " + rMax;
            }
        }
    }
}
