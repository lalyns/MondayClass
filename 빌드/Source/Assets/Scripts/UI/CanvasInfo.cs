using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.UI {

    #region Player User Interface Class(6)
    [System.Serializable]
    public class PlayerUI
    {
        public Image PCIcon;
        public HPBar PlayerHpBar;
        public PlayerSpecialUI Special;
        public PlayerSkillUI[] Skill;
        public PlayerDashUI[] Dash;
        public PlayerBuffUI[] Buff;
    }

    [System.Serializable]
    public class PlayerInterfaceResources
    {
        public Sprite[] PCIconSprites;
    }

    [System.Serializable]
    public class PlayerSkillUI
    {
        public Image SkillActive;
        public Image SkillCoolTime;
        public ParticleSystem[] SkillEffects;
    }

    [System.Serializable]
    public class PlayerSpecialUI
    {
        public Image SpecialActive;
        public Image SpecialCoolTime;
        public ParticleSystem[] SpecialEffects;
    }

    [System.Serializable]
    public class PlayerDashUI
    {
        public Image DashActive;
        public Image DashCoolTime;
    }

    [System.Serializable]
    public class PlayerBuffUI
    {
        public Image BuffBackground;
        public Image Buff;
    }
    #endregion

    #region Mission Progress User Interface Class(2)
    [System.Serializable]
    public class ProgressFullUI
    {
        public GameObject FullMode;
        public Image MissionIcon;
        public Text MissionText;
        public Image TimeBack;
        public Text TimeText;
        public Image GoalIcon;
        public Text GoalText;
    }

    [System.Serializable]
    public class ProgressSimpleUI
    {
        public GameObject SimpleMode;
        public Image TimeBack;
        public Text TimeText;
        public Image GoalIcon;
        public Text GoalText;
    }
    #endregion

    #region System User Interface Class(4)
    [System.Serializable]
    public class MousePointer
    {
        public Transform transform;
        public Image pointer;
    }

    [System.Serializable]
    public class ScreenEffect
    {
        public Fading fading;
        public Blur blur;
    }

    [System.Serializable]
    public class Fading
    {
        public Image image;
    }

    [System.Serializable]
    public class Blur
    {
        public Image image;
        public Color color;
    }
    #endregion


    public class CanvasInfo : MonoBehaviour
    {
        private static CanvasInfo _Instance;
        public static CanvasInfo Instance {
            get {
                if (_Instance == null) { _Instance = GameObject.Find("Canvases").GetComponent<CanvasInfo>(); }
                return _Instance;
            }
        }

        public GameObject UICanvas;
        public GameObject SystemUICanvas;
        public GameObject PlayerUICanvas;
        public GameObject MissionProgressUICanvas;
        public GameObject MissionSelectionUICanvas;

        public GameObject[] Layers;
        public GameObject EventSystem;

        [Space(5)]
        [Header ("Player User Interface")]
        public PlayerUI playerUI;
        public PlayerInterfaceResources playerResources;

        //[Space(5)]
        //[Header("Mission Selector Interface")]

        [Space(5)]
        [Header("Mission Progress Interface")]
        public ProgressFullUI progressFullUI;
        public ProgressSimpleUI progressSimpleUI;

        [Space(5)]
        [Header("System Interface")]
        public MousePointer mousePointer;
        public ScreenEffect screenEffect;

        private void Awake()
        {
            screenEffect.blur.image.material.SetColor("_Color", Color.white);
            screenEffect.blur.image.material.SetFloat("_Size", 0);
        }

    }
}