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
    public Transform moneyUIPannel;

    IEnumerator[] tempCoroutine;
    IEnumerator timeCheckCoroutine;
    IEnumerator GiftOpenCoroutine;

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

        if (GiftOpenCoroutine != null)
        {
            StopCoroutine(GiftOpenCoroutine);
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

        giftList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").GetComponent<Button>().onClick.RemoveAllListeners();
        giftList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").GetComponent<Button>().onClick.AddListener(() => PurchaseCheckOpen(index, 1));
        giftList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("Text").GetComponent<Text>().text = giftList[index].price.ToString();
    }

    public void PurchaseCheckOpen(int index, int num)
    {
        // 유저가 가진 돈이 더 많거나 같다면 
        if (GameManager.instance.userInfoManager.GetUserMoney(giftList[index].moneyKind) >= giftList[index].price)
        {
            OverrideCanvas.instance.PurchaseAlram(() => {
                // 돈 차감 
                GameManager.instance.userInfoManager.SetUserMoney(giftList[index].moneyKind, GameManager.instance.userInfoManager.GetUserMoney(giftList[index].moneyKind) - giftList[index].price);
                MoneySetting();
                GameManager.instance.userInfoManager.SaveUserMoney();

                GiftOpen(index, num); });
        }
        else // 돈이 없다면
        {
            OverrideCanvas.instance.RedAlram("돈이 부족합니다.");
        }
    }

    public void GiftOpen(int index , int num)
    {
        if (GiftOpenCoroutine != null)
        {
            StopCoroutine(GiftOpenCoroutine);
        }
        GiftOpenCoroutine = TempGiftOpenCoroutine(index, num);
        StartCoroutine(GiftOpenCoroutine);
    }

    IEnumerator TempGiftOpenCoroutine(int index, int num)
    {
        giftList[index].gfitPannel.Find("상자이미지").Find("상자스파인").GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "open", false);
        yield return new WaitForSeconds(1);

        List<UserSkin> boxItem = new List<UserSkin>();
        List<int> boxNum = new List<int>();

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

            boxItem.Add(new UserSkin(GameManager.instance.databaseManager.Box_DB.GetRowData(count)[1], randomColor01, randomColor02));
            boxNum.Add(1);
        }

        // 골드와 크리스탈 시행 
        int goldCount = 0;
        int totalGold = 0;
        for (int i = 0; i < num; i++)
        {
            // n%확률로 골드 얻기
            int randomGold = Random.RandomRange(0, 100);
            if (randomGold < 60)
            {
                goldCount++;
                totalGold += Random.RandomRange(500, 1000);
            }
        }
        if (goldCount > 0)
        {
            boxItem.Add(new UserSkin("Gold", Color.white, Color.white));
            boxNum.Add(totalGold);
        }


        int crystalCount = 0;
        int totalCrystal = 0;
        for (int i = 0; i < num; i++)
        {
            // n%확률로 크리스탈 얻기
            int randomCrystal = Random.RandomRange(0, 100);
            if (randomCrystal < 60)
            {
                crystalCount++;
                totalCrystal += Random.RandomRange(500, 1000);
            }
        }
        if (crystalCount > 0)
        {
            boxItem.Add(new UserSkin("Crystal", Color.white, Color.white));
            boxNum.Add(totalCrystal);
        }

        OverrideCanvas.instance.GetItem(boxItem, boxNum);
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

    System.TimeSpan timestamp;

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

