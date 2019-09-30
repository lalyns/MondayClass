using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.UI {

    public class MissionButton : MonoBehaviour
    {
        // 선택지의 번호(위쪽부터 1,2,3 변경되면 안됨)
        public int choiceNum;

        // 변경하면 안되는 정보들
        [HideInInspector] public Button button;

        // 외부에서 변경되어야하는 정보들
        [HideInInspector] public Image missioType;
        [HideInInspector] public Image rewardType;
        [HideInInspector] public Text rewardText;

        [HideInInspector] public Mission mission;
        [HideInInspector] public MissionType missionType;

        public void Awake()
        {
            SetValue();
        }

        public void SetValue()
        {
            button = GetComponent<Button>();

            missioType = transform.GetComponent<Image>();
            rewardType = transform.GetChild(0).GetComponent<Image>();
            rewardText = transform.GetChild(1).GetComponent<Text>();
        }

        public void ChangeMission(Mission mission)
        {
            missioType.sprite = mission.Data.missionTypeSprite;

            this.mission = mission;
        }

        public void ChangeReward(RewardData rewardData)
        {
            var rewardSprite = rewardData.RewardIcon;
            var rewardText = rewardData.RewardText;

            rewardType.sprite = rewardSprite;
            this.rewardText.text = rewardText;
        }

        public void SetMissionOnClick()
        {
            MissionManager.SelectMission(mission);
        }
    }
}
