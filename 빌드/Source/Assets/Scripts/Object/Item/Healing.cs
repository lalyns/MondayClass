using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MC.Sound;

public class Healing : MonoBehaviour
{
    [Tooltip("True: HOT, False: NORMAL")]
    public bool HOT;

    public int _HealValue;

    [Header("HOT가 True일떄")]
    public float _HealingTime;
    public float interval = 0.2f;

    PlayerFSMManager player;
    SphereCollider sphere;
    BoxCollider box;
    ParticleSystem particle;

    float _time;
    public float CoolTime = 10f;

    private void Awake()
    {
        player = PlayerFSMManager.Instance;

        sphere = GetComponent<SphereCollider>();
        box = GetComponentInChildren<BoxCollider>();

        particle = GetComponentInChildren<ParticleSystem>();
        particle.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (!box.gameObject.activeSelf)
        {
            _time += Time.deltaTime;

            if (_time >= CoolTime)
            {
                _time = 0;
                box.gameObject.SetActive(true);
                sphere.enabled = true;
                particle.gameObject.SetActive(false);
            }
        }

    }
    public void HealPlayer()
    {
        if (!HOT)
        {
            if (player.Stat._hp + _HealValue > player.Stat.MaxHp)
                player.Stat._hp = player.Stat.MaxHp;
            else
                player.Stat._hp += _HealValue;
        }
        else
        {
            
            StartCoroutine("TimeWaiting");

        }
    }

    public IEnumerator TimeWaiting()
    {
        for (int i = 0; i < _HealingTime; i++)
        {
            if (player.Stat._hp + _HealValue > player.Stat.MaxHp)
                player.Stat._hp = player.Stat.MaxHp;
            else
                player.Stat._hp += _HealValue;
            yield return new WaitForSeconds(interval);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            HealPlayer();
            sphere.enabled = false;
            box.gameObject.SetActive(false);
            particle.gameObject.SetActive(true);
            var sound = MCSoundManager.Instance.objectSound.objectSFX;
            sound.PlaySound(this.gameObject, sound.itemGet);
        }
    }
}
