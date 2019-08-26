using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField]
    InputHandler input;

    public TrailRenderer trailRenderer;

    public ParticleSystem particle;

    private void Start()    
    {
        input = InputHandler.instance;        
    }

    void hitCheck()
    {
        if (null != input)
        {
            input.AttackCheck();
            trailRenderer.gameObject.SetActive(true);
        }
    }
    void hitCancel()
    {
        if (null != input)
        {
            input.AttackCancel();
            trailRenderer.gameObject.SetActive(false);
        }
    }

    public void PlayParticle()
    {
        //Debug.Log("시작");

        var main = particle.main;
        try
        {
            main.startLifetime = 1;
            particle.Play();
        }
        catch
        {

        }
    }

    public void StopParticle()
    {
        Debug.Log("끝");

        //particle.Stop();
        //particle.Clear();
    }
}
