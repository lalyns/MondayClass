using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    [System.Serializable]
    public class UIMoveDashTutorial
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

        public Image space;
        public Sprite[] spaceSprites;
    }

    [System.Serializable]
    public class UIAttackTutorial
    {
        public GameObject gameObject;

        public Image Attack;
        public Sprite[] AttackSprites;
    }

    public UIMoveDashTutorial moveDash;
    public UIAttackTutorial attack;


}
