using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public bool lockon;
    public float followSpeed = 9;
    public float mouseSpeed = 2;
    public float controllerSpeed = 7;

    public Transform target;

    public float dist = 10.0f;
    public float height = 5.0f;
    public float dampRotate = 5.0f;



    [HideInInspector]
    public Transform pivot;
    [HideInInspector]
    public Transform camTrans;

    float turnSmoothing = .1f;
    public float minAngle = -5;
    public float maxAngle = 5;

    float smoothX;
    float smoothY;
    float smoothXvelocity;
    float smoothYvelocity;
    public float lookAngle;
    public float tiltAngle;

    float delta;

    public Camera cams;



    public void Init(Transform t)
    {
        //target = t;
        target = GameObject.Find("PC_Rig").GetComponent<Transform>();

        camTrans = Camera.main.transform;
        pivot = camTrans.parent;

        cams = gameObject.GetComponentInChildren<Camera>();
    }

    public void camInit(Transform t)
    {
        transform.position = new Vector3(t.position.x, t.position.y + 2.36f, t.position.z);
        transform.rotation = t.rotation;
        lookAngle = t.eulerAngles.y;

    }

    public bool isKey;
    public void Tick(float d)
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        float targetSpeed = mouseSpeed;

        FollowTarget(d);
        HandleRotations(d, v, h, targetSpeed);

    }


    void FollowTarget(float d)
    {
        float speed = d * followSpeed;
        Vector3 targetPosition = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y + 2.36f, target.position.z), speed);
        transform.position = targetPosition;
    }

    void HandleRotations(float d, float v, float h, float targetSpeed)
    {

        smoothX = h;
        smoothY = v;

        lookAngle += smoothX * targetSpeed;
        transform.rotation = Quaternion.Euler(0, lookAngle, 0);

        tiltAngle -= smoothY * targetSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, -50, 50);//minAngle, maxAngle);
                                                    //pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
    }

    public static CameraManager singleton;
    void Awake()
    {
        singleton = this;
    }
}
