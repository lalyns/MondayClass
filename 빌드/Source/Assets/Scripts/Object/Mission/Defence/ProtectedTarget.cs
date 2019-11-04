﻿using System.Collections;
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

        public SkinnedMeshRenderer smr;
        public MeshRenderer[] mr;
        List<Material> materials = new List<Material>();

        [System.NonSerialized] public int hp;
        public int damage;

        // Start is called before the first frame update
        void Start()
        {
            var sound = MCSoundManager.Instance.objectSound.objectSFX;
            sound.PlaySound(this.gameObject, sound.pillarActive);

            materials.AddRange(smr.materials);
            for (int i = 0; i < mr.Length; i++)
                materials.AddRange(mr[i].materials);
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
                StartCoroutine(GameLib.Blinking(materials, Color.red));
            }
        }
    }
}