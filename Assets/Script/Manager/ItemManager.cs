using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] itemList;

    [System.Serializable]
    public struct Item
    {
        public string itemName;
        public string itemCode;
        public string inGameName;
        public ItemKind itemKind;
        public GameObject iconObj;
    }

    public void Start()
    {
        int Length = GameManager.instance.databaseManager.Item_DB.GetLineSize();
        itemList = new Item[Length];
        for (int i = 0; i < Length; i++)
        {
            List<string> dataList = GameManager.instance.databaseManager.Item_DB.GetRowData(i);
            itemList[i].itemName = dataList[0];
            itemList[i].itemCode = dataList[1];
            itemList[i].inGameName = dataList[2];
            itemList[i].itemKind = (ItemKind)System.Enum.Parse(typeof(ItemKind) ,dataList[3]);
            itemList[i].iconObj = Resources.Load<GameObject>(System.IO.Path.Combine("스킨Obj", dataList[1]));
        }
    }

    public Item GetItemInfo(string itemName)
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i].itemName == itemName)
            {
                return itemList[i];
            }
        }
        return new Item();
    }
}

public enum ItemKind
{
    골드,
    크리스탈,
    랜덤염색약,
    염색약,
    스킨
}
