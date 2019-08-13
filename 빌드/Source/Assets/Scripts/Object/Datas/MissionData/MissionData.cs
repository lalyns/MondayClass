using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class MissionData : ScriptableObject
{
    public Sprite MissionName;
    public string MissionString;
    public string MissionSubject;
    public Sprite MissionIcon;
    public string MissionText;

    public MissionManager.MissionType[] MissionTypes;
    public int[] MissionLevel;
    public int[] MissionLength;

    public int NumberOfTimesSpawn;
    public float CycleOfTimeRespawn;

    public int[] DreamCatcherCount;
    public int[] MacCount;
    public int[] TiberCount;

}
