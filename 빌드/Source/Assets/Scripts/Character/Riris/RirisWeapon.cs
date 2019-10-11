using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class RirisWeapon : MonoBehaviour
{
    public bool _Dameged = false;
    public RirisFSMManager riris;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_Dameged)
            return;

        if (riris.CurrentState == RirisState.PATTERNA)
        {
            if (other.transform.tag == "Player")
            {
                float damage = riris.Stat.damageCoefiiecient[1] * 0.01f *
                (riris.Stat.Str + riris.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
                - PlayerFSMManager.Instance.Stat.Defense;

                var hitTarget = GameLib.SimpleDamageProcess(this.transform, 0.01f, "Player", riris.Stat, MonsterType.RedHat, damage);

                Invoke("AttackSupport", 0.5f);
                _Dameged = true;
            }
        }
        else
        {
            if (other.transform.tag == "Player")
            {
                float damage = riris.Stat.damageCoefiiecient[2] * 0.01f *
                (riris.Stat.Str + riris.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
                - PlayerFSMManager.Instance.Stat.Defense;

                var hitTarget = GameLib.SimpleDamageProcess(this.transform, 0.01f, "Player", riris.Stat, damage);

                Invoke("AttackSupport", 0.5f);
                _Dameged = true;
            }
        }
    }

    public void AttackSupport()
    {
        UserInterface.Instance.UIPlayer.hpBar.HitBackFun();
        _Dameged = false;
    }
}
