﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MC.Sound;

namespace MC.Mission
{

    public class DropStar : MonoBehaviour
    {
        public float _MoveSpeed = 10f;
        public float _TurnSpeed = 60f;

        public bool stop;

        public GameObject _StarMesh;
        public GameObject _FloorEffect;
        public GameObject _GetEffect;

        public void PlaySound()
        {
            var sound = MCSoundManager.Instance.objectSound.objectSFX;
            sound.PlaySound(this.gameObject, sound.startCreate);
            sound.PlaySound(this.gameObject, sound.starDrop);
        }

        private void Update()
        {
            if (stop) return;

            this.transform.position += Vector3.down * _MoveSpeed * Time.deltaTime;
            _StarMesh.transform.Rotate(0, _TurnSpeed * Time.deltaTime, 0);

            Ray ray = new Ray();
            ray.origin = this.transform.position;
            ray.direction = Vector3.down;

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 15f, 1 << 17, QueryTriggerInteraction.Collide))
            {
                _FloorEffect.transform.position = hit.point + Vector3.up * 0.05f;
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (stop) return;

            if (other.transform.tag == "Player")
            {
                stop = true;

                var sound = MCSoundManager.Instance.objectSound.objectSFX;
                sound.StopSound(this.gameObject, sound.starDrop);
                sound.PlaySound(this.gameObject, sound.starGet);

                _StarMesh.SetActive(false);
                _FloorEffect.SetActive(false);
                _GetEffect.transform.position = other.transform.position + Vector3.up;
                _GetEffect.SetActive(true);

                MissionB mission = MissionManager.Instance.CurrentMission as MissionB;
                mission.currentScore++;

                if (mission.activeStar.Contains(this.gameObject))
                    mission.activeStar.Remove(this.gameObject);

                Invoke("ReturnStar", 2f);
            }
            else if (other.transform.tag == "Stage")
            {
                _StarMesh.SetActive(false);

                var sound = MCSoundManager.Instance.objectSound.objectSFX;
                sound.StopSound(this.gameObject, sound.starDrop);

                MissionB mission = MissionManager.Instance.CurrentMission as MissionB;

                if (mission.activeStar.Contains(this.gameObject))
                    mission.activeStar.Remove(this.gameObject);

                mission.starPool.ItemReturnPool(this.gameObject);
            }
        }


        void ReturnStar()
        {
            MissionB mission = MissionManager.Instance.CurrentMission as MissionB;

            mission.starPool.ItemReturnPool(this.gameObject);
        }
    }
}