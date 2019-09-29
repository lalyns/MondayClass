﻿using System.Collections;

using UnityEngine;

using UnityEngine.UI;

namespace MC.UI
{

    public class UserInterface : MonoBehaviour
    {
        private static UserInterface instance;
        public static UserInterface Instance => instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = GetComponent<UserInterface>();
            }

            if (instance.gameObject != this.gameObject)
                Destroy(gameObject);
        }

        #region Instance Caching
        private PlayerFSMManager playerFSMMgr;
        private MissionManager missionMgr;
        private GameStatus gameStatus;
        private GameManager gameMgr;
        #endregion

        private void Start()
        {
            playerFSMMgr = PlayerFSMManager.Instance;
            missionMgr = MissionManager.Instance;
            gameStatus = GameStatus.Instance;
            gameMgr = GameManager.Instance;

            SetMPMode(MPSimpleMode);
        }

        #region Canvas Control Function
        [SerializeField] private bool activeAllUI = true;
        [SerializeField] private bool activePlayerUI = true;
        [SerializeField] private bool activeMonsterUI = true;
        [SerializeField] private bool activeSystemUI = true;
        [SerializeField] private bool activeMissionProgressUI = true;
        [SerializeField] private bool activeMissionSelectionUI = true;

        public GameObject UICanvas;
        public static void SetAllUserInterface(bool isActive)
        {
            Instance.activeAllUI = isActive;
            Instance.UICanvas.SetActive(isActive);
        }

        public GameObject SystemUICanvas;
        public static void SetSystemInterface(bool isActive)
        {
            Instance.activeSystemUI = isActive;
            Instance.SystemUICanvas.SetActive(isActive);
        }

        public GameObject PlayerUICanvas;
        public static void SetPlayerUserInterface(bool isActive)
        {
            Instance.activePlayerUI = isActive;
            Instance.PlayerUICanvas.SetActive(isActive);
        }

        public GameObject MissionProgressUICanvas;
        public static void SetMissionProgressUserInterface(bool isActive)
        {
            Instance.activeMissionProgressUI = isActive;
            Instance.MissionProgressUICanvas.SetActive(isActive);
        }

        public GameObject MissionSelectionUICanvas;
        public static void SetMissionSelectionUI(bool isActive)
        {
            Instance.activeMissionSelectionUI = isActive;
            Instance.MissionSelectionUICanvas.SetActive(isActive);
        }
        #endregion

        private void Update()
        {
            if (!activeAllUI) { return; }

            if (activePlayerUI) {
                PlayerUI();
            }

            if (activeMissionProgressUI) {
                ProgressUI();
            }

            if (pointerMode) PointerLocation();
        }

        #region Player User Interface
        private PlayerUI _IPlayer;
        public PlayerUI IPlayer 
        {
            get {
                if (_IPlayer == null) _IPlayer = CanvasInfo.Instance.playerUI;
                return _IPlayer;
            }
        }

        private void PCIconImageSet(bool isSpecial)
        {
            IPlayer.PCIcon.sprite = isSpecial ? 
                CanvasInfo.Instance.playerResources.PCIconSprites[0] : 
                CanvasInfo.Instance.playerResources.PCIconSprites[1];
        }

        private void PlayerSkillUISet(int i, float value)
        {
            var gaugeValue = Mathf.Clamp01(value / playerFSMMgr.Stat.skillCTime[i]);
            IPlayer.Skill[i].SkillCoolTime.fillAmount = gaugeValue;
        }

        public static void PlayerSkillEffect(int i)
        {
            var length = Instance.IPlayer.Skill[i].SkillEffects.Length;
            for(int j=0; j<length; j++)
            {
                Instance.IPlayer.Skill[i].SkillEffects[j].gameObject.SetActive(true);
                Instance.IPlayer.Skill[i].SkillEffects[j].Play();
            }
        }

        private void PCSpecialGaugeSet(float value)
        {
            var gaugeValue = Mathf.Clamp01(value * 0.01f);
            IPlayer.Special.SpecialCoolTime.fillAmount = gaugeValue;

            OnSpecialEffect(value);
        }

        private void OnSpecialEffect(float value)
        {
            var effectActive = value >= 1.0;
            for (int i = 0; i < IPlayer.Special.SpecialEffects.Length; i++)
                IPlayer.Special.SpecialEffects[i].gameObject.SetActive(effectActive);
        }

        private void DashCountSet()
        {
            for (int i = 0; i < 3; i++)
            {
                if (playerFSMMgr.isDashCTime[i])
                {
                    var fillValue = Mathf.Clamp01(1f - (playerFSMMgr.DashCTime[i] / 3f));
                    IPlayer.Dash[i].DashCoolTime.fillAmount = fillValue;
                }
            }
        }

        private void PlayerUI()
        {
            // 나중에 변신에 포함시킬것
            PCIconImageSet(playerFSMMgr.isNormal);

            HPChangeEffect(playerFSMMgr.Stat, IPlayer.PlayerHpBar);
            PCSpecialGaugeSet(playerFSMMgr.SpecialGauge);
            DashCountSet();

            if (playerFSMMgr.isSkill1CTime) PlayerSkillUISet(0, playerFSMMgr.Skill1CTime);
            if (playerFSMMgr.isSkill2CTime) PlayerSkillUISet(1, playerFSMMgr.Skill2CTime);
            if (playerFSMMgr.isSkill3CTime) PlayerSkillUISet(2, playerFSMMgr.Skill3CTime);
            if (playerFSMMgr.isSkill4CTime) PlayerSkillUISet(3, playerFSMMgr.Skill4CTime);
        }
        #endregion

        #region System User Interface
        // 커서가 보이는지 여부
        [Space(5)]
        [Header("System User Interface")]
        private bool pointerMode = false;
        private MousePointer _MousePointer;
        public MousePointer MousePointer {
            get {
                if (_MousePointer == null) _MousePointer = CanvasInfo.Instance.mousePointer;
                return _MousePointer;
            }
        }

        public static void SetPointerMode(bool mode)
        {
            Instance.pointerMode = mode;

            Cursor.lockState = mode ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = false;
            Instance.MousePointer.pointer.enabled = mode;
        }

        private void PointerLocation()
        {
            var screenPoint = Input.mousePosition;
            screenPoint.z = 10.0f;
            MousePointer.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }

        // Screen Effect : FadeIn FadeOut
        private ScreenEffect _ScreenEffect;
        public ScreenEffect ScreenEffect {
            get {
                if (_ScreenEffect == null) _ScreenEffect = CanvasInfo.Instance.screenEffect;
                return _ScreenEffect;
            }
        }

        public Image FadeInOutImage;
        public float FadeInOutSpeed = 10.0f;

        public static IEnumerator FadeIn(System.Action callback, float speed = 10.0f, float delay = 0.15f)
        {
            var alpha = Instance.ScreenEffect.fading.image.color;
            for (float i = 100; i >= 0;)
            {
                i -= speed;
                alpha.a = i * 0.01f;
                Instance.ScreenEffect.fading.image.color = alpha;

                yield return new WaitForSeconds(delay);
            }

            yield return Instance.StartCoroutine(Instance.FadeInOutReturnValue(callback));
        }

        public static IEnumerator FadeOut(System.Action callback, float speed = 10.0f, float delay = 0.15f)
        {
            var alpha = Instance.ScreenEffect.fading.image.color;
            for (float i = 0; i <= 100;)
            {
                i += speed;
                alpha.a = i * 0.01f;
                Instance.ScreenEffect.fading.image.color = alpha;

                yield return new WaitForSeconds(delay);
            }

            yield return Instance.StartCoroutine(Instance.FadeInOutReturnValue(callback));
        }

        IEnumerator FadeInOutReturnValue(System.Action callback)
        {
            yield return null;
            callback();
        }

        // Screen Effect : Blur
        public static void BlurSet(bool isOn)
        {
            var value = isOn ? 20f : 0f;
            Instance.ScreenEffect.blur.image.material.SetFloat("_Size", value);

            var color = isOn ? Instance.ScreenEffect.blur.color : Color.white;
            Instance.ScreenEffect.blur.image.material.SetColor("_Color", color);
        }
        // 게임 설정
        #endregion
        
        #region MissionProgress User Interface
        // 간략모드 활성화 여부
        private bool MPSimpleMode = false;
        public static void SetMPMode(bool value)
        {
            Instance.MPSimpleMode = value;
            Instance.FullMode.FullMode.SetActive(!value);
            Instance.SimpleMode.SimpleMode.SetActive(value);
            Instance.CurrentTimer = value ? Instance.SimpleMode.TimeText : Instance.FullMode.TimeText;
            Instance.CurrentGoal = value ? Instance.SimpleMode.GoalText : Instance.FullMode.GoalText;
            Instance.CurrentTimeBack = value ? Instance.SimpleMode.TimeBack : Instance.FullMode.TimeBack;
        }

        private ProgressFullUI _FullMode;
        public ProgressFullUI FullMode {
            get {
                if (_FullMode == null) _FullMode = CanvasInfo.Instance.progressFullUI;
                return _FullMode;
            }
        }

        public static void FullModeSetMP()
        {
            Instance.FullMode.MissionIcon.sprite =
                Instance.missionMgr.CurrentMission.Data.MissionIcon;
            Instance.FullMode.MissionText.text =
                Instance.missionMgr.CurrentMission.Data.MissionText;
            Instance.FullMode.GoalIcon.sprite =
                Instance.missionMgr.CurrentMission.Data.MissionIcon;
        }

        private ProgressSimpleUI _SimpleMode;
        public ProgressSimpleUI SimpleMode {
            get { if(_SimpleMode ==null) _SimpleMode = CanvasInfo.Instance.progressSimpleUI;
                return _SimpleMode;
            }
        }

        private Text CurrentTimer;
        private Image CurrentTimeBack;
        private void SetTimer(float value)
        {
            int min = (int)(value / 60f);
            int sec = (int)(value % 60f);

            var text = sec >= 10 ? min + "'" + sec + "''" : min + "'0" + sec + "''";
            CurrentTimer.text = text;
            var timeValue = Mathf.Clamp01(value / missionMgr.CurrentMission._LimitTime);
            CurrentTimeBack.fillAmount = timeValue;
        }

        private Text CurrentGoal;
        private void SetGoal(MissionType type)
        {
            var text = "";
            switch (type)
            {
                case MissionType.Annihilation:
                    text = "남은 몬스터 " + gameStatus.ActivedMonsterList.Count + " 마리";
                    break;
                case MissionType.Defence:
                    MissionC mission = MissionManager.Instance.CurrentMission as MissionC;
                    text = "남은 기둥 체력 " + mission.protectedTarget.hp + " / " + mission._ProtectedTargetHP;
                    break;
                case MissionType.Survival:
                    text = gameMgr.curScore + " 개 / 5 개";
                    break;
                case MissionType.Boss:
                    text = "리리스를 처치하시오";
                    break;
            }

            CurrentGoal.text = text;
        }

        private void SetSimpleGoal(MissionType type)
        {
            var text = "";
            switch (type)
            {
                case MissionType.Annihilation:
                    text = gameStatus.ActivedMonsterList.Count + " ";
                    break;
                case MissionType.Defence:
                    MissionC mission = MissionManager.Instance.CurrentMission as MissionC;
                    text = mission.protectedTarget.hp + " / " + mission._ProtectedTargetHP;
                    break;
                case MissionType.Survival:
                    text = gameMgr.curScore + " / 5";
                    break;
                case MissionType.Boss:
                    text = "리리스를 처치하시오";
                    break;
            }

            CurrentGoal.text = text;
        }

        private void ProgressUI()
        {
            SetTimer(gameStatus._LimitTime);
            if (!MPSimpleMode)
                SetGoal(MissionManager.Instance.CurrentMissionType);
            else
                SetSimpleGoal(MissionManager.Instance.CurrentMissionType);
        }

        #endregion

        #region User Interface Effect Support Functions

        /// <summary>
        /// HP Bar UI 변화에 효과를 주는 매소드
        /// </summary>
        /// <param name="stat"> 적용해야 하는 HP 정보 </param>
        /// <param name="hpBar"> 대상 HPBar </param>
        /// <param name="speed"> HP바가 줄어드는 속도 (기본:5) </param>
        public void HPChangeEffect(CharacterStat stat, HPBar hpBar, float speed = 5f)
        {

            var value1 = Mathf.Lerp(hpBar.currentValue, stat.Hp / stat.MaxHp, Time.deltaTime * speed);
            hpBar.currentValue = value1;

            if (hpBar.backHitMove)
            {
                var value2 = Mathf.Lerp(hpBar.laterValue, hpBar.currentValue, speed * 2f * Time.deltaTime);
                hpBar.laterValue = value2;

                if (hpBar.currentValue >= hpBar.laterValue - 0.01f)
                {
                    hpBar.backHitMove = false;
                    hpBar.laterValue = hpBar.currentValue;
                }
            }

        }

        #endregion

    }
}