using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserInfoManager : MonoBehaviour
{
    private void Awake()
    {
        LoadSkinColor();
    }
    public void Start()
    {
        PushItem("파란 염색약", 5);
        PushItem("빨간 염색약", 5);
        PushItem("초록 염색약", 5);
        PushItem("랜덤 염색약", 5);

    }
    //*아이템
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

    //*스킨 컬러 
    public Dictionary<string, Color> skinColor = new Dictionary<string, Color>();
    public bool ExistSkinColor(string skinName)
    {
        if (skinColor.ContainsKey(skinName))
        {
            return true;
        }
        return false;
    }

    public Color GetUserSkinColor(string skinName)
    {
        if (skinColor.ContainsKey(skinName))
        {
            return skinColor[skinName];
        }
        return Color.black;
    }

    public void PushSkinColor(string skinName, Color color)
    {
        if (ExistSkinColor(skinName))
        {
            skinColor[skinName] = color;
        }
        else
        {
            skinColor.Add(skinName, color);
        }

        PlayerPrefs.SetString(skinName, color.ToString());
    }

    public void LoadSkinColor()
    {
        for (int i = 0; i < GameManager.instance.spineSkinInfoManager.SpineSkinInfoList.Length; i++)
        {
            if (PlayerPrefs.HasKey(GameManager.instance.spineSkinInfoManager.SpineSkinInfoList[i].skinName))
            {
                string key = GameManager.instance.spineSkinInfoManager.SpineSkinInfoList[i].skinName;
               // skinColor.Add(key, StringToColor(PlayerPrefs.GetString(key)));
            }
        }
    }


    Color StringToColor(string ColorString)
    {
        string tempString = ColorString;
        tempString = tempString.Remove(0, 5);
        tempString = tempString.Remove(tempString.IndexOf(')'));

        string[] tempStringList = tempString.Split(',');

        float R = float.Parse(tempStringList[0]);
        float G = float.Parse(tempStringList[1]);
        float B = float.Parse(tempStringList[2]);

        return new Color(R, G, B, 1);
    }

}
