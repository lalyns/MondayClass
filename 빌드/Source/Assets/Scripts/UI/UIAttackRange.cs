using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAttackRange : MonoBehaviour
{
    public Transform target;
    bool targetSet = false;

    public float _Time = 0;
    public float _DashReadyTime = 0.5f;
    public float _DashTime = 2.1f;
    [Range(0.01f, 1.00f)] public float rotateSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetTarget(Collider targetCol)
    {
        target = targetCol.transform;
        Vector3 pos = target.position;
        pos.y = 0.1f;
        transform.LookAt(PlayerFSMManager.GetLookTargetPos(this.transform));
        targetSet = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetSet)
        {
            return;
        }

        _Time += Time.deltaTime;
        if (_Time < _DashReadyTime)
        {
            //EffectRotate();
        }

        if (_Time > _DashReadyTime && _Time < _DashReadyTime + _DashTime)
        {
            EffectEnd();
        }
    }

    public void EffectRotate()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;

        // Quaternion rotation = Quaternion.Slerp(transform.rotation, )
        // transform.eulerAngles = rotation;
        Vector3 look = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(dir), rotateSpeed).eulerAngles;
        look.z = 0;

        transform.eulerAngles = look;
    }

    public void EffectEnd()
    {
        _Time = 0;
        targetSet = false;
        gameObject.SetActive(false);
    }
}
