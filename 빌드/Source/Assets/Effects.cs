using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    public float duration;
    float time = 0;

    public ObjectPool targetPool;

    private void Start()
    {
    }

    public void EffectPlay()
    {
        for(int i=0; i<particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > duration)
        {
            time = 0;
            targetPool.ItemReturnPool(this.gameObject);
        }
    }


}
