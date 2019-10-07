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

    public class UIDialog : MonoBehaviour
    {
        public Dialog dialog;

        public DialogUI player;
        public DialogUI boss;

        public DialogUI currentLog;

        public int currentLogNum;

        public void Start()
        {
            currentLog = player;
        }

        public void SetDialogText(DialogUI value)
        {
            currentLog = value;
        }

        public void SetDialog(string text)
        {
            currentLog.text.text = text;
            currentLogNum++;
        }

        public int CurrentDialogLength()
        {
            return dialog.dialog.Length;
        }
    }


}