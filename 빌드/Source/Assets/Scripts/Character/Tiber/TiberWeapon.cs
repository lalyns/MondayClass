using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class TiberWeapon : MonoBehaviour
{
    public bool _Dameged = false;
    public TiberFSMManager Tiber;

    void Awake()
    {
        Tiber = GetComponentInParent<TiberFSMManager>();
    }

    private void OnDisable()
    {
        _Dameged = false;
    }

    //캐릭터주변블러 = 맞았을때? 공격할때? 스킬쓸떄? 연구해야징
    private void OnTriggerEnter(Collider other)
    {
        if (_Dameged)
            return;
        if (Tiber.CurrentState == TiberState.ATTACK2)
        {
            if (other.transform.tag == "Player")
            {
                //float damage = Tiber.Stat.damageCoefiiecient[0] * 0.01f *
                //(Tiber.Stat.Str + Tiber.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
                //- PlayerFSMManager.Instance.Stat.Defense;

                var hitTarget = GameLib.SimpleDamageProcess(this.transform, 0.01f, "Player", Tiber.Stat, 30);
                Invoke("AttackSupport", 0.5f);
                _Dameged = true;
                PlayerFSMManager.Instance.SetState(PlayerState.HIT2);
                Debug.Log("티버 내려짝기.");
            }
        }
        if(Tiber.CurrentState == TiberState.ATTACK3)
        {
            if (other.transform.tag == "Player")
            {
                //float damage = Tiber.Stat.damageCoefiiecient[1] * 0.01f *
                //(Tiber.Stat.Str + Tiber.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
                //- PlayerFSMManager.Instance.Stat.Defense;

                var hitTarget = GameLib.SimpleDamageProcess(this.transform, 0.01f, "Player", Tiber.Stat, 30);
                Invoke("AttackSupport", 0.5f);
                _Dameged = true;
            }
        }

    }


    public void AttackSupport()
    {
        Debug.Log("attackCall");
        UserInterface.Instance.UIPlayer.hpBar.HitBackFun();
    }
}
