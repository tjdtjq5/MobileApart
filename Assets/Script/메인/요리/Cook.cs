using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cook : MonoBehaviour
{
    [Header("SetOff")]
    public GameObject[] setOff;

    [Header("Mobile Blur")]
    public MobileBlur mobileBlur;
    public GameObject pannel;

    [Header("식자선택")]
    public GameObject select;
    string selectString;

    [Header("플레이유무")]
    public GameObject playBtn;

    [Header("게임 진행")]
    public GameObject[] game;

    [Header("카운트다운")]
    public GameObject countDown;
    IEnumerator CountDownCoroutine;

    [Header("슬라이더")]
    public Transform slider;

    [Header("정지")]
    public Transform stop;

    [Header("진행도")]
    public GameObject gameIndex01;
    public GameObject gameIndex02;
    public GameObject gameIndex03;

    [Header("캐릭터 애니")]
    public TransformSkin transformSkin;

    [Header("다이아")]
    public Image[] dia;

    int gameIndex;
    int score;

    public void CookOpen()
    {
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(false);
        }

        pannel.SetActive(true);
        select.SetActive(true);

        mobileBlur.enabled = true;

        score = 0;
    }

    public void CoolClose()
    {
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }

        pannel.SetActive(false);
        select.SetActive(false);
        playBtn.SetActive(false);

        for (int i = 0; i < game.Length; i++)
        {
            game[i].SetActive(false);
        }

        if (CountDownCoroutine != null)
        {
            StopCoroutine(CountDownCoroutine);
        }

        mobileBlur.enabled = false;

        int randomFood = Random.RandomRange(0, 4);
        string food = "";
        switch (randomFood)
        {
            case 0:
                food = "food/apple";
                break;
            case 1:
                food = "food/banana";
                break;
            case 2:
                food = "food/sandwich1";
                break;
            case 3:
                food = "food/sandwich2";
                break;
        }
        transformSkin.Animation("eating_food", 5.5f, false, food);

        if (score > 20)
            score = 20;

        GameManager.instance.userInfoManager.SetUserNeed(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움),
                   GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감) + score,
                   GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함),
                   GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력));

        GameManager.instance.userInfoManager.SaveUserNeed(GameManager.instance.userInfoManager.currentCharacter);
    }

    public void Select(int index)
    {
        int needP = 0;
        switch (index)
        {
            case 0: // 인공재료 
                selectString = "인공재료";
                needP = 50;
                break;
            case 1: // 기본재료
                selectString = "기본재료";
                int userGold = GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Gold);
                if (userGold < 100000000)
                {
                    OverrideCanvas.instance.RedAlram("골드가 부족합니다.");
                    return;
                }
                GameManager.instance.userInfoManager.SetUserMoney(MoneyKind.Gold, userGold - 1000);
                GameManager.instance.userInfoManager.SaveUserMoney();
                needP = 70;
                break;
            case 2: // 고급재료
                selectString = "고급재료";
                int userCrystal = GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Crystal);
                if (userCrystal < 1000)
                {
                    OverrideCanvas.instance.RedAlram("크리스탈이 부족합니다.");
                    return;
                }
                GameManager.instance.userInfoManager.SetUserMoney(MoneyKind.Gold, userCrystal - 1000);
                GameManager.instance.userInfoManager.SaveUserMoney();
                needP = 100;
                GameManager.instance.userInfoManager.SetUserNeed(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움),
                                                             needP,
                                                             GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함),
                                                             GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력));

                GameManager.instance.userInfoManager.SaveUserNeed(GameManager.instance.userInfoManager.currentCharacter);
                CoolClose();
                return;
        }

        if (GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감) < needP)
        {
            GameManager.instance.userInfoManager.SetUserNeed(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움),
                                                             needP,
                                                             GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함),
                                                             GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력));

            GameManager.instance.userInfoManager.SaveUserNeed(GameManager.instance.userInfoManager.currentCharacter);
        }

        select.SetActive(false);
        playBtn.SetActive(true);
    }

    public void PlayBtn(int index)
    {
        playBtn.SetActive(false);
        switch (index)
        {
            case 0: // 직접요리
                for (int i = 0; i < game.Length; i++)
                {
                    game[i].SetActive(true);
                }
                gameIndex = 0;
                GamePlay(0);
                break;
            case 1: // 볼일을 보러 간다
                CoolClose();
                break;
        }
    }

    public void GamePlay(int index)
    {
        switch (index)
        {
            case 0:
                gameIndex01.SetActive(true);
                gameIndex02.SetActive(false);
                gameIndex03.SetActive(false);
                break;
            case 1:
                gameIndex01.SetActive(false);
                gameIndex02.SetActive(true);
                gameIndex03.SetActive(false);
                break;
            case 2:
                gameIndex01.SetActive(false);
                gameIndex02.SetActive(false);
                gameIndex03.SetActive(true);
                break;
            default:
                CoolClose();
                return;
        }
        if (CountDownCoroutine != null)
        {
            StopCoroutine(CountDownCoroutine);
        }
        slider.Find("foreground").GetComponent<Image>().fillAmount = 0;
        for (int i = 0; i < dia.Length; i++)
        {
            dia[i].color = new Color(142 / 255f, 139 / 255f, 143 / 255f, 1);
        }
        CountDownCoroutine = CountDown();
        StartCoroutine(CountDownCoroutine);
    }

    IEnumerator CountDown()
    {
 

        countDown.SetActive(true);
        countDown.transform.GetChild(0).GetComponent<Text>().text = "3";
        yield return new WaitForSeconds(1f);
        countDown.transform.GetChild(0).GetComponent<Text>().text = "2";
        yield return new WaitForSeconds(1f);
        countDown.transform.GetChild(0).GetComponent<Text>().text = "1";
        yield return new WaitForSeconds(1f);
        countDown.SetActive(false);

        WaitForSeconds waitTime = new WaitForSeconds(0.02f);
        while (slider.Find("foreground").GetComponent<Image>().fillAmount != 1)
        {
            slider.Find("foreground").GetComponent<Image>().fillAmount += 0.015f;

            if (slider.Find("foreground").GetComponent<Image>().fillAmount >= 0)
            {
                dia[0].color = new Color(0, 213 / 255f, 165 / 255f, 1);
            }
            if (slider.Find("foreground").GetComponent<Image>().fillAmount >= 0.3)
            {
                dia[1].color = new Color(0, 213 / 255f, 165 / 255f, 1);
            }
            if (slider.Find("foreground").GetComponent<Image>().fillAmount >= 0.7)
            {
                dia[2].color = new Color(0, 213 / 255f, 165 / 255f, 1);
            }
            if (slider.Find("foreground").GetComponent<Image>().fillAmount >= 0.8)
            {
                dia[3].color = new Color(0, 213 / 255f, 165 / 255f, 1);
            }
            if (slider.Find("foreground").GetComponent<Image>().fillAmount >= 1)
            {
                dia[4].color = new Color(0, 213 / 255f, 165 / 255f, 1);
            }
            yield return waitTime;
        }
        Stop();
    }

    public void Stop()
    {
        stop.GetComponent<Animator>().SetTrigger("play");

        int tempScore = 0;
        if (slider.Find("foreground").GetComponent<Image>().fillAmount != 0)
        {
            if (slider.Find("foreground").GetComponent<Image>().fillAmount <= 0.8) // 베스트
            {
                tempScore = 7;
            }
            if (slider.Find("foreground").GetComponent<Image>().fillAmount <= 0.7) // 노멀
            {
                tempScore = 5;
            }
            if (slider.Find("foreground").GetComponent<Image>().fillAmount <= 0.3) // 워스트
            {
                tempScore = 3;
            }
            score += tempScore;
            gameIndex++;
            GamePlay(gameIndex);
        }
    }
}
