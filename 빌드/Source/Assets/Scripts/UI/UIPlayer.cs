using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.UI
{
    [System.Serializable]
    [ExecuteInEditMode]
    public class UIPlayer : MonoBehaviour
    {
        public Image profile;
        public HPBar hpBar;
        public PlayerSpecialUI special;
        public PlayerSkillUI[] skill;
        public PlayerDashUI[] dash;
        public PlayerBuffUI[] Buff;

        public RectTransform root;
        public PlayerUIResource resource;

        private static UIPlayer Instance;
        private PlayerFSMManager playerFSM;


        private void Awake()
        {
            SetValue();
        }

        public void SetValue()
        {
            Instance = this;
            playerFSM = PlayerFSMManager.Instance;
        }

        public void ProfileImage(bool isSpecial)
        {
            profile.sprite = isSpecial ?
                CanvasInfo.Instance.player.resource.profiles[0] :
                CanvasInfo.Instance.player.resource.profiles[1];
        }

        public void SkillSetActive(int i, float value)
        {
            var gaugeValue = Mathf.Clamp01(value / playerFSM.Stat.skillCTime[i]);
            skill[i].inActive.fillAmount = gaugeValue;
        }

        public void SpecialGauge(float value)
        {
            var gaugeValue = Mathf.Clamp01(value * 0.01f);
            special.inActive.fillAmount = gaugeValue;

            var effectActive = gaugeValue >= 1.0;
            for (int i = 0; i < special.effects.Length; i++)
                special.effects[i].gameObject.SetActive(effectActive);
        }

        public static void SkillSetUp(int num)
        {
            var length = Instance.skill[num].effects.Length;
            for (int j = 0; j < length; j++)
            {
                Instance.skill[num].effects[j].gameObject.SetActive(true);
                Instance.skill[num].effects[j].Play();
            }
        }

        public void DashSetActive()
        {
            if (playerFSM.remainingDash < playerFSM.maxDash)
            {
                playerFSM.currentDashCoolTime += Time.deltaTime;
                if (playerFSM.currentDashCoolTime >= playerFSM.dashCoolTime)
                {
                    playerFSM.remainingDash++;
                    playerFSM.currentDashCoolTime = 0;
                }
                for (int i = 0; i < 3; i++)
                    if (playerFSM.remainingDash == i)
                        dash[i].active.fillAmount = playerFSM.currentDashCoolTime / 3f;
                //    var fillValue = Mathf.Clamp01(1f - (playerFSM.DashCTime[i] / 3f));
                //    dash[i].inActive.fillAmount = fillValue;
            }
        }
        public void DashStart()
        {
            if (playerFSM.remainingDash == 1)
            {
                dash[1].active.fillAmount = 0;
                playerFSM.remainingDash--;
            }
            if (playerFSM.remainingDash == 2)
            {
                dash[2].active.fillAmount = 0;
                playerFSM.remainingDash--;
            }
            if (playerFSM.remainingDash == 3)
            {
                playerFSM.remainingDash--;
            }
        }
    }

    [System.Serializable]
    public class PlayerUIResource
    {
        public Sprite[] profiles;
    }

    [System.Serializable]
    public class PlayerSkillUI
    {
        public Image active;
        public Image inActive;
        public ParticleSystem[] effects;
    }

    [System.Serializable]
    public class PlayerSpecialUI
    {
        public Image active;
        public Image inActive;
        public ParticleSystem[] effects;
    }

    [System.Serializable]
    public class PlayerDashUI
    {
        public Image active;
        public Image inActive;
    }

    [System.Serializable]
    public class PlayerBuffUI
    {
        public Image inActive;
        public Image active;
    }
}