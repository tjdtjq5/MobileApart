using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neglect : MonoBehaviour
{
    public Transform NeglectTransform;

    [Header("SetOff")]
    public GameObject[] setOff;

    public void NeglectOpen()
    {
        NeglectTransform.localPosition = Vector2.zero;

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(false);
        }
    }

    public void NeglectClose()
    {
        NeglectTransform.localPosition = new Vector2(-305, 1303);

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }
    }
}
