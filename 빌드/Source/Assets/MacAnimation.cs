using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacAnimation : MonoBehaviour
{
    public int type;

    public GameObject bulletEffect;
    public float _Time = 0;

    public float _MakeTime = 0.5f;
    public float _DestroyTime = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (type == 0)
        {
            _Time += Time.deltaTime;

            if (_Time > _MakeTime)
            {
                GameObject temp = Instantiate(bulletEffect, this.transform.position, Quaternion.identity);

                temp.GetComponent<Bullet>().LookAtTarget(GameObject.FindGameObjectWithTag("Player").transform);
                temp.GetComponent<Bullet>().dir = GameLib.DirectionToCharacter(this.GetComponentInParent<MacFSMState>()._manager.CC, this.GetComponentInParent<MacFSMState>()._manager.PlayerCapsule);
                temp.GetComponent<Bullet>()._Move = true;
                Destroy(this.gameObject);
            }
        }

        if(type == 2)
        {

            if (_Time > _DestroyTime)
            {
                _Time += Time.deltaTime;
                Destroy(this.gameObject);
            }
        }
    }
}
