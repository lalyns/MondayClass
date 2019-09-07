using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PlayerCtrl : MonoBehaviour
    {
        float h = 0.01f;
        float v = 0.01f;
        float r_x = 0.00f;
        float r_y = 0.00f;
        private Transform _tr;
        private Animator _anim;
        private float moveSpeed = 10.0f;
        private float rotSpeed = 80.0f;

        public bool isAlt;

        public CameraManager camManager;
        // Start is called before the first frame update
        void Start()
        {
            _tr = GetComponent<Transform>();
            _anim = GetComponentInChildren<Animator>();

            //camManager = CameraManager.singleton;
            //camManager.Init(this.transform);
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKey(KeyCode.Q))
            //{
            //    isAlt = true;
            //}
            //else
            //    isAlt = false;

            //if (!isAlt)
            //{
               
              
            //}
            //if (isAlt)
            //{
            //    camManager.Tick(Time.deltaTime);
            //}
            r_x = Input.GetAxis("Mouse X");
            r_y = Input.GetAxis("Mouse Y");

            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
     
            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
            float vec = Vector3.Magnitude(moveDir.normalized);
            //Debug.Log("moveDir의 크기 : " + vec);

            _tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.World);

            _tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r_x);

            //_tr.Rotate(Vector3.right * rotSpeed * Time.deltaTime * r_y);
            if (OnMove())
            {
                _anim.SetBool("isRun", true);
            }
            else
            {
                _anim.SetBool("isRun", false);
            }
        }

        public bool OnMove()
        {
            return Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Horizontal") <= -0.01f ||
                Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Vertical") <= -0.01f;
        }
    }
}