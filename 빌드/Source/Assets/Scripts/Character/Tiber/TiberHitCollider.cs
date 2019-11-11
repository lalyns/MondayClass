using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MC.UI;
using MC.Sound;


public class TiberHitCollider : MonoBehaviour
{
    TiberFSMManager tiber;
    public DamageDisplay display;

    public CapsuleCollider capsule;
    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        tiber = GetComponentInParent<TiberFSMManager>();
    }

    private void FixedUpdate()
    {
        this.transform.position = tiber.transform.position;
    }

    public void OnHitForMonster(AttackType attackType)
    {
        if ((attackType == AttackType.ATTACK1
            || attackType == AttackType.ATTACK2
            || attackType == AttackType.ATTACK3)
            && ((int)tiber.CurrentAttackType & (int)attackType) != 0)
        {
            return;
        }

        if (tiber.CurrentState == TiberState.DEAD) return;

        if (PlayerFSMManager.Instance.isNormal)
            PlayerEffects.Instance.basicNormal.ItemSetActive(tiber.hitLocation, "Effect");

        if (!PlayerFSMManager.Instance.isNormal)
            PlayerEffects.Instance.basicSpecial.ItemSetActive(tiber.hitLocation, "Effect");

        tiber.CurrentAttackType = attackType;
        int value = GameLib.TransformTypeToInt(attackType);
        PlayerStat playerStat = PlayerFSMManager.Instance.Stat;

        float damage = (playerStat.GetStr() * playerStat.dmgCoefficient[value] * 0.01f);
        StartCoroutine(display.DamageDisplaying(damage));
        CharacterStat.ProcessDamage(playerStat, tiber.Stat, damage);

        if (MCSoundManager.SoundCall >= MCSoundManager.SoundSkill3Break)
        {
            var sound = GetComponentInParent<MonsterSound>().monsterSFX;
            sound.PlayMonsterSFX(this.gameObject, sound.attackSFX[value]);

            if (attackType == AttackType.SKILL3) MCSoundManager.SoundCall = 0;
        }

        //SetKnockBack(playerStat, value);
        Invoke("AttackSupport", 0.5f);

        tiber.RigidBody.velocity = Vector3.zero;
        tiber.RigidBody.velocity = -PlayerFSMManager.Instance.Anim.transform.forward
            * PlayerFSMManager.Instance.Stat.KnockBackPower;

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

        if (tiber.Stat.Hp > 0)
        {
            if (tiber.CurrentState == TiberState.HIT) return;

            tiber.SetState(TiberState.HIT);

            //플레이어 쳐다본 후
            try
            {
                transform.localEulerAngles = Vector3.zero;
                transform.LookAt(PlayerFSMManager.Instance.Anim.transform);
                //플레이어피버게이지증가?
            }
            catch
            {

            }
        }
        else
        {
            tiber.SetDeadState();
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
            if (tiber.Stat.Hp > 0)
                OnHitForMonster(PlayerFSMManager.Instance.attackType);
        }

        if (other.transform.tag == "Ball")
        {
            if (PlayerFSMManager.Instance.isNormal)
                PlayerEffects.Instance.skill1Normal.ItemSetActive(tiber.hitLocation, "Effect");

            if (!PlayerFSMManager.Instance.isNormal)
                PlayerEffects.Instance.skill1Special.ItemSetActive(tiber.hitLocation, "Effect");

            //other.transform.gameObject.SetActive(false);

            if (tiber.Stat.Hp > 0)
            {
                //OnHit();
                OnHitForMonster(AttackType.SKILL1);
            }
        }
        if (other.transform.tag == "Skill2" && PlayerFSMManager.Instance.isSkill2AttackTime)
        {
            StartCoroutine("Skill2Timer");

            tiber.SetState(TiberState.HIT);
        }
        if (other.transform.tag == "Weapon" && PlayerFSMManager.Instance.isSkill3)
        {
            StartCoroutine("Skill3Timer");
        }
    }


    public IEnumerator Skill2Timer()
    {

        PlayerStat stat = PlayerFSMManager.Instance.Stat;
        while (PlayerFSMManager.Instance.isSkill2AttackTime)
        {
            PlayerStat playerStat = PlayerFSMManager.Instance.Stat;
            float damage = (playerStat.GetStr() * playerStat.dmgCoefficient[4] * 0.002f);
            StartCoroutine(display.DamageDisplaying(damage));
            CharacterStat.ProcessDamage(playerStat, tiber.Stat, damage);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator Skill3Timer()
    {
        while (PlayerFSMManager.Instance.isSkill3)
        {
            OnHitForMonster(AttackType.SKILL3);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool isOne = false;
    void isOneSet()
    {
        isOne = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Skill2" && !isOne)
        {
            if (tiber.Stat.Hp > 0)
            {
                PlayerStat playerStat = PlayerFSMManager.Instance.Stat;
                float damage = (playerStat.GetStr() * playerStat.dmgCoefficient[4] * 0.01f);
                StartCoroutine(display.DamageDisplaying(damage));
                CharacterStat.ProcessDamage(playerStat, tiber.Stat, damage);
            }
            isOne = true;
            Invoke("isOneSet", 1f);
        }
    }

}
