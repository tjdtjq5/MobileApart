using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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
                iconList[i].iconObj.GetComponent<Image>().color = Color.white;
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