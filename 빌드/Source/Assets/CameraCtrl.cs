using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    public float followSpeed = 9;
    public float mouseSpeed = 2;
    public float controllerSpeed = 7;


    public Transform target;
    

    float turnSmoothing = .1f;
    public float minAngle = -35;
    public float maxAngle = 35;

    float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        transform.position = targetPosition;
    }
}
