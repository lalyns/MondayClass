using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Mission
{

    public class MissionB : MissionBase
    {
        public int goalScore = 5;

        public float starHeight = 10f;

        float starDropTime = 0;
        public float starDropCool = 5f;
        public GameObject star;
        public Transform[] dropLocation;

        float spawnTime = 0;
        public float spawnCool = 3f;
        public Vector2[] mobSet;
        public Transform[] spawnLocation;

        int NumberOfMaxMonster = 20;

        public MonsterWave[] waves;

        public int currentWave = 0;
        public int totalWave = 3;

        public List<Transform> oldSpawnList = new List<Transform>();
        public List<GameObject> activeStar = new List<GameObject>();

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

            if (GameManager.Instance.curScore >= goalScore)
            {
                ClearMission();
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
        }

        public override void ClearMission()
        {
            base.ClearMission();

            foreach (GameObject star in activeStar)
                EffectPoolManager._Instance._MissionBstarPool.ItemReturnPool(star);

        }

        void DropStar()
        {
            var randPos = UnityEngine.Random.Range(0, Grid._MapPosition.Count);

            GameObject star = EffectPoolManager._Instance._MissionBstarPool.ItemSetActive(Grid._MapPosition[randPos] + Vector3.up * starHeight);

            activeStar.Add(star);
        }
    }
}