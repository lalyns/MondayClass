using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.Sound;
using MC.UI;

namespace MC.Mission
{

    public class MissionB : MissionBase
    {
        public static bool isDialogB = false;
        public Button manual;

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

            UserInterface.SetPointerMode(false);

            MC.Sound.MCSoundManager.LoadBank();
            var sound = MCSoundManager.Instance.objectSound;
            StartCoroutine(MCSoundManager.AmbFadeIn(1f));
            StartCoroutine(MCSoundManager.BGMFadeIn(1f));
            MCSoundManager.ChangeBGM(sound.bgm.stageBGM);
            MCSoundManager.ChangeAMB(sound.ambient.tutoAmbient);
        }

        float _manualTime = 0;
        protected override void Update()
        {
            base.Update();

            _manualTime += Time.realtimeSinceStartup;
            if (_manualTime > 10.0f && !isDialogB)
            {
                UserInterface.SetPointerMode(true);
                manual.interactable = true;
            }
            if (missionEnd) return;

            if (GameStatus.currentGameState == CurrentGameState.Dead ||
                GameStatus.currentGameState == CurrentGameState.Product) return;

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

                if (GameStatus.Instance._LimitTime <= 0)
                {
                    Debug.Log(GameStatus.Instance._LimitTime);
                    FailMission();
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

        public override void FailMission()
        {
            base.FailMission();
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
            var randPos = UnityEngine.Random.Range(0, MapGrid.mapPositions.Count);

            GameObject star = starPool.ItemSetActive(
                MapGrid.mapPositions[randPos] + Vector3.up * starHeight);

            star.GetComponent<DropStar>().stop = false;
            star.GetComponent<DropStar>().PlaySound();

            activeStar.Add(star);
        }

        public void ManualSupport()
        {
            isDialogB = true;
            GameStatus.SetCurrentGameState(CurrentGameState.Wait);
            GameManager.Instance.IsPuase = false;
            UserInterface.SetPointerMode(false);
            manual.gameObject.SetActive(false);
        }
    }
}