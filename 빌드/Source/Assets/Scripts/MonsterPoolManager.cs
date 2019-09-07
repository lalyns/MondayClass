using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour
{
    public static MonsterPoolManager _Instance;

    public ObjectPool _Mac;
    public ObjectPool _RedHat;

    // Start is called before the first frame update
    void Start()
    {
        if(_Instance == null)
        {
            _Instance = this.GetComponent<MonsterPoolManager>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
