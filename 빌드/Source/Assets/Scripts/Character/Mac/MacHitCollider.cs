using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MC.UI;
using MC.Sound;

public class MacHitCollider : MonoBehaviour
{
    MacFSMManager mac;

    public CapsuleCollider capsule;
    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
    }
    private void Start()
    {
        mac = GetComponentInParent<MacFSMManager>();
    }
    private void FixedUpdate()
    {
       // this.transform.position = mac.transform.position;
    }

    public void OnHitForMonster(AttackType attackType)
    {
        if ((attackType == AttackType.ATTACK1
            || attackType == AttackType.ATTACK2
            || attackType == AttackType.ATTACK3)
            && ((int)mac.CurrentAttackType & (int)attackType) != 0)
        {
            return;
        }

        if (mac.CurrentState == MacState.DEAD) return;

        if (PlayerFSMManager.Instance.isNormal)
            PlayerEffects.Instance.basicNormal.ItemSetActive(mac.hitLocation, "Effect");

        if (!PlayerFSMManager.Instance.isNormal)
            PlayerEffects.Instance.basicSpecial.ItemSetActive(mac.hitLocation, "Effect");

        mac.CurrentAttackType = attackType;
        int value = GameLib.TransformTypeToInt(attackType);

        PlayerStat playerStat = PlayerFSMManager.Instance.Stat;

        float damage = (playerStat.GetStr() * playerStat.dmgCoefficient[value] * 0.01f);
        CharacterStat.ProcessDamage(playerStat, mac.Stat, damage);

        var sound = GetComponentInParent<MonsterSound>().monsterSFX;
        sound.PlayMonsterSFX(this.gameObject, sound.attackSFX[value]);


        //SetKnockBack(playerStat, value);
        Invoke("AttackSupport", 0.5f);

        mac.RigidBody.velocity = Vector3.zero;
        mac.RigidBody.velocity = -PlayerFSMManager.Instance.Anim.transform.forward
            * PlayerFSMManager.Instance.Stat.KnockBackPower;

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
        //if (attackType == AttackType.SKILL3)
        //StartCoroutine(Shake.instance.ShakeCamera(0.1f, 0.08f, 0.01f));

        if (mac.Stat.Hp > 0)
        {
            if (mac.CurrentState == MacState.HIT) return;

            mac.SetState(MacState.HIT);
            //플레이어 쳐다본 후
            //transform.localEulerAngles = Vector3.zero;
            //transform.LookAt(PlayerFSMManager.Instance.Anim.transform);
            // 뒤로 밀림
            //transform.Translate(Vector3.back * 20f * Time.smoothDeltaTime, Space.Self);
            //플레이어피버게이지증가?
            //PlayerFSMManager.instance.FeverGauge++;
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

    //public void SetKnockBack(PlayerStat stat,int attackType)
    //{
    //    KnockBackFlag = stat.KnockBackFlag[attackType];
    //    KnockBackDuration = stat.KnockBackDuration[attackType];
    //    KnockBackPower = stat.KnockBackPower[attackType];
    //    KnockBackDelay = stat.KnockBackDelay[attackType];
    //}

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
