using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1Shoots : MonoBehaviour
{
    PlayerFSMManager player;
    public float _time = 0;
    private void Awake()
    {
        player = PlayerFSMManager.Instance;
    }    
    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= 7f){
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        player.Skill1PositionSet(player.Skill1_Effects, player.Skill1_Shoots, player.Skill1_Special_Shoots, player.isNormal);
    }
    private void OnDisable()
    {
        _time = 0;
    }
}
