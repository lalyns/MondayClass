using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<SphereCollider> ItemList = new List<SphereCollider>();
    public List<BoxCollider> itemObject = new List<BoxCollider>();
    public List<ParticleSystem> particle = new List<ParticleSystem>();

    public int randItem;

    bool isAte;

    float _time = 0;
    private void Awake()
    {
    }
    void Start()
    {
        randItem = Random.Range((int)0, (int)6);

        for (int i = 0; i < 6; i++)
        {
            ItemList[i].transform.gameObject.SetActive(false);
            itemObject[i].transform.gameObject.SetActive(false);
            particle[i].gameObject.SetActive(false);
        }


        ItemList[randItem].gameObject.SetActive(true);
        itemObject[randItem].gameObject.SetActive(true);
    }

    void Update()
    {
        
        if (!itemObject[randItem].gameObject.activeSelf)
        {
            _time += Time.deltaTime;

            if(_time >= 10f)
            {
                ItemList[randItem].gameObject.SetActive(false);

                _time = 0;

                randItem = Random.Range((int)0, (int)6);

                ItemList[randItem].gameObject.SetActive(true);
                ItemList[randItem].enabled = true;
                itemObject[randItem].gameObject.SetActive(true);
                particle[randItem].gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
