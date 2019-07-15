using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    GameObject _player;
    [SerializeField]
    Vector3 CameraPos = new Vector3(0.0f, 15f, -8f);
	// Use this for initialization
	void Start () {
        _player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = _player.transform.position +
            Vector3.up * CameraPos.y + Vector3.forward * CameraPos.z;
	}
}
