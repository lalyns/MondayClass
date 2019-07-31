using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _Speed = 5f;
    public float _DestroyTime = 5f;
    [System.NonSerialized]public float _Time;

    [System.NonSerialized] public Vector3 dir;

    [System.NonSerialized] public bool _Move = false;

    // Update is called once per frame
    void Update()
    {
        if (_Move)
        {
            _Time += Time.deltaTime;
            if (_Time >= _DestroyTime)
            {
                Destroy(this.gameObject);
            }

            transform.position += dir * _Speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
