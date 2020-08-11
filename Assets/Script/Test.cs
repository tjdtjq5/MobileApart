using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CheckSecretShopTime());
    }

    IEnumerator CheckSecretShopTime()
    {
        yield return null;
        StartCoroutine(WebChk(() =>
        {
           
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
                Debug.Log(date);

                System.DateTime dateTime = System.DateTime.Parse(date).ToUniversalTime();

                timestamp = dateTime - new System.DateTime(1970, 1, 1, 0, 0, 0);

                callback();
            }
        }
    }
}
