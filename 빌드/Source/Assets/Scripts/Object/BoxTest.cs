using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTest : MonoBehaviour
{
    public InputHandler input;
    // Start is called before the first frame update
    void Start()
    {
        input = InputHandler.singleton;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Weapon")
        {
            if(input.isAttackOne || input.isAttackTwo || input.isAttackThree)
            Debug.Log("체크");
        }
    }
}
