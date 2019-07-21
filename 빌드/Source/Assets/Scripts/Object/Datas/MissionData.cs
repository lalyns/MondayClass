using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MissionData : ScriptableObject
{
    public int NumberOfTimesRespawn;
    public float CycleOfTimeRespawn;

    public Sprite MissionIcon;
    public string MissionText;

    //두 배열의 크기는 같아야함
    public int[] NumberOfMeleeMonsterOnWaves;
    public int[] NumberOfRangeMonsterOnWaves;
}
