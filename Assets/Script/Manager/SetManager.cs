using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetManager : MonoBehaviour
{
    public Set[] setList;

    public Set GetSetInfo(string setName)
    {
        for (int i = 0; i < setList.Length; i++)
        {
            if (setList[i].setName == setName)
            {
                return setList[i];
            }
        }
        return new Set();
    }

    private void Start()
    {
        int length = GameManager.instance.databaseManager.Set_DB.GetLineSize();
        setList = new Set[length];
        for (int i = 0; i < length; i++)
        {
            List<string> data = GameManager.instance.databaseManager.Set_DB.GetRowData(i);
            setList[i].setName = data[0];
            setList[i].skinNameList = data[1].Split('-');
        }
    }
}

[System.Serializable]
public struct Set
{
    public string setName;
    public string[] skinNameList;
}
