using System.Collections;
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

        public void SetDialog(Dialog dialog)
        {
            currentDialog = dialog;
            dialogLength = currentDialog.talker.Count;
            currentTurn = 1;
            SetDialog(0);
        }

        public void NextDialog()
        {
            if(currentTurn < dialogLength)
            {
                SetDialog(currentTurn);
                currentTurn++;
            }
            else
            {
                EndDialog();
            }
        }

        //public void SetTalker(TalkerType talker)
        //{
        //    switch (talker)
        //    {
        //        case TalkerType.Galaxy:
        //            dialogs[0].gameObject.SetActive(true);
        //            dialogs[1].gameObject.SetActive(false);
        //            dialogs[2].gameObject.SetActive(false);
        //            currentDialogUI = dialogs[0];
        //            break;
        //        case TalkerType.Staff:
        //            dialogs[0].gameObject.SetActive(false);
        //            dialogs[1].gameObject.SetActive(true);
        //            dialogs[2].gameObject.SetActive(false);
        //            currentDialogUI = dialogs[1];
        //            break;
        //        case TalkerType.Riris:
        //            dialogs[0].gameObject.SetActive(false);
        //            dialogs[1].gameObject.SetActive(false);
        //            dialogs[2].gameObject.SetActive(true);
        //            currentDialogUI = dialogs[2];
        //            break;
        //        case TalkerType.None:
        //            dialogs[0].gameObject.SetActive(false);
        //            dialogs[1].gameObject.SetActive(false);
        //            dialogs[2].gameObject.SetActive(false);

        //            break;

        //    }
        //}

        public void SetDialog(int turn)
        {
            currentTalker = currentDialog.talker[turn];
            dialogUI.conversation.text = currentDialog.dialog[turn];
            dialogUI.rightStanding.sprite = standing[currentDialog.right[turn]];
            dialogUI.leftStanding.sprite = standing[currentDialog.left[turn]];
            dialogUI.textUI.sprite = text[currentDialog.text[turn]];
        }

        public void EndDialog()
        {
            GameStatus.SetCurrentGameState(CurrentGameState.Wait);
            UserInterface.DialogSetActive(false);
            GameManager.Instance.AfterDialog();
            GameManager.Instance.CharacterControl = true;
        }
    }


}