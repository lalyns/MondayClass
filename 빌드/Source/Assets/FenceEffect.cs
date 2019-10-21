using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceEffect : MonoBehaviour
{
    public MeshRenderer[] MR;
    List<Material> materials = new List<Material>();

    public Collider Collider => GetComponent<Collider>();

    public void Start()
    {
        for(int i=0; i<MR.Length; i++)
        {
            materials.Add(MR[i].material);
        }
    }

    public void OpenFence()
    {
        GameLib.DissoveActive(materials, true);
        StartCoroutine(GameLib.Dissolving(materials));
        Collider.enabled = false;

        Invoke("SetActive", 1f);
    }

    public void SetActive()
    {
        gameObject.SetActive(false);
    }
}
