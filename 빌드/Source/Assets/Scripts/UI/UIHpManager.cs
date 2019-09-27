using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHpManager : MonoBehaviour {

    [SerializeField]
    Image enemyHp, myHp;
    [SerializeField]
    Text targetName;
    [SerializeField]
    GameObject enemyHpObject;

    public void ShowEnemyHpBar(bool isOn)
    {
        enemyHpObject.SetActive(isOn);
    }
    public void SetTargetName(string name)
    {
        targetName.text = name;
    }
    public void SetMyHpRate(float rate)
    {
        myHp.fillAmount = rate;
    }
    public void SetEnemyRate(float rate)
    {
        enemyHp.fillAmount = rate;
    }


}
