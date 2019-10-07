using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Mission { 

public class ProtectedTarget : MonoBehaviour
    {
        public MissionC _Defence;
        [HideInInspector] public Collider Collider => GetComponent<Collider>();

        [System.NonSerialized] public int hp;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void SetProtectedHP()
        {
            hp = _Defence._ProtectedTargetHP;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "MonsterWeapon")
            {
                hp -= 10;
            }
        }
    }
}