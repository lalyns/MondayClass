using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField]
    InputHandler input;
    PlayerFSMManager player;
    public TrailRenderer trailRenderer;

    public ParticleSystem particle;

    private void Start()    
    {
        //input = InputHandler.instance;
        player = PlayerFSMManager.instance;
    }

    void hitCheck()
    {
        if (null != player)
        {
            //input.AttackCheck();
            player.AttackCheck();
            try
            {
                trailRenderer.gameObject.SetActive(true);
            }
            catch
            {

            }
        }
    }
    void hitCancel()
    {
        if (null != player)
        {
            //input.AttackCancel();
            player.AttackCancel();
            try
            {
                trailRenderer.gameObject.SetActive(false);
            }
            catch
            {

            }
        }
    }
    void skill3Check()
    {
        player.Skill3Attack();
    }
    void skill3Cancel()
    {
        player.Skill3Cancel();
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
