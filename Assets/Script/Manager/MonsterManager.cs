using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public MonsterStage[] monsterInfoList;

    private void Start()
    {
        int dataLength = GameManager.instance.databaseManager.monster_DB.GetLineSize();
        monsterInfoList = new MonsterStage[dataLength];
        for (int i = 0; i < dataLength; i++)
        {
            List<string> dataList = GameManager.instance.databaseManager.monster_DB.GetRowData(i);
            monsterInfoList[i].stage = int.Parse(dataList[0]);
            monsterInfoList[i].monsterName = dataList[1];
            monsterInfoList[i].properties = (Properties)System.Enum.Parse(typeof(Properties), dataList[2]);
            monsterInfoList[i].hp = int.Parse(dataList[3]);
            monsterInfoList[i].defence = int.Parse(dataList[4]);
            monsterInfoList[i].colorItemDropPercent = float.Parse(dataList[5]);
            monsterInfoList[i].nomalClothChip = float.Parse(dataList[6]);
            monsterInfoList[i].middleClothChip = float.Parse(dataList[7]);
            monsterInfoList[i].legendClothChip = float.Parse(dataList[8]);
        }
    }

    public MonsterStage GetMonsterInfo(int stage)
    {
        for (int i = 0; i < monsterInfoList.Length; i++)
        {
            if (monsterInfoList[i].stage == stage)
            {
                return monsterInfoList[i];
            }
        }
        return new MonsterStage();
    }
}

[System.Serializable]
public struct MonsterStage
{
    public int stage;
    public string monsterName;
    public Properties properties;
    public int hp;
    public int defence;
    public float colorItemDropPercent;
    public float nomalClothChip;
    public float middleClothChip;
    public float legendClothChip;
}