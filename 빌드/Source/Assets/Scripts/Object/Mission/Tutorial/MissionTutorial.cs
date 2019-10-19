using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

namespace MC.Mission
{
    public enum TutorialEvent
    {
        None = 0,
        Two,
        Three,
        Move,
        Dash,
        Item,
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        End,
    }

    public class MissionTutorial : MissionBase
    {
        public TutorialEvent currentTutorial = TutorialEvent.None;
        public UITutorial tutorialUI;

        public bool tutostart = false;
        bool tutorial = false;
        [SerializeField]int count = 0;

        bool wChange, sChange, aChange, dChange = false;
        bool spaceChange = false;
        bool attack1, attack2, attack3, attack4 = false;

        public MonsterWave[] tutoWave;
        public FenceEffect[] fences;

        // Start is called before the first frame update
        protected override void Awake()
        {
            //base.Awake();

            currentTutorial = TutorialEvent.None;
        }

        // Update is called once per frame

        protected override void Update()
        {
            // base.Update();
            if (currentTutorial == TutorialEvent.None && !tutorial && tutostart)
            {
                GameStatus.currentGameState = CurrentGameState.Dialog;
                var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

                //tutorialUI.gameObject.SetActive(false);
                UserInterface.DialogSetActive(true);
                UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[0],
                    () => {
                        GameStatus.currentGameState = CurrentGameState.Tutorial;
                        currentTutorial = TutorialEvent.Two;
                        //tutorialUI.gameObject.SetActive(true);
                        Invoke("SetDialog2", 2f);
                    });

                tutorial = true;
            }

            if (currentTutorial == TutorialEvent.Move)
            {
                if (Input.GetKeyDown(KeyCode.W) && !wChange)
                {
                    tutorialUI.move.W.sprite = tutorialUI.move.WSprites[1];
                    wChange = true;
                    count++;
                }

                if (Input.GetKeyDown(KeyCode.S) && !sChange)
                {
                    tutorialUI.move.S.sprite = tutorialUI.move.SSprites[1];
                    sChange = true;
                    count++;
                }

                if (Input.GetKeyDown(KeyCode.A) && !aChange)
                {
                    tutorialUI.move.A.sprite = tutorialUI.move.ASprites[1];
                    aChange = true;
                    count++;
                }

                if (Input.GetKeyDown(KeyCode.D) && !dChange)
                {
                    tutorialUI.move.D.sprite = tutorialUI.move.DSprites[1];
                    dChange = true;
                    count++;
                }

                if(count == 4)
                {
                    GameStatus.currentGameState = CurrentGameState.Dialog;
                    var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

                    currentTutorial = TutorialEvent.None;

                    tutorialUI.move.gameObject.SetActive(false);
                    UserInterface.DialogSetActive(true);
                    UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[3],
                        () => {
                            GameStatus.currentGameState = CurrentGameState.Tutorial;
                            currentTutorial = TutorialEvent.Dash;
                            tutorialUI.dash.gameObject.SetActive(true);
                            fences[0].OpenFence();
                        });

                }
            }

            if (currentTutorial == TutorialEvent.Dash)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !spaceChange)
                {
                    tutorialUI.dash.space.sprite = tutorialUI.dash.spaceSprites[1];
                    Invoke("ReturnSpace", 0.5f);
                    if (count <= 6)
                    {
                        count++;
                    }

                    if (count == 7)
                    {
                        spaceChange = true;
                        fences[1].OpenFence();
                        Invoke("SetDialogItem", 0.5f);
                    }
                }
            }

            if(currentTutorial != TutorialEvent.End && Input.GetKeyDown(KeyCode.F10))
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

            if(!attack1 && GameStatus.Instance.ActivedMonsterList.Count == 0 && currentTutorial == TutorialEvent.Attack1)
            {
                attack1 = true;
                ClearMission();
            }
        }

        void ReturnSpace()
        {
            tutorialUI.dash.space.sprite = tutorialUI.dash.spaceSprites[0];
        }

        void SetDialog2()
        {
            GameStatus.currentGameState = CurrentGameState.Dialog;
            var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

            UserInterface.DialogSetActive(true);
            UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[1],
                () => {
                    GameStatus.currentGameState = CurrentGameState.Tutorial;
                    currentTutorial = TutorialEvent.Three;
                    Invoke("SetDialog3", 2f);
                });

        }

        void SetDialog3()
        {
            GameStatus.currentGameState = CurrentGameState.Dialog;
            var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

            UserInterface.DialogSetActive(true);
            UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[2],
                () => {
                    GameStatus.currentGameState = CurrentGameState.Tutorial;
                    currentTutorial = TutorialEvent.Move;
                    tutorialUI.move.gameObject.SetActive(true);
                    GameManager.Instance.CharacterControl = true;
                });

        }

        void SetDialogItem()
        {
            GameStatus.currentGameState = CurrentGameState.Dialog;
            var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

            fences[2].OpenFence();
            UserInterface.DialogSetActive(true);
            tutorialUI.dash.gameObject.SetActive(false);
            UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[4],
                () => {
                    GameStatus.currentGameState = CurrentGameState.Tutorial;
                    currentTutorial = TutorialEvent.Item;
                    //tutorialUI.move.gameObject.SetActive(true);
                    GameManager.Instance.CharacterControl = true;
                });

        }

        public void SetAttack1Event()
        {
            GameStatus.currentGameState = CurrentGameState.Dialog;
            var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

            UserInterface.DialogSetActive(true);
            tutorialUI.dash.gameObject.SetActive(false);
            UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[6],
                () => {
                    GameStatus.currentGameState = CurrentGameState.Tutorial;
                    currentTutorial = TutorialEvent.Attack1;
                    //tutorialUI.move.gameObject.SetActive(true);
                    StartCoroutine(SetSommonLocation(tutoWave[0].monsterTypes));
                    GameManager.Instance.CharacterControl = true;
                });
        }


        void NextTutorial(TutorialEvent state)
        {
            currentTutorial = state;
        }
    }
}