using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.UI
{
    [System.Serializable]
    public class Dialog
    {
        public string[] dialog;
    }

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

        [HideInInspector] public DialogUI currentDialog;

        public TalkerType currentTalker = TalkerType.None;

        public void SetTalker(TalkerType talker)
        {
            switch (talker)
            {
                case TalkerType.Galaxy:
                    dialogs[0].gameObject.SetActive(true);
                    dialogs[1].gameObject.SetActive(false);
                    dialogs[2].gameObject.SetActive(false);
                    currentDialog = dialogs[0];
                    break;
                case TalkerType.Staff:
                    dialogs[0].gameObject.SetActive(false);
                    dialogs[1].gameObject.SetActive(true);
                    dialogs[2].gameObject.SetActive(false);
                    currentDialog = dialogs[1];
                    break;
                case TalkerType.Riris:
                    dialogs[0].gameObject.SetActive(false);
                    dialogs[1].gameObject.SetActive(false);
                    dialogs[2].gameObject.SetActive(true);
                    currentDialog = dialogs[2];
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
                    currentDialog = null;
                    GameStatus.currentGameState = CurrentGameState.Wait;
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

            currentDialog.text.text = str;
        }

    }


}