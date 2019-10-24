using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUltEffect : MonoBehaviour
{
    [System.Serializable]
    public class SetEffect
    {
        public GameObject gameObject;
        public ParticleSystem[] particles;
        public Animator anim;

        public void PlayEffects()
        {
            gameObject.SetActive(true);
            for(int i=0; i<particles.Length; i++)
            {
                particles[i].Play();
            }
            anim.Play("play");
        }
    }


    [System.Serializable]
    public class ImpactEffect
    {
        public GameObject gameObject;
        public ParticleSystem[] particles;
        public Animator anim;

        public void PlayEffects()
        {
            gameObject.SetActive(true);
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
            anim.Play("play");
        }
    }

    public SetEffect setEffect;
    public ImpactEffect impactEffect;

}
