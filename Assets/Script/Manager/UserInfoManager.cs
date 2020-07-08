using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UserInfoManager : MonoBehaviour
{
    public string nickname;
    public string inDate;

    //현재 유저가 장착중인 옷
    public UserEqip acc = new UserEqip();
    public UserEqip top = new UserEqip();
    public UserEqip pan = new UserEqip();
    public UserEqip eye = new UserEqip();
    public UserEqip face = new UserEqip();
    public UserEqip haF = new UserEqip();
    public UserEqip haB = new UserEqip();
    public UserEqip outt = new UserEqip();
    public UserEqip sho = new UserEqip();
    public UserEqip cap = new UserEqip();
    public UserEqip set = new UserEqip();
    public UserEqip body = new UserEqip();

    private void Start()
    {
        Initialized();

        PushColorItem(Color.clear);
        PushColorItem(Color.clear);
        PushColorItem(Color.clear);
        PushColorItem(Color.clear);
        PushColorItem(Color.white);
        PushColorItem(Color.black);
        PushColorItem(Color.yellow);
        PushColorItem(Color.grey);
    }

    //초기 
    void Initialized()
    {
        if (!PlayerPrefs.HasKey("첫시작"))
        {
            PlayerPrefs.SetString("첫시작", "True");

            skinItem.Add(new UserSkin("acc/top_ribbon", Color.white, Color.white));
            skinItem.Add(new UserSkin("top/clo_top_school", Color.white, Color.white));
            skinItem.Add(new UserSkin("pan/skirt_01", Color.white, Color.white));
            skinItem.Add(new UserSkin("eye/eye_01", Color.white, Color.white));
            skinItem.Add(new UserSkin("face/face_01", Color.white, Color.white));
            skinItem.Add(new UserSkin("haF/hair_f_01", Color.white, Color.white));
            skinItem.Add(new UserSkin("haB/hair_b_01", Color.white, Color.white));
            skinItem.Add(new UserSkin("sho/shoes_01", Color.white, Color.white));
            skinItem.Add(new UserSkin("body", Color.white, Color.white));

            PushUserEqip(skinItem[0]);
            PushUserEqip(skinItem[1]);
            PushUserEqip(skinItem[2]);
            PushUserEqip(skinItem[3]);
            PushUserEqip(skinItem[4]);
            PushUserEqip(skinItem[5]);
            PushUserEqip(skinItem[6]);
            PushUserEqip(skinItem[7]);
            PushUserEqip(skinItem[8]);

            SaveSkinItem();
            SaveUserEqip();
        }
        else
        {
            LoadSkinItem();
            LoadUserEqip();
        }
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
    public void DeleteColorItem(int index)
    {
        colorItem[index].num--;

        if (colorItem[index].num == 0 && colorItem[index].color != Color.clear )
        {
            colorItem.RemoveAt(index);
        }
    }

    public int GetIndexColorItem(Color color)
    {
        for (int i = 0; i < colorItem.Count; i++)
        {
            if (colorItem[i].color == color)
            {
                return i;
            }
        }
        return -1;
    }

    public void DeleteColorItem(Color color)
    {
        for (int i = 0; i < colorItem.Count; i++)
        {
            if (colorItem[i].color == color)
            {
                colorItem[i].num--;
                if (colorItem[i].num == 0 && colorItem[i].color != Color.clear)
                {
                    colorItem.RemoveAt(i);
                }
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
        if (color == Color.clear)
        {
            float randR = (float)Random.RandomRange(0, 255) / 255;
            float randG = (float)Random.RandomRange(0, 255) / 255;
            float randB = (float)Random.RandomRange(0, 255) / 255;
            color = new Color(randR, randG, randB, 1);
        }

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

    // 저장 
    // 유저가 가지고있는 옷 정보들 저장 
    public void SaveSkinItem()
    {
        string tempSkinItem = "";
        for (int i = 0; i < skinItem.Count; i++)
        {
            tempSkinItem += skinItem[i].skinName + "-";
            tempSkinItem += skinItem[i].color_01 + "-";
            tempSkinItem += skinItem[i].color_02 + "=";
        }
        PlayerPrefs.SetString("SkinItem", tempSkinItem);
    }

    public void LoadSkinItem()
    {
        if (PlayerPrefs.HasKey("SkinItem"))
        {
            string tempSkinItem = PlayerPrefs.GetString("SkinItem");
            string[] SkinItemList = tempSkinItem.Split('=');
            List<UserSkin> userSkinList = new List<UserSkin>();
            for (int i = 0; i < SkinItemList.Length - 1; i++)
            {
                UserSkin tempUserSkin = new UserSkin(SkinItemList[i].Split('-')[0], StringToColor(SkinItemList[i].Split('-')[1]), StringToColor(SkinItemList[i].Split('-')[2]));
                userSkinList.Add(tempUserSkin);
            }
            skinItem = userSkinList;
        }
    }

    // 장착중인 스킨들 저장 
    public void SaveUserEqip()
    {
        string tempUserEqip = "";

        tempUserEqip += acc.skinKind.ToString() + "-";
        tempUserEqip += acc.skinName + "-";
        tempUserEqip += acc.color_01.ToString() + "-";
        tempUserEqip += acc.color_02.ToString() + "=";

        tempUserEqip += top.skinKind.ToString() + "-";
        tempUserEqip += top.skinName + "-";
        tempUserEqip += top.color_01.ToString() + "-";
        tempUserEqip += top.color_02.ToString() + "=";

        tempUserEqip += pan.skinKind.ToString() + "-";
        tempUserEqip += pan.skinName + "-";
        tempUserEqip += pan.color_01.ToString() + "-";
        tempUserEqip += pan.color_02.ToString() + "=";

        tempUserEqip += eye.skinKind.ToString() + "-";
        tempUserEqip += eye.skinName + "-";
        tempUserEqip += eye.color_01.ToString() + "-";
        tempUserEqip += eye.color_02.ToString() + "=";

        tempUserEqip += face.skinKind.ToString() + "-";
        tempUserEqip += face.skinName + "-";
        tempUserEqip += face.color_01.ToString() + "-";
        tempUserEqip += face.color_02.ToString() + "=";

        tempUserEqip += haF.skinKind.ToString() + "-";
        tempUserEqip += haF.skinName + "-";
        tempUserEqip += haF.color_01.ToString() + "-";
        tempUserEqip += haF.color_02.ToString() + "=";

        tempUserEqip += haB.skinKind.ToString() + "-";
        tempUserEqip += haB.skinName + "-";
        tempUserEqip += haB.color_01.ToString() + "-";
        tempUserEqip += haB.color_02.ToString() + "=";

        tempUserEqip += outt.skinKind.ToString() + "-";
        tempUserEqip += outt.skinName + "-";
        tempUserEqip += outt.color_01.ToString() + "-";
        tempUserEqip += outt.color_02.ToString() + "=";

        tempUserEqip += sho.skinKind.ToString() + "-";
        tempUserEqip += sho.skinName + "-";
        tempUserEqip += sho.color_01.ToString() + "-";
        tempUserEqip += sho.color_02.ToString() + "=";

        tempUserEqip += cap.skinKind.ToString() + "-";
        tempUserEqip += cap.skinName + "-";
        tempUserEqip += cap.color_01.ToString() + "-";
        tempUserEqip += cap.color_02.ToString() + "=";

        tempUserEqip += set.skinKind.ToString() + "-";
        tempUserEqip += set.skinName + "-";
        tempUserEqip += set.color_01.ToString() + "-";
        tempUserEqip += set.color_02.ToString() + "=";

        tempUserEqip += body.skinKind.ToString() + "-";
        tempUserEqip += body.skinName + "-";
        tempUserEqip += body.color_01.ToString() + "-";
        tempUserEqip += body.color_02.ToString() + "=";

        PlayerPrefs.SetString("UserEqip", tempUserEqip);
    }

    public void LoadUserEqip()
    {
        if (PlayerPrefs.HasKey("UserEqip"))
        {
            string tempUserEqip = PlayerPrefs.GetString("UserEqip");
            string[] tempUserEqipList = tempUserEqip.Split('=');
            acc = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[0].Split('-')[0]), tempUserEqipList[0].Split('-')[1], StringToColor(tempUserEqipList[0].Split('-')[2]), StringToColor(tempUserEqipList[0].Split('-')[3]));
            top = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[1].Split('-')[0]), tempUserEqipList[1].Split('-')[1], StringToColor(tempUserEqipList[1].Split('-')[2]), StringToColor(tempUserEqipList[1].Split('-')[3]));
            pan = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[2].Split('-')[0]), tempUserEqipList[2].Split('-')[1], StringToColor(tempUserEqipList[2].Split('-')[2]), StringToColor(tempUserEqipList[2].Split('-')[3]));
            eye = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[3].Split('-')[0]), tempUserEqipList[3].Split('-')[1], StringToColor(tempUserEqipList[3].Split('-')[2]), StringToColor(tempUserEqipList[3].Split('-')[3]));
            face = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[4].Split('-')[0]), tempUserEqipList[4].Split('-')[1], StringToColor(tempUserEqipList[4].Split('-')[2]), StringToColor(tempUserEqipList[4].Split('-')[3]));
            haF = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[5].Split('-')[0]), tempUserEqipList[5].Split('-')[1], StringToColor(tempUserEqipList[5].Split('-')[2]), StringToColor(tempUserEqipList[5].Split('-')[3]));
            haB = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[6].Split('-')[0]), tempUserEqipList[6].Split('-')[1], StringToColor(tempUserEqipList[6].Split('-')[2]), StringToColor(tempUserEqipList[6].Split('-')[3]));
            outt = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[7].Split('-')[0]), tempUserEqipList[7].Split('-')[1], StringToColor(tempUserEqipList[7].Split('-')[2]), StringToColor(tempUserEqipList[7].Split('-')[3]));
            sho = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[8].Split('-')[0]), tempUserEqipList[8].Split('-')[1], StringToColor(tempUserEqipList[8].Split('-')[2]), StringToColor(tempUserEqipList[8].Split('-')[3]));
            cap = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[9].Split('-')[0]), tempUserEqipList[9].Split('-')[1], StringToColor(tempUserEqipList[9].Split('-')[2]), StringToColor(tempUserEqipList[9].Split('-')[3]));
            set = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[10].Split('-')[0]), tempUserEqipList[10].Split('-')[1], StringToColor(tempUserEqipList[10].Split('-')[2]), StringToColor(tempUserEqipList[10].Split('-')[3]));
            body = new UserEqip((SkinKind)System.Enum.Parse(typeof(SkinKind), tempUserEqipList[11].Split('-')[0]), tempUserEqipList[11].Split('-')[1], StringToColor(tempUserEqipList[11].Split('-')[2]), StringToColor(tempUserEqipList[11].Split('-')[3]));
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