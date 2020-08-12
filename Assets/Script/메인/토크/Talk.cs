using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Talk : MonoBehaviour
{
    [Header("토크 풍선 오브젝트")]
    public GameObject[] talkBullon;
    [Header("기타 스크립트")]
    public CharacterInfo characterInfo;
    public Sleep sleep;
    public Cook cook;

    string[] bullonText;
    float fadeSpeed = 0.3f;
    bool flag = false;

    [Header("다른 스크립트 불러오기")]
    public Bath bath;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        if (GameManager.instance.userInfoManager.GetUserNeed(characterInfo.theWorstNeed()) < 20)
        {
            bullonText = new string[3];
            bullonText[0] = "잡담";
            switch (characterInfo.theWorstNeed())
            {
                case NeedKind.즐거움:
                    bullonText[1] = "놀까?";
                    break;
                case NeedKind.포만감:
                    bullonText[1] = "먹을래?";
                    break;
                case NeedKind.청결함:
                    bullonText[1] = "씻을까?";
                    break;
                case NeedKind.활력:
                    bullonText[1] = "잘래?";
                    break;
            }
            bullonText[2] = "상태어때?";
        }
        else
        {
            bullonText = new string[2];
            bullonText[0] = "잡담";
            bullonText[1] = "상태어때?";
        }

       

    }

    
    public void TalkBtn(bool re = false)
    {
        if (!flag)
        {
            Init();
        }

        for (int i = 0; i < talkBullon.Length; i++)
        {
            talkBullon[i].SetActive(false);
        }

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
        }
    }

    public void TalkAction(string text)
    {
        List<string> tempString = new List<string>();
        switch (text)
        {
            case "상태어때?":
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
            case "놀까?":
                tempString.Add("가위바위보");
                TalkBullonTextChange(tempString);
                TalkBtn(true);
                break;
            case "잘래?":
                ScreenTrans.instance.Play(()=> { sleep.SleepOpen(); });
                TalkBtn();
                break;
            case "먹을래?":
                ScreenTrans.instance.Play(() => { cook.CookOpen(); });
                TalkBtn();
                break;
            case "가위바위보":
                RockPaperScissors.instance.RockPaperScissorsOpen();
                TalkBtn();
                break;
        }
    }

    public void TalkBullonTextChange(List<string> bullonText)
    {

        this.bullonText = new string[bullonText.Count];

        for (int i = 0; i < bullonText.Count; i++)
        {
            this.bullonText[i] = bullonText[i];
        }
    }
}
