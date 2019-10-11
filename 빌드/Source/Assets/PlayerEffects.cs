using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    private static PlayerEffects instance;
    public static PlayerEffects Instance {
        get {
            if (instance == null)
                instance = GameObject.Find("PC_Prefab").GetComponentInChildren<PlayerEffects>();
            return instance;
        }
    }

    public ObjectPool basicNormal;
    public ObjectPool basicSpecial;

    public ObjectPool skill1Normal;
    public ObjectPool skill1Special;

}
