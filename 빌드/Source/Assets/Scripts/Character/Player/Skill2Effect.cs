using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2Effect : MonoBehaviour
{

    public float _time;
    SphereCollider Sphere;
    PlayerFSMManager player;
    float _triggerTime = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        Sphere = GetComponent<SphereCollider>();
        player = PlayerFSMManager.Instance;

    }
    private void OnDisable()
    {
        Sphere.enabled = true;
        _time = 0;
    }

    // Update is called once per framea
    void Update()
    {
        _time += Time.deltaTime;


        if (_time <= 0.1f)
        {
            transform.position = player.Skill2_Parent.position;
        }



        if (_time >= 3f)
        {
            _triggerTime = 0;
            Sphere.enabled = false;
            // gameObject.SetActive(false);
            //player.isSkill2CTime = true;
        }
        //if (_time >= 10f)
        //{
        //    player.isSkill2 = false;
        //}
        player.Skill2UIReset();

    }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.transform.tag == "Monster")
        {
            _triggerTime += Time.deltaTime;

            if (_triggerTime <= 2f)
            {
                other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, 2f * Time.deltaTime);
                other.transform.LookAt(transform.position);
            }
            
        }
    }

}
