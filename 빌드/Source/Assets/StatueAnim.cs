using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueAnim : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GetComponent<Animator>().Play("Play");
        }
    }
}
