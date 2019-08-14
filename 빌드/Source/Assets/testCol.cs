using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("고블린 공격성공");
            this.transform.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
