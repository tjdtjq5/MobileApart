using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserInfoManager : MonoBehaviour
{
    //랜덤박스아이템 이름, 아이템 수
    public Dictionary<string, int> randomBoxItem = new Dictionary<string, int>();
    //컬러아이템 이름, 아이템 수
    public Dictionary<string, int> colorItem = new Dictionary<string, int>();
    //스킨이름, 해당 스킨의 컬러 색 리스트 
    public Dictionary<string, List<Color>> skinItem = new Dictionary<string, List<Color>>();




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
