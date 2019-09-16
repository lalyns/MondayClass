using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2Effect : MonoBehaviour
{
    private List<GameObject> _monster = new List<GameObject>();

    public float _time;
    SphereCollider Sphere;
    PlayerFSMManager player;
    public float randX, randZ;
    // Start is called before the first frame update
    void Start()
    {
        //Sphere.enabled = false;
        Sphere = GetComponent<SphereCollider>();
        player = PlayerFSMManager.instance;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (_time <= 0.1f)
        {
            transform.position = player.Skill2_Parent.position;
        }

        _time += Time.deltaTime;
        
   

        randX = Random.Range(0, 3f);
        randZ = Random.Range(0, 3f);

        

        if(_time>= 3f)
        {
            Debug.Log("시간 지남");
            try
            {
              
            }
            catch
            {

            }
            player.isSkill2 = false;
            _time = 0;
            //Sphere.transform.gameObject.SetActive(false);
            //Destroy(this.gameObject);
            gameObject.SetActive(false);

        }

    }

}
