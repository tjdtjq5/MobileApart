using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoManager : MonoBehaviour
{
    public Dictionary<string, int> item = new Dictionary<string, int>();

    //아이템을 가지고 있는지 반환
    public bool ExistItem(string itemName)
    {
        if (item.ContainsKey(itemName))
        {
            return true;
        }
        return false;
    }

    // 아이템 수를 얻음, 없으면 0
    public int GetUserItemNum(string itemName)
    {
        if (item.ContainsKey(itemName))
        {
            int outNum = 0;
            item.TryGetValue(itemName, out outNum);
            return outNum;
        }
        return 0;
    }

    // 아이템을 유저 정보에 넣음 
    public void PushItem(string itemName, int num)
    {
        if (ExistItem(itemName))
        {
            int outNum = 0;
            item.TryGetValue(itemName, out outNum);
            item.Remove(itemName);
            item.Add(itemName, outNum + num);
        }
        else
        {
            item.Add(itemName, num);
        }
    }

    // 아이템을 유저정보에서 뺌 
    public void PullItem(string itemName, int num)
    {
        if (ExistItem(itemName))
        {
            int outNum = 0;
            item.TryGetValue(itemName, out outNum);
            item.Remove(itemName);

            outNum -= num;
            if (outNum > 1)
            {
                item.Add(itemName, outNum);
            }
        }
    }

    public void Start()
    {
        PushItem("파란 염색약", 5);
        PushItem("빨간 염색약", 5);
        PushItem("초록 염색약", 5);
        PushItem("랜덤 염색약", 5);
    }
}
