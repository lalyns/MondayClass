using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoolManager : MonoBehaviour
{
    public static EffectPoolManager _Instance;

    public ObjectPool _RedHatSkillRange;
    public ObjectPool _RedHatSkillEffect1;
    public ObjectPool _RedHatSkillEffect2;
    public ObjectPool _RedHatAttackEffect;


    public ObjectPool _MacBulletPool;
    public ObjectPool _MacSkillPool;
    public ObjectPool _BossBulletPool;

    public ObjectPool _MissionBstarPool;

    public ObjectPool[] _PlayerEffectPool;

    private void Awake()
    {
        if(_Instance == null)
        {
            _Instance = this.GetComponent<EffectPoolManager>();
        }
    }


}
