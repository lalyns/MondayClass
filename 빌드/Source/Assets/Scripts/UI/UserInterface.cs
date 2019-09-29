using System.Collections;

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

        }

        #region Instance Caching
        private PlayerFSMManager playerFSMMgr;
        private MissionManager missionMgr;
        private GameStatus gameStatus;
        private GameManager gameMgr;
        #endregion

        private void Start()
        {
            SetValue();
        }

        #region Canvas Control Function
        private bool activeAllUI = true; 
        private bool activePlayerUI = true;
        private bool activeMonsterUI = true;
        private bool activeSystemUI = true;
        private bool activeMissionProgressUI = true;
        private bool activeMissionSelectionUI = true;
        private bool activeTitleUI = true;

        private GameObject _UICanvas;
        public GameObject UICanvas {
            get {
                if (_UICanvas == null)
                    _UICanvas = CanvasInfo.Instance.UICanvas;
                return _UICanvas;
            }
        }
        public static void SetAllUserInterface(bool isActive)
        {
            Instance.activeAllUI = isActive;
            Instance.UICanvas.SetActive(isActive);
        }

        private GameObject _SystemUICanvas;
        public GameObject SystemUICanvas {
            get {
                if (_SystemUICanvas == null)
                    _SystemUICanvas = CanvasInfo.Instance.SystemUICanvas;
                return _SystemUICanvas;
            }
        }
        public static void SetSystemInterface(bool isActive)
        {
            Instance.activeSystemUI = isActive;
            Instance.SystemUICanvas.SetActive(isActive);
        }

        private GameObject _PlayerUICanvas;
        public GameObject PlayerUICanvas {
            get {
                if (_PlayerUICanvas == null)
                    _PlayerUICanvas = CanvasInfo.Instance.playerUI.gameObject;
                return _PlayerUICanvas;
            }
        }
        public static void SetPlayerUserInterface(bool isActive)
        {
            Instance.activePlayerUI = isActive;
            Instance.PlayerUICanvas.SetActive(isActive);
        }

        private GameObject _MissionProgressUICanvas;
        public GameObject MissionProgressUICanvas {
            get {
                if (_MissionProgressUICanvas == null)
                    _MissionProgressUICanvas = CanvasInfo.Instance.progress.gameObject;
                return _MissionProgressUICanvas;
            }
        }
        public static void SetMissionProgressUserInterface(bool isActive)
        {
            Instance.activeMissionProgressUI = isActive;
            Instance.MissionProgressUICanvas.SetActive(isActive);
        }

        private GameObject _MissionSelectionUICanvas;
        public GameObject MissionSelectionUICanvas {
            get {
                if (_MissionSelectionUICanvas == null)
                    _MissionSelectionUICanvas = CanvasInfo.Instance.MissionSelectionUICanvas;
                return _MissionSelectionUICanvas;
            }
        }
        public static void SetMissionSelectionUI(bool isActive)
        {
            Instance.activeMissionSelectionUI = isActive;
            Instance.MissionSelectionUICanvas.SetActive(isActive);
        }

        private GameObject _TitleUI;
        public GameObject TitleUI {
            get {
                if (_TitleUI == null)
                    _TitleUI = CanvasInfo.Instance.title.gameObject;
                return _TitleUI;
            }
        }
        public static void SetTitleUI(bool isActive)
        {
            Instance.activeTitleUI = isActive;
            Instance.TitleUI.SetActive(isActive);
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
                CanvasInfo.Instance.playerUIResources.PCIconSprites[0] : 
                CanvasInfo.Instance.playerUIResources.PCIconSprites[1];
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

        public void MousePointerSpeed(float value)
        {
            MousePointer.animator.SetFloat("Speed", value);
        }

        public static void SetPointerMode(bool mode)
        {
            Instance.pointerMode = mode;

            Cursor.lockState = mode ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = true;
            Instance.MousePointer.pointer.enabled = mode;
        }

        private void PointerLocation()
        {
            var screenPoint = Input.mousePosition;
            screenPoint.z = 8.0f;
            MousePointer.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
            //Debug.Log(string.Format("Screen : {0}, Pointer : {1}", screenPoint, MousePointer.transform.position));
        }

        // Screen Effect : FadeIn FadeOut
        private ScreenEffect _ScreenEffect;
        public ScreenEffect ScreenEffect {
            get {
                if (_ScreenEffect == null) _ScreenEffect = CanvasInfo.Instance.screenEffect;
                return _ScreenEffect;
            }
        }

        public float FadeInOutSpeed = 10.0f;

        public static IEnumerator FadeIn(System.Action callback, float speed = 10.0f, float delay = 0.15f)
        {
            var alpha = Instance.ScreenEffect.fading.image.color;
            for (float i = 100; i >= 0;)
            {
                i -= speed;
                var value = Mathf.Clamp01(i * 0.01f);
                Debug.Log("Fade In Progress : " + value);
                alpha.a = value;
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
                var value = Mathf.Clamp01(i * 0.01f);
                Debug.Log("Fade Out Progress : " + value);
                alpha.a = value;
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

        #region Mission Selector User Interface
        private MissionSelectorUI selectorUI;
        public MissionSelectorUI SelectorUI {
            get
            {
                if (selectorUI == null)
                    selectorUI = CanvasInfo.Instance.selector;
                return selectorUI;
            }
        }

        #endregion

        #region MissionProgress User Interface
        // 간략모드 활성화 여부
        private bool MPSimpleMode = false;
        public static void SetMPMode(bool value)
        {
            Instance.MPSimpleMode = value;
            Instance.FullMode.gameObject.SetActive(!value);
            Instance.SimpleMode.gameObject.SetActive(value);
            Instance.CurrentTimer = value ? Instance.SimpleMode.TimeText : Instance.FullMode.TimeText;
            Instance.CurrentGoal = value ? Instance.SimpleMode.GoalText : Instance.FullMode.GoalText;
            Instance.CurrentTimeBack = value ? Instance.SimpleMode.TimeBack : Instance.FullMode.TimeBack;
        }

        private ProgressFullUI _FullMode;
        public ProgressFullUI FullMode {
            get {
                if (_FullMode == null)
                    _FullMode = CanvasInfo.Instance.progress.full;

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
            get {
                if (_SimpleMode ==null)
                    _SimpleMode = CanvasInfo.Instance.progress.simple;

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

        #region Title User Interface

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

        #region Null Support
        public void SetValue()
        {
            playerFSMMgr = PlayerFSMManager.Instance;
            missionMgr = MissionManager.Instance;
            gameStatus = GameStatus.Instance;
            gameMgr = GameManager.Instance;

            SetMPMode(MPSimpleMode);
        }
        #endregion
    }
}