using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Spine.Unity;
using Spine;
using Boo.Lang;

public class Gift : MonoBehaviour
{
    public GfitKind[] gfitList;

    public Sprite glodSprite;
    public Sprite crystalSprite;
    public GameObject alramPurchaseCheck;
    public Transform alramBoxOpen;
    public Transform moneyUIPannel;

    IEnumerator[] tempCoroutine;
    float[] currentStreamTime;

    private void Start()
    {
        tempCoroutine = new IEnumerator[gfitList.Length];
        currentStreamTime = new float[gfitList.Length];
        Load();
    }

    public void GiftSetting(int index ,float streamTime)
    {
        MoneySetting();

        // 정보 입력
        gfitList[index].gfitPannel.Find("상자이름 텍스트").GetComponent<Text>().text = gfitList[index].gfitName;
        switch (gfitList[index].moneyKind)
        {
            case MoneyKind.Gold:
                gfitList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = glodSprite;
                gfitList[index].gfitPannel.Find("상자열기").Find("10회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = glodSprite;
                break;
            case MoneyKind.Crystal:
                gfitList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = crystalSprite;
                gfitList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = crystalSprite;
                break;
        }
        gfitList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("Text").GetComponent<Text>().text = gfitList[index].price.ToString();
        gfitList[index].gfitPannel.Find("상자열기").Find("10회 열기").Find("재화").Find("Text").GetComponent<Text>().text = (gfitList[index].price * 10).ToString();
        // 상자 구매 버튼 넣기 
        int tempIndex = index;

        gfitList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").GetComponent<Button>().onClick.RemoveAllListeners();
        gfitList[index].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").GetComponent<Button>().onClick.AddListener(() => PurchaseCheckOpen(tempIndex, 1));
        gfitList[index].gfitPannel.Find("상자열기").Find("10회 열기").Find("재화").GetComponent<Button>().onClick.RemoveAllListeners();
        gfitList[index].gfitPannel.Find("상자열기").Find("10회 열기").Find("재화").GetComponent<Button>().onClick.AddListener(() => PurchaseCheckOpen(tempIndex, 10));

        //버튼 넣기 
        float touchSecond = gfitList[index].touchCountSecond;
        gfitList[index].gfitPannel.Find("상자이미지").GetComponent<Button>().onClick.RemoveAllListeners();
        gfitList[index].gfitPannel.Find("상자이미지").GetComponent<Button>().onClick.AddListener(() => ClickGfit(tempIndex, touchSecond));

        //슬라이드 코르틴 돌리기 
        tempCoroutine[index] = SliderCountCoroutine(gfitList[index].gfitPannel.Find("슬라이더배경"), gfitList[index].touchMaxCountSecond, streamTime, index);
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
            currentStreamTime[index] = touchMaxCountSecond - remainTime;
            PlayerPrefs.SetFloat("currentStreamTime" + index, currentStreamTime[index]);
        }

      //gfitList[index].gfitPannel.Find("상자열기").
    }

    public void ClickGfit(int index, float clickTime)
    {
        if (tempCoroutine[index] == null)
            return;
        StopCoroutine(tempCoroutine[index]);

        float streamTime = currentStreamTime[index] + clickTime;
        if (streamTime > gfitList[index].touchMaxCountSecond)
        {
            streamTime = gfitList[index].touchMaxCountSecond;
        }

        tempCoroutine[index] = SliderCountCoroutine(gfitList[index].gfitPannel.Find("슬라이더배경"), gfitList[index].touchMaxCountSecond, streamTime, index);
        StartCoroutine(tempCoroutine[index]);

        gfitList[index].gfitPannel.Find("상자이미지").Find("상자스파인").GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "touch", false);
    }


    public void FreeGfit()
    {

    }

    public void PurchaseCheckOpen(int index, int num)
    {
        // 유저가 가진 돈이 더 많거나 같다면 
        if (GameManager.instance.userInfoManager.GetUserMoney(gfitList[index].moneyKind) >= gfitList[index].price)
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
        gfitList[index].gfitPannel.Find("상자이미지").Find("상자스파인").GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "open", false);


        // 돈 차감 
        GameManager.instance.userInfoManager.SetUserMoney(gfitList[index].moneyKind, GameManager.instance.userInfoManager.GetUserMoney(gfitList[index].moneyKind) - gfitList[index].price);
        MoneySetting();
        GameManager.instance.userInfoManager.SaveUserMoney();

        // 아이템 얻기 
        for (int i = 0; i < num; i++)
        {
            float randomPercent = Random.RandomRange(0, 1000000) / 10000f;
            Debug.Log(randomPercent);
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


            // 스킨아이템 목록에 있다면 유저정보에 넣어라 
            for (int j = 0; j < GameManager.instance.spineSkinInfoManager.SpineSkinInfoList.Length; j++)
            {
                if (GameManager.instance.spineSkinInfoManager.SpineSkinInfoList[i].skinName == GameManager.instance.databaseManager.Box_DB.GetRowData(count)[2])
                {
                    GameManager.instance.userInfoManager.PushSkinItem(new UserSkin(GameManager.instance.databaseManager.Box_DB.GetRowData(count)[2], randomColor01, randomColor02));
                    break;
                }
            }
            // 랜덤 컬러아이템 
            if (GameManager.instance.databaseManager.Box_DB.GetRowData(count)[2] == "randomColorItem")
            {
                GameManager.instance.userInfoManager.PushColorItem(Color.clear);
            }
            // 컬러아이템 
            if (GameManager.instance.databaseManager.Box_DB.GetRowData(count)[2] == "colorItem")
            {
                GameManager.instance.userInfoManager.PushColorItem(randomColor01);
            }
        }
    }

    void AlramBoxOpen(List<AlramBox> alramBoxList)
    {
        alramBoxOpen.gameObject.SetActive(true);
        alramBoxOpen.Find("배경노란줄").Find("뽑기개수").GetComponent<Text>().text = 1 + " / " + alramBoxList.Count;
        alramBoxOpen.Find("배경노란줄").Find("아이템이름").GetComponent<Text>().text = alramBoxList[0].itemName;
      //  GameObject iconObj = Instantiate(GameManager.instance.spineSkinInfoManager.GetSpineSkinInfo(userSkinList[j].skinName).iconObj, prepab.transform.Find("ImgPos").position, Quaternion.identity, prepab.transform.Find("ImgPos"));
    }

    public void Save()
    {
        for (int i = 0; i < currentStreamTime.Length; i++)
        {
            PlayerPrefs.SetFloat("currentStreamTime" + i, currentStreamTime[i]);
        }

        StartCoroutine(WebChk(() => {
            PlayerPrefs.SetInt("net", (int)timestamp.TotalSeconds);
        }));

    }

    public void Load()
    {
        for (int i = 0; i < currentStreamTime.Length; i++)
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
        StartCoroutine(WebChk(() => {
            int stopwatch = 0;
            if (PlayerPrefs.HasKey("net"))
            {
                stopwatch =
                       (int)timestamp.TotalSeconds - PlayerPrefs.GetInt("net", (int)timestamp.TotalSeconds);
                for (int i = 0; i < gfitList.Length; i++)
                {
                    ClickGfit(i, stopwatch);
                }
            }

            Save();
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

