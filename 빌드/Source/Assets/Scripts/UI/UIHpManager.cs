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

    public PlayerStat _playerStat;

    private void Awake()
    {
        _playerStat = PlayerFSMManager.Instance.Stat;
    }
    private void Update()
    {
        this.transform.localPosition =
            new Vector3(-321 + 321 * (_playerStat.Hp / _playerStat.MaxHp), -1, 0);
    }
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
