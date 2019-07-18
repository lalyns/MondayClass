using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MissionData : ScriptableObject
{
    public Sprite MissionIcon;
    public string MissionText;

    public Sprite RewardIcon;
    public string RewardText;
}
