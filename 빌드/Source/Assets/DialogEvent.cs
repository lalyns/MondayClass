using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.UI
{
    [System.Serializable]
    public class Dialog
    {
        public List<TalkerType> talker;
        public List<string> dialog;
        public List<int> standingImageIndex;
        public List<int> dialogImageIndex;

        public Dialog()
        {
            talker = new List<TalkerType>();
            dialog = new List<string>();
        }
    }

    public class DialogEvent : MonoBehaviour
    {
        List<Dictionary<string, object>> dialogList = new List<Dictionary<string, object>>();

        public Dialog[] dialogs;

        void Start()
        {
            dialogList = CSVReader.Read("TextList");

            dialogs = new Dialog[8];

            for (int i = 0; i < dialogs.Length; i++)
                dialogs[i] = new Dialog();

            EventClassification();
        }

        void EventClassification()
        {
            foreach (var value in dialogList)
            {
                var id = (int)value["#ID"];
                var type = (string)value["Target"];
                var dialog = (string)value["Dialog"];
                //var standingIndex = (int)value[]

                dialogs[id - 1].talker.Add(TypeDefine(type));
                dialogs[id - 1].dialog.Add(dialog);
            }
        }

        TalkerType TypeDefine(string str)
        {
            switch (str)
            {
                case "마법 지팡이":
                    return TalkerType.Staff;
                case "은하":
                    return TalkerType.Galaxy;
                case "리리스":
                    return TalkerType.Riris;
                case "None":
                    return TalkerType.None;
                default:
                    return TalkerType.End;
            }
        }



    }
}