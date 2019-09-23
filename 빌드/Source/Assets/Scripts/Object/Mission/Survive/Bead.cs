using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bead : MonoBehaviour
{
    public float _MoveSpeed = 10f;

    bool stop;

    public GameObject _StarMesh;
    public GameObject _FloorEffect;
    public GameObject _GetEffect;

    private void Update()
    {
        if (stop) return;

        this.transform.position += Vector3.down * _MoveSpeed * Time.deltaTime;

        Ray ray = new Ray();
        ray.origin = this.transform.position;
        ray.direction = Vector3.down;

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 15f, 1<<17, QueryTriggerInteraction.Collide))
        {
            _FloorEffect.transform.position = hit.point;
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);

        if (GameManager.Instance._IsDummyScene)
        {
            if (other.transform.tag == "Player")
            {
                GameManager.TempScoreAdd();
                stop = true;

                _StarMesh.SetActive(false);
                _FloorEffect.SetActive(false);
                _GetEffect.transform.position = other.transform.position + Vector3.up;
                _GetEffect.SetActive(true);

                Destroy(this.gameObject, 3.5f);
            }
            else if(other.transform.tag == "Stage")
            {
                _StarMesh.SetActive(false);

                Destroy(this.gameObject);
            }
        }
        else
        {
            //if (other.transform.tag == "Player")
            //{
            //    DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._Progress += 10;
            //    Debug.Log(DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._Progress);

            //    DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._BeadPositionEffect[
            //            DungeonManager.GetCurrentDungeon().GetComponent<Survive>().pos].SetActive(false);

            //    _StarMesh.SetActive(false);
            //    _GetEffect.SetActive(true);

            //    Destroy(this.gameObject, 3.5f);
            //}

            //if (other.transform.tag == "Stage")
            //{
            //    DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._BeadPositionEffect[
            //            DungeonManager.GetCurrentDungeon().GetComponent<Survive>().pos].SetActive(false);

            //    _StarMesh.SetActive(false);

            //    Destroy(this.gameObject);
            //}
        }
    }

}
