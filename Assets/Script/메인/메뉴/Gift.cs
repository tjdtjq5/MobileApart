using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using Spine;
using Spine.Unity;

public class Gift : MonoBehaviour
{
    public GfitKind[] gfitList;
    public Sprite glodSprite;
    public Sprite crystalSprite;

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

        //버튼 넣기 
        int tempIndex = index;
        float touchSecond = gfitList[index].touchCountSecond;
        gfitList[index].gfitPannel.Find("상자이미지").GetComponent<Button>().onClick.RemoveAllListeners();
        gfitList[index].gfitPannel.Find("상자이미지").GetComponent<Button>().onClick.AddListener(() => ClickGfit(tempIndex, touchSecond));

        //슬라이드 코르틴 돌리기 
        tempCoroutine[index] = SliderCountCoroutine(gfitList[index].gfitPannel.Find("슬라이더배경"), gfitList[index].touchMaxCountSecond, streamTime, index);
        StartCoroutine(tempCoroutine[index]);
    }

    [Serializable]
    public struct GfitKind
    {
        public string gfitName;
        public Transform gfitPannel;
        public int price;
        public MoneyKind moneyKind;
        public float touchMaxCountSecond;
        public float touchCountSecond;
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
        TimeSpan timespan = TimeSpan.FromSeconds(remainTime);
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
            timespan = TimeSpan.FromSeconds(remainTime);
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
    TimeSpan timestamp;

    IEnumerator WebChk(Action callback)
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

                DateTime dateTime = DateTime.Parse(date).ToUniversalTime();

                timestamp = dateTime - new DateTime(1970, 1, 1, 0, 0, 0);

                callback();
            }
        }
    }

}


