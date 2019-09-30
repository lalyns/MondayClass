using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.UI {

    public class MissionButton : MonoBehaviour
    {
        // 선택지의 번호(위쪽부터 1,2,3 변경되면 안됨)
        public int _ChoiceNum;

        // 변경하면 안되는 정보들
        public Button _Button;

        // 외부에서 변경되어야하는 정보들
        public Image missioType;

        public Image rewardIcon;
        public Text rewardText;

        public Mission _Mission;
        public MissionType _MissionType;

        public void Awake()
        {
            _Button = GetComponent<Button>();

            missioType = transform.GetComponent<Image>();
            rewardIcon = transform.GetChild(0).GetComponent<Image>();
            rewardText = transform.GetChild(1).GetComponent<Text>();
        }

        public void ChangeMission(Mission mission)
        {
            missioType.sprite = mission.Data.missionTypeSprite;

            _Mission = mission;
        }

        public void ChangeReward(RewardData rewardData)
        {
            var rewardSprite = rewardData.RewardIcon;
            var rewardText = rewardData.RewardText;

            rewardIcon.sprite = rewardSprite;
            this.rewardText.text = rewardText;
        }

        public void SetMissionOnClick()
        {
            MissionManager.SelectMission(_Mission);
        }
    }
}
