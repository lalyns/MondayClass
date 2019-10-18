using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCBall : MonoBehaviour
{
    PlayerFSMManager player;
    SphereCollider collder;
    List<GameObject> _monster = new List<GameObject>();
    public bool isOne = false;
    bool isEnter = false;
    public int RandomShoot, RandomShoot2;

    void Start()
    {
        player = PlayerFSMManager.Instance;
        collder = GetComponent<SphereCollider>();
    }
    void Update()
    {
        if (!isOne)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(_monster[RandomShoot].transform.position.x, 
                _monster[RandomShoot].transform.position.y + 1f,
                _monster[RandomShoot].transform.position.z),
                30f * Time.deltaTime);
        }
        if (isEnter && player.isSkill1Upgrade)
        {
            transform.position = Vector3.MoveTowards(transform.position,
              new Vector3(_monster[RandomShoot2].transform.position.x,
              _monster[RandomShoot2].transform.position.y + 1f,
              _monster[RandomShoot2].transform.position.z),
              20f * Time.deltaTime);
        }
        
    }
    void isOneSet()
    {
        isOne = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if((other.transform.tag == "Monster" || other.transform.tag == "Boss") && !isOne && player.isSkill1Upgrade)
        {
            //isOne = true;
            Invoke("isOneSet", 0.3f);
            isEnter = true;
            //_monster = GameStatus.Instance.ActivedMonsterList;
            if (_monster.Count == 1)
                return;
            if (_monster.Count >= 2)
            {
                RandomShoot2 = Random.Range((int)0, (int)_monster.Count);
                if(RandomShoot == RandomShoot2)
                {
                    if (RandomShoot2 == 0)
                        RandomShoot2++;
                    if (RandomShoot2 != 0)
                        RandomShoot2--;
                }
            }
        }
        if ((other.transform.tag == "Monster" || other.transform.tag == "Boss") && !isOne && !player.isSkill1Upgrade)
        {
            this.gameObject.SetActive(false);
        }
        if ((other.transform.tag == "Monster" || other.transform.tag == "Boss") && isOne)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        _monster = GameStatus.Instance.ActivedMonsterList;
        RandomShoot = Random.Range((int)0, (int)_monster.Count);
    }
    private void OnDisable()
    {
        isOne = false;
        isEnter = false;
    }
}
