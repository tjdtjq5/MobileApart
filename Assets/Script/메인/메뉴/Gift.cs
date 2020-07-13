using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

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
        GiftSetting();
    }

    public void GiftSetting()
    {
        for (int i = 0; i < gfitList.Length; i++)
        {
            // 정보 입력
            gfitList[i].gfitPannel.Find("상자이름 텍스트").GetComponent<Text>().text = gfitList[i].gfitName;
            switch (gfitList[i].moneyKind)
            {
                case MoneyKind.Gold:
                    gfitList[i].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = glodSprite;
                    gfitList[i].gfitPannel.Find("상자열기").Find("10회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = glodSprite;
                    break;
                case MoneyKind.Crystal:
                    gfitList[i].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = crystalSprite;
                    gfitList[i].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("이미지").GetComponent<Image>().sprite = crystalSprite;
                    break;
            }
            gfitList[i].gfitPannel.Find("상자열기").Find("1회 열기").Find("재화").Find("Text").GetComponent<Text>().text = gfitList[i].price.ToString();
            gfitList[i].gfitPannel.Find("상자열기").Find("10회 열기").Find("재화").Find("Text").GetComponent<Text>().text = (gfitList[i].price * 10).ToString();

            //버튼 넣기 
            int index = i;
            float touchSecond = gfitList[i].touchCountSecond;
            gfitList[i].gfitPannel.Find("상자이미지").GetComponent<Button>().onClick.RemoveAllListeners();
            gfitList[i].gfitPannel.Find("상자이미지").GetComponent<Button>().onClick.AddListener(() => ClickGfit(index, touchSecond));
         
            //슬라이드 코르틴 돌리기 
            tempCoroutine[i] = SliderCountCoroutine(gfitList[i].gfitPannel.Find("슬라이더배경"), gfitList[i].touchMaxCountSecond, 300 , i);
            StartCoroutine(tempCoroutine[i]);
        }
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
           
        }
    }

    public void ClickGfit(int index, float clickTime)
    {
        Debug.Log(index);
        if (tempCoroutine[index] == null)
            return;
        StopCoroutine(tempCoroutine[index]);

        float streamTime = currentStreamTime[index] + clickTime;
        Debug.Log(currentStreamTime[index]);
        if (streamTime > gfitList[index].touchMaxCountSecond)
        {
            streamTime = gfitList[index].touchMaxCountSecond;
        }

        tempCoroutine[index] = SliderCountCoroutine(gfitList[index].gfitPannel.Find("슬라이더배경"), gfitList[index].touchMaxCountSecond, streamTime, index);
        StartCoroutine(tempCoroutine[index]);
    }
}


