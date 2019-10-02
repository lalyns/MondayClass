using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Mission
{

    public class MissionBoss : MissionBase
    {
        public static MissionBoss _Instance;

        protected override void Start()
        {
            if (_Instance == null)
            {
                _Instance = GetComponent<MissionBoss>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }
}