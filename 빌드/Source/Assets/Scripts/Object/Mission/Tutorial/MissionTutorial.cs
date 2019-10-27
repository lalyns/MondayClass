﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Sound;

namespace MC.Mission
{
    public enum TutorialEvent
    {
        Start = 0,
        Description,
        Three,
        MoveDash,
        Item,
        Attack,
        Skill1,
        Skill2,
        Skill3,
        Transform,
        End,
    }

    public class MissionTutorial : MissionBase
    {
        public TutorialEvent currentTutorial = TutorialEvent.Start;
        public UITutorial tutorialUI;

        public bool tutostart = false;
        bool tutorial = false;
        [SerializeField]int count = 0;
        int dashCount = 0;

        bool wChange, sChange, aChange, dChange = false;
        bool spaceChange = false;
        bool attack1, skill1, skill2, skill3, skill4, trans = false;
        bool attackChange = false;
        [HideInInspector] public bool skill1Change = false;
        [HideInInspector] public bool skill2Change = false;
        [HideInInspector] public bool skill3Change = false;
        [HideInInspector] public bool transChange = false;

        public MonsterWave[] tutoWave;
        public FenceEffect[] fences;

        // Start is called before the first frame update
        protected override void Awake()
        {
            //base.Awake();

            currentTutorial = TutorialEvent.Start;

            MC.Sound.MCSoundManager.LoadBank();
            var sound = MCSoundManager.Instance.objectSound;
            StartCoroutine(MCSoundManager.AmbFadeIn(1f));
            StartCoroutine(MCSoundManager.BGMFadeIn(1f));
            MCSoundManager.ChangeBGM(sound.bgm.tutoBGM);
            MCSoundManager.ChangeAMB(sound.ambient.tutoAmbient);
        }

        // Update is called once per frame

        protected override void Update()
        {
            tutorialUI.gameObject.SetActive(GameStatus.currentGameState == CurrentGameState.Tutorial);

            // base.Update();
            if (currentTutorial == TutorialEvent.Start && !tutorial && tutostart)
            {
                PlayerFSMManager.Instance.isInputLock = true;
                GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
                var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

                //tutorialUI.gameObject.SetActive(false);
                UserInterface.DialogSetActive(true);
                UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[0],
                    () => {
                        GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
                        currentTutorial = TutorialEvent.Description;
                        //tutorialUI.gameObject.SetActive(true);
                        Invoke("SetDiscription", 2f);
                    });

                tutorial = true;
            }

            if (currentTutorial == TutorialEvent.MoveDash)
            {
                if (Input.GetKeyDown(KeyCode.W) && !wChange)
                {
                    tutorialUI.moveDash.W.sprite = tutorialUI.moveDash.WSprites[1];
                    wChange = true;
                    count++;
                }

                if (Input.GetKeyDown(KeyCode.S) && !sChange)
                {
                    tutorialUI.moveDash.S.sprite = tutorialUI.moveDash.SSprites[1];
                    sChange = true;
                    count++;
                }

                if (Input.GetKeyDown(KeyCode.A) && !aChange)
                {
                    tutorialUI.moveDash.A.sprite = tutorialUI.moveDash.ASprites[1];
                    aChange = true;
                    count++;
                }

                if (Input.GetKeyDown(KeyCode.D) && !dChange)
                {
                    tutorialUI.moveDash.D.sprite = tutorialUI.moveDash.DSprites[1];
                    dChange = true;
                    count++;
                }

                if (Input.GetKeyDown(KeyCode.Space) && !spaceChange)
                {
                    tutorialUI.moveDash.space.sprite = tutorialUI.moveDash.spaceSprites[1];
                    if (GameStatus.currentGameState != CurrentGameState.Dialog && dashCount < 3)
                    {
                        dashCount++;
                        Invoke("ReturnSpace", 0.5f);
                    }
                }

                if (count == 4 && dashCount == 3)
                {
                    spaceChange = true;
                    currentTutorial = TutorialEvent.Start;

                    Invoke("MoveAndDashEventSupport", 0.5f);
                    tutorialUI.moveDash.gameObject.SetActive(false);
                    UserInterface.DialogSetActive(true);
                    
                }
            }

