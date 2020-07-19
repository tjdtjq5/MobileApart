using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Spine.Unity;
using System.Collections.Generic;

public class Gift : MonoBehaviour
{
    public GfitKind[] giftList;

    public Sprite glodSprite;
    public Sprite crystalSprite;
    public GameObject alramPurchaseCheck;
    public Transform alramBoxOpen;
    public Transform moneyUIPannel;

    IEnumerator[] tempCoroutine;
    IEnumerator timeCheckCoroutine;

    //결과 담을 주머니
    List<AlramBox> alramBoxList = new List<AlramBox>();

    void OnEnable()
    {
        tempCoroutine = new IEnumerator[giftList.Length];
        Load();
    }

    private void OnDisable()
    {
        for (int i = 0; i < tempCoroutine.Length; i++)
        {
            if (tempCoroutine[i] != null)
            {
                StopCoroutine(tempCoroutine[i]);
            }
        }

        if (timeCheckCoroutine != null)
        {
            StopCoroutine(timeCheckCoroutine);
        }
    }

    public void GiftSetting(int index ,float streamTime)
    {
        MoneySetting();

        // 정보 입력
        giftList[index].gfitPannel.Find("상자이름 텍스트").GetComponent<Text>().text = giftList[index].gfitName;
        switch (giftList[index].moneyKind)
        {
            case MoneyKind.Gold:
                giftList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = glodSprite;
                giftList[index].gfitPannel.Find("상자열기").Find("10회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = glodSprite;
                break;
            case MoneyKind.Crystal:
                giftList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = crystalSprite;
                giftList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = crystalSprite;
                break;
        }
        giftList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("Text").GetComponent<Text>().text = giftList[index].price.ToString();
        giftList[index].gfitPannel.Find("상자열기").Find("10회 열기").Find("재화").Find("Text").GetComponent<Text>().text = (giftList[index].price * 10).ToString();
        // 상자 구매 버튼 넣기 
        int tempIndex = index;

        giftList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").GetComponent<Button>().onClick.RemoveAllListeners();
        giftList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").GetComponent<Button>().onClick.AddListener(() => PurchaseCheckOpen(tempIndex, 1));
        giftList[index].gfitPannel.Find("상자열기").Find("10회 열기").Find("재화").GetComponent<Button>().onClick.RemoveAllListeners();
        giftList[index].gfitPannel.Find("상자열기").Find("10회 열기").Find("재화").GetComponent<Button>().onClick.AddListener(() => PurchaseCheckOpen(tempIndex, 10));

        //버튼 넣기 
        float touchSecond = giftList[index].touchCountSecond;
        giftList[index].gfitPannel.Find("상자이미지").GetComponent<Button>().onClick.RemoveAllListeners();
        giftList[index].gfitPannel.Find("상자이미지").GetComponent<Button>().onClick.AddListener(() => ClickGfit(tempIndex, touchSecond));

        //슬라이드 코르틴 돌리기 
        tempCoroutine[index] = SliderCountCoroutine(giftList[index].gfitPannel.Find("슬라이더배경"), giftList[index].touchMaxCountSecond, streamTime, index);
        StartCoroutine(tempCoroutine[index]);
    }

    [System.Serializable]
    public struct GfitKind
    {
        public string gfitName;
        public Transform gfitPannel;
        public int price;
        public MoneyKind moneyKind;
        public float touchMaxCountSecond;
        public float touchCountSecond;
    }

    void MoneySetting()
    {
        moneyUIPannel.Find("보석").Find("Text").GetComponent<Text>().text = string.Format("{0:#,###0}", GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Crystal));
        moneyUIPannel.Find("골드").Find("Text").GetComponent<Text>().text = string.Format("{0:#,###0}", GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Gold));
    }


    IEnumerator SliderCountCoroutine(Transform sliderImg, float touchMaxCountSecond, float streamTime, int index)
    {
        // 슬라이더
        float maxValue = sliderImg.GetComponent<RectTransform>().rect.width;
        float streamValue = maxValue * (streamTime / touchMaxCountSecond);
        sliderImg.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(streamValue, sliderImg.GetChild(0).GetComponent<RectTransform>().rect.height);

        //남은시간 텍스트
        string initText = "무료로 열기까지 ";
        float remainTime = touchMaxCountSecond - streamTime;
        System.TimeSpan timespan = System.TimeSpan.FromSeconds(remainTime);
        int hour = timespan.Hours;
        int min = timespan.Minutes;
        int sec = timespan.Seconds;
        string remainText = initText + hour.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00");
        if (hour == 0)
        {
            remainText = initText + min.ToString("00") + ":" + sec.ToString("00");
        }
        sliderImg.GetChild(1).GetComponent<Text>().text = remainText;

        float waitTime = 0.02f;
        WaitForSeconds wait = new WaitForSeconds(waitTime);
        while (sliderImg.GetChild(0).GetComponent<RectTransform>().rect.width <= maxValue)
        {
            yield return wait;

            // 슬라이더
            streamValue += maxValue / touchMaxCountSecond * waitTime;
            sliderImg.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(streamValue, sliderImg.GetChild(0).GetComponent<RectTransform>().rect.height);

            //텍스트 
            remainTime -= waitTime;
            timespan = System.TimeSpan.FromSeconds(remainTime);
            hour = timespan.Hours;
            min = timespan.Minutes;
            sec = timespan.Seconds;
            remainText = initText + hour.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00");
            if (hour == 0)
                remainText = initText + min.ToString("00") + ":" + sec.ToString("00");
            sliderImg.GetChild(1).GetComponent<Text>().text = remainText;

            //남은시간 전역변수로
            PlayerPrefs.SetFloat("currentStreamTime" + index, touchMaxCountSecond - remainTime);
        }
        giftList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("Text").GetComponent<Text>().text = "무료";
        giftList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").GetComponent<Button>().onClick.AddListener(() => FreeGfit(index));
    }

    public void ClickGfit(int index, float clickTime)
    {
        if (tempCoroutine[index] == null)
            return;
        StopCoroutine(tempCoroutine[index]);

        float streamTime = PlayerPrefs.GetFloat("currentStreamTime" + index) + clickTime;
        if (streamTime > giftList[index].touchMaxCountSecond)
        {
            streamTime = giftList[index].touchMaxCountSecond;
        }

        tempCoroutine[index] = SliderCountCoroutine(giftList[index].gfitPannel.Find("슬라이더배경"), giftList[index].touchMaxCountSecond, streamTime, index);
        StartCoroutine(tempCoroutine[index]);

        giftList[index].gfitPannel.Find("상자이미지").Find("상자스파인").GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "touch", false);
    }


    public void FreeGfit(int index)
    {
        giftList[index].gfitPannel.Find("상자이미지").GetComponent<Button>().onClick.RemoveAllListeners();
        giftList[index].gfitPannel.Find("상자이미지").GetComponent<Button>().onClick.AddListener(() => ClickGfit(index, giftList[index].touchCountSecond));
        //슬라이드 코르틴 돌리기 
        if (tempCoroutine[index] != null)
            StopCoroutine(tempCoroutine[index]);

        tempCoroutine[index] = SliderCountCoroutine(giftList[index].gfitPannel.Find("슬라이더배경"), giftList[index].touchMaxCountSecond, 0, index);
        StartCoroutine(tempCoroutine[index]);

        GiftOpen(index, 1);
    }

    public void PurchaseCheckOpen(int index, int num)
    {
        // 유저가 가진 돈이 더 많거나 같다면 
        if (GameManager.instance.userInfoManager.GetUserMoney(giftList[index].moneyKind) >= giftList[index].price)
        {
            alramPurchaseCheck.SetActive(true);
            alramPurchaseCheck.transform.Find("확인").GetComponent<Button>().onClick.RemoveAllListeners();
            alramPurchaseCheck.transform.Find("확인").GetComponent<Button>().onClick.AddListener(()=> GiftOpen(index, num));

            alramPurchaseCheck.transform.Find("취소").GetComponent<Button>().onClick.RemoveAllListeners();
            alramPurchaseCheck.transform.Find("취소").GetComponent<Button>().onClick.AddListener(() => PurchaseCheckClose());
        }
        else // 돈이 없다면
        {

        }
    }

    public void PurchaseCheckClose()
    {
        alramPurchaseCheck.SetActive(false);
    }

    public void GiftOpen(int index , int num)
    {
        alramPurchaseCheck.SetActive(false);
        giftList[index].gfitPannel.Find("상자이미지").Find("상자스파인").GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "open", false);


        // 돈 차감 
        GameManager.instance.userInfoManager.SetUserMoney(giftList[index].moneyKind, GameManager.instance.userInfoManager.GetUserMoney(giftList[index].moneyKind) - giftList[index].price);
        MoneySetting();
        GameManager.instance.userInfoManager.SaveUserMoney();

        //알람박스 초기화
        alramBoxList.Clear();

        // 아이템 얻기 
        for (int i = 0; i < num; i++)
        {
            float randomPercent = Random.RandomRange(0, 1000000) / 10000f;

            float itemPercent = 0;
            int count = 0;

            while (itemPercent < randomPercent)
            {
                switch (index)
                {
                    case 0:
                        itemPercent += float.Parse(GameManager.instance.databaseManager.Box_DB.GetRowData(count)[3]);
                        break;
                    case 1:
                        itemPercent += float.Parse(GameManager.instance.databaseManager.Box_DB.GetRowData(count)[4]);
                        break;
                    case 2:
                        itemPercent += float.Parse(GameManager.instance.databaseManager.Box_DB.GetRowData(count)[5]);
                        break;
                }
                count++;
            }
            if (itemPercent != 0)
            {
                count--;
            }

            float randR = Random.RandomRange(0, 255) / (float)255;
            float randG = Random.RandomRange(0, 255) / (float)255;
            float randB = Random.RandomRange(0, 255) / (float)255;
            Color randomColor01 = new Color(randR, randG, randB, 1);
            float randR2 = Random.RandomRange(0, 255) / (float)255;
            float randG2 = Random.RandomRange(0, 255) / (float)255;
            float randB2 = Random.RandomRange(0, 255) / (float)255;
            Color randomColor02 = new Color(randR2, randG2, randB2, 1);

            ItemKind itemKind = GameManager.instance.itemManager.GetItemInfo(GameManager.instance.databaseManager.Box_DB.GetRowData(count)[1]).itemKind;

            switch (itemKind)
            {
                case ItemKind.골드:
                    break;
                case ItemKind.크리스탈:
                    break;
                case ItemKind.랜덤염색약:
                    GameManager.instance.userInfoManager.PushColorItem(Color.clear);
                    break;
                case ItemKind.염색약:
                    GameManager.instance.userInfoManager.PushColorItem(randomColor01);
                    break;
                case ItemKind.스킨:
                    GameManager.instance.userInfoManager.PushSkinItem(new UserSkin(GameManager.instance.databaseManager.Box_DB.GetRowData(count)[1], randomColor01, randomColor02));
                    break;
                default:
                    break;
            }

            alramBoxList.Add(new AlramBox(GameManager.instance.databaseManager.Box_DB.GetRowData(count)[1], GameManager.instance.databaseManager.Box_DB.GetRowData(count)[0], randomColor01, randomColor02));
        }

        //옷 저장
        GameManager.instance.userInfoManager.SaveSkinItem();

        alramBoxOpen.gameObject.SetActive(true);
        alramBoxOpen.Find("배경노란줄").Find("뽑기개수").GetComponent<Text>().text = 1 + " / " + alramBoxList.Count;
        alramBoxOpen.Find("배경노란줄").Find("아이템이름").GetComponent<Text>().text = GameManager.instance.itemManager.GetItemInfo(alramBoxList[0].itemName).inGameName;
        for (int i = 0; i < alramBoxOpen.Find("배경노란줄").Find("네모박스").childCount; i++)
        {
            Destroy(alramBoxOpen.Find("배경노란줄").Find("네모박스").GetChild(i).gameObject);
        }
        GameObject iconObj = Instantiate(GameManager.instance.itemManager.GetItemInfo(alramBoxList[0].itemName).iconObj, alramBoxOpen.Find("배경노란줄").Find("네모박스").position, Quaternion.identity, alramBoxOpen.Find("배경노란줄").Find("네모박스"));
        for (int i = 0; i < iconObj.transform.childCount; i++)
        {
            if (iconObj.transform.GetChild(i).name == "color_01")
            {
                iconObj.transform.GetChild(i).GetComponent<Image>().color = alramBoxList[0].color01;
            }
            if (iconObj.transform.GetChild(i).name == "color_02")
            {
                iconObj.transform.GetChild(i).GetComponent<Image>().color = alramBoxList[0].color02;
            }
        }
        
        alramBoxCount = 1;
        alramBoxOpen.Find("touchPannel").GetComponent<Button>().onClick.RemoveAllListeners();
        alramBoxOpen.Find("touchPannel").GetComponent<Button>().onClick.AddListener(() => AlramBoxOpen());
    }

    int alramBoxCount;
    void AlramBoxOpen()
    {
        if (alramBoxCount == alramBoxList.Count)
        {
            alramBoxOpen.gameObject.SetActive(false);
            return;
        }

        alramBoxOpen.Find("배경노란줄").Find("뽑기개수").GetComponent<Text>().text = 1 + " / " + alramBoxList.Count;
        alramBoxOpen.Find("배경노란줄").Find("아이템이름").GetComponent<Text>().text = GameManager.instance.itemManager.GetItemInfo(alramBoxList[alramBoxCount].itemName).inGameName;
        for (int i = 0; i < alramBoxOpen.Find("배경노란줄").Find("네모박스").childCount; i++)
        {
            Destroy(alramBoxOpen.Find("배경노란줄").Find("네모박스").GetChild(i).gameObject);
        }

        GameObject iconObj = Instantiate(GameManager.instance.itemManager.GetItemInfo(alramBoxList[alramBoxCount].itemName).iconObj, alramBoxOpen.Find("배경노란줄").Find("네모박스").position, Quaternion.identity, alramBoxOpen.Find("배경노란줄").Find("네모박스"));
        for (int i = 0; i < iconObj.transform.childCount; i++)
        {
            if (iconObj.transform.GetChild(i).name == "color_01")
            {
                iconObj.transform.GetChild(i).GetComponent<Image>().color = alramBoxList[alramBoxCount].color01;
            }
            if (iconObj.transform.GetChild(i).name == "color_02")
            {
                iconObj.transform.GetChild(i).GetComponent<Image>().color = alramBoxList[alramBoxCount].color02;
            }
        }
        alramBoxCount++;
    }

    

    WaitForSeconds secondTime = new WaitForSeconds(1f);
    IEnumerator TimeCheckSave(int totalSeconds)
    {
        int tempTotalSecond = totalSeconds;

        while (true)
        {
            yield return secondTime;
            tempTotalSecond++;
            PlayerPrefs.SetInt("net", tempTotalSecond);
        }
    }



    public void Load()
    {
        StartCoroutine(WebChk(() => {

            for (int i = 0; i < giftList.Length; i++)
            {
                if (PlayerPrefs.HasKey("currentStreamTime" + i))
                {
                    GiftSetting(i, PlayerPrefs.GetFloat("currentStreamTime" + i));
                }
                else
                {
                    GiftSetting(i, 0);
                }
            }

            int stopwatch = 0;
            if (PlayerPrefs.HasKey("net"))
            {
                stopwatch =
                       (int)timestamp.TotalSeconds - PlayerPrefs.GetInt("net");

                for (int i = 0; i < giftList.Length; i++)
                {

                    ClickGfit(i, stopwatch);

                }
            }

            timeCheckCoroutine = TimeCheckSave((int)timestamp.TotalSeconds);
            StartCoroutine(timeCheckCoroutine);
        }));
    }



    System.TimeSpan timestamp;

    IEnumerator WebChk(System.Action callback)
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get("www.naver.com"))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string date = request.GetResponseHeader("date");

                System.DateTime dateTime = System.DateTime.Parse(date).ToUniversalTime();

                timestamp = dateTime - new System.DateTime(1970, 1, 1, 0, 0, 0);

                callback();
            }
        }
    }
}

class AlramBox
{
    public string itemName;
    public string itemCode;
    public Color color01;
    public Color color02;

    public AlramBox(string itemName, string itemCode, Color color01, Color color02)
    {
        this.itemName = itemName;
        this.itemCode = itemCode;
        this.color01 = color01;
        this.color02 = color02;
    }
}

