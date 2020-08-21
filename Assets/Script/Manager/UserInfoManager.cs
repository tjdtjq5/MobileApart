using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class UserInfoManager : MonoBehaviour
{

    [HideInInspector] public string nickname;
    [HideInInspector] public string inDate;
    [HideInInspector] public string currentCharacter;
    [HideInInspector] public string currentAnimation;


    //현재 유저가 장착중인 옷
    public UserEqip userEqip = new UserEqip();

    private void Start()
    {
        PullCurrentAni();

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
    public void Initialized()
    {
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

        SetUserNeed(100, 100, 100, 100);
    }

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

    public void PushSkinItem(UserSkin userSkin)
    {
        skinItem.Add(userSkin);
    }

    public Color RandColor()
    {
        float randR = Random.RandomRange(0, 255) /(float)255;
        float randG = Random.RandomRange(0, 255) / (float)255;
        float randB = Random.RandomRange(0, 255) / (float)255;

        return new Color(randR, randG, randB , 1);
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

        int skinkindLength = System.Enum.GetNames(typeof(SkinKind)).Length;

        for (int k = 0; k < skinkindLength; k++)
        {
            if ((SkinKind)k == skinKind)
            {
                string skinName = userSkin.skinName;
                Color color_01 = userSkin.color_01;
                Color color_02 = userSkin.color_02;

                for (int i = 0; i < userEqip.skinKind.Length; i++)
                {
                    if (userEqip.skinKind[i] == skinKind)
                    {
                        userEqip.skinName[i] = skinName;
                        userEqip.color_01[i] = color_01;
                        userEqip.color_02[i] = color_02;
                    }
                }
            }
        }
    }

    public UserSkin GetUserEqip(SkinKind skinKind)
    {
        for (int i = 0; i < userEqip.skinKind.Length; i++)
        {
            if (userEqip.skinKind[i] == skinKind)
            {
                UserSkin tempSkin = new UserSkin(userEqip.skinName[i], userEqip.color_01[i], userEqip.color_02[i]);
               
                return tempSkin;
            }
        }
        return null;
    }

    // 저장 
    // 유저가 가지고있는 옷 정보들 저장 
    public void SaveSkinItem(System.Action action = null)
    {
        string tempSkinItem = "";
        for (int i = 0; i < skinItem.Count; i++)
        {
            tempSkinItem += skinItem[i].skinName + "-";
            tempSkinItem += skinItem[i].color_01 + "-";
            tempSkinItem += skinItem[i].color_02 + "=";
        }

        Param saveSkinData = new Param();
        saveSkinData.Add("SkinItem", tempSkinItem);

        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            // 이후 처리
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            string dataIndate = jsonData["inDate"]["S"].ToString();

            BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "UserInfo", dataIndate, saveSkinData, (callback2) =>
            {
                Debug.Log("성공했습니다");

                // 이후 처리
                if (action != null)
                {

                    action();
                }
            });
        });
    }

    public void LoadSkinItem(System.Action action = null)
    {
        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            // 이후 처리
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            if (jsonData.Keys.Contains("SkinItem"))
            {
                string tempSkinItem = jsonData["SkinItem"][0].ToString();
                string[] SkinItemList = tempSkinItem.Split('=');
                List<UserSkin> userSkinList = new List<UserSkin>();
                for (int i = 0; i < SkinItemList.Length - 1; i++)
                {
                    UserSkin tempUserSkin = new UserSkin(SkinItemList[i].Split('-')[0], StringToColor(SkinItemList[i].Split('-')[1]), StringToColor(SkinItemList[i].Split('-')[2]));
                    userSkinList.Add(tempUserSkin);
                }
                skinItem = userSkinList;

                if (action != null)
                {
                    action();
                }
            }
        });
    }

    // 장착중인 스킨들 저장 
    public void SaveUserEqip(string currentCharacter ,System.Action action = null)
    {
        string tempUserEqip = "";

        for (int i = 0; i < userEqip.skinKind.Length; i++)
        {
            tempUserEqip += userEqip.skinKind[i].ToString() + "-";
            tempUserEqip += userEqip.skinName[i] + "-";
            tempUserEqip += userEqip.color_01[i].ToString() + "-";
            tempUserEqip += userEqip.color_02[i].ToString() + "=";
        }

        Param saveEqipData = new Param();
        saveEqipData.Add( currentCharacter + "Eqip", tempUserEqip);

        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            // 이후 처리
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            string dataIndate  = jsonData["inDate"]["S"].ToString();

            BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "UserInfo", dataIndate, saveEqipData, (callback2) =>
            {
                Debug.Log("성공했습니다");

                // 이후 처리
                if (action != null)
                {
                    action();
                }
            });
        });
    }

    public void LoadUserEqip(string currentCharacter ,System.Action action = null)
    {
        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            // 이후 처리
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            if (jsonData.Keys.Contains( currentCharacter + "Eqip"))
            {
                string tempUserEqip = jsonData[currentCharacter+ "Eqip"][0].ToString();
                string[] tempUserEqipList = tempUserEqip.Split('=');

                if (action != null)
                {
                    action();
                }

                for (int i = 0; i < tempUserEqipList.Length; i++)
                {
                    if (tempUserEqipList[i].Split('-')[1].Length != 0)
                    {
                        PushUserEqip(new UserSkin(tempUserEqipList[i].Split('-')[1], StringToColor(tempUserEqipList[i].Split('-')[2]), StringToColor(tempUserEqipList[i].Split('-')[3])));
                    }
                }
               
            }
        });
    
    }

    // 유저 재화 
    public UserMoney userMoney = new UserMoney();

    public void SetUserMoney(MoneyKind moneyKind, int money)
    {
        switch (moneyKind)
        {
            case MoneyKind.Gold:
                userMoney.Gold = money;
                break;
            case MoneyKind.Crystal:
                userMoney.Crystal = money;
                break;
        }
    }

    public int GetUserMoney(MoneyKind moneyKind)
    {
        switch (moneyKind)
        {
            case MoneyKind.Gold:
                return userMoney.Gold;
            case MoneyKind.Crystal:
                return userMoney.Crystal;
        }
        return 0;
    }

    public void SaveUserMoney(System.Action action = null)
    {
        Param moneyData = new Param();
        moneyData.Add("Gold", GetUserMoney(MoneyKind.Gold));
        moneyData.Add("Crystal", GetUserMoney(MoneyKind.Crystal));

        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            // 이후 처리
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            string dataIndate = jsonData["inDate"]["S"].ToString();

            BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "UserInfo", dataIndate, moneyData, (callback2) =>
            {
                Debug.Log("성공했습니다");

                // 이후 처리
                if (action != null)
                {
                    action();
                }
            });
        });
    }

    public void LoadUserMoney(System.Action action = null)
    {
        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            if (jsonData.Keys.Contains("Gold"))
            {
                int Gold = int.Parse(jsonData["Gold"][0].ToString());
                SetUserMoney(MoneyKind.Gold, Gold);
            }
            if (jsonData.Keys.Contains("Crystal"))
            {
                int Crystal = int.Parse(jsonData["Crystal"][0].ToString());
                SetUserMoney(MoneyKind.Crystal, Crystal);
            }

            if (action != null)
            {
                action();
            }
        });
    }

    public Need userNeed = new Need();
    float pleasureTime = 216f; //6시간
    float satietyTime = 532f; //12시간
    float cleanlinessTime = 1064f; //24시간
    float vitalityTime = 1064f; //24시간

    public void SetUserNeed(int pleasure, int satiety, int cleanliness, int vitality)
    {
        if (pleasure < 0)
            pleasure = 0;
        if (pleasure > 100)
            pleasure = 100;
        if (satiety < 0)
            satiety = 0;
        if (satiety > 100)
            satiety = 100;
        if (cleanliness < 0)
            cleanliness = 0;
        if (cleanliness > 100)
            cleanliness = 100;
        if (vitality < 0)
            vitality = 0;
        if (vitality > 100)
            vitality = 100;


        userNeed.pleasure = pleasure;
        userNeed.satiety = satiety;
        userNeed.cleanliness = cleanliness;
        userNeed.vitality = vitality;

        StartCoroutine(UserNeedStream(NeedKind.즐거움, pleasureTime));
        StartCoroutine(UserNeedStream(NeedKind.청결함, satietyTime));
        StartCoroutine(UserNeedStream(NeedKind.포만감, cleanlinessTime));
        StartCoroutine(UserNeedStream(NeedKind.활력, vitalityTime));
    }

    IEnumerator UserNeedStream(NeedKind needKind, float time)
    {
        if (!PlayerPrefs.HasKey(needKind.ToString() + "Time"))
        {
            PlayerPrefs.SetFloat(needKind.ToString() + "Time", time);
        }

        WaitForSeconds loopTime = new WaitForSeconds(1f);

        while (true)
        {
            yield return loopTime;
            PlayerPrefs.SetFloat(needKind.ToString() + "Time", PlayerPrefs.GetFloat(needKind.ToString() + "Time") - 1);
            if (PlayerPrefs.GetFloat(needKind.ToString() + "Time") < 0)
            {
                PlayerPrefs.SetFloat(needKind.ToString() + "Time", time);

                switch (needKind)
                {
                    case NeedKind.즐거움:
                        SetUserNeed(userNeed.pleasure--, userNeed.satiety, userNeed.cleanliness, userNeed.vitality);
                        break;
                    case NeedKind.포만감:
                        SetUserNeed(userNeed.pleasure, userNeed.satiety--, userNeed.cleanliness, userNeed.vitality);
                        break;
                    case NeedKind.청결함:
                        SetUserNeed(userNeed.pleasure, userNeed.satiety, userNeed.cleanliness--, userNeed.vitality);
                        break;
                    case NeedKind.활력:
                        SetUserNeed(userNeed.pleasure, userNeed.satiety, userNeed.cleanliness, userNeed.vitality--);
                        break;
                }
                SaveUserNeed(currentCharacter);
            }
        }
    }


    public int GetUserNeed(NeedKind needKind)
    {
        switch (needKind)
        {
            case NeedKind.즐거움:
                return userNeed.pleasure;
            case NeedKind.포만감:
                return userNeed.satiety;
            case NeedKind.청결함:
                return userNeed.cleanliness;
            case NeedKind.활력:
                return userNeed.vitality;
        }
        return -1;
    }

    public void SaveUserNeed(string currentCharacter, System.Action action = null)
    {
        StartCoroutine(NeedTime(() => {
            string dataString = userNeed.pleasure + "-" + userNeed.satiety + "-" + userNeed.cleanliness + "-" + userNeed.vitality;
            int second = (int)needTimestamp.TotalSeconds;

            Param dataParam = new Param();
            dataParam.Add(currentCharacter + "Need", dataString);
            dataParam.Add(currentCharacter + "NeedTime", second);

            BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
            {
                // 이후 처리
                JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
                string dataIndate = jsonData["inDate"]["S"].ToString();

                BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "UserInfo", dataIndate, dataParam, (callback2) =>
                {
                    Debug.Log("성공했습니다");

                    // 이후 처리
                    if (action != null)
                    {
                        action();
                    }
                });
            });
        }));
    }

    public void LoadUserNeed(string currentCharacter, System.Action action = null)
    {
        StartCoroutine(NeedTime(() => {
            BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
            {
                // 이후 처리
                JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
                if (jsonData.Keys.Contains(currentCharacter + "Need"))
                {
                    string tempUserNeed = jsonData[currentCharacter + "Need"][0].ToString();
                    int tempStreamTime = (int)needTimestamp.TotalSeconds - int.Parse(jsonData[currentCharacter + "NeedTime"][0].ToString());
                  
                    string[] tempUserNeedList = tempUserNeed.Split('-');
                    int tempPleasure = int.Parse(tempUserNeedList[0]) - (int)(tempStreamTime / pleasureTime);
                    int tempSatiety = int.Parse(tempUserNeedList[1]) - (int)(tempStreamTime / satietyTime);
                    int tempCleanliness = int.Parse(tempUserNeedList[2]) - (int)(tempStreamTime / cleanlinessTime);
                    int tempVitality = int.Parse(tempUserNeedList[3]) - (int)(tempStreamTime / vitalityTime);

                    SetUserNeed(tempPleasure, tempSatiety, tempCleanliness, tempVitality);

                    if (action != null)
                    {
                        action();
                    }
                }
            });
        }));
    }

    System.TimeSpan needTimestamp;
    IEnumerator NeedTime(System.Action callback)
    {
        UnityWebRequest request = new UnityWebRequest();
        bool flag = false;
        using (request = UnityWebRequest.Get("www.naver.com"))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                flag = true;

                string date = request.GetResponseHeader("date");

                System.DateTime dateTime = System.DateTime.Parse(date).ToUniversalTime();

                needTimestamp = dateTime - new System.DateTime(1970, 1, 1, 0, 0, 0);

            }
        }
        if (flag)
        {
            callback();
        }
    }

    public void PullCurrentAni()
    {
        if (!PlayerPrefs.HasKey("CurrentAni"))
        {
            PlayerPrefs.SetString("CurrentAni", "idle_01");
            currentAnimation = "idle_01";
            return;
        }
        currentAnimation = PlayerPrefs.GetString("CurrentAni");
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
    public SkinKind[] skinKind;
    public string[] skinName;
    public Color[] color_01;
    public Color[] color_02;

    public UserEqip()
    {
        int enumLength = System.Enum.GetNames(typeof(SkinKind)).Length;
        skinKind = new SkinKind[enumLength];
        skinName = new string[enumLength];
        color_01 = new Color[enumLength];
        color_02 = new Color[enumLength];

        for (int i = 0; i < skinKind.Length; i++)
        {
            skinKind[i] = (SkinKind)i;
        }

        for (int i = 0; i < skinName.Length; i++)
        {
            skinName[i] = "";
        }

        for (int i = 0; i < color_01.Length; i++)
        {
            color_01[i] = Color.clear;
        }
        for (int i = 0; i < color_02.Length; i++)
        {
            color_02[i] = Color.clear;
        }
    }

   
}

public class UserMoney
{
    public int Gold;
    public int Crystal;
}

public class Status
{
    // 지혜 마력 의지 
    public int wisdom;
    public int intelligent;
    public int will; 
}

public class Need 
{
    // 즐거움, 포만감, 청결함, 활력
    public int pleasure;
    public int satiety;
    public int cleanliness;
    public int vitality;
} 