            if(currentTutorial == TutorialEvent.Attack)
            {
                if(!attackChange)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        tutorialUI.attack.Attack.sprite = tutorialUI.attack.AttackSprites[1];
                        Invoke("ReturnAttack", 0.5f);
                    }
                }

                if (GameStatus.Instance.ActivedMonsterList.Count == 0 &&!attackChange)
                {
                    attackChange = true;
                    Invoke("SetSkill1Event", 0.5f);
                }
            }

            if(currentTutorial == TutorialEvent.Skill1)
            {
                if (!skill1Change)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        tutorialUI.attack.Attack.sprite = tutorialUI.attack.AttackSprites[1];
                        Invoke("ReturnAttack", 0.5f);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        tutorialUI.attack.Skill1.sprite = tutorialUI.attack.skill1Sprites[1];
                        Invoke("ReturnSkill1", 0.5f);
                    }
                }

                if (GameStatus.Instance.ActivedMonsterList.Count == 0 && !skill1Change)
                {
                    skill1Change = true;
                    Invoke("SetSkill2Event", 0.5f);

                }
            }

            if (currentTutorial == TutorialEvent.Skill2)
            {
                if (!skill2Change)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        tutorialUI.attack.Attack.sprite = tutorialUI.attack.AttackSprites[1];
                        Invoke("ReturnAttack", 0.5f);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        tutorialUI.attack.Skill1.sprite = tutorialUI.attack.skill1Sprites[1];
                        Invoke("ReturnSkill1", 0.5f);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        tutorialUI.attack.Skill2.sprite = tutorialUI.attack.skill2Sprites[1];
                        Invoke("ReturnSkill2", 0.5f);
                    }
                }

                if (GameStatus.Instance.ActivedMonsterList.Count == 0 && !skill2Change)
                {
                    skill2Change = true;
                    Invoke("SetSkill3Event", 0.5f);

                }
            }

            if (currentTutorial == TutorialEvent.Skill3)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && !skill3Change)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        tutorialUI.attack.Attack.sprite = tutorialUI.attack.AttackSprites[1];
                        Invoke("ReturnAttack", 0.5f);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        tutorialUI.attack.Skill1.sprite = tutorialUI.attack.skill1Sprites[1];
                        Invoke("ReturnSkill1", 0.5f);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        tutorialUI.attack.Skill2.sprite = tutorialUI.attack.skill2Sprites[1];
                        Invoke("ReturnSkill2", 0.5f);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        tutorialUI.attack.Skill3.sprite = tutorialUI.attack.skill3Sprites[1];
                        Invoke("ReturnSkill3", 0.5f);
                    }
                }

                if (GameStatus.Instance.ActivedMonsterList.Count == 0 && !skill3Change)
                {
                    skill3Change = true;
                    Invoke("SetTransformEvent", 0.5f);

                } 
            }

            if (currentTutorial == TutorialEvent.Transform)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && !transChange)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        tutorialUI.attack.Attack.sprite = tutorialUI.attack.AttackSprites[1];
                        Invoke("ReturnAttack", 0.5f);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        tutorialUI.attack.Skill1.sprite = tutorialUI.attack.skill1Sprites[1];
                        Invoke("ReturnSkill1", 0.5f);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        tutorialUI.attack.Skill2.sprite = tutorialUI.attack.skill2Sprites[1];
                        Invoke("ReturnSkill2", 0.5f);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        Invoke("ReturnSkill3", 0.5f);
                        tutorialUI.attack.Skill3.sprite = tutorialUI.attack.skill3Sprites[1];
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        Invoke("ReturnSkill4", 0.5f);
                        tutorialUI.attack.Skill4.sprite = tutorialUI.attack.skill4Sprites[1];
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        Invoke("ReturnSpecial", 0.5f);
                        tutorialUI.attack.Special.sprite = tutorialUI.attack.specialSprites[1];
                    }

                    PlayerFSMManager.Instance.SpecialGauge = 100;

                }

                if (GameStatus.Instance.ActivedMonsterList.Count == 0 && !transChange)
                {
                    transChange = true;

                    tutorialUI.attack.special.gameObject.SetActive(false);
                    tutorialUI.attack.Attack.gameObject.SetActive(false);
                    currentTutorial = TutorialEvent.End;
                }
            }

            if (currentTutorial != TutorialEvent.End && Input.GetKeyDown(KeyCode.F10))
            {
                currentTutorial = TutorialEvent.End;
            }

            if(currentTutorial == TutorialEvent.End)
            {
                if (!GetComponentInChildren<MissionExit>()._PortalEffect.activeSelf)
                {
                    GetComponentInChildren<MissionExit>()._PortalEffect.SetActive(true);
                    GetComponentInChildren<MissionExit>().Colliders.enabled = true;
                }
            }

            //if(!attack1 && GameStatus.Instance.ActivedMonsterList.Count == 0 && currentTutorial == TutorialEvent.Attack)
            //{
            //    attack1 = true;
            //    currentTutorial = TutorialEvent.End;
            //}
        }

        void ReturnSpace()
        {
            tutorialUI.moveDash.space.sprite = tutorialUI.moveDash.spaceSprites[0];
        }

        void ReturnAttack()
        {
            tutorialUI.attack.Attack.sprite = tutorialUI.attack.AttackSprites[0];
        }

        void ReturnSkill1()
        {
            tutorialUI.attack.Skill1.sprite = tutorialUI.attack.skill1Sprites[0];
        }

        void ReturnSkill2()
        {
            tutorialUI.attack.Skill2.sprite = tutorialUI.attack.skill2Sprites[0];
        }

        void ReturnSkill3()
        {
            tutorialUI.attack.Skill3.sprite = tutorialUI.attack.skill3Sprites[0];
        }

        void ReturnSkill4()
        {
            tutorialUI.attack.Skill4.sprite = tutorialUI.attack.skill4Sprites[0];
        }

        void ReturnSpecial()
        {
            tutorialUI.attack.Special.sprite = tutorialUI.attack.specialSprites[0];
        }

        void SetDiscription()
        {
            GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

            UserInterface.DialogSetActive(true);
            UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[1],
                () => {
                    GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
                    currentTutorial = TutorialEvent.Three;
                    Invoke("SetMoveAndDashEvent", 2f);
                });

        }

        void SetMoveAndDashEvent()
        {
            PlayerFSMManager.Instance.isInputLock = false;
            PlayerFSMManager.Instance.isAttackOne = false;
            GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

            UserInterface.DialogSetActive(true);
            UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[2],
                () => {
                    GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
                    currentTutorial = TutorialEvent.MoveDash;
                    tutorialUI.moveDash.gameObject.SetActive(true);
                    GameManager.Instance.CharacterControl = true;
                });

        }

        void MoveAndDashEventSupport()
        {
            GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();
            PlayerFSMManager.Instance.isAttackOne = false;
            UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[3],
                () => 
                {
                    GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
                    currentTutorial = TutorialEvent.Item;
                    fences[0].OpenFence();
                });
        }

        void SetDialogItem()
        {
            GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();
            PlayerFSMManager.Instance.isAttackOne = false;
            fences[2].OpenFence();
            UserInterface.DialogSetActive(true);
            tutorialUI.moveDash.gameObject.SetActive(false);
            UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[4],
                (() => {
                    GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
                    currentTutorial = TutorialEvent.Item;
                    //tutorialUI.move.gameObject.SetActive(true);
                    GameManager.Instance.CharacterControl = true;
                }));

        }

        public void SetAttackEvent()
        {
            GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

            UserInterface.DialogSetActive(true);
            UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[4],
                () => {
                    GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
                    currentTutorial = TutorialEvent.Attack;
                    tutorialUI.attack.gameObject.SetActive(true);
                    tutorialUI.attack.attack.gameObject.SetActive(true);
                    //tutorialUI.move.gameObject.SetActive(true);
                    StartCoroutine(SetSommonLocation(tutoWave[0].monsterTypes));
                    GameManager.Instance.CharacterControl = true;
                });
        }

        public void SetSkill1Event()
        {
            //GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            //var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

            //UserInterface.DialogSetActive(true);
            //tutorialUI.attack.gameObject.SetActive(true);
            //UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[4],
            //    () => {
            //        GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
            //        currentTutorial = TutorialEvent.Skill1;
            //        tutorialUI.move.gameObject.SetActive(true);
            //        StartCoroutine(SetSommonLocation(tutoWave[0].monsterTypes));
            //        GameManager.Instance.CharacterControl = true;
            //    });

            currentTutorial = TutorialEvent.Skill1;
            StartCoroutine(SetSommonLocation(tutoWave[0].monsterTypes));
            tutorialUI.attack.attack.gameObject.SetActive(false);
            tutorialUI.attack.skill1.gameObject.SetActive(true);


        }

        public void SetSkill2Event()
        {
            //GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            //var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

            //UserInterface.DialogSetActive(true);
            //tutorialUI.attack.gameObject.SetActive(true);
            //UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[4],
            //    () => {
            //        GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
            //        currentTutorial = TutorialEvent.Skill2;
            //        //tutorialUI.move.gameObject.SetActive(true);
            //        StartCoroutine(SetSommonLocation(tutoWave[0].monsterTypes));
            //        GameManager.Instance.CharacterControl = true;
            //    });


            currentTutorial = TutorialEvent.Skill2;
            StartCoroutine(SetSommonLocation(tutoWave[0].monsterTypes));
            tutorialUI.attack.skill1.gameObject.SetActive(false);
            tutorialUI.attack.skill2.gameObject.SetActive(true);
        }

        public void SetSkill3Event()
        {
            //GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            //var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

            //UserInterface.DialogSetActive(true);
            //tutorialUI.attack.gameObject.SetActive(true);
            //UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[4],
            //    () => {
            //        GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
            //        currentTutorial = TutorialEvent.Skill3;
            //        //tutorialUI.move.gameObject.SetActive(true);
            //        StartCoroutine(SetSommonLocation(tutoWave[0].monsterTypes));
            //        GameManager.Instance.CharacterControl = true;
            //    });


            currentTutorial = TutorialEvent.Skill3;
            StartCoroutine(SetSommonLocation(tutoWave[0].monsterTypes));
            tutorialUI.attack.skill2.gameObject.SetActive(false);
            tutorialUI.attack.skill3.gameObject.SetActive(true);
        }

        public void SetTransformEvent()
        {
            //GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            //var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

            //UserInterface.DialogSetActive(true);
            //tutorialUI.attack.gameObject.SetActive(true);
            //UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[4],
            //    () => {
            //        GameStatus.SetCurrentGameState(CurrentGameState.Tutorial);
            //        currentTutorial = TutorialEvent.Transform;
            //        //tutorialUI.move.gameObject.SetActive(true);
            //        StartCoroutine(SetSommonLocation(tutoWave[0].monsterTypes));
            //        GameManager.Instance.CharacterControl = true;
            //    });

            currentTutorial = TutorialEvent.Transform;
            StartCoroutine(SetSommonLocation(tutoWave[0].monsterTypes));
            tutorialUI.attack.skill3.gameObject.SetActive(false);
            tutorialUI.attack.special.gameObject.SetActive(true);
        }

        void NextTutorial(TutorialEvent state)
        {
            currentTutorial = state;
        }
    }
}