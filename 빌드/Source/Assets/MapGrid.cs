using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapGrid : MonoBehaviour
{
    public Transform _Center;
    public GameObject _Actor;
    public List<Vector3> _MapPosition = new List<Vector3>();

    public float _GridSize;

    public int loopCount = 0;
    public bool _IsGridGizmos = true;


    private void Start()
    {
        _Actor.transform.position = _Center.transform.position;
        _MapPosition.Clear();
        SetCoord();
    }

    void SetCoord()
    {
        for(int x = -22; x <= 22; x++)
        {
            for(int y = -22; y <= 22; y++)
            {
                Vector3 correct = new Vector3(x, 0, y);

                Ray ray = new Ray();
                ray.origin = _Actor.transform.position + correct;
                ray.direction = -_Actor.transform.up;

                bool isGround = Physics.Raycast(ray, 0.1f, (1 << 17), QueryTriggerInteraction.Ignore);

                Debug.Log(string.Format("Ground Hit : {0}, Coord : {1}", isGround, ray.origin));

                if (isGround)
                {
                    _MapPosition.Add(ray.origin);
                }


            }
        }


    }

    private void OnDrawGizmos()
    {
        if (!_IsGridGizmos) return;

        Gizmos.color = Color.blue;

        foreach(Vector3 pos in _MapPosition)
        {
            Gizmos.DrawWireSphere(pos, 0.1f);
        }
        
    }


}
