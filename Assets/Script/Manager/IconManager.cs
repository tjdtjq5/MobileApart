using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IconManager : MonoBehaviour
{
    public Icon[] iconList;

    private void Awake()
    {
        for (int i = 0; i < iconList.Length; i++)
        {
            iconList[i].itemName = iconList[i].iconObj.name;
        }
    }

    public GameObject GetIcon(string itemName)
    {
        for (int i = 0; i < iconList.Length; i++)
        {
            if (iconList[i].itemName == itemName)
            {
                return iconList[i].iconObj;
            }
        }
        return null;
    }
}
[Serializable]
public struct Icon
{
    public string itemName;
    public GameObject iconObj;
}