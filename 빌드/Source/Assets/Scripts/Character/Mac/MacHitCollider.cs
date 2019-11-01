using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MC.UI;
using MC.Sound;

public class MacHitCollider : MonoBehaviour
{
    MacFSMManager mac;
    public DamageDisplay display;

    public CapsuleCollider capsule;
    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
    }
    private void Start()
    {
        mac = GetComponentInParent<MacFSMManager>();
    }

    public void OnHitForMonster(AttackType attackType)
    {
        if ((attackType == AttackType.ATTACK1
            || attackType == AttackType.ATTACK2
            || attackType == AttackType.ATTACK3)
            && ((int)mac.currentAttackType & (int)attackType) != 0)
        {
            return;
        }

        if (mac.CurrentState == MacState.DEAD) return;

        if (PlayerFSMManager.Instance.isNormal)
            PlayerEffects.Instance.basicNormal.ItemSetActive(mac.hitLocation, "Effect");

        if (!PlayerFSMManager.Instance.isNormal)
            PlayerEffects.Instance.basicSpecial.ItemSetActive(mac.hitLocation, "Effect");

        mac.currentAttackType = attackType;
        int value = GameLib.TransformTypeToInt(attackType);

        PlayerStat playerStat = PlayerFSMManager.Instance.Stat;

        float damage = (playerStat.GetStr() * playerStat.dmgCoefficient[value] * 0.01f);
        StartCoroutine(display.DamageDisplaying(damage));
        CharacterStat.ProcessDamage(playerStat, mac.Stat, damage);

        if (MCSoundManager.SoundCall >= MCSoundManager.SoundSkill3Break)
        {
            var sound = GetComponentInParent<MonsterSound>().monsterSFX;
            sound.PlayMonsterSFX(this.gameObject, sound.attackSFX[value]);

            if (attackType == AttackType.SKILL3) MCSoundManager.SoundCall = 0;
        }

        Invoke("AttackSupport", 0.5f);


        if (attackType == AttackType.ATTACK1)
            StartCoroutine(Shake.instance.ShakeCamera(0.03f, 0.04f, 0.1f));
        if (attackType == AttackType.ATTACK2)
            StartCoroutine(Shake.instance.ShakeCamera(0.03f, 0.04f, 0.1f));
        if (attackType == AttackType.ATTACK3)
            StartCoroutine(Shake.instance.ShakeCamera(0.07f, 0.07f, 0.1f));
        if (attackType == AttackType.SKILL1)
            StartCoroutine(Shake.instance.ShakeCamera(0.2f, 0.1f, 0.1f));
        if (attackType == AttackType.SKILL2)
            StartCoroutine(Shake.instance.ShakeCamera(0.15f, 0.1f, 0.1f));

        if (mac.Stat.Hp > 0)
        {
            if (mac.CurrentState == MacState.HIT) return;

            mac.SetState(MacState.HIT);
        }
        else
        {


            mac.SetDeadState();
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
            if (mac.Stat.Hp > 0)
                OnHitForMonster(PlayerFSMManager.Instance.attackType);

            if (mac.CurrentState == MacState.ATTACK)
            {
            }
        }
        if (other.transform.tag == "Ball")
        {

            if (PlayerFSMManager.Instance.isNormal)
                PlayerEffects.Instance.skill1Normal.ItemSetActive(mac.hitLocation, "Effect");

            if (!PlayerFSMManager.Instance.isNormal)
                PlayerEffects.Instance.skill1Special.ItemSetActive(mac.hitLocation, "Effect");

            if (mac.Stat.Hp > 0)
            {
                OnHitForMonster(AttackType.SKILL1);
                //other.transform.gameObject.SetActive(false);
            }
        }

        if (other.transform.tag == "Skill2" && PlayerFSMManager.Instance.isSkill2)
        {
            StartCoroutine("Skill2Timer");

            mac.SetState(MacState.HIT);
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
            CharacterStat.ProcessDamage(stat, mac.Stat, 14);
            attackTime += Time.deltaTime;
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

    private void OnTriggerStay(Collider other)
    {

    }



    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Skill2")
        {
            if (mac.Stat.Hp > 0)
            {
                OnHitForMonster(AttackType.SKILL2);
            }
        }
    }
}
