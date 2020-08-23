﻿using DG.Tweening;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverrideCanvas : MonoBehaviour
{
    public static OverrideCanvas instance;

    public Transform theCam; Vector2 originPos;

    public GameObject donTouchPannel;
    public Transform alram;
    public Transform redAlram;
    public Transform caution;
    public Transform purchaseAlram;
    public Transform polaroid;
    public Transform screenPhoto;
    public Transform wating;

    void Start()
    {
        instance = this;
        originPos = theCam.position;
    }

    public void Alram(string text)
    {
        this.transform.position = originPos;
        this.transform.position = new Vector2(this.transform.position.x + theCam.position.x - originPos.x, this.transform.position.y + theCam.position.y - originPos.y);

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
        this.transform.position = originPos;
        this.transform.position = new Vector2(this.transform.position.x + theCam.position.x - originPos.x, this.transform.position.y + theCam.position.y - originPos.y);

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
        this.transform.position = originPos;
        this.transform.position = new Vector2(this.transform.position.x + theCam.position.x - originPos.x, this.transform.position.y + theCam.position.y - originPos.y);

        donTouchPannel.SetActive(true);

        caution.gameObject.SetActive(true);
        caution.Find("Text").GetComponent<Text>().text = text;
        caution.Find("확인").GetComponent<Button>().onClick.RemoveAllListeners();
        caution.Find("확인").GetComponent<Button>().onClick.AddListener(() => { function(); donTouchPannel.SetActive(false); caution.gameObject.SetActive(false); });
        caution.Find("취소").GetComponent<Button>().onClick.RemoveAllListeners();
        caution.Find("취소").GetComponent<Button>().onClick.AddListener(() => { donTouchPannel.SetActive(false); caution.gameObject.SetActive(false); });
    }

    public void PurchaseAlram(System.Action function)
    {
        this.transform.position = originPos;
        this.transform.position = new Vector2(this.transform.position.x + theCam.position.x - originPos.x, this.transform.position.y + theCam.position.y - originPos.y);

        donTouchPannel.SetActive(true);

        purchaseAlram.gameObject.SetActive(true);
        purchaseAlram.Find("확인").GetComponent<Button>().onClick.RemoveAllListeners();
        purchaseAlram.Find("확인").GetComponent<Button>().onClick.AddListener(() => { function(); donTouchPannel.SetActive(false); purchaseAlram.gameObject.SetActive(false); });
        caution.Find("취소").GetComponent<Button>().onClick.RemoveAllListeners();
        caution.Find("취소").GetComponent<Button>().onClick.AddListener(() => { donTouchPannel.SetActive(false); purchaseAlram.gameObject.SetActive(false); });
    }

    public void PolaroidPhoto(Sprite screenShot, int month, int day)
    {
        this.transform.position = originPos;
        this.transform.position = new Vector2(this.transform.position.x + theCam.position.x - originPos.x, this.transform.position.y + theCam.position.y - originPos.y);

        if (screenShot == null)
        {
            Debug.Log("aa");
        }
        Debug.Log("bb");
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
        this.transform.position = originPos;
        this.transform.position = new Vector2(this.transform.position.x + theCam.position.x - originPos.x, this.transform.position.y + theCam.position.y - originPos.y);

        screenPhoto.gameObject.SetActive(true);

        screenPhoto.GetChild(1).GetComponent<Image>().sprite = screenShot;
        screenPhoto.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        screenPhoto.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { screenPhoto.gameObject.SetActive(false); });
    }

    public void Wating(string text, bool flag)
    {
        this.transform.position = originPos;
        this.transform.position = new Vector2(this.transform.position.x + theCam.position.x - originPos.x, this.transform.position.y + theCam.position.y - originPos.y);

        wating.gameObject.SetActive(flag);
        wating.GetChild(0).GetChild(0).GetComponent<Text>().text = text;
    }
}
