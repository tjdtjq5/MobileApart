using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpineSkinInfoManager : MonoBehaviour
{
    public SpineSkinInfo[] SpineSkinInfoList;

    public SpineSkinInfo GetSpineSkinInfo(string skinName)
    {
        for (int i = 0; i < SpineSkinInfoList.Length; i++)
        {
            if (SpineSkinInfoList[i].skinName == skinName)
            {
                return SpineSkinInfoList[i];
            }
        }
        return new SpineSkinInfo();
    }
    public List<SpineSkinInfo> GetSpineSkinInfo(SkinKind skinKind)
    {
        List<SpineSkinInfo> tempList = new List<SpineSkinInfo>();
        for (int i = 0; i < SpineSkinInfoList.Length; i++)
        {
            if (SpineSkinInfoList[i].skinKind == skinKind)
            {
                tempList.Add(SpineSkinInfoList[i]);
            }
        }
        return tempList;
    }

    private void Start()
    {
        int SkinNum = GameManager.instance.databaseManager.Skin_DB.GetLineSize();
        SpineSkinInfoList = new SpineSkinInfo[SkinNum];
        for (int i = 0; i < SkinNum; i++)
        {
            List<string> dataList = GameManager.instance.databaseManager.Skin_DB.GetRowData(i);
            SpineSkinInfoList[i].skinName = dataList[0];
            SpineSkinInfoList[i].skinKind = (SkinKind)Enum.Parse(typeof(SkinKind), dataList[1]);
            SpineSkinInfoList[i].inGameName = dataList[2];
            SpineSkinInfoList[i].grade = (Grade)Enum.Parse(typeof(Grade), dataList[3]);
        }
    }

   
}
[Serializable]
public struct SpineSkinInfo
{
    public string skinName;
    public SkinKind skinKind;
    public string inGameName;
    public Grade grade;
}

public enum SkinKind
{
    acc,
    top,
    pan,
    eye,
    face,
    haF,
    haB,
    outt,
    sho
}
