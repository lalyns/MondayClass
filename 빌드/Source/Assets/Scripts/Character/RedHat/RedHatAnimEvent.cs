using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatAnimEvent : MonoBehaviour
{
    public RedHatATTACK _attackCp;
    public RedHatHIT _hitCp;
    public RedHatDEAD _deadCp;

    RedHatFSMManager _Manager;
    RedHatFSMManager FSMManager {
        get {
            if (_Manager == null)
            {
                _Manager = this.GetComponentInParent<RedHatFSMManager>();
            }

            return _Manager;
        }
    }

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
        _WeaponCapsule.enabled = true;
    }

    void DisableWeaponTrigger()
    {
        _WeaponCapsule.enabled = false;
    }

    public void PopupOver()
    {
        FSMManager.SetState(RedHatState.CHASE);
    }

    void HitCheck()
    {
        //if (null != _attackCp)
            //_attackCp.AttackCheck();
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
