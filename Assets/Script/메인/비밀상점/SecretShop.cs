using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SecretShop : MonoBehaviour
{
    public Button btn;
    public Text timeText;
    IEnumerator tempCoroutine;

    [Header("SetOn")]
    public GameObject[] setOn;
    [Header("SetOff")]
    public GameObject[] setOff;

    private void OnEnable()
    {
        tempCoroutine = CheckSecretShopTime();
        StartCoroutine(tempCoroutine);
    }

    private void OnDisable()
    {
        StopCoroutine(tempCoroutine);
    }

    System.TimeSpan timestamp;
    string week;

    IEnumerator CheckSecretShopTime()
    {
        yield return null;
        int openTime = 13;
        int closeTime = 15;
        StartCoroutine(WebChk(() =>
        {
            if (week == "Mon" || week == "Wed" || week == "Sat")
            {
                int hour = timestamp.Hours;

                if (openTime <= hour && hour <= closeTime)
                {
                    btn.gameObject.SetActive(true);
                    btn.GetComponent<Image>().color = Color.white;
                    timeText.text = "OPEN";
                    return;
                }

                if (hour < openTime)
                {
                    btn.gameObject.SetActive(true);
                    btn.GetComponent<Image>().color = Color.gray;
                    hour = openTime - hour - 1;
                    int minute = 60 - timestamp.Minutes;
                    timeText.text = hour + " : " + minute;
                    return;
                }
            }

            btn.gameObject.SetActive(false);
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
                week = date.Split(',')[0];

                System.DateTime dateTime = System.DateTime.Parse(date);

                timestamp = dateTime - new System.DateTime(1970, 1, 1, 0, 0, 0);

                callback();
            }
        }
    }

    public void OpenSecretShop()
    {
        if (btn.GetComponent<Image>().color == Color.white)
        {
            return;
        }

        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].SetActive(true);
        }
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(false);
        }
    }

    public void CloseSecretShop()
    {
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }
        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].SetActive(false);
        }
    }
}
