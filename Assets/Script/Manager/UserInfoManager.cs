using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserInfoManager : MonoBehaviour
{
    private void Start()
    {
        PushSkinItem("cap/race_animal_01");
        PushSkinItem("cap/race_animal_01");
        PushSkinItem("cap/race_animal_01");
        PushSkinItem("cap/race_animal_01");
        PushSkinItem("cap/race_animal_01");
        PushSkinItem("cap/race_animal_01");
        PushSkinItem("cap/race_animal_01");
        PushSkinItem("cap/race_animal_01");
        PushSkinItem("cap/race_animal_01");
        PushSkinItem("cap/race_animal_01");

        PushSkinItem("eye/eye_01");
        PushSkinItem("eye/eye_01");

        PushColorItem(Color.white);
        PushColorItem(Color.black);
        PushColorItem(Color.yellow);
        PushColorItem(Color.grey);
    }

    //랜덤박스아이템 이름, 아이템 수
    public Dictionary<string, int> randomBoxItem = new Dictionary<string, int>();
    //컬러아이템 이름, 아이템 수
    public List<UserColorItem> colorItem = new List<UserColorItem>();

    public void PushColorItem(Color color)
    {
        for (int i = 0; i < colorItem.Count; i++)
        {
            if (colorItem[i].color == color)
            {
                colorItem[i].num++;
                return;
            }
        }
        colorItem.Add(new UserColorItem(color, 1));
    }

    public void DeleteColorItem(Color color)
    {
        for (int i = 0; i < colorItem.Count; i++)
        {
            if (colorItem[i].color == color)
            {
                colorItem[i].num--;
                if (colorItem[i].num < 1)
                {
                    colorItem.RemoveAt(i);
                }
                return;
            }
        }
    }

    //스킨이름, 해당 스킨의 컬러 색 리스트 
    public List<UserSkin> skinItem = new List<UserSkin>();

    public List<UserSkin> GetSkinItemList(string skinName)
    {
        List<UserSkin> tempUserSkin = new List<UserSkin>();
        for (int i = 0; i < skinItem.Count; i++)
        {
            if (skinItem[i].skinName.Contains(skinName))
            {
                tempUserSkin.Add(skinItem[i]);
            }
        }
        return tempUserSkin;
    }

    public void PushSkinItem(string skinName)
    {
        skinItem.Add(new UserSkin(skinName, Color.clear, Color.clear));
    }

    public void ChangeColorSkinItem(int index, Color color_01, Color color_02)
    {
        string tempSkinName = skinItem[index].skinName;
        skinItem[index] = new UserSkin(tempSkinName, color_01, color_02);
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

public class UserSkin
{
    public string skinName;
    public Color color_01;
    public Color color_02;

    public UserSkin(string skinName, Color color_01, Color color_02)
    {
        this.skinName = skinName;
        this.color_01 = color_01;
        this.color_02 = color_02;
    }
}

public class UserColorItem
{
    public Color color;
    public int num;

    public UserColorItem(Color color, int num)
    {
        this.color = color;
        this.num = num;
    }
}