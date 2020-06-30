using Spine;
using Spine.Unity;
using Spine.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserInfoManager : MonoBehaviour
{
    public GameObject userCharacter;

    //현재 유저가 장착중인 옷
    UserEqip acc = new UserEqip();
    UserEqip top = new UserEqip();
    UserEqip pan = new UserEqip();
    UserEqip eye = new UserEqip();
    UserEqip face = new UserEqip();
    UserEqip haF = new UserEqip();
    UserEqip haB = new UserEqip();
    UserEqip outt = new UserEqip();
    UserEqip sho = new UserEqip();
    UserEqip cap = new UserEqip();
    UserEqip set = new UserEqip();
    UserEqip body = new UserEqip();

    private void Start()
    {
        Initialized();
    }

    void Initialized()
    {
        PushSkinItem("body");
        PushSkinItem("cap/race_animal_01");
        PushSkinItem("eye/eye_01");
        PushSkinItem("face/face_01");
        PushSkinItem("haF/hair_f_01");
        PushSkinItem("haB/hair_b_01");
        PushSkinItem("pan/clo_under_01");
        PushSkinItem("top/clo_top_01");
        PushSkinItem("sho/shoes_01");

        PushUserEqip(skinItem[0]);
        PushUserEqip(skinItem[1]);
        PushUserEqip(skinItem[2]);
        PushUserEqip(skinItem[3]);
        PushUserEqip(skinItem[4]);
        PushUserEqip(skinItem[5]);
        PushUserEqip(skinItem[6]);
        PushUserEqip(skinItem[7]);
        PushUserEqip(skinItem[8]);

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
        PushSkinItem("top/clo_top_01");
        PushSkinItem("top/clo_top_01");
        PushSkinItem("top/clo_top_01");

        PushColorItem(Color.white);
        PushColorItem(Color.black);
        PushColorItem(Color.yellow);
        PushColorItem(Color.grey);


        userCharacter.GetComponent<TransformSkin>().UserEqipInfoSetting();
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

    public int GetSkinItemIndex(UserSkin userSkin)
    {
        string skinName = userSkin.skinName;
        Color color_01 = userSkin.color_01;
        Color color_02 = userSkin.color_02;

        for (int i = 0; i < skinItem.Count; i++)
        {
            if (skinItem[i].skinName == skinName)
            {
                if (skinItem[i].color_01 == color_01)
                {
                    if (skinItem[i].color_02 == color_02)
                    {
                        return i;
                    }
                }
            }
        }
        return -1;
    }

    public void PushSkinItem(string skinName)
    {
        Color color_01 = RandColor();
        Color color_02 = RandColor();
        skinItem.Add(new UserSkin(skinName, color_01, color_02));
    }

    Color RandColor()
    {
        float randR = Random.RandomRange(0, 255) /(float)255;
        float randG = Random.RandomRange(0, 255) / (float)255;
        float randB = Random.RandomRange(0, 255) / (float)255;

        return new Color(randR, randG, randB);
    }

    public void ChangeColorSkinItem(int index, Color color, int slotNum = 1)
    {
        if (slotNum == 1)
        {
            skinItem[index].color_01 = color;
        }
        if (slotNum == 2)
        {
            skinItem[index].color_02 = color;
        }
    }

    public void ChangeUserSkin(UserSkin orignUserSkin, UserSkin nextUserSkin)
    {
        for (int i = 0; i < skinItem.Count; i++)
        {
            if (skinItem[i] == orignUserSkin)
            {
                skinItem[i] = nextUserSkin;
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

  
    public void PushUserEqip(UserSkin userSkin)
    {
        SkinKind skinKind = (SkinKind)System.Enum.Parse(typeof(SkinKind), userSkin.skinName.Split('/')[0]);
        string skinName = userSkin.skinName;
        Color color_01 = userSkin.color_01;
        Color color_02 = userSkin.color_02;

        switch (skinKind)
        {
            case SkinKind.acc:
                acc = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            case SkinKind.top:
                top = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            case SkinKind.pan:
                pan = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            case SkinKind.eye:
                eye = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            case SkinKind.face:
                face = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            case SkinKind.haF:
                haF = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            case SkinKind.haB:
                haB = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            case SkinKind.outt:
                outt = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            case SkinKind.sho:
                sho = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            case SkinKind.cap:
                cap = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            case SkinKind.set:
                set = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            case SkinKind.body:
                body = new UserEqip(skinKind, skinName, color_01, color_02);
                break;
            default:
                break;
        }
    }

    public UserEqip GetUserEqip(SkinKind skinKind)
    {
        switch (skinKind)
        {
            case SkinKind.acc:
                return acc;
            case SkinKind.top:
                return top;
            case SkinKind.pan:
                return pan;
            case SkinKind.eye:
                return eye;
            case SkinKind.face:
                return face;
            case SkinKind.haF:
                return haF;
            case SkinKind.haB:
                return haB;
            case SkinKind.outt:
                return outt;
            case SkinKind.sho:
                return sho;
            case SkinKind.cap:
                return cap;
            case SkinKind.set:
                return set;
            case SkinKind.body:
                return body;
            default:
                return null;
        }
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

public class UserEqip
{
    public SkinKind skinKind;
    public string skinName;
    public Color color_01;
    public Color color_02;

    public UserEqip()
    {
        this.skinKind = SkinKind.acc;
        this.skinName = "";
        this.color_01 = Color.clear;
        this.color_02 = Color.clear;
    }

    public UserEqip(SkinKind skinKind, string skinName, Color color_01, Color color_02)
    {
        this.skinKind = skinKind;
        this.skinName = skinName;
        this.color_01 = color_01;
        this.color_02 = color_02;
    }
}