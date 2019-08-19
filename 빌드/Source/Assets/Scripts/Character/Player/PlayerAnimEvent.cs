using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField]
    InputHandler input;

    public TrailRenderer trailRenderer;

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
}
