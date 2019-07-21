using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField]
    InputHandler input;
    private void Start()    
    {
        input = InputHandler.singleton;        
    }

    void hitCheck()
    {
        if(null!=input)
            input.AttackCheck();
    }
}
