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
        public Text text;
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
        public DialogUI[] dialogs;

        [HideInInspector] public DialogUI currentDialogUI;

        public TalkerType currentTalker = TalkerType.None;

        public Dialog currentDialog;
        public int dialogLength = 0;
        int currentTurn = 1;

        public Sprite[] right;
        public Sprite[] left;
        public Sprite[] text;

        public void SetDialog(Dialog dialog)
        {
            currentDialog = dialog;
            dialogLength = currentDialog.talker.Count;
            SetDialog(dialog.talker[0], dialog.dialog[0]);
        }

        public void NextDialog()
        {
            if(currentTurn < dialogLength)
            {
                SetDialog(currentDialog.talker[currentTurn], currentDialog.dialog[currentTurn]);
                currentTurn++;
            }
            else
            {
                EndDialog();
            }
        }

        public void SetTalker(TalkerType talker)
        {
            switch (talker)
            {
                case TalkerType.Galaxy:
                    dialogs[0].gameObject.SetActive(true);
                    dialogs[1].gameObject.SetActive(false);
                    dialogs[2].gameObject.SetActive(false);
                    currentDialogUI = dialogs[0];
                    break;
                case TalkerType.Staff:
                    dialogs[0].gameObject.SetActive(false);
                    dialogs[1].gameObject.SetActive(true);
                    dialogs[2].gameObject.SetActive(false);
                    currentDialogUI = dialogs[1];
                    break;
                case TalkerType.Riris:
                    dialogs[0].gameObject.SetActive(false);
                    dialogs[1].gameObject.SetActive(false);
                    dialogs[2].gameObject.SetActive(true);
                    currentDialogUI = dialogs[2];
                    break;
                case TalkerType.None:
                    dialogs[0].gameObject.SetActive(false);
                    dialogs[1].gameObject.SetActive(false);
                    dialogs[2].gameObject.SetActive(false);

                    break;
                case TalkerType.End:
                    dialogs[0].gameObject.SetActive(false);
                    dialogs[1].gameObject.SetActive(false);
                    dialogs[2].gameObject.SetActive(false);
                    currentDialogUI = null;
                    break;

            }
        }

        public void SetDialog(TalkerType talker, string str)
        {
            if (currentTalker != talker)
            {
                SetTalker(talker);
                currentTalker = talker;
            }

            currentDialogUI.text.text = str;
        }

        public void EndDialog()
        {
            GameStatus.SetCurrentGameState(CurrentGameState.Wait);
            UserInterface.DialogSetActive(false);
        }
    }


}