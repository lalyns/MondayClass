using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MC.Mission;
using MC.Sound;

namespace MC.UI {

    public class MissionButton : MonoBehaviour
    {
        // 선택지의 번호(위쪽부터 1,2,3 변경되면 안됨)
        public int choiceNum;

        // 변경하면 안되는 정보들
        [HideInInspector] public Button button;

        // 외부에서 변경되어야하는 정보들
        [HideInInspector] public Image missioType;

        [HideInInspector] public MissionBase mission;
        [HideInInspector] public MissionType missionType;

        [HideInInspector] public Image rewardIcon;
        [HideInInspector] public Text[] rewardText;
        [HideInInspector] public MissionRewardType[] rewardType;

        public static bool isPush = false;

        public void Awake()
        {
            SetValue();
        }

        public void SetValue()
        {
            button = GetComponent<Button>();

            rewardText = new Text[2];
            rewardType = new MissionRewardType[2];

            missioType = transform.GetComponent<Image>();
            rewardIcon = transform.GetChild(0).GetComponent<Image>();
            rewardText[0] = transform.GetChild(1).GetComponent<Text>();
            rewardText[1] = transform.GetChild(2).GetComponent<Text>();
        }

        public void ChangeMission(int type)
        {
            missioType.sprite = MissionManager.Instance.resources.types[type];
            missionType = (MissionType)type;
            
        }

        public MissionRewardType ChangeReward(int num, MissionRewardType type)
        {
            if(num == 0)
            {
                this.rewardIcon.sprite = MissionManager.Instance.rewardData.RewardIcon[(int)type];
            }
            rewardType[num] = type;
            rewardText[num].text = MissionManager.Instance.rewardData.RewardText[(int)type];

            return type;
        }

        public void SetMissionOnClick()
        {
            if (!isPush) {

                var sound = MCSoundManager.Instance.objectSound.objectSFX;
                sound.StopSound(MissionManager.Instance.CurrentMission.Exit.gameObject, sound.portalLoop);
                sound.PlaySound(this.gameObject, sound.portalExit);

                Invoke("SetInvoke", 0.56f);
                isPush = true;
            }

        }

        public void SetInvoke()
        {
            MissionManager.SelectMission(missionType);
        }
    }
}
