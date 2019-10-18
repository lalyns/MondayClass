using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    [System.Serializable]
    public class UIMoveTutorial
    {
        public GameObject gameObject;
        public Image W;
        public Image A;
        public Image S;
        public Image D;

        public Sprite[] WSprites;
        public Sprite[] ASprites;
        public Sprite[] SSprites;
        public Sprite[] DSprites;
    }

    [System.Serializable]
    public class UIDashTutorial
    {
        public GameObject gameObject;
        public Image space;
        public Sprite[] spaceSprites;
    }

    public UIMoveTutorial move;
    public UIDashTutorial dash;


}
