﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using MC.SceneDirector;

namespace MC.UI {

    // 변수 명 통일 및 정리 필요

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

        [Space(5)][Header("HUDs")]
        public UIPlayer player;
        public EnemyHPBar enemyHP;
        public UIMission mission;
        public SystemUI systemUI;
        public MousePointer mousePointer;
        public ScreenEffect screenEffect;

        public UIDialog dialog;

        public GameObject pauseMenu;

        public Texture2D cursorTexture;
        public bool hotSpotIsCenter = false;
        public Vector2 adjustHotSpot = Vector2.zero;
        private Vector2 hotSpot;

        public Animator missionStartAnim;

        public GameObject setting;

        public UIClearMission clearUI;
        public UIFailMission failUI;

        public void Start()
        {
            StartCoroutine("MyCursor");
        }
        IEnumerator MyCursor()
        {

            yield return new WaitForEndOfFrame();

            if (hotSpotIsCenter)
            {
                hotSpot.x = cursorTexture.width / 2;
                hotSpot.y = cursorTexture.height / 2;
            }
            else
            {
                hotSpot = adjustHotSpot;
            }
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        }


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

        public static void PauseMenuActive(bool isActive)
        {
            Debug.Log("Pause Mode : " + isActive);
            GameManager.Instance.IsPuase = isActive;
            UserInterface.SetPointerMode(isActive);
            //GameManager.Instance.CharacterControl = isActive;
            Instance.pauseMenu.SetActive(isActive);
        }

        public void ToTitle()
        {
            PauseMenuActive(false);
            failUI.gameObject.SetActive(false);
            MCSceneManager.Instance.NextScene(MCSceneManager.TITLE, 1f, true);

            UserInterface.SetPointerMode(true);

            if(PlayerFSMManager.Instance != null)
            {
                Debug.Log("체력바 삭제");
                PlayerFSMManager.Instance.Stat.lastHitBy = null;
                Instance.enemyHP.SetFalse();
            }

            UserInterface.Instance.SetValue();
            UserInterface.SetPlayerUserInterface(false);
            UserInterface.SetMissionProgressUserInterface(false);
            UserInterface.SetMissionSelectionUI(false);
            UserInterface.SetSystemInterface(false);

            GameStatus.Instance.StageLevel = 0;
        }

        public void RestartScene()
        {
            PauseMenuActive(false);
            failUI.gameObject.SetActive(false);
            GameStatus.SetCurrentGameState(CurrentGameState.Loading);
            MCSceneManager.Instance.NextScene(MCSceneManager.currentScene, 1f, true);
            GameStatus.Instance.StageLevel--;

            if(GameStatus.Instance.ActivedMonsterList.Count != 0)
            {
                GameStatus.Instance.ActivedMonsterList.Clear();
            }
        }

        public void ExitGame()
        {
            PauseMenuActive(false);
            failUI.gameObject.SetActive(false);
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#endif
        }
    }
}