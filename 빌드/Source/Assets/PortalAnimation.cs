using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalAnimation : MonoBehaviour
{
    public GameObject _AfterEffect;

    public void EndEffect()
    {
        _AfterEffect.SetActive(true);
        this.gameObject.SetActive(false);
    }

}
