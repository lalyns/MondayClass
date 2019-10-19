using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.UI
{
    public class EnemyHPBar : MonoBehaviour
    {
        public HPBar hpBar;

        public void HpBarView()
        {
            if (PlayerFSMManager.Instance.LastHit() == null)
            {
                hpBar.gameObject.SetActive(false);
            }
            else
            {
                hpBar.gameObject.SetActive(true);
                UserInterface.Instance.HPChangeEffect(PlayerFSMManager.Instance.LastHit(), hpBar);
            }
        }

        public void AttackSupport()
        {
            hpBar.HitBackFun();
        }



    }
}