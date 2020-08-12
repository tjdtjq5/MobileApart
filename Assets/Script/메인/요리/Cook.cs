using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MonoBehaviour
{
    [Header("SetOff")]
    public GameObject[] setOff;

    [Header("SetOn")]
    public GameObject[] setOn;

    [Header("Mobile Blur")]
    public MobileBlur mobileBlur;

    public void CookOpen()
    {
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(false);
        }
        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].SetActive(true);
        }
        mobileBlur.enabled = true;
    }

    public void CoolClose()
    {
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }
        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].SetActive(false);
        }
        mobileBlur.enabled = false;
    }
}
