using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class MissionData : ScriptableObject
{
    // 미션 종류
    public MissionType MissionType;

    // 미션 이름
    public Sprite MissionName;
    public Sprite MissionIcon;
    public string MissionGoal;

    // 미션 진행상황 UI 표기 변수
    public string MissionText;
    public Sprite GoalIcon;
    //public string MissionSubject;
    //public string MissionText;

    //// 미션 레벨
    //public int MissionLevel;
    //public int MissionLength;

    //// 스폰 수
    //public int NumberOfTimesSpawn;
    //// 스폰 주기
    //public float CycleOfTimeRespawn;

    //// 각 몬스터의 수
    //public int[] RedHatCount;
    //public int[] MacCount;
    //public int[] TiberCount;

}
