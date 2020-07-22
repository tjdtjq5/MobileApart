using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.CodeDom.Compiler;

public class Talk : MonoBehaviour
{
    public GameObject[] talkBullon;
    [Header("출력 대사 텍스트")]
    public string[] bullonText;
    string[] originText;

    float fadeSpeed = 0.3f;
    bool flag = false;

    [Header("다른 스크립트 불러오기")]
    public Bath bath;

    private void Start()
    {
        originText = new string[bullonText.Length];
        for (int i = 0; i < bullonText.Length; i++)
        {
            originText[i] = bullonText[i];
        }
    }

    
    public void TalkBtn(bool re = false)
    {
        if (!flag || re)
        {
            flag = true;
            for (int i = 0; i < bullonText.Length; i++)
            {
                talkBullon[i].SetActive(true);

                talkBullon[i].GetComponent<Button>().onClick.RemoveAllListeners();
                string tempText = bullonText[i];
                talkBullon[i].GetComponent<Button>().onClick.AddListener(() => TalkAction(tempText));

                talkBullon[i].transform.Find("Text").GetComponent<Text>().text = bullonText[i];

                Vector3 tempLocalSalce = talkBullon[i].transform.localScale;
                talkBullon[i].transform.localScale = new Vector2(tempLocalSalce.x * .7F, tempLocalSalce.y*.7F);
                talkBullon[i].transform.DOScale(tempLocalSalce, fadeSpeed);

                talkBullon[i].GetComponent<Image>().DOFade(0, 0);
                talkBullon[i].GetComponent<Image>().DOFade(1, fadeSpeed);
                for (int j = 0; j < talkBullon[i].transform.childCount; j++)
                {
                    if (talkBullon[i].transform.GetChild(j).GetComponent<Image>() != null)
                    {
                        talkBullon[i].transform.GetChild(j).GetComponent<Image>().DOFade(0, 0);
                        talkBullon[i].transform.GetChild(j).GetComponent<Image>().DOFade(1, fadeSpeed);
                    }
                    if (talkBullon[i].transform.GetChild(j).GetComponent<Text>() != null)
                    {
                        talkBullon[i].transform.GetChild(j).GetComponent<Text>().DOFade(0, 0);
                        talkBullon[i].transform.GetChild(j).GetComponent<Text>().DOFade(1, fadeSpeed);
                    }
                }
            }
        }
        else
        {
            flag = false;
            for (int i = 0; i < bullonText.Length; i++)
            {
                bullonText[i] = originText[i];
                talkBullon[i].SetActive(false);
            }
        }
    }

    public void TalkAction(string text)
    {
        switch (text)
        {
            case "상태어때?":
                List<string> tempString = new List<string>();
                tempString.Add("씻을까?");
                tempString.Add("놀까?");
                tempString.Add("잘래?");
                tempString.Add("먹을래?");
                TalkBullonTextChange(tempString);
                TalkBtn(true);
                break;
            case "씻을까?":
                TalkBtn();
                bath.BathOpen();
                break;
        }
    }

    public void TalkBullonTextChange(List<string> bullonText)
    {
        for (int i = 0; i < this.bullonText.Length; i++)
        {
            this.bullonText[i] = "";
        }

        for (int i = 0; i < bullonText.Count; i++)
        {
            this.bullonText[i] = bullonText[i];
        }
    }
}
