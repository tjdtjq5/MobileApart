using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatabaseManager : MonoBehaviour
{
    public DataBase Box_DB;
    public DataBase DailyShop_DB;
    public DataBase Item_DB;
    public DataBase stage_DB;
    public DataBase monster_DB;
    public DataBase Weapone_DB;
    public DataBase Script_DB;
    public DataBase Set_DB;
}

[Serializable]
public class DataBase
{
    public TextAsset txt;
    string[,] sentence;
    int lineSize, rowSize;

    public string GetData(string columeKey, string rowKey)
    {
        string currentTxt = txt.text.Substring(0, txt.text.Length);
        string[] line = currentTxt.Split('\n');
        lineSize = line.Length;
        rowSize = line[0].Split('\t').Length;
        sentence = new string[lineSize, rowSize];

        for (int i = 0; i < lineSize; i++)
        {
            string[] row = line[i].Split('\t');
            for (int j = 0; j < rowSize; j++)
            {
                sentence[i, j] = row[j];
            }
        }

        int cKey = 0;
        int rKey = 0;

        for (int i = 0; i < lineSize; i++)
        {
            if (sentence[i, 0] == columeKey) cKey = i;

        }
        for (int i = 0; i < rowSize; i++)
        {
            if (sentence[0, i] == rowKey) rKey = i;
        }
        return sentence[cKey, rKey];
    }

    public List<string> GetRowData(int columeInt)
    {
        List<string> tempDataList = new List<string>();

        string currentTxt = txt.text.Substring(0, txt.text.Length);
        string[] line = currentTxt.Split('\n');
        lineSize = line.Length;
        rowSize = line[0].Split('\t').Length;
        sentence = new string[lineSize, rowSize];

        string[] row = line[columeInt].Split('\t');

        for (int i = 0; i < row.Length; i++)
        {
            tempDataList.Add(row[i]);
        }

        return tempDataList;
    }


    public int GetLineSize()
    {
        string currentTxt = txt.text.Substring(0, txt.text.Length);
        string[] line = currentTxt.Split('\n');
        lineSize = line.Length;
        return lineSize;
    }
}
