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
        }

        public void SetProtectedHP()
        {
            hp = _Defence._ProtectedTargetHP;
        }

        public void OnTriggerEnter(Collider other)
        {
            Debug.Log("HP 감소!");
            if (other.transform.tag == "MonsterWeapon")
            {
                Debug.Log("HP 감소!");
                hp -= damage;
            }
        }
    }
}