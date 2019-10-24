using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffects : MonoBehaviour
{
    private static BossEffects instance;
    public static BossEffects Instance {
        get {
            if (instance == null)
                instance = GameObject.Find("EffectPool").GetComponent<BossEffects>();
            return instance;
        }
    }

    public ObjectPool bullet;
    public ObjectPool tornaedo;
    public ObjectPool beam;
    public ObjectPool flower;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this.GetComponent<BossEffects>();
        }
    }

}
