using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

namespace MC.Mission
{
    public enum TutorialEvent
    {
        None = 0,
        Move,
        Dodge,
        Attack,
    }

    public class MissionTutorial : MonoBehaviour
    {
        public TutorialEvent currentTutorial = TutorialEvent.None;
        public UITutorial tutorialUI;

        bool tutorial = false;

        // Start is called before the first frame update
        void Awake()
        {
            //base.Awake();

            currentTutorial = TutorialEvent.None;
        }

        // Update is called once per frame
        void Start()
        {
            //.Start();
        }

        void Update()
        {
            // base.Update();

            if(currentTutorial == TutorialEvent.None && !tutorial)
            {
                GameStatus.currentGameState = CurrentGameState.Dialog;
                var dialogEvent = GameManager.Instance.GetComponent<DialogEvent>();

                tutorialUI.gameObject.SetActive(false);
                UserInterface.DialogSetActive(true);
                UserInterface.Instance.Dialog.SetDialog(dialogEvent.dialogs[0],
                    () => {
                        GameStatus.currentGameState = CurrentGameState.Tutorial;
                        currentTutorial = TutorialEvent.Move;
                        tutorialUI.gameObject.SetActive(true);
                    });

                tutorial = true;
            }

            if (currentTutorial == TutorialEvent.Move)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    tutorialUI.move.W.sprite = tutorialUI.move.WSprites[1];
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    tutorialUI.move.S.sprite = tutorialUI.move.SSprites[1];
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    tutorialUI.move.A.sprite = tutorialUI.move.ASprites[1];
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    tutorialUI.move.D.sprite = tutorialUI.move.DSprites[1];
                }
            }
        }

        void NextTutorial(TutorialEvent state)
        {
            currentTutorial = state;
        }
    }
}