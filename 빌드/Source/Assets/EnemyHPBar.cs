using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.UI
{
    public class EnemyHPBar : MonoBehaviour
    {
        public HPBar hpBar;
        public Text name;

        public void HpBarView()
        {
            if (PlayerFSMManager.Instance.LastHit() == null)
            {
                hpBar.gameObject.SetActive(false);
                name.gameObject.SetActive(false);
            }
            else
            {
                hpBar.gameObject.SetActive(true);
                name.gameObject.SetActive(true);
                UserInterface.Instance.HPChangeEffect(PlayerFSMManager.Instance.LastHit(), hpBar);
                name.text = SetName(PlayerFSMManager.Instance.LastHit());
            }
        }

        string SetName(CharacterStat stat)
        {
            switch (stat.monsterType)
            {
                case MonsterType.Mac:
                    return "맥";
                case MonsterType.RedHat:
                    return "레드 햇";
                case MonsterType.Tiber:
                    return "티버";
                case MonsterType.Length:
                    return "리리스";
                default:
                    return "AA";
            }
        }

        public void AttackSupport()
        {
            hpBar.HitBackFun();
        }



    }
}