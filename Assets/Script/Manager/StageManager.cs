using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Stage[] stageInfoList;

    private void Start()
    {
        int dataLength = GameManager.instance.databaseManager.stage_DB.GetLineSize();
        stageInfoList = new Stage[dataLength];
        for (int i = 0; i < dataLength; i++)
        {
            List<string> dataList = GameManager.instance.databaseManager.stage_DB.GetRowData(i);
            stageInfoList[i].stageName = dataList[0];
            stageInfoList[i].monsterName = dataList[1].Split('-');
            stageInfoList[i].properAtkPower = int.Parse(dataList[2]);
            stageInfoList[i].hp = int.Parse(dataList[3]);
            stageInfoList[i].defence = int.Parse(dataList[4]);
            stageInfoList[i].colorItemDropPercent = float.Parse(dataList[5]);
            stageInfoList[i].nomalClothChip = float.Parse(dataList[6]);
            stageInfoList[i].middleClothChip = float.Parse(dataList[7]);
            stageInfoList[i].legendClothChip = float.Parse(dataList[8]);
        }
    }

    public Stage GetStageInfo(string stageName)
    {
        for (int i = 0; i < stageInfoList.Length; i++)
        {
            if (stageInfoList[i].stageName == stageName)
            {
                return stageInfoList[i];
            }
        }
        return new Stage();
    }
}

[System.Serializable]
public struct Stage
{
    public string stageName;
    public string[] monsterName;
    public int properAtkPower;
    public int hp;
    public int defence;
    public float colorItemDropPercent;
    public float nomalClothChip;
    public float middleClothChip;
    public float legendClothChip;
}