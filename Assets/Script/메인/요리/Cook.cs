using DG.Tweening;
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

    public void CookOpen()
    {
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(false);
        }

        pannel.SetActive(true);
        select.SetActive(true);

        mobileBlur.enabled = true;
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
    }

    public void Select(int index)
    {
        switch (index)
        {
            case 0: // 인공재료 
                selectString = "인공재료";
                break;
            case 1: // 기본재료
                selectString = "기본재료";
                break;
            case 2: // 고급재료
                selectString = "고급재료";
                break;
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
                GamePlay();
                break;
            case 1: // 볼일을 보러 간다
                CoolClose();
                break;
        }
    }

    public void GamePlay()
    {
        if (CountDownCoroutine != null)
        {
            StopCoroutine(CountDownCoroutine);
        }
        slider.Find("foreground").GetComponent<Image>().fillAmount = 0;
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
            yield return waitTime;
        }
    }

    public void Stop()
    {
        stop.GetComponent<Animator>().SetTrigger("play");

        if (slider.Find("foreground").GetComponent<Image>().fillAmount != 0)
        {
            GamePlay();
        }
    }
}
