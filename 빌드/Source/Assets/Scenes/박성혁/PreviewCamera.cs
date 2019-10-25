using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewCamera : MonoBehaviour
{
    // Start is called before the first frame update

    float r_x;
    float r_y;
    float _v;
    float _h;
    public float speed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        r_x = Input.GetAxis("Mouse X");
        r_y = Input.GetAxis("Mouse Y");
        _v = Input.GetAxis("Vertical");
        _h = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.right * _h * speed * Time.deltaTime);
    }
}
