using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopPannel;
    public Transform context;
    public Sprite goldSprite;
    public Sprite crystalSprite;

    public DailyGoods dailyGoods;
    public RandomGoods[] randomGoods;

    IEnumerator DailyChkCoroutine;

    [System.Serializable]
    public struct DailyGoods
    {
        public string GoodsName;
        public Sprite GoodsSprite;
        [Header("상품 설명")]
        [Multiline(3)]
        public string explanation;
        [Header("판매 종료 날짜")]
        public int year;
        public int month;
        public int day;
        [Header("가격")]
        public MoneyKind MoneyKind;
        public int price;
    }

    [System.Serializable]
    public struct RandomGoods
    {
        public string GoodsName;
        public GameObject GoodsObj;
        [Header("상품 설명")]
        [Multiline(3)]
        public string explanation;
        [Header("가격")]
        public MoneyKind MoneyKind;
        public int price;
    }

    public void ShopOpen()
    {
        shopPannel.gameObject.SetActive(true);
        SetMoneyText();
        SetDailyGoods();
    }

    public void ShopClose()
    {
        shopPannel.gameObject.SetActive(false);

        if (DailyChkCoroutine != null)
        {
            StopCoroutine(DailyChkCoroutine);
        }
    }

    public void SetMoneyText()
    {
        shopPannel.transform.Find("재화UI").Find("골드").GetChild(0).GetComponent<Text>().text = string.Format("{0:#,###0}", GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Gold));
        shopPannel.transform.Find("재화UI").Find("보석").GetChild(0).GetComponent<Text>().text = string.Format("{0:#,###0}", GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Crystal));
    }
 
    public void SetDailyGoods()
    {
        context.Find("데일리 상품").Find("상품그림자").GetComponent<Image>().sprite = dailyGoods.GoodsSprite;
        context.Find("데일리 상품").Find("상품그림자").GetChild(0).GetComponent<Image>().sprite = dailyGoods.GoodsSprite;
        context.Find("데일리 상품").Find("상품설명").GetComponent<Text>().text = dailyGoods.explanation;
        DailyChkCoroutine = DailyChk();
        StartCoroutine(DailyChkCoroutine); // 남은시간 계산
        switch (dailyGoods.MoneyKind)
        {
            case MoneyKind.Gold:
                context.Find("데일리 상품").Find("재화").GetComponent<Image>().sprite = goldSprite;
                context.Find("데일리 상품").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(229 / 255f, 191 / 255f, 0, 1);
                break;
            case MoneyKind.Crystal:
                context.Find("데일리 상품").Find("재화").GetComponent<Image>().sprite = crystalSprite;
                context.Find("데일리 상품").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(69 / 255f, 237 / 255f, 234/255f, 1);
                break;
        }
        context.Find("데일리 상품").Find("재화").GetChild(0).GetComponent<Text>().text = string.Format("{0:#,###0}", dailyGoods.price);
    }

    IEnumerator DailyChk()
    {
        while (true)
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
                    TimeSpan timestamp = new DateTime(dailyGoods.year, dailyGoods.month, dailyGoods.day, 0, 0, 0) - dateTime;
                    context.Find("데일리 상품").Find("판매종료시간").GetComponent<Text>().text = "판매종료 " + timestamp.Days + "일 " + timestamp.Hours + "시간 " + timestamp.Minutes + "분 남음";
                }
            }

            yield return new WaitForSeconds(10f);
        }
      
    }
}

public enum MoneyKind
{
    Gold,
    Crystal
}
