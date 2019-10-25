using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.UI
{

    public class UIMission : MonoBehaviour
    {
        public MissionSelectorUI selector;
        public MissionProgressUI progress;
    }

    [System.Serializable]
    public class MissionSelectorUI
    {
        public GameObject gameObject;
        public MissionButton[] buttons;
    }

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
        public Image missionType;
        public Text missionText;
        public Image timeImage;
        public Text timeText;
        public Image goalImage;
        public Image goalType;
        public Text goalText;
        public ParticleSystem goalEffect;
    }

    [System.Serializable]
    public class ProgressSimpleUI
    {
        public GameObject gameObject;
        public Image timeImage;
        public Text timeText;
        public Image goalImage;
        public Image goalType;
        public Text goalText;
        public ParticleSystem goalEffect;
    }
}