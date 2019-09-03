using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHatAnimEvent : MonoBehaviour
{
    public CapsuleCollider _WeaponCapsule;
    // Start is called before the first frame update
    void Start()
    {
        _WeaponCapsule.enabled = true;
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
}
