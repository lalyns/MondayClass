using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bead : MonoBehaviour
{
    public float _MoveSpeed = 10f;

    private void Update()
    {
        this.transform.position += Vector3.down * _MoveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._Progress += 10f;
            Debug.Log(DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._Progress);

            DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._BeadPositionEffect[
                    DungeonManager.GetCurrentDungeon().GetComponent<Survive>().pos].SetActive(false);

            Destroy(this.gameObject);
        }

        if(other.transform.tag == "Stage")
        {
            DungeonManager.GetCurrentDungeon().GetComponent<Survive>()._BeadPositionEffect[
                    DungeonManager.GetCurrentDungeon().GetComponent<Survive>().pos].SetActive(false);

            Destroy(this.gameObject);
        }

    }

}
