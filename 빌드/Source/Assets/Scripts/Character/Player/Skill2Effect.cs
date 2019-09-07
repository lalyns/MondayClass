using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2Effect : MonoBehaviour
{
    private List<GameObject> _monster = new List<GameObject>();

    public float _time;
    BoxCollider box;

    public float randX, randZ;
    public bool isAttack;
    // Start is called before the first frame update
    void Start()
    {
        //box.enabled = false;
        isAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttack)
            return;

        _time += Time.deltaTime;

   

        randX = Random.Range(0, 3f);
        randZ = Random.Range(0, 3f);

        

        if(_time>= 1.5f && _time < 1.6f)
        {
            Debug.Log("시간 지남");
            try
            {
                // 몬스터 가져온 후
                _monster.AddRange(GameObject.FindGameObjectsWithTag("Monster"));


                if (_monster.Count == 0)
                    return;

                for (int i = 0; i < _monster.Count; i++)
                {
                    if (Vector3.Distance(transform.position, _monster[i].transform.position) >= 3f)
                    {
                        _monster[i].transform.position = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);
                    }
                }
            }
            catch
            {

            }
            _time = 0;
            isAttack = false;
            //box.enabled = true;
        }
      
    }
    private void OnTriggerStay(Collider other)
    {
    }
}
