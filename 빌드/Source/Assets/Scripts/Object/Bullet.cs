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

    public GameObject _AttackEffect;

    // Update is called once per frame
    void Update()
    {
        //if (_Size < _MaxSize)
        //{
        //    _Size += (float)_IncreaseSize / 100f * Time.deltaTime;
        //}
        //else
        //{
        //    _Size = _MaxSize;
        //}

        //transform.localScale = Vector3.one * _Size;

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

    public void LookAtTarget(Transform target)
    {
        this.transform.LookAt(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GameObject attack = Instantiate(_AttackEffect, this.transform.position, Quaternion.identity);
            attack.transform.LookAt(other.transform);
            Destroy(this.gameObject);
        }

        if (other.transform.tag == "")
        {

        }
    }
}
