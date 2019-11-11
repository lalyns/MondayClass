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
        Sphere.enabled = false;
    }
    private void OnEnable()
    {
        Sphere.enabled = false;
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
        if(_time>= 0.3f && _time <= 3f)
        {
            Sphere.enabled = true;
        }


        if (_time >= 2f)
        {
            _triggerTime = 0;
            Sphere.enabled = false;
            // gameObject.SetActive(false);
            //player.isSkill2CTime = true;
            //player.isSkill2 = false;


        }
        //if (_time >= 10f)
        //{
        //    player.isSkill2 = false;
        //}
        player.Skill2Reset();

    }

    void PlayerSound()
    {
        var sfx = player._Sound.sfx;
        sfx.PlayPlayerSFX(this.gameObject, sfx.skill2LastSFX);
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.transform.tag == "Monster" || other.transform.tag == "Boss")
        {
            _triggerTime += Time.deltaTime;

            if (_triggerTime <= 2f)
            {
                other.GetComponentInParent<FSMManager>().transform.position
                    = Vector3.MoveTowards(other.transform.position, transform.position, 2f * Time.deltaTime);
                other.GetComponentInParent<FSMManager>().transform.LookAt(transform.position);
            }
            
        }
    }

}
