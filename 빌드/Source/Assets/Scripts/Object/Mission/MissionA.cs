﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Mission
{

    public class MissionA : MissionBase
    {
        public static bool isDialogA = false;

        public bool spawning = false;

        public int currentWave = 0;
        public int totalWave = 3; 

        public MonsterWave[] waves;
        public Canvas canvas;

        protected override void Start()
        {
            base.Start();

            totalWave = waves.Length;

            if (!isDialogA)
            {
                
            }
        }

        protected override void Update()
        {
            if (missionEnd) return;

            if (MissionOperate)
            {
                if (!spawning && currentWave != totalWave)
                {
                    Spawn();
                    spawning = true;
                }
            }

            if (currentWave == totalWave && GameStatus.Instance.ActivedMonsterList.Count == 0 && !missionEnd) {
                ClearMission();
                PlayerFSMManager.Instance.CurrentClear = Random.Range((int)0, (int)2);
                PlayerFSMManager.Instance.SetState(PlayerState.CLEAR);
                missionEnd = true;
            }

            if(GameStatus.Instance._LimitTime <= 0)
            {
                FailMission();
            }
        }

        public override void FailMission()
        {
            base.FailMission();


        }

        public void MonsterCheck()
        {
            //Debug.Log("Check Call");
            if (spawning)
            {
                bool monsterCheck = GameStatus.Instance.ActivedMonsterList.Count == 0;

                if (monsterCheck)
                {
                    spawning = false;
                }

                if (GameStatus.Instance.usingKeward)
                    GameStatus.Instance.usingKeward = false;
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
            canvas.gameObject.SetActive(true);
            StartCoroutine(SetSommonLocation(waves[currentWave].monsterTypes));
            currentWave++;
            //Debug.Log(currentWave);
            Invoke("CanvasOff", 3f);
        }

        void CanvasOff()
        {
            canvas.gameObject.SetActive(false);
        }
    }
}