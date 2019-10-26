using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.SceneDirector;

namespace MC.UI
{
    [System.Serializable]
    [ExecuteInEditMode]
    public class UIPlayer : MonoBehaviour
    {
        public Image profile;
        public HPBar hpBar;
        public Text hpValue;
        public Text maxHPValue;

        public HPBar changeBar;
        public PlayerSpecialUI special;
        public PlayerSkillUI[] skill;
        public GameObject skill4;
        public PlayerDashUI[] dash;
        public PlayerBuffUI[] Buff;

        public RectTransform root;
        public PlayerUIResource resource;

        private static UIPlayer Instance;
        private PlayerFSMManager playerFSM;


        private void Awake()
        {
            if(MCSceneManager.currentScene != MCSceneManager.TITLE)
                SetValue();
        }

        public void SetValue()
        {
            Instance = this;
            playerFSM = PlayerFSMManager.Instance;
        }

        public void HPValueText()
        {
            hpValue.text = "" + PlayerFSMManager.Instance.Stat.Hp;
            maxHPValue.text = " / " + PlayerFSMManager.Instance.Stat.MaxHp;
        }

        public void ProfileImage(bool isSpecial)
        {
            profile.sprite = isSpecial ?
                CanvasInfo.Instance.player.resource.profiles[0] :
                CanvasInfo.Instance.player.resource.profiles[1];
        }

        public void SkillSetActive(int i, float value, bool isCool)
        {
            if (isCool)
            {
                var gaugeValue = Mathf.Clamp01(value / playerFSM.Stat.skillCTime[i]);
                skill[i].inActive.fillAmount = gaugeValue;
                skill[i].cooltime.enabled = true;
                skill[i].cooltime.text = "" + (int)value;
            }
            else
            {
                skill[i].inActive.fillAmount = 0;
                skill[i].cooltime.enabled = false;
            }
        }

        public void Skill4SetActive(bool isActive)
        {
            skill4.SetActive(isActive);
        }

        public void SpecialGauge()
        {
            var value = Mathf.Clamp01(Mathf.Lerp(changeBar.currentValue,
                (PlayerFSMManager.Instance.SpecialGauge) / 100.0f, Time.deltaTime * 5f));

            
            special.value.text = (PlayerFSMManager.Instance.SpecialGauge > 100 ? 100 : PlayerFSMManager.Instance.SpecialGauge)  + "%";
            special.inActive.fillAmount = value;
            
            special.effects[0].gameObject.SetActive(value == 1);

            changeBar.currentValue = value;
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

        public bool isEnd = false;
        public void DashSetActive()
        {

            if (playerFSM.remainingDash < playerFSM.maxDash)
            {
                if (!isEnd)
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
                }
                if (isEnd)
                {

                    playerFSM.currentDashCoolTime += Time.deltaTime;

                    for (int i = 0; i < 3; i++)
                        dash[i].active.fillAmount = playerFSM.currentDashCoolTime / 8f;

                    if (playerFSM.currentDashCoolTime >= 8f)
                    {
                        playerFSM.remainingDash = 3;
                        playerFSM.currentDashCoolTime = 0;
                        isEnd = false;
                        dash[0].active.fillAmount = 1;
                        dash[1].active.fillAmount = 1;
                        dash[2].active.fillAmount = 1;
                    }
                    
                }
            }
        }
        public void DashStart()
        {
            if (playerFSM.remainingDash == 1)
            {
                dash[0].active.fillAmount = 0;
                dash[1].active.fillAmount = 0;
                playerFSM.remainingDash--;
                isEnd = true;
                playerFSM.currentDashCoolTime = 0;
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
        public Text cooltime;
    }

    [System.Serializable]
    public class PlayerSpecialUI
    {
        public Text value;
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