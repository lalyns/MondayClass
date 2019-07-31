using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowedSlime : MonoBehaviour {
    [SerializeField]
    GameObject slime;
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(slime, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}