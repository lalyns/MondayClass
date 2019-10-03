using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatAnimEvent : MonoBehaviour
{
    public RedHatATTACK _attackCp;
    public RedHatHIT _hitCp;
    public RedHatDEAD _deadCp;

    private void Awake()
    {
        _attackCp = GetComponentInParent<RedHatATTACK>();
    }

    public CapsuleCollider _WeaponCapsule;
    // Start is called before the first frame update
    void Start()
    {
        //_WeaponCapsule.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnWeaponTrigger()
    {
        _WeaponCapsule.gameObject.SetActive(true);
    }

    void DisableWeaponTrigger()
    {
        _WeaponCapsule.gameObject.SetActive(false);
    }

    void HitCheck()
    {
        if (null != _attackCp)
            _attackCp.AttackCheck();
    }

    public void HitEnd()
    {
        _hitCp.HitEnd();
    }

    public void NotifyDead()
    {
        _deadCp.DeadHelper();
    }
}
