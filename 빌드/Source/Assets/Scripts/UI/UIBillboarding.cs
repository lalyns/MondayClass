using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
    Transform cam;
    Transform Cam {
        get {
            if (cam == null)
                cam = GameObject.FindGameObjectWithTag("UICam").GetComponent<Camera>().transform;
            return cam;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Cam.rotation * Vector3.forward, Cam.rotation * Vector3.up);
    }
}
