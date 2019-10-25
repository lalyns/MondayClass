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

        public Image Skill1;
        public Sprite[] skill1Sprites;
        public Image Skill2;
        public Sprite[] skill2Sprites;
        public Image Skill3;
        public Sprite[] skill3Sprites;
        public Image Skill4;
        public Sprite[] skill4Sprites;
        public Image Special;
        public Sprite[] specialSprites;

        public Text attack;
        public Text skill1;
        public Text skill2;
        public Text skill3;
        public Text special;
    }


    public UIMoveDashTutorial moveDash;
    public UIAttackTutorial attack;


}
