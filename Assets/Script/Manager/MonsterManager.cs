using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public Monster[] MonsterInfoList;

    private void Start()
    {
        int length = GameManager.instance.databaseManager.monster_DB.GetLineSize();
        MonsterInfoList = new Monster[length];
        for (int i = 0; i < length; i++)
        {
            List<string> dataList = GameManager.instance.databaseManager.monster_DB.GetRowData(i);
            MonsterInfoList[i].monsterName = dataList[0];
            MonsterInfoList[i].properties = (Properties)System.Enum.Parse(typeof(Properties), dataList[1]);
        }
    }

    public Monster GetMonsterInfo(string monsterName)
    {
        for (int i = 0; i < MonsterInfoList.Length; i++)
        {
            if (MonsterInfoList[i].monsterName == monsterName)
            {
                return MonsterInfoList[i];
            }
        }
        return new Monster();
    }
}

[System.Serializable]
public struct Monster
{
    public string monsterName;
    public Properties properties;
}
