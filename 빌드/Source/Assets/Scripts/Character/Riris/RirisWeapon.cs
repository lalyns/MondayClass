using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class RirisWeapon : MonoBehaviour
{
    public bool _Dameged = false;
    public RirisFSMManager riris;

    void Start()
    {
        riris = RirisFSMManager.Instance;
    }

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

                CharacterStat.ProcessDamage(riris.Stat, PlayerFSMManager.Instance.Stat, damage);
                var hitTarget = GameLib.SimpleDamageProcess(this.transform, 1f, "Player", riris.Stat, 0);
                PlayerFSMManager.Instance.SetState(PlayerState.HIT2);

                Invoke("AttackSupport", 0.5f);
                _Dameged = true;
            }
        }
        else if(riris.CurrentState == RirisState.PATTERNB)
        {
            if (other.transform.tag == "Player")
            {
                float damage = riris.Stat.damageCoefiiecient[2] * 0.01f *
                (riris.Stat.Str + riris.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
                - PlayerFSMManager.Instance.Stat.Defense;

                CharacterStat.ProcessDamage(riris.Stat, PlayerFSMManager.Instance.Stat, damage);
                var hitTarget = GameLib.SimpleDamageProcess(this.transform, 1f, "Player", riris.Stat, 0);
                PlayerFSMManager.Instance.SetState(PlayerState.HIT2);

                Invoke("AttackSupport", 0.5f);
                _Dameged = true;
            }
        }
        else if(riris.CurrentState == RirisState.ULTIMATE)
        {
            if (other.transform.tag == "Player")
            {
                float damage = riris.Stat.damageCoefiiecient[4] * 0.01f *
                (riris.Stat.Str + riris.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
                - PlayerFSMManager.Instance.Stat.Defense;

                CharacterStat.ProcessDamage(riris.Stat, PlayerFSMManager.Instance.Stat, damage);
                var hitTarget = GameLib.SimpleDamageProcess(this.transform, 1f, "Player", riris.Stat, 0);
                PlayerFSMManager.Instance.SetState(PlayerState.HIT2);

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
