using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image BackGround;
    public Image CurrentFillGround;
    public Image LaterFillGround;

    public bool backHitMove = false;

    public void HitBackFun()
    {
        backHitMove = true;
    }
}
