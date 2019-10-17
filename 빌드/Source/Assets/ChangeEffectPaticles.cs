using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEffectPaticles : MonoBehaviour
{
    public ParticleSystem bigStar;
    public ParticleSystem smallStar;
    public ParticleSystem[] kira;
    public ParticleSystem[] kirakira;
    public ParticleSystem fire;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KiraPlay()
    {
        kira[0].Play();
        kira[1].Play();
    }

    public void KiraKiraPlay()
    {
        kirakira[0].Play();
        kirakira[1].Play();
    }

    public void BigStarPlay()
    {
        bigStar.gameObject.SetActive(true);
        bigStar.Play();
    }

    public void SmallStarPlay()
    {
        smallStar.gameObject.SetActive(true);
        smallStar.Play();
    }

    public void FirePlay()
    {
        fire.gameObject.SetActive(true);
        fire.Play();
    }
}
