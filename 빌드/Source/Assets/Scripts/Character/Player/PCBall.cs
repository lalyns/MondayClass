using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCBall : MonoBehaviour
{
    PlayerFSMManager player;
    SphereCollider collider;
    List<GameObject> _monster = new List<GameObject>();
    public bool isOne, isTwo, isThree, isFour, isFive, isSix, isSeven, isEight = false;
    public int BounceCount = 0;
    bool isEnter = false;
    public int RandomShoot, RandomShoot2;

    void Start()
    {
        player = PlayerFSMManager.Instance;
        collider = GetComponent<SphereCollider>();
        BounceCount = 0;
    }
    void Update()
    {
        if (!isOne && BounceCount == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(_monster[RandomShoot].transform.position.x, 
                _monster[RandomShoot].transform.position.y + 1f,
                _monster[RandomShoot].transform.position.z),
                30f * Time.deltaTime);
        }
        if (BounceCount == 1 && player.Skill1BounceCount >= 1)//player.isSkill1Upgrade)
        {
            transform.position = Vector3.MoveTowards(transform.position,
              new Vector3(_monster[RandomShoot2].transform.position.x,
              _monster[RandomShoot2].transform.position.y + 1f,
              _monster[RandomShoot2].transform.position.z),
              30f * Time.deltaTime);
        }
        if(BounceCount == 2 && player.Skill1BounceCount >= 2)
        {
            transform.position = Vector3.MoveTowards(transform.position,
               new Vector3(_monster[RandomShoot].transform.position.x,
               _monster[RandomShoot].transform.position.y + 1f,
               _monster[RandomShoot].transform.position.z),
               30f * Time.deltaTime);
        }
        if(BounceCount == 3 && player.Skill1BounceCount >= 3)
        {
            transform.position = Vector3.MoveTowards(transform.position,
              new Vector3(_monster[RandomShoot2].transform.position.x,
              _monster[RandomShoot2].transform.position.y + 1f,
              _monster[RandomShoot2].transform.position.z),
              30f * Time.deltaTime);
        }
        if (BounceCount == 4 && player.Skill1BounceCount >= 4)
        {
            transform.position = Vector3.MoveTowards(transform.position,
               new Vector3(_monster[RandomShoot].transform.position.x,
               _monster[RandomShoot].transform.position.y + 1f,
               _monster[RandomShoot].transform.position.z),
               30f * Time.deltaTime);
        }
        if (BounceCount == 5 && player.Skill1BounceCount >= 5)
        {
            transform.position = Vector3.MoveTowards(transform.position,
              new Vector3(_monster[RandomShoot2].transform.position.x,
              _monster[RandomShoot2].transform.position.y + 1f,
              _monster[RandomShoot2].transform.position.z),
              30f * Time.deltaTime);
        }
        if (BounceCount == 6 && player.Skill1BounceCount >= 6)
        {
            transform.position = Vector3.MoveTowards(transform.position,
               new Vector3(_monster[RandomShoot].transform.position.x,
               _monster[RandomShoot].transform.position.y + 1f,
               _monster[RandomShoot].transform.position.z),
               30f * Time.deltaTime);
        }
        if (BounceCount == 7 && player.Skill1BounceCount >= 7)
        {
            transform.position = Vector3.MoveTowards(transform.position,
              new Vector3(_monster[RandomShoot2].transform.position.x,
              _monster[RandomShoot2].transform.position.y + 1f,
              _monster[RandomShoot2].transform.position.z),
              30f * Time.deltaTime);
        }
    }
    void isOneSet() { isOne = true; }
    void isTwoSet() { isTwo = true; }
    void isThreeSet() { isThree = true; }
    void isFourSet() { isFour = true; }
    void isFiveSet() { isFive = true; }
    void isSixSet() { isSix = true; }
    void isSevenSet() { isSeven = true; }
    void isEightSet() { isEight = true; }
    void SetEnabled()
    {
        collider.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Monster" && !isOne && player.Skill1BounceCount >= 1)//player.isSkill1Upgrade)
        {
            BounceCount++;
            Invoke("isOneSet", 0.1f);            
            _monster = GameStatus.Instance.ActivedMonsterList;

            if (_monster.Count == 1)
            {
                collider.enabled = false;
                Invoke("SetEnabled", 0.15f);
            }
                
            if (_monster.Count >= 2)
            {
                RandomShoot2 = Random.Range((int)0, (int)_monster.Count);
                if (RandomShoot == RandomShoot2)
                {
                    if (RandomShoot2 == 0)
                        RandomShoot2++;
                    if (RandomShoot2 != 0)
                        RandomShoot2--;
                }
            }
        }
        if(other.transform.tag == "Monster" && isOne && !isTwo && player.Skill1BounceCount >= 2)
        {
            BounceCount++;
            Invoke("isTwoSet", 0.1f);
            _monster = GameStatus.Instance.ActivedMonsterList;
            if (_monster.Count == 1)
            {
                collider.enabled = false;
                Invoke("SetEnabled", 0.15f);
            }
            if (_monster.Count >= 2)
            {
                RandomShoot = Random.Range((int)0, (int)_monster.Count);
                if (RandomShoot == RandomShoot2)
                {
                    if (RandomShoot == 0)
                        RandomShoot++;
                    if (RandomShoot != 0)
                        RandomShoot--;
                }
            }
        }
        if (other.transform.tag == "Monster" && isTwo && !isThree && player.Skill1BounceCount >= 3)
        {
            BounceCount++;
            Invoke("isThreeSet", 0.1f);
            _monster = GameStatus.Instance.ActivedMonsterList;
            if (_monster.Count == 1)
            {
                collider.enabled = false;
                Invoke("SetEnabled", 0.15f);
            }
            if (_monster.Count >= 2)
            {
                RandomShoot2 = Random.Range((int)0, (int)_monster.Count);
                if (RandomShoot == RandomShoot2)
                {
                    if (RandomShoot2 == 0)
                        RandomShoot2++;
                    if (RandomShoot2 != 0)
                        RandomShoot2--;
                }
            }
        }
        if (other.transform.tag == "Monster" && isThree && !isFour && player.Skill1BounceCount >= 4)
        {
            BounceCount++;
            Invoke("isFourSet", 0.1f);
            _monster = GameStatus.Instance.ActivedMonsterList;
            if (_monster.Count == 1)
            {
                collider.enabled = false;
                Invoke("SetEnabled", 0.15f);
            }
            if (_monster.Count >= 2)
            {
                RandomShoot = Random.Range((int)0, (int)_monster.Count);
                if (RandomShoot == RandomShoot2)
                {
                    if (RandomShoot == 0)
                        RandomShoot++;
                    if (RandomShoot != 0)
                        RandomShoot--;
                }
            }
        }
        if (other.transform.tag == "Monster" && isFour && !isFive && player.Skill1BounceCount >= 5)
        {
            BounceCount++;
            Invoke("isFiveSet", 0.1f);
            _monster = GameStatus.Instance.ActivedMonsterList;
            if (_monster.Count == 1)
            {
                collider.enabled = false;
                Invoke("SetEnabled", 0.15f);
            }
            if (_monster.Count >= 2)
            {
                RandomShoot2 = Random.Range((int)0, (int)_monster.Count);
                if (RandomShoot == RandomShoot2)
                {
                    if (RandomShoot2 == 0)
                        RandomShoot2++;
                    if (RandomShoot2 != 0)
                        RandomShoot2--;
                }
            }
        }
        if (other.transform.tag == "Monster" && isFive && !isSix && player.Skill1BounceCount >= 6)
        {
            BounceCount++;
            Invoke("isSixSet", 0.1f);
            _monster = GameStatus.Instance.ActivedMonsterList;
            if (_monster.Count == 1)
            {
                collider.enabled = false;
                Invoke("SetEnabled", 0.15f);
            }
            if (_monster.Count >= 2)
            {
                RandomShoot = Random.Range((int)0, (int)_monster.Count);
                if (RandomShoot == RandomShoot2)
                {
                    if (RandomShoot == 0)
                        RandomShoot++;
                    if (RandomShoot != 0)
                        RandomShoot--;
                }
            }
        }
        if (other.transform.tag == "Monster" && isSix && !isSeven && player.Skill1BounceCount >= 7)
        {
            BounceCount++;
            Invoke("isSevenSet", 0.1f);
            _monster = GameStatus.Instance.ActivedMonsterList;
            if (_monster.Count == 1)
            {
                collider.enabled = false;
                Invoke("SetEnabled", 0.15f);
            }
            if (_monster.Count >= 2)
            {
                RandomShoot2 = Random.Range((int)0, (int)_monster.Count);
                if (RandomShoot == RandomShoot2)
                {
                    if (RandomShoot2 == 0)
                        RandomShoot2++;
                    if (RandomShoot2 != 0)
                        RandomShoot2--;
                }
            }
        }
        if (other.transform.tag == "Monster" && isSeven && !isEight && player.Skill1BounceCount >= 8)
        {
            BounceCount++;
            Invoke("isEightSet", 0.1f);
            _monster = GameStatus.Instance.ActivedMonsterList;
            if (_monster.Count == 1)
            {
                collider.enabled = false;
                Invoke("SetEnabled", 0.15f);
            }
            if (_monster.Count >= 2)
            {
                RandomShoot = Random.Range((int)0, (int)_monster.Count);
                if (RandomShoot == RandomShoot2)
                {
                    if (RandomShoot == 0)
                        RandomShoot++;
                    if (RandomShoot != 0)
                        RandomShoot--;
                }
            }
        }
        if (other.transform.tag == "Monster" && player.Skill1BounceCount == 1 && isOne)
        {
            this.gameObject.SetActive(false);
        }
        if (other.transform.tag == "Monster" && player.Skill1BounceCount == 2 && isTwo)
        {
            this.gameObject.SetActive(false);
        }
        if (other.transform.tag == "Monster" && player.Skill1BounceCount == 3 && isThree)
        {
            this.gameObject.SetActive(false);
        }
        if (other.transform.tag == "Monster" && player.Skill1BounceCount == 4 && isFour)
        {
            this.gameObject.SetActive(false);
        }
        if (other.transform.tag == "Monster" && player.Skill1BounceCount == 5 && isFive)
        {
            this.gameObject.SetActive(false);
        }
        if (other.transform.tag == "Monster" && player.Skill1BounceCount == 6 && isSix)
        {
            this.gameObject.SetActive(false);
        }
        if (other.transform.tag == "Monster" && player.Skill1BounceCount == 7 && isSeven)
        {
            this.gameObject.SetActive(false);
        }
        if (other.transform.tag == "Monster" && player.Skill1BounceCount == 8 && isEight)
        {
            this.gameObject.SetActive(false);
        }
        //if ((other.transform.tag == "Monster" || other.transform.tag == "Boss") && !isOne && !player.isSkill1Upgrade)
        //{
        //    this.gameObject.SetActive(false);
        //}
        //if ((other.transform.tag == "Monster" || other.transform.tag == "Boss") && isOne)
        //{
        //    this.gameObject.SetActive(false);
        //}
    }
    private void OnEnable()
    {
        _monster = GameStatus.Instance.ActivedMonsterList;
        RandomShoot = Random.Range((int)0, (int)_monster.Count);
    }
    private void OnDisable()
    {
        isOne = false;
        isTwo = false;
        isThree = false;
        isFour = false;
        isFive = false;
        isSix = false;
        isSeven = false;
        isEight = false;
        isEnter = false;
        BounceCount = 0;
        collider.enabled = true;
    }
}
