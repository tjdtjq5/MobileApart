using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Mail : MonoBehaviour
{
    public Transform mail;
    public GameObject post;
    public GameObject notice;
    public GameObject releaseNote;

    public void MailOpen()
    {
        mail.gameObject.SetActive(true);
        post.SetActive(true);
        notice.SetActive(false);
        releaseNote.SetActive(false);
    }

    public void MailClose()
    {
        mail.gameObject.SetActive(false);
    }

    public void Post()
    {
        post.SetActive(true);
        notice.SetActive(false);
        releaseNote.SetActive(false);
    }

    public void Notice()
    {
        post.SetActive(false);
        notice.SetActive(true);
        releaseNote.SetActive(false);
    }

    public void ReleaseNote()
    {
        post.SetActive(false);
        notice.SetActive(false);
        releaseNote.SetActive(true);
    }

    public void PostCheck(int index)
    {
        BackendAsyncClass.BackendAsync(Backend.Social.Post.GetPostListV2, callback =>
        {
            JsonData jsonData = callback.GetReturnValuetoJSON()["fromAdmin"][index];
            if (jsonData.Keys.Contains("content"))
            {
                //Indate
                string Indate = jsonData["inDate"][0].ToString();
                Debug.Log(" Indate : = " + Indate);

                //아이템 정보 
                string[] temIitemNameList = jsonData["item"][0][1][0].ToString().Split('=');
                List<string> itemNameList = new List<string>();
                List<int> itemNumList = new List<int>();
                for (int i = 0; i < temIitemNameList.Length; i++)
                {
                    string tempItemName = temIitemNameList[i].Split('-')[0];
                    int tempItemNum = int.Parse(temIitemNameList[i].Split('-')[1]);
                    itemNameList.Add(tempItemName);
                    itemNumList.Add(tempItemNum);
                }

                //타이틀
                string title = jsonData["title"][0].ToString();

                //내용
                string content = jsonData["content"][0].ToString();
            }
        });
    }

    public void PostPull(string indate)
    {
        BackendAsyncClass.BackendAsync(Backend.Social.Post.ReceiveAdminPostItemV2, indate, callback =>
        {
            //아이템 정보 
            string[] temIitemNameList = callback.GetReturnValuetoJSON()[0][0][1][0].ToString().Split('=');
            List<string> itemNameList = new List<string>();
            List<int> itemNumList = new List<int>();
            for (int i = 0; i < temIitemNameList.Length; i++)
            {
                string tempItemName = temIitemNameList[i].Split('-')[0];
                int tempItemNum = int.Parse(temIitemNameList[i].Split('-')[1]);
                itemNameList.Add(tempItemName);
                itemNumList.Add(tempItemNum);
            }

            for (int i = 0; i < itemNameList.Count; i++)
            {
                Debug.Log(itemNameList[i]);
            }

            Debug.Log("수령완료");
        });
    }

    public void NoticeCheck(int index)
    {
        BackendAsyncClass.BackendAsync(Backend.Notice.NoticeList, 2, (callback) =>
        {
            // 이후 처리
            if (callback.IsSuccess())
            {
                JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][index];
                if (jsonData.Keys.Contains("content"))
                {
                    string content = jsonData["content"][0].ToString();
                    Debug.Log(content);

                    string inDate = jsonData["inDate"][0].ToString();
                    Debug.Log(inDate);

                    string imageKey = jsonData["imageKey"][0].ToString();
                    Debug.Log(imageKey);
                  //  StartCoroutine(WWWImageDown("http://upload-console.thebackend.io" + imageKey));

                    string title = jsonData["title"][0].ToString();
                    Debug.Log(title);
                }
            }
        });
    }


    Sprite sprite;
    IEnumerator WWWImageDown(string url)
    {
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();

        if (!(wr.isNetworkError || wr.isHttpError))
        {
            if (texDl.texture != null)
            {
                Texture2D t = texDl.texture;
                Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
                sprite = s;

            }
        }

        else
        {
            Debug.LogError(wr.error);
        }
    }
}
