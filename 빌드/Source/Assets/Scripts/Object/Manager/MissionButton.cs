using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour
{
    // 선택지의 번호(위쪽부터 1,2,3 변경되면 안됨)
    public int _ChoiceNum;

    // 변경하면 안되는 정보들
    public Button _Button;
    public Image _NumberImage;

    // 외부에서 변경되어야하는 정보들
    public Image _MissionIcon;
    public Text _MissionText;

    public Image _RewardIcon;
    public Text _RewardText;

    public void Awake()
    {
        _Button = GetComponent<Button>();

        _NumberImage = transform.GetChild(0).GetComponent<Image>();
        _MissionIcon = transform.GetChild(1).GetComponent<Image>();
        _MissionText = transform.GetChild(2).GetComponent<Text>();
        _RewardIcon = transform.GetChild(3).GetComponent<Image>();
        _RewardText = transform.GetChild(4).GetComponent<Text>();
    }

    /// <summary>
    /// 미션의 목표 설명 변경을 도와주는 메소드
    /// </summary>
    /// <param name="missionIcon"> 미션의 목표 표기 아이콘 </param>
    /// <param name="missionText"> 미션의 목표 설명 </param>
    public void MissionChange(Sprite missionIcon, string missionText)
    {
        _MissionIcon.sprite = missionIcon;
        _MissionText.text = missionText;
    }

    /// <summary>
    /// 미션의 보상 설명 변경을 도와주는 메소드
    /// </summary>
    /// <param name="rewardIcon"> 미션의 보상 표기 아이콘 </param>
    /// <param name="rewardText"> 미션의 보상 설명 </param>
    public void RewardChange(Sprite rewardIcon, string rewardText)
    {
        _RewardIcon.sprite = rewardIcon;
        _RewardText.text = rewardText;
    }

    public void SetMissionOnClick()
    {
        DungeonManager.SetMissionOnClick(_ChoiceNum);
    }
}
