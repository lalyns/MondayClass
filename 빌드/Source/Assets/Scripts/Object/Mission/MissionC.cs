using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Mission
{

    public class MissionC : MissionBase
    {
        public static bool isDialogC = false;

        public ProtectedTarget protectedTarget;
        public int _ProtectedTargetHP;

        float spawnTime = 0;
        public float spawnCool = 3f;

        int NumberOfMaxMonster = 20;

        public MonsterWave[] waves;

        public int currentWave = 0;
        public int totalWave = 3;

        public List<Transform> oldSpawnList = new List<Transform>();

        bool isClear = false;

        public override void OperateMission()
        {
            base.OperateMission();

            protectedTarget.hp = _ProtectedTargetHP;
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (missionEnd) return;

            if (GameStatus.Instance.ActivedMonsterList.Count >= NumberOfMaxMonster) return;


            if (MissionOperate)
            {
                spawnTime += Time.deltaTime;

                if (spawnTime > spawnCool)
                {
                    Spawn();
                    spawnTime = 0;
                }
            }

            if (!isClear && GameStatus.Instance._LimitTime <= 0 && protectedTarget.hp >= 0)
            {
                ClearMission();
                PlayerFSMManager.Instance.CurrentClear = Random.Range((int)0, (int)2);
                PlayerFSMManager.Instance.SetState(PlayerState.CLEAR);
                isClear = true;
            }
        }

        public override void RestMission()
        {
            base.RestMission();

            spawnTime = 0;
        }

        public override void ClearMission() {
            base.ClearMission();

            StopAllCoroutines();
        }

        void Spawn()
        {
            if (currentWave >= totalWave) currentWave = 0;

            StartCoroutine(SetSommonLocation(waves[currentWave].monsterTypes));
            currentWave++;
        }

    }
}