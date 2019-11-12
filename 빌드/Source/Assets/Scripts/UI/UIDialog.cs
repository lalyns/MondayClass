﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.UI
{
    [System.Serializable]
    public class DialogUI
    {
        public GameObject gameObject;
        public Text conversation;
        public Image rightStanding;
        public Image leftStanding;
        public Image textUI;
    }

    public enum TalkerType
    {
        Galaxy,
        Staff,
        Riris,
        None,
        End,
    }

    public class UIDialog : MonoBehaviour
    {
        public DialogUI dialogUI;

        public TalkerType currentTalker = TalkerType.None;

        public Dialog currentDialog;
        public int dialogLength = 0;
        int currentTurn = 1;

        public Sprite[] standing;
        public Sprite[] text;

        System.Action NextAction;

        public void SetNextAction(System.Action action)
        {
            NextAction = action;
        }

        float time = 0;
        public void Update()
        {
            time += Time.deltaTime;
            if(time > 3f)
            {
                time = 0;
                GameStatus.SetCurrentGameState(CurrentGameState.Dialog);
            }
        }

        public void SetDialog(Dialog dialog, System.Action action)
        {
            CharacterStop();
            currentDialog = dialog;
            dialogLength = currentDialog.talker.Count;
            currentTurn = 1;
            SetNextAction(action);
            SetDialog(0);

            var player = PlayerFSMManager.Instance;
            if(player != null)
            {
                player.isAttackOne = false;
                player.isAttackTwo = false;
                player.isAttackThree = false;
            }

            var voice = MC.Sound.MCSoundManager.Instance.objectSound.dialogVoice;
            voice.PlaySound(MC.Sound.MCSoundManager.Instance.gameObject, voice.voice[currentDialog.voice[0]]);
        }

        public void NextDialog()
        {
            if(currentTurn < dialogLength)
            {
                SetDialog(currentTurn);
                var sound = MC.Sound.MCSoundManager.Instance.objectSound.ui;
                sound.PlaySound(this.gameObject, sound.nextPage);

                currentTurn++;
            }
            else
            {
                EndDialog();
            }
        }

        public void SetDialog(int turn)
        {
            currentTalker = currentDialog.talker[turn];
            dialogUI.conversation.text = currentDialog.dialog[turn];
            dialogUI.rightStanding.sprite = standing[currentDialog.right[turn]];
            dialogUI.leftStanding.sprite = standing[currentDialog.left[turn]];
            dialogUI.textUI.sprite = text[currentDialog.text[turn]];

            var voice = MC.Sound.MCSoundManager.Instance.objectSound.dialogVoice;
            voice.PlaySound(MC.Sound.MCSoundManager.Instance.gameObject, voice.voice[currentDialog.voice[turn]]);
        }

        public void CharacterStop()
        {
            GameManager.Instance.CharacterControl = false;
            PlayerFSMManager.Instance.vertical = 0;
            PlayerFSMManager.Instance.horizontal = 0;
            PlayerFSMManager.Instance.SetState(PlayerState.IDLE);
        }

        public void EndDialog()
        {
            GameStatus.SetCurrentGameState(CurrentGameState.Wait);
            UserInterface.DialogSetActive(false);
            UserInterface.BlurSet(false);
            GameManager.Instance.AfterDialog();
            GameManager.Instance.CharacterControl = true;

            try
            {
                NextAction();
            }
            catch
            {

            }

            NextAction = null;
        }
    }


}