using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bead : MonoBehaviour
{
    public float _MoveSpeed = 10f;

    public GameObject _FloorEffect;

    private void Update()
    {
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

        if (GameManager._Instance._IsDummyScene)
        {
            if (other.transform.tag == "Player")
            {
                GameManager.TempScoreAdd();


                Destroy(this.gameObject);
            }
            else if(other.transform.tag == "Stage")
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (other.transform.tag == "Player")
            {
                DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._Progress += 10;
                Debug.Log(DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._Progress);

                DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._BeadPositionEffect[
                        DungeonManager.GetCurrentDungeon().GetComponent<Survive>().pos].SetActive(false);

                Destroy(this.gameObject);
            }

            if (other.transform.tag == "Stage")
            {
                DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._BeadPositionEffect[
                        DungeonManager.GetCurrentDungeon().GetComponent<Survive>().pos].SetActive(false);

                Destroy(this.gameObject);
            }
        }
    }

}
