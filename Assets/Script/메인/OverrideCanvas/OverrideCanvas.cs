using DG.Tweening;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverrideCanvas : MonoBehaviour
{
    public static OverrideCanvas instance;

    public GameObject donTouchPannel;
    public Transform alram;
    public Transform redAlram;
    public Transform caution;
    public Transform purchaseAlram;
    public Transform polaroid;
    public Transform screenPhoto;

    void Start()
    {
        instance = this;
    }

    public void Alram(string text)
    {
        donTouchPannel.SetActive(true);

        alram.gameObject.SetActive(true);
        alram.GetComponent<Image>().DOFade(1, 0);
        alram.transform.GetChild(0).GetComponent<Text>().DOFade(1, 0);
        alram.transform.GetChild(0).GetComponent<Text>().text = text;
        alram.GetComponent<Image>().DOFade(0, 1.3f).OnComplete(() => { alram.gameObject.SetActive(false); donTouchPannel.SetActive(false); });
        alram.transform.GetChild(0).GetComponent<Text>().DOFade(0, 1.3f);
    }

    public void RedAlram(string text)
    {
        donTouchPannel.SetActive(true);

        redAlram.gameObject.SetActive(true);
        redAlram.GetComponent<Image>().DOFade(1, 0);
        redAlram.transform.GetChild(0).GetComponent<Text>().DOFade(1, 0);
        redAlram.transform.GetChild(0).GetComponent<Text>().text = text;
        redAlram.GetComponent<Image>().DOFade(0, 1.3f).OnComplete(() => { redAlram.gameObject.SetActive(false); donTouchPannel.SetActive(false); });
        redAlram.transform.GetChild(0).GetComponent<Text>().DOFade(0, 1.3f);
    }

    public void Caution(string text, System.Action function)
    {
        donTouchPannel.SetActive(true);

        caution.gameObject.SetActive(true);
        caution.Find("Text").GetComponent<Text>().text = text;
        caution.Find("확인").GetComponent<Button>().onClick.RemoveAllListeners();
        caution.Find("확인").GetComponent<Button>().onClick.AddListener(() => { function(); donTouchPannel.SetActive(false); });
        caution.Find("취소").GetComponent<Button>().onClick.RemoveAllListeners();
        caution.Find("취소").GetComponent<Button>().onClick.AddListener(() => { donTouchPannel.SetActive(false); });
    }

    public void PurchaseAlram(System.Action function)
    {
        donTouchPannel.SetActive(true);

        purchaseAlram.gameObject.SetActive(true);
        purchaseAlram.Find("확인").GetComponent<Button>().onClick.RemoveAllListeners();
        purchaseAlram.Find("확인").GetComponent<Button>().onClick.AddListener(() => { function(); donTouchPannel.SetActive(false); });
        caution.Find("취소").GetComponent<Button>().onClick.RemoveAllListeners();
        caution.Find("취소").GetComponent<Button>().onClick.AddListener(() => { donTouchPannel.SetActive(false); });
    }

    public void PolaroidPhoto(Sprite screenShot, int month, int day)
    {
        polaroid.gameObject.SetActive(true);
        polaroid.Find("ScreenShot").GetComponent<Image>().sprite = screenShot;
        polaroid.Find("ScreenShot").GetComponent<Button>().onClick.RemoveAllListeners();
        polaroid.Find("ScreenShot").GetComponent<Button>().onClick.AddListener(() => { ScreenPhoto(screenShot); });
        polaroid.Find("날짜").GetComponent<Text>().text = month + "월" + day + "일";
        polaroid.Find("Skip").GetComponent<Button>().onClick.RemoveAllListeners();
        polaroid.Find("Skip").GetComponent<Button>().onClick.AddListener(() => {
            polaroid.gameObject.SetActive(false);
        });
    }

    public void ScreenPhoto(Sprite screenShot)
    {
        screenPhoto.gameObject.SetActive(true);

        screenPhoto.GetChild(0).GetComponent<Image>().sprite = screenShot;
        screenPhoto.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        screenPhoto.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { screenPhoto.gameObject.SetActive(false); });
    }
}
