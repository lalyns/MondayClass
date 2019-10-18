using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Mission
{

    public class MissionB : MissionBase
    {
        public static bool isDialogB = false;

        public int goalScore = 5;
        public int currentScore = 0;

        public float starHeight = 10f;

        float starDropTime = 0;
        public float starDropCool = 5f;

        float spawnTime = 0;
        public float spawnCool = 3f;

        int NumberOfMaxMonster = 20;

        public MonsterWave[] waves;

        public int currentWave = 0;
        public int totalWave = 3;

        public List<Transform> oldSpawnList = new List<Transform>();
        public List<GameObject> activeStar = new List<GameObject>();

        public ObjectPool starPool;

        // Start is called before the first frame update
        protected override void Start()
        {
            totalWave = waves.Length;
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (missionEnd) return;

            if (GameStatus.Instance.ActivedMonsterList.Count >= NumberOfMaxMonster) return;

            if (MissionOperate)
            {
                starDropTime += Time.deltaTime;
                spawnTime += Time.deltaTime;

                if (starDropTime > starDropCool)
                {
                    DropStar();
                    starDropTime = 0;
                }

                if (spawnTime > spawnCool)
                {
                    Spawn();
                    spawnTime = 0;
                }
            }

            if (currentScore == goalScore)
            {
                ClearMission();
                PlayerFSMManager.Instance.CurrentClear = Random.Range((int)0, (int)2);
                PlayerFSMManager.Instance.SetState(PlayerState.CLEAR);
                missionEnd = true;
            }
        }

        public void Spawn()
        {
            if (currentWave >= totalWave) currentWave = 0;

            StartCoroutine(SetSommonLocation(waves[currentWave].monsterTypes));
        }

        public override void RestMission()
        {
            base.RestMission();

            spawnTime = 0;
            currentScore = 0;
        }

        public override void ClearMission()
        {
            base.ClearMission();

            foreach (GameObject star in activeStar)
                starPool.ItemReturnPool(star);

            StopAllCoroutines();
        }

        void DropStar()
        {
            var randPos = UnityEngine.Random.Range(0, Grid.mapPositions.Count);

            GameObject star = starPool.ItemSetActive(
                Grid.mapPositions[randPos] + Vector3.up * starHeight);

            star.GetComponent<DropStar>().stop = false;

            activeStar.Add(star);
        }
    }
}