using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public Script[] scriptList;

    public Script GetscriptInfo(int code)
    {
        for (int i = 0; i < scriptList.Length; i++)
        {
            if (scriptList[i].code == code)
            {
                return scriptList[i];
            }
        }

        return new Script();
    }

    private void Start()
    {
        int length = GameManager.instance.databaseManager.Script_DB.GetLineSize();
        scriptList = new Script[length];
        for (int i = 0; i < length; i++)
        {
            List<string> data = GameManager.instance.databaseManager.Script_DB.GetRowData(i);
            scriptList[i].code = int.Parse(data[0]);
            scriptList[i].name = data[1];
            scriptList[i].script = data[2];

            int count = 0;
            if (data[3] != "Null")
                count++;
            if (data[5] != "Null")
                count++;
            if (data[7] != "Null")
                count++;
            if (data[9] != "Null")
                count++;

            scriptList[i].selectScript = new string[count];
            scriptList[i].selectCode = new int[count];

            for (int j = 0; j < count; j++)
            {
                scriptList[i].selectScript[j] = data[3 + (j * 2)];
                scriptList[i].selectCode[j] = int.Parse(data[4 + (j * 2)]);
            }
        }


    }
}

[System.Serializable]
public struct Script
{
    public int code;
    public string name;
    public string script;
    public string[] selectScript;
    public int[] selectCode;
}
