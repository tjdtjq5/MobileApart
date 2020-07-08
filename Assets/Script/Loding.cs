using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BackEnd;
using LitJson;

public class Loding : MonoBehaviour
{
    public string nextScene;
    public Slider progressbar;
    public Text loadtext;
    public GameObject progress;

    public void Start()
    {
        LoadScene();
    }



    public void LoadScene()
    {
        progressbar.gameObject.SetActive(true);
        progressbar.value = 0;
        loadtext.gameObject.SetActive(true);
        StartCoroutine(LoadSceneCoroutine());
    }

    bool flag = false;
    IEnumerator LoadSceneCoroutine()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            if (progressbar.value < 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
                loadtext.text = ((int)(progressbar.value * 100)).ToString() + "%";
            }
            else if (operation.progress >= 0.9f)
            {
                loadtext.text = ((int)(progressbar.value * 100)).ToString() + "%";
                if (!flag)
                {
                    flag = true;
                    AsyncGetUserInfo();
                }
            }

       

            if (progressbar.value >= 1f)
            {
                progress.SetActive(false);
                loadtext.text = "준비완료";
            }

            if (Input.GetMouseButtonDown(0) && progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }

    }


    string nickName = "";
    string inDate = "";

    // 닉네임 정보 받아오기 순서 1 
    public void AsyncGetUserInfo()
    {
        BackendAsyncClass.BackendAsync(Backend.BMember.GetUserInfo, (callback) =>
        {
            JsonData returnJsonData = callback.GetReturnValuetoJSON();
            if (returnJsonData.Keys.Contains("row"))
            {
                JsonData row = returnJsonData["row"];
                inDate = row["inDate"].ToString();
                nickName = row["nickname"].ToString();
                GameManager.instance.userInfoManager.nickname = nickName;
                GameManager.instance.userInfoManager.inDate = inDate;
                Debug.Log("닉네임 받아오기 완료");
                progressbar.value = 0.92f;
                loadtext.text = ((int)(progressbar.value * 100)).ToString() + "%";
            }
        });
    }

    // 스킨 아이템 정보 받아오기 

}
