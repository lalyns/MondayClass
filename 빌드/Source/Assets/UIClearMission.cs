using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClearMission : MonoBehaviour
{
    public Text timer;
    public Image reward1;
    public Image reward2;

    public Text rewardName1;
    public Text rewardName2;

    public Sprite temp;
    public Sprite temp2;

    public void SetClearMission(float value, MissionRewardType type1, MissionRewardType type2)
    {
        SetTimer(value);
        SetReward1(type1);
        SetReward2(type2);
    }

    public void SetTimer(float value)
    {
        int min = (int)(value / 60f);
        int sec = (int)(value % 60f);

        var text = sec >= 10 ? min + "'" + sec + "''" : min + "'0" + sec + "''";
        timer.text = text;
    }
    
    public void SetReward1(MissionRewardType type)
    {
        reward1.sprite = temp;
        rewardName1.text = MissionManager.Instance.rewardData.RewardText[(int)type];
    }

    public void SetReward2(MissionRewardType type)
    {
        reward1.sprite = temp2;
        rewardName2.text = MissionManager.Instance.rewardData.RewardText[(int)type];
    }

}
