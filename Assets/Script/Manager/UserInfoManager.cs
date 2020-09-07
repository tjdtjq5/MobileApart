using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System.Collections;
using UnityEngine.Networking;

public class UserInfoManager : MonoBehaviour
{

    [HideInInspector] public string nickname;
    [HideInInspector] public string inDate;
    [HideInInspector] public string currentCharacter;
    [HideInInspector] public string currentAnimation;

    [ContextMenu("테스트")]
    public void Test()
    {
        Debug.Log(currentCharacter);
    
    }

    private void Start()
    {
        PublicInitialized();
    }

    public void PublicInitialized()
    {
        currentAnimation = "idle_01";

        for (int i = 0; i < 99; i++)
        {
            PushColorItem(Color.clear);
            PushColorItem(Color.white);
            PushColorItem(Color.yellow);
            PushColorItem(Color.black);
            PushColorItem(Color.grey);
        }

        PushWeapon("단검");

        ChangeStage("스테이지1");
    }

    //초기 
    public void Character01Initialized()
    {
        skinItem.Add(new UserSkin("Body", Color.white, Color.white));
        PushUserEqip(new UserSkin("Body", Color.white, Color.white));

        for (int i = 0; i < GameManager.instance.itemManager.itemList.Length; i++)
        {
            if (GameManager.instance.itemManager.itemList[i].itemKind == ItemKind.스킨)
            {
                string skinName = GameManager.instance.itemManager.itemList[i].itemName;
                Color color_01 = RandColor();
                Color color_02 = RandColor();
                PushSkinItem(new UserSkin(skinName, color_01, color_02));
                if (skinName == "unde/undedefault")
                {
                    PushUserEqip(new UserSkin(skinName, color_01, color_02));
                }
                if (skinName == "haF/hair_f_01")
                {
                    PushUserEqip(new UserSkin(skinName, color_01, color_02));
                }
                if (skinName == "haB/hair_b_01")
                {
                    PushUserEqip(new UserSkin(skinName, color_01, color_02));
                }
                if (skinName == "eye/eye_01")
                {
                    PushUserEqip(new UserSkin(skinName, color_01, color_02));
                }
                if (skinName == "face/face_01")
                {
                    PushUserEqip(new UserSkin(skinName, color_01, color_02));
                }
            }
        }

        PushSkinItem(new UserSkin("accneck/flowerribbon", new Color(117 / 255f, 197 / 255f, 69 / 255f, 1), new Color(244 / 255f, 53 / 255f, 53 / 255f, 1)));
        PushSkinItem(new UserSkin("set/mu", new Color(237 / 255f, 107 / 255f, 95 / 255f, 1), new Color(81 / 255f, 139 / 255f, 28 / 255f, 1)));
        PushSkinItem(new UserSkin("accbody/flowerribbon", new Color(70 / 255f, 129 / 255f, 73 / 255f, 1), Color.white));
        PushSkinItem(new UserSkin("sho/shoes_02", new Color(92 / 255f, 14 / 255f, 14 / 255f, 1), Color.clear));

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

    public List<UserSkin> GetSkinItemList(SkinKind skinKind)
    {
        List<UserSkin> tempUserSkin = new List<UserSkin>();
        for (int i = 0; i < skinItem.Count; i++)
        {
            SkinKind userEqipskinKind = (SkinKind)System.Enum.Parse(typeof(SkinKind), skinItem[i].skinName.Split('/')[0]);
            if (userEqipskinKind == skinKind)
            {
                tempUserSkin.Add(skinItem[i]);
            }
        }
        return tempUserSkin;
    }

    public List<int> GetSkinItemIndexList(SkinKind skinKind)
    {
        List<int> tempUserSkin = new List<int>();
        for (int i = 0; i < skinItem.Count; i++)
        {
            SkinKind userEqipskinKind = (SkinKind)System.Enum.Parse(typeof(SkinKind), skinItem[i].skinName.Split('/')[0]);
            if (userEqipskinKind == skinKind)
            {
               
                if (GameManager.instance.itemManager.GetItemInfo(skinItem[i].skinName).characterName.Contains(currentCharacter))
                {
                    tempUserSkin.Add(i);
                }
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
        int index = GetSkinItemIndex(userSkin);
        if (index == -1)
        {
            return;
        }

        for (int i = 0; i < skinItem.Count; i++)
        {
            if (skinItem[i].skinName == userSkin.skinName && skinItem[i].isEqip)
            {
                skinItem[i].SetEqip(false);
            }
        }
        skinItem[index].SetEqip(true);
    }

    public void PushUserEqip(int index)
    {
        if (index < 0 || index >= skinItem.Count)
        {
            return;
        }

        for (int i = 0; i < skinItem.Count; i++)
        {
            if (skinItem[i].skinName == skinItem[index].skinName && skinItem[i].isEqip)
            {
                skinItem[i].SetEqip(false);
            }
        }

        skinItem[index].SetEqip(true);
    }

    public List<UserSkin> GetUserEqip(SkinKind skinKind)
    {
        List<UserSkin> tempUserSkin = new List<UserSkin>();
        for (int i = 0; i < skinItem.Count; i++)
        {
            if (skinItem[i].isEqip)
            {
                SkinKind userEqipskinKind = (SkinKind)System.Enum.Parse(typeof(SkinKind), skinItem[i].skinName.Split('/')[0]);
                if (userEqipskinKind == skinKind)
                {
                    tempUserSkin.Add(skinItem[i]);
                }
            }
        }
        return tempUserSkin;
    }

    public void PullUserEqip(SkinKind skinKind)
    {
        if (skinKind == SkinKind.unde)
        {
            if (GetUserEqip(SkinKind.set).Count == 0)
            {
                OverrideCanvas.instance.RedAlram("세트 스킨을 장착해야 합니다.");
                return;
            }
        }
        if (skinKind == SkinKind.set)
        {
            if (GetUserEqip(SkinKind.unde).Count == 0)
            {
                int defalultUnde = GetSkinItemIndexList(SkinKind.unde)[0];
                PushUserEqip(defalultUnde);
            }
        }

        List<int> list = GetSkinItemIndexList(skinKind);

        for (int i = 0; i < list.Count; i++)
        {
            skinItem[list[i]].SetEqip(false);
        }
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
        string tempSkinItem = "";
        for (int i = 0; i < skinItem.Count; i++)
        {
            tempSkinItem += skinItem[i].isEqip + "=";
        }

        Param saveSkinData = new Param();
        saveSkinData.Add(currentCharacter + "Eqip", tempSkinItem);

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
                    skinItem[i].isEqip = bool.Parse(tempUserEqipList[i]);
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

    // 무기 코인 
    int weaponCoin;
    public void SetWeaponCoin(int weaponCoin)
    {
        this.weaponCoin = weaponCoin;
    }
    public int GetWeaponCoin()
    {
        return weaponCoin;
    }
    public void SaveWeaponCoin(System.Action action = null)
    {
        Param weaponCoinParam = new Param();
        weaponCoinParam.Add("WeaponCoin", GetWeaponCoin());

        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            // 이후 처리
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            string dataIndate = jsonData["inDate"]["S"].ToString();

            BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "UserInfo", dataIndate, weaponCoinParam, (callback2) =>
            {
                // 이후 처리
                if (action != null)
                {
                    action();
                }
            });
        });
    }
    public void LoadWeaponCoin(System.Action action = null)
    {
        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            if (jsonData.Keys.Contains("WeaponCoin"))
            {
                int weaponCoin = int.Parse(jsonData["WeaponCoin"][0].ToString());
                SetWeaponCoin(weaponCoin);
            }
            if (action != null)
            {

                action();
            }
        });
    }

    //무기 정보 
    public UserWeapon userWeapon = new UserWeapon();
    void SetUserWeapon(string weaponName, int enhance, int num)
    {
        userWeapon.weaponName = weaponName;
        userWeapon.enhance = enhance;
        userWeapon.num = num;
    }
    public void WeaponEnhance()
    {
        userWeapon.enhance++;
    }
    public void PushWeapon(string weaponName)
    {
        if (userWeapon.weaponName == weaponName)
        {
            userWeapon.num++;
        }
        else
        {
            SetUserWeapon(weaponName, 0, 1);
        }
    }
    public void SaveWeapon(System.Action action = null)
    {
        Param weaponParam = new Param();
        weaponParam.Add("WeaponName", userWeapon.weaponName);
        weaponParam.Add("WeaponEnhance", userWeapon.enhance);
        weaponParam.Add("WeaponNum", userWeapon.num);

        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            // 이후 처리
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            string dataIndate = jsonData["inDate"]["S"].ToString();

            BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "UserInfo", dataIndate, weaponParam, (callback2) =>
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
    public void LoadWeapon(System.Action action = null)
    {
        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            if (jsonData.Keys.Contains("WeaponName"))
            {
                string weaponName = jsonData["WeaponName"][0].ToString();
                userWeapon.weaponName = weaponName;
            }
            if (jsonData.Keys.Contains("WeaponEnhance"))
            {
                int enhance = int.Parse(jsonData["WeaponEnhance"][0].ToString());
                userWeapon.enhance = enhance;
            }
            if (jsonData.Keys.Contains("WeaponNum"))
            {
                int num = int.Parse(jsonData["WeaponNum"][0].ToString());
                userWeapon.num = num;
            }

            if (action != null)
            {
                action();
            }
        });
    }

    // 스테이지 정보 
    public UserStage userStage = new UserStage();
    void SetUserStage(string stageName , int hp)
    {
        userStage.stageName = stageName;
        userStage.hp = hp;
    }
    public void ChangeStage(string stageName)
    {
        SetUserStage(stageName, GameManager.instance.stageManager.GetStageInfo(stageName).hp);
    }
    public void SaveStage(System.Action action = null)
    {
        Param stageParam = new Param();
        stageParam.Add("StageName", userStage.stageName);
        stageParam.Add("StageHp", userStage.hp);

        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            // 이후 처리
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            string dataIndate = jsonData["inDate"]["S"].ToString();

            BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "UserInfo", dataIndate, stageParam, (callback2) =>
            {
                // 이후 처리
                if (action != null)
                {
                    action();
                }
            });
        });
    }
    public void LoadStage(System.Action action = null)
    {
        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            if (jsonData.Keys.Contains("StageName"))
            {
                string stageName = jsonData["StageName"][0].ToString();
                userStage.stageName = stageName;
            }
            if (jsonData.Keys.Contains("StageHp"))
            {
                int stageHp = int.Parse(jsonData["StageHp"][0].ToString());
                userStage.hp = stageHp;
            }

            if (action != null)
            {
                action();
            }
        });
    }
}

public class UserSkin
{
    public string skinName;
    public Color color_01;
    public Color color_02;
    public bool isEqip;

    public UserSkin()
    {
        this.skinName = "";
        this.color_01 = Color.white;
        this.color_02 = Color.white;
        this.isEqip = false;
    }

    public UserSkin(string skinName, Color color_01, Color color_02)
    {
        this.skinName = skinName;
        this.color_01 = color_01;
        this.color_02 = color_02;
        this.isEqip = false;
    }

    public void SetUserSkin(string skinName, Color color_01, Color color_02)
    {
        this.skinName = skinName;
        this.color_01 = color_01;
        this.color_02 = color_02;
        this.isEqip = false;
    }

    public void SetEqip(bool flag)
    {
        isEqip = flag;
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


public class UserMoney
{
    public int Gold;
    public int Crystal;
}


public class Need 
{
    // 즐거움, 포만감, 청결함, 활력
    public int pleasure;
    public int satiety;
    public int cleanliness;
    public int vitality;
} 

public class UserWeapon
{
    public string weaponName;
    public int enhance;
    public int num;
}

public class UserStage
{
    public string stageName;
    public int hp;
}