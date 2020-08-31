using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponeManager : MonoBehaviour
{
    public Weapon[] weaponeInfoList;

    private void Start()
    {
        int length = GameManager.instance.databaseManager.Weapone_DB.GetLineSize();
        weaponeInfoList = new Weapon[length];
        for (int i = 0; i < length; i++)
        {
            List<string> dataList = GameManager.instance.databaseManager.Weapone_DB.GetRowData(i);
            weaponeInfoList[i].weaponeName = dataList[0];
            weaponeInfoList[i].properties = (Properties)System.Enum.Parse(typeof(Properties), dataList[1]);
            weaponeInfoList[i].atk = int.Parse(dataList[2]);
            weaponeInfoList[i].getForAtk = int.Parse(dataList[3]);
            weaponeInfoList[i].enhancePrice = int.Parse(dataList[4]);
            weaponeInfoList[i].enhanceAtk = int.Parse(dataList[5]);
            weaponeInfoList[i].effectName = dataList[6];
            weaponeInfoList[i].stageName = dataList[7];
            weaponeInfoList[i].probability = float.Parse(dataList[8]);
        }
    }

    public Weapon GetWeaponInfo(string weaponName)
    {
        for (int i = 0; i < weaponeInfoList.Length; i++)
        {
            if (weaponeInfoList[i].weaponeName == weaponName)
            {
                return weaponeInfoList[i];
            }
        }

        return new Weapon();
    }
}

[System.Serializable]
public struct Weapon
{
    public string weaponeName;
    public Properties properties;
    public int atk;
    public int getForAtk;
    public int enhancePrice;
    public int enhanceAtk;
    public string effectName;
    public string stageName;
    public float probability;
}
