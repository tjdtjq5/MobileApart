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
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
                loadtext.text = ((int)(progressbar.value * 100)).ToString() + "%";
            }



            if (progressbar.value >= 1f)
            {
                progress.SetActive(false);
            }

            if (Input.GetMouseButtonDown(0) && progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }

    }




    // 스킨 아이템 정보 받아오기 

}
