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
                if (!spawning)
                {
                    Spawn();
                    spawning = true;
                }
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

        void Spawn()
        {
            if (currentWave >= 3)
            {
                ClearMission();
                return;
            }

            StartCoroutine(SetSommonLocation(waves[currentWave].monsterTypes));
            currentWave++;
        }
    }
}