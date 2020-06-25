using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : MonoBehaviour
{
    [Header("컬러 아이템 리스트")] public ColorItem[] colorItemList;
    public ColorItem GetColorItem(string name)
    {
        for (int i = 0; i < colorItemList.Length; i++)
        {
            if (colorItemList[i].name == name)
            {
                return colorItemList[i];
            }
        }
        return new ColorItem();
    }
}

[Serializable]
public struct ColorItem
{
    public string name;
    public Sprite sprite;
    public Grade grade;
    public Color color;
}

public enum Grade
{
    HR,
    SR,
    SSR
}
