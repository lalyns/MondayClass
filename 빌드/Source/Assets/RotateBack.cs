using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBack : MonoBehaviour
{
    public float _speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localRotation = Quaternion.Euler(_speed * Vector3.up * Time.time);
    }
}
