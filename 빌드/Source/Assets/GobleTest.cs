using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobleTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            ObjectManager.ReturnPool(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Weapon")
        {
            ObjectManager.ReturnPool(gameObject);
        }
    }
}
