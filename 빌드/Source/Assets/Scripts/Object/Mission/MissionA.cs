﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.Sound;
using MC.UI;

namespace MC.Mission
{

    public class MissionA : MissionBase
    {
        public static bool isDialogA = false;
        public Button manual;
        public float manualTimer = 5f;

        public bool spawning = false;

        public int currentWave = 0;
        public int totalWave = 3; 

        public MonsterWave[] waves;
        public Text text;

        public string waiting;
        public string appearing;

        protected override void Start()
        {
            base.Start();

            totalWave = waves.Length;

            if (!isDialogA)
            {
                // 미션 설명창 등장해야됨
                Invoke("ManualPopup", manualTimer);
            }

            MC.Sound.MCSoundManager.LoadBank();
            var sound = MCSoundManager.Instance.objectSound;
            StartCoroutine(MCSoundManager.AmbFadeIn(1f));
            StartCoroutine(MCSoundManager.BGMFadeIn(1f));
            MCSoundManager.ChangeBGM(sound.bgm.stageBGM);
            MCSoundManager.ChangeAMB(sound.ambient.stageAmbient);
        }

        float _manualTime = 0;
        protected override void Update()
        {
            _manualTime += Time.realtimeSinceStartup;
            if (_manualTime > 10.0f && !isDialogA)
            {
                UserInterface.SetPointerMode(true);
                manual.interactable = true;
            }

            base.Update();

            if (GameStatus.currentGameState == CurrentGameState.Dead ||
                GameStatus.currentGameState == CurrentGameState.Product) return;

            if (missionEnd) return;

            if (MissionOperate)
            {
                if (!spawning && currentWave != totalWave)
                {
                    Spawn();
                    spawning = true;
                }

                if (currentWave == totalWave && GameStatus.Instance.ActivedMonsterList.Count == 0 && !missionEnd)
                {
                    ClearMission();
                    PlayerFSMManager.Instance.CurrentClear = Random.Range((int)0, (int)2);
                    PlayerFSMManager.Instance.SetState(PlayerState.CLEAR);
                    missionEnd = true;
                }

                if(currentWave != totalWave && GameStatus.Instance.ActivedMonsterList.Count == 0)
                {
                    text.gameObject.SetActive(true);
                    text.text = waiting;
                }

                if (GameStatus.Instance._LimitTime <= 0)
                {
                    FailMission();
                }
            }

        }

        public override void FailMission()
        {
            base.FailMission();


        }

        public void MonsterCheck()
        {
            if (spawning)
            {
                bool monsterCheck = GameStatus.Instance.ActivedMonsterList.Count == 0;

                if (monsterCheck)
                {
                    spawning = false;
                }

                if (DeveloperEditor.Instance.usingKeward)
                    DeveloperEditor.Instance.usingKeward = false;
            }
        }

        public override void RestMission()
        {
            base.RestMission();

            currentWave = 0;
        }

        public override void ClearMission() {
            base.ClearMission();

            StopAllCoroutines();
        }

        void Spawn()
        {
            text.gameObject.SetActive(true);
            text.text = appearing;

            StartCoroutine(SetSommonLocation(waves[currentWave].monsterTypes));
            currentWave++;
            //Debug.Log(currentWave);
            Invoke("CanvasOff", 3f);
        }

        void CanvasOff()
        {
            text.gameObject.SetActive(false);
        }

        public void ManualPopup()
        {
            UserInterface.BlurSet(true, 8);
            GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            GameManager.Instance.IsPuase = true;
            manual.gameObject.SetActive(true);
        }

        public void ManualSupport()
        {
            isDialogA = true;
            UserInterface.BlurSet(false);
            GameStatus.SetCurrentGameState(CurrentGameState.Wait);
            GameManager.Instance.IsPuase = false;
            UserInterface.SetPointerMode(false);
            manual.gameObject.SetActive(false);
        }
    }
}