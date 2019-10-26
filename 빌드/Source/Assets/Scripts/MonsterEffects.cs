using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffects : MonoBehaviour
{
    private static MonsterEffects instacne;
    public static MonsterEffects Instance {
        get {
            if(instacne == null) {
                instacne = GameObject.Find("MonsterEffects").GetComponent<MonsterEffects>();
            }
            return instacne;
        }
    }

    public ObjectPool redHatSkillRange;
    public ObjectPool redHatSkillEffect1;
    public ObjectPool redHatSkillEffect2;
    public ObjectPool redHatAttackEffect;

    public ObjectPool macBulletPool;
    public ObjectPool macSkillPool;

    private void Awake()
    {
        if(instacne == null)
        {
            instacne = this.GetComponent<MonsterEffects>();
        }
    }


}
