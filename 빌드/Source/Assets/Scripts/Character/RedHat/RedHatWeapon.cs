﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;

public class RedHatWeapon : MonoBehaviour
{
    public bool _Dameged = false;
    public RedHatFSMManager redHat;

    void Awake()
    {
        redHat = GetComponentInParent<RedHatFSMManager>();
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
        if (redHat.CurrentState == RedHatState.ATTACK)
        {
            if (other.transform.tag == "Player")
            {
                float damage = redHat.Stat.damageCoefiiecient[0] * 0.01f *
                (redHat.Stat.Str + redHat.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
                - PlayerFSMManager.Instance.Stat.Defense;

                var hitTarget = GameLib.SimpleDamageProcess(this.transform, 0.01f, "Player", redHat.Stat, MonsterType.RedHat, damage);


                Transform effectTransform =
                    MonsterEffects.Instance.redHatAttackEffect.
                    ItemSetActive(PlayerFSMManager.Instance.Anim.transform, "Effect");

                effectTransform.rotation = redHat.transform.rotation;

                Invoke("AttackSupport", 0.5f);
                _Dameged = true;
            }
        }
        else
        {
            if (other.transform.tag == "Player")
            {
                float damage = redHat.Stat.damageCoefiiecient[1] * 0.01f *
                (redHat.Stat.Str + redHat.Stat.addStrPerRound * GameStatus.Instance.StageLevel)
                - PlayerFSMManager.Instance.Stat.Defense;

                var hitTarget = GameLib.SimpleDamageProcess(this.transform, 0.01f, "Player", redHat.Stat, damage);

                Transform effectTransform =
                    MonsterEffects.Instance.redHatSkillEffect2.
                    ItemSetActive(PlayerFSMManager.Instance.Anim.transform, "Effect");

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
