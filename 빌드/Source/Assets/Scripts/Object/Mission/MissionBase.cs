using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.SceneDirector;
using UnityEngine.Playables;
using MC.Sound;
using MC.UI;

namespace MC.Mission
{
    [System.Serializable]
    public class MonsterWave
    {
        public MonsterType[] monsterTypes;
    }

    public class MissionBase : MonoBehaviour
    {
        protected MapGrid grid;
        public MapGrid Grid {
            get {
                if (grid == null)
                    grid = GetComponent<MapGrid>();
                return grid;
            }
        }

        protected MissionEnter _Enter;
        public MissionEnter Enter {
            get {
                if (_Enter == null)
                {
                    _Enter = GetComponentInChildren<MissionEnter>();
                }
                return _Enter;
            }
            internal set {
                _Enter = value;
            }
        }

        protected MissionExit _Exit;
        public MissionExit Exit {
            get {
                if (_Exit == null)
                {
                    _Exit = GetComponentInChildren<MissionExit>();
                }
                return _Exit;
            }
            internal set {
                _Exit = value;
            }
        }

        [SerializeField] protected MissionData _Data;
        public MissionData Data {
            get { return _Data; }
        }

        [SerializeField] protected bool _MissionOperate;
        public bool MissionOperate {
            get { return _MissionOperate; }
            internal set { _MissionOperate = value; }
        }

        public int _LimitTime = 180;
        public MissionType MissionType;

        public bool missionEnd = false;


        protected virtual void Awake()
        {
            Enter = GetComponentInChildren<MissionEnter>();
            //Enter.Colliders.enabled = false;

            Exit = GetComponentInChildren<MissionExit>();
            try
            {
                Exit.Colliders.enabled = false;
            }
            catch
            {

            }
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        public virtual void RestMission()
        {
            MissionOperate = false;
            Exit.Colliders.enabled = false;
            Exit._PortalEffect.SetActive(false);

        }

        public virtual void OperateMission()
        {
            GameStatus.Instance.StageLevel++;
            GameStatus.Instance._MissionStatus = true;
            GameStatus.Instance._LimitTime = _LimitTime;

            MissionOperate = true;
            Exit.Colliders.enabled = false;
        }

        public virtual void ClearMission()
        {
            GameStatus.Instance._MissionStatus = false;

            if (!GameStatus.Instance.usingKeward && MissionManager.Instance.CurrentMissionType != MissionType.Annihilation)
            {
                GameStatus.Instance.RemoveAllActiveMonster();
            }

            if(GameStatus.Instance.StageLevel == 3)
            {
                var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();
                UserInterface.DialogSetActive(true);
                UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[5]);
                GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            }

            if(GameStatus.Instance.StageLevel == 8)
            {
                var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();
                UserInterface.DialogSetActive(true);
                UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[6]);
                GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            }

            Exit._PortalEffect.SetActive(true);
            Exit.Colliders.enabled = true;

            var sound = MCSoundManager.Instance.objectSound.objectSFX;
            sound.PlaySound(Exit.gameObject, sound.portalCreate);

            MissionManager.RewardMission();
        }

        public virtual void EnterDirector()
        {
        }

        public IEnumerator SetSommonLocation(MonsterType[] monsterTypes)
        {
            GameObject a = null;
            foreach (MonsterType monsterType in monsterTypes)
            {
                var position = Grid.mapPositions.Count;
                var rand = UnityEngine.Random.Range(0, position);

                switch (monsterType)
                {
                    case MonsterType.Mac:
                        a = MonsterPoolManager._Instance._Mac.ItemSetActive(Grid.mapPositions[rand], monsterType);
                        break;
                    case MonsterType.RedHat:
                        a = MonsterPoolManager._Instance._RedHat.ItemSetActive(Grid.mapPositions[rand], monsterType);
                        break;
                    case MonsterType.Tiber:
                        a = MonsterPoolManager._Instance._Tiber.ItemSetActive(Grid.mapPositions[rand], monsterType);
                        break;
                }

                yield return new WaitForSeconds(0.1f);
            }

        }
    }
}