using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MC.UI;
using MC.Sound;

public class RedHatHitCollider : MonoBehaviour
{
    RedHatFSMManager redhat;

    public CapsuleCollider capsule;
    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
    }
    private void Start()
    {
        redhat = GetComponentInParent<RedHatFSMManager>();
    }

    private void FixedUpdate()
    {
        this.transform.position = redhat.transform.position;
    }

    public void OnHitForMonster(AttackType attackType)
    {

        if ((attackType == AttackType.ATTACK1
            || attackType == AttackType.ATTACK2
            || attackType == AttackType.ATTACK3)
            && ((int)redhat.CurrentAttackType & (int)attackType) != 0)
        {
            return;
        }

        if (redhat.CurrentState == RedHatState.DEAD) return;

        if (PlayerFSMManager.Instance.isNormal)
            PlayerEffects.Instance.basicNormal.ItemSetActive(redhat.hitLocation, "Effect");

        if (!PlayerFSMManager.Instance.isNormal)
            PlayerEffects.Instance.basicSpecial.ItemSetActive(redhat.hitLocation, "Effect");

        redhat.CurrentAttackType = attackType;
        int value = GameLib.TransformTypeToInt(attackType);
        PlayerStat playerStat = PlayerFSMManager.Instance.Stat;

        float damage = (playerStat.GetStr() * playerStat.dmgCoefficient[value] * 0.01f) - redhat.Stat.Defense;
        CharacterStat.ProcessDamage(playerStat, redhat.Stat, damage);

        var sound = GetComponentInParent<MonsterSound>().monsterSFX;
        sound.PlayMonsterSFX(this.gameObject, sound.attackSFX[value]);

        //SetKnockBack(playerStat, value);
        Invoke("AttackSupport", 0.5f);

        redhat.RigidBody.velocity = Vector3.zero;
        redhat.RigidBody.velocity = -PlayerFSMManager.Instance.Anim.transform.forward
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

        if (redhat.Stat.Hp > 0)
        {
            if (redhat.CurrentState == RedHatState.HIT) return;
            if (redhat.isNotChangeState) return;

            redhat.SetState(RedHatState.HIT);
        }
        else
        {
            StopAllCoroutines();
            redhat.SetDeadState();
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
            if (redhat.Stat.Hp > 0)
                OnHitForMonster(PlayerFSMManager.Instance.attackType);
        }

        if (other.transform.tag == "Ball")
        {
            if (PlayerFSMManager.Instance.isNormal)
                PlayerEffects.Instance.skill1Normal.ItemSetActive(redhat.hitLocation, "Effect");

            if (!PlayerFSMManager.Instance.isNormal)
                PlayerEffects.Instance.skill1Special.ItemSetActive(redhat.hitLocation, "Effect");

            //other.transform.gameObject.SetActive(false);

            if (redhat.Stat.Hp > 0)
            {
                OnHitForMonster(AttackType.SKILL1);
            }
        }
        if (other.transform.tag == "Skill2" && PlayerFSMManager.Instance.isSkill2)
        {
            StartCoroutine("Skill2Timer");

            redhat.SetState(RedHatState.HIT);
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
            CharacterStat.ProcessDamage(stat, redhat.Stat, 10);
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
    
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Skill2")
        {
            if (redhat.Stat.Hp > 0)
            {
                OnHitForMonster(AttackType.SKILL2);
            }
        }
    }

}
