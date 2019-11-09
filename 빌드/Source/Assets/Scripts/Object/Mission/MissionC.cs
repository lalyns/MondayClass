using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MC.Sound;
using MC.UI;

namespace MC.Mission
{

    public class MissionC : MissionBase
    {
        public static bool isDialogC = false;
        public Button manual;
        public float manualTimer = 5f;

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
        
        protected override void Start()
        {
            base.Start();

            if (!isDialogC)
            {
                // 미션 설명창 등장해야됨
                Invoke("ManualPopup", manualTimer);
            }

            MC.Sound.MCSoundManager.LoadBank();
            var sound = MCSoundManager.Instance.objectSound;
            StartCoroutine(MCSoundManager.AmbFadeIn(1f));
            StartCoroutine(MCSoundManager.BGMFadeIn(1f));
            MCSoundManager.ChangeBGM(sound.bgm.stageBGM);
            MCSoundManager.ChangeAMB(sound.ambient.tutoAmbient);
        }

        public override void OperateMission()
        {
            base.OperateMission();

            protectedTarget.hp = _ProtectedTargetHP;
            MissionOperate = true;
        }

        float _manualTime = 0;
        protected override void Update()
        {
            base.Update();

            _manualTime += Time.realtimeSinceStartup;
            if (_manualTime > 10.0f && !isDialogC)
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
                spawnTime += Time.deltaTime;

                if (spawnTime > spawnCool)
                {
                    Spawn();
                    spawnTime = 0;
                }

                if (!isClear && GameStatus.Instance._LimitTime <= 0 && protectedTarget.hp > 0 && !missionEnd)
                {
                    ClearMission();
                    PlayerFSMManager.Instance.CurrentClear = Random.Range((int)0, (int)2);
                    PlayerFSMManager.Instance.SetState(PlayerState.CLEAR);
                    isClear = true;
                    missionEnd = true;
                }

                if (!isClear && protectedTarget.hp <= 0)
                {
                    Debug.Log(protectedTarget.hp);
                    FailMission();
                    missionEnd = true;
                }
            }
        }

        public override void RestMission()
        {
            base.RestMission();

            spawnTime = 0;
            GameStatus.Instance._LimitTime = _LimitTime;
        }

        public override void ClearMission() {
            base.ClearMission();


            var sound = MCSoundManager.Instance.objectSound.objectSFX;
            sound.StopSound(protectedTarget.gameObject, sound.pillarActive);

            StopAllCoroutines();
        }

        public override void FailMission()
        {
            base.FailMission();

            protectedTarget.DestroyPillar();
        }

        void Spawn()
        {
            if (currentWave >= totalWave) currentWave = 0;

            StartCoroutine(SetSommonLocation(waves[currentWave].monsterTypes));
            currentWave++;
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
            isDialogC = true;
            GameStatus.SetCurrentGameState(CurrentGameState.Wait);
            GameManager.Instance.IsPuase = false;
            UserInterface.SetPointerMode(false);
            UserInterface.BlurSet(false);
            manual.gameObject.SetActive(false);
        }
    }
}