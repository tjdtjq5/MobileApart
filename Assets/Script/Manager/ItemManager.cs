using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : MonoBehaviour
{
    [Header("컬러 아이템 리스트")] public ColorItem[] colorItemList;
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
