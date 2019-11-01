using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Sound;

namespace MC.Mission { 

public class ProtectedTarget : MonoBehaviour
    {
        public MissionC _Defence;
        [HideInInspector] public Collider Collider => GetComponent<Collider>();

        public Animator anim;
        public GameObject activeEffect;
        public GameObject destroyEffect;

        [System.NonSerialized] public int hp;
        public int damage;

        // Start is called before the first frame update
        void Start()
        {
            var sound = MCSoundManager.Instance.objectSound.objectSFX;
            sound.PlaySound(this.gameObject, sound.pillarActive);
        }

        public void DestroyPillar()
        {
            var sound = MCSoundManager.Instance.objectSound.objectSFX;
            sound.PlaySound(this.gameObject, sound.pillarDestroy);
            activeEffect.SetActive(false);
            GetComponentInChildren<MeshRenderer>().enabled = false;
            Instantiate(destroyEffect, this.transform.position + Vector3.up * 1.5f, Quaternion.identity);
        }

        public void SetProtectedHP()
        {
            hp = _Defence._ProtectedTargetHP;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "MonsterWeapon")
            {
                hp -= damage;
            }
        }
    }
}