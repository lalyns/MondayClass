using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapGrid : MonoBehaviour
{
    public Transform center;
    public Transform actor;
    public List<Vector3> mapPositions = new List<Vector3>();

    public float gridSize;
    public float actorHeight = 1f;
    public float mapSize = 22;

    public int loopCount = 0;
    public bool isDrawGizmos = true;

    public bool draw = false;

    private void Awake()
    {
        if(center == null)
            center = this.transform;
        if(actor == null)
            actor = transform.GetChild(0);

        if (gridSize <= 0)
            gridSize = 0.5f;
    }

    private void Start()
    {
        actor.position = center.transform.position;
        mapPositions.Clear();

        SetCoord(gridSize);
    }

    public void Update()
    {
        if (draw)
        {
            SetCoord(gridSize);
            draw = false;
        }
    }

    void SetCoord(float gridSize)
    {
        mapPositions.Clear();
        for (float x = -mapSize; x <= mapSize; x += gridSize)
        {
            for(float y = -mapSize; y <= mapSize; y += gridSize)
            {
                Vector3 correct = new Vector3(x, actorHeight, y);

                Ray ray = new Ray();
                ray.origin = actor.position + correct;
                ray.direction = -actor.up;

                bool isGround = Physics.Raycast(ray, 1f, (1 << 17), QueryTriggerInteraction.Ignore);

                //Debug.Log(string.Format("Ground Hit : {0}, Coord : {1}", isGround, ray.origin));

                if (isGround)
                {
                    mapPositions.Add(ray.origin);
                }


            }
        }


    }

    private void OnDrawGizmos()
    {
        if (!isDrawGizmos) return;

        Gizmos.color = Color.blue;

        foreach(Vector3 pos in mapPositions)
        {
            Gizmos.DrawWireSphere(pos, 0.1f);
        }
        
    }


}
