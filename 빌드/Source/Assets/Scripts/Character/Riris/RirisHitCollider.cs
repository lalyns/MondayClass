﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.UI;
using MC.Sound;

public class RirisHitCollider : MonoBehaviour
{
    RirisFSMManager riris;

    //public Collider collider => GetComponent<Collider>();

    public CapsuleCollider capsule;
    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        riris = GetComponentInParent<RirisFSMManager>();
    }

    private void FixedUpdate()
    {
        this.transform.position = riris.transform.position;

        capsule.center = new Vector3(0, riris.Pevis.position.y, 0) + (Vector3.up * -0.915f);
    }

    public void OnHitForBoss(AttackType attackType)
    {
        if ((attackType == AttackType.ATTACK1
            || attackType == AttackType.ATTACK2
            || attackType == AttackType.ATTACK3)
            && ((int)riris.CurrentAttackType & (int)attackType) != 0)
        {
            return;
        }

        if (riris.CurrentState == RirisState.DEAD) return;

        if (PlayerFSMManager.Instance.isNormal)
            PlayerEffects.Instance.basicNormal.ItemSetActive(riris.Pevis, "Effect");

        if (!PlayerFSMManager.Instance.isNormal)
            PlayerEffects.Instance.basicSpecial.ItemSetActive(riris.Pevis, "Effect");

        riris.CurrentAttackType = attackType;
        int value = GameLib.TransformTypeToInt(attackType);
        PlayerStat playerStat = PlayerFSMManager.Instance.Stat;

        float damage = (playerStat.GetStr() * playerStat.dmgCoefficient[value] * 0.01f);
        if(damage <= 10.0f)
        {
            damage = 100.0f;
        }
        Debug.Log(damage);
        riris.Stat.TakeDamage(playerStat, damage);
        //CharacterStat.ProcessDamage(playerStat, riris.Stat, damage);


        var sound = GetComponentInParent<MonsterSound>().monsterSFX;
        sound.PlayMonsterSFX(this.gameObject, sound.attackSFX[value]);

        //SetKnockBack(playerStat, value);
        Invoke("AttackSupport", 0.5f);

        if (attackType == AttackType.ATTACK1)
            StartCoroutine(Shake.instance.ShakeCamera(0.03f, 0.04f, 0.1f));
        if (attackType == AttackType.ATTACK2)
            StartCoroutine(Shake.instance.ShakeCamera(0.03f, 0.04f, 0.1f));
        if (attackType == AttackType.ATTACK3)
            StartCoroutine(Shake.instance.ShakeCamera(0.07f, 0.07f, 0.1f));
        if (attackType == AttackType.SKILL1)
            StartCoroutine(Shake.instance.ShakeCamera(0.05f, 0.1f, 0.1f));
        if (attackType == AttackType.SKILL2)
            StartCoroutine(Shake.instance.ShakeCamera(0.15f, 0.1f, 0.1f));
        //if (attackType == AttackType.SKILL3)
        //    StartCoroutine(Shake.instance.ShakeCamera(0.01f, 0.01f, 0.01f));

        if (riris.Stat.Hp > 0)
        {

            if (damage > 0)
            {
                StartCoroutine(GameLib.Blinking(riris.materials, Color.white));
            }

        }
        else
        {
            riris.SetDeadState();
            StopAllCoroutines();
        }
    }

    public void AttackSupport()
    {
        CanvasInfo.Instance.enemyHP.hpBar.HitBackFun();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon" && !PlayerFSMManager.Instance.isSkill3)
        {
            if (riris.Stat.Hp > 0)
                OnHitForBoss(PlayerFSMManager.Instance.attackType);
        }


        if (other.transform.tag == "Ball")
        {
            if (PlayerFSMManager.Instance.isNormal)
                PlayerEffects.Instance.skill1Normal.ItemSetActive(riris.Pevis, "Effect");

            if (!PlayerFSMManager.Instance.isNormal)
                PlayerEffects.Instance.skill1Special.ItemSetActive(riris.Pevis, "Effect");

            other.transform.gameObject.SetActive(false);

            if (riris.Stat.Hp > 0)
            {
                //OnHit();
                OnHitForBoss(AttackType.SKILL1);
            }
        }
        if (other.transform.tag == "Skill2" && PlayerFSMManager.Instance.isSkill2)
        {
            StartCoroutine("Skill2Timer");
        }

        if (other.transform.tag == "Weapon" && PlayerFSMManager.Instance.isSkill3)
        {
            StartCoroutine("Skill3Timer");
        }
    }


    public IEnumerator Skill2Timer()
    {
        PlayerStat stat = PlayerFSMManager.Instance.Stat;
        float attackTime = 0.0f;
        while (attackTime < 0.3f)
        {

            //stats.TakeDamage(PlayerFSMManager.Instance.stats, 30);
            CharacterStat.ProcessDamage(stat, riris.Stat, 25);
            attackTime += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator Skill3Timer()
    {
        while (PlayerFSMManager.Instance.isSkill3)
        {
            OnHitForBoss(AttackType.SKILL3);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Skill2")
        {
            if (riris.Stat.Hp > 0)
            {
                OnHitForBoss(AttackType.SKILL2);
            }
        }
    }

}
