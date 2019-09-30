﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.UI {

    // 변수 명 통일 및 정리 필요
    #region Player User Interface Class(6)
    [System.Serializable]
    public class PlayerUI
    {
        public GameObject gameObject;
        public Canvas canvas;

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

    #region Mission Selector User Interface
    [System.Serializable]
    public class MissionSelectorUI
    {
        public GameObject gameObject;
        public MissionButton[] buttons;
    }

    #endregion

    #region Mission Progress User Interface Class(2)
    [System.Serializable]
    public class MissionProgressUI
    {
        public GameObject gameObject;

        public ProgressFullUI full;
        public ProgressSimpleUI simple;
    }

    [System.Serializable]
    public class ProgressFullUI
    {
        public GameObject gameObject;
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
        public GameObject gameObject;
        public Image TimeBack;
        public Text TimeText;
        public Image GoalIcon;
        public Text GoalText;
    }
    #endregion

    #region System User Interface Class(4)
    [System.Serializable]
    public class SystemUI
    {
        public GameObject gameObject;
    }

    [System.Serializable]
    public class MousePointer
    {
        public Transform transform;
        public Image pointer;
        public Animator animator;
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

    #region Title User Interface Class
    [System.Serializable]
    public class TitleUI
    {
        public GameObject gameObject;

        public Button start;
        public Button howToPlay;
        public Button config;
        public Button exit;
    }

    #endregion



    public class CanvasInfo : MonoBehaviour
    {
        private static CanvasInfo instance;
        public static CanvasInfo Instance {
            get {
                if (instance == null) {
                    instance = GameObject.Find("Canvases").GetComponent<CanvasInfo>();
                }
                return instance;
            }
        }

        public GameObject UICanvas;
        public GameObject SystemUICanvas;

        public Canvas[] Layers;
        public GameObject EventSystem;

        [Space(5)]
        [Header ("Player User Interface")]
        public PlayerUI playerUI;
        public PlayerInterfaceResources playerUIResources;

        [Space(5)]
        [Header("Mission Selector Interface")]
        public MissionSelectorUI selector;

        [Space(5)]
        [Header("Mission Progress Interface")]
        public MissionProgressUI progress;

        [Space(5)]
        [Header("System Interface")]
        public SystemUI systemUI;
        public MousePointer mousePointer;
        public ScreenEffect screenEffect;

        [Space(5)]
        [Header("Title Interface")]
        public TitleUI title;

        

        private void Awake()
        {
            SetSingleton();
            SetRenderCam();

            screenEffect.blur.image.material.SetColor("_Color", Color.white);
            screenEffect.blur.image.material.SetFloat("_Size", 0);

        }

        public void SetSingleton()
        {
            if (instance == null)
            {
                instance = GetComponent<CanvasInfo>();
            }

            if (instance.gameObject != this.gameObject)
                Destroy(gameObject);

            DontDestroyOnLoad(this.gameObject);
        }

        public void SetRenderCam()
        {
            Camera uiCam = GameObject.FindGameObjectWithTag("UICam").GetComponent<Camera>();

            for(int i=0; i<Layers.Length; i++)
            {
                Layers[i].worldCamera = uiCam;
            }
        }
    }
}