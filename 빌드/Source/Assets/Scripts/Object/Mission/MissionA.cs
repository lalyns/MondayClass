using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Mission
{

    public class MissionA : MissionBase
    {
        public bool spawning = false;

        public int currentWave = 0;
        public int totalWave = 3; 

        public MonsterWave[] waves;

        protected override void Start()
        {
            base.Start();

            totalWave = waves.Length;
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
                missionEnd = true;
                ClearMission();

            }


        }

        public void MonsterCheck()
        {
            Debug.Log("Check Call");
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
            StartCoroutine(SetSommonLocation(waves[currentWave].monsterTypes));
            currentWave++;
            Debug.Log(currentWave);
        }
    }
}