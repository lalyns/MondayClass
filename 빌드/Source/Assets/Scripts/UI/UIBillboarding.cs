using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
    Transform cam;
    Transform Cam {
        get {
            if (cam == null)
                if (!billboardingType)
                {
                    cam = GameObject.FindGameObjectWithTag("UICam").GetComponent<Camera>().transform;
                }
                else
                {
                    cam = Camera.main.transform;
                }
            return cam;
        }
    }

    public bool billboardingType;

    public float Offset = 100f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        //var forwardVec = Cam.transform.forward;
        //forwardVec.Normalize();

        //this.transform.position = Cam.transform.position + (forwardVec * (Cam.GetComponent<Camera>().nearClipPlane + this.Offset));

        transform.LookAt(transform.position + Cam.rotation * Vector3.forward, Cam.rotation * Vector3.up);
    }
}
