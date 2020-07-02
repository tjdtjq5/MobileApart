using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class Cloth : MonoBehaviour
{
    [Header("옷장")]
    public Transform clothPannel;
    public GameObject clothBtn;
    public GameObject backBtn;
    public Transform context;     
    [Header("카메라")]
    public Camera characterCamera;         Vector2 originChracterCamera;
    public Camera uiCamera;                Vector2 originUiCamera;
    float cameraMoveSpeed = 0.3f;
    [Header("그림자")] 
    public Transform shadow;               Vector2 originShadow;
    [Header("아틀라스")]
    public SpriteAtlas atlas;
    [Header("아이콘프리팹")]
    public GameObject stage_01_btn_Prepab;
    public GameObject stage_02_btn_Prepab;

    int currentState;

    int[] selectBtnNum = new int[5];
    string[] selectString = new string[5];

    private void Start()
    {
        originChracterCamera = characterCamera.transform.position;
        originUiCamera = uiCamera.transform.position;
        originShadow = shadow.transform.position;
    }

    bool clickFlag;
    void ClickFlagFalse()
    {
        clickFlag = false;
    }

    float uiCamMoveX = 16f;
    float characterCamMoveX = .85f;
    float shadowMoveX = .85f;

    public void ClothOpen()
    {
        if (currentState == 0 && !clickFlag)
        {
            currentState = 1;

            backBtn.SetActive(true);
            clothBtn.SetActive(false);
            StartCoroutine(Open_01_Coroutine(() =>
            {
                context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, 0);

                // 스킨 종류가 리스트에 담긴다
                List<SkinKind> tempSkinKindList = GameManager.instance.spineSkinInfoManager.GetSkinKindList();
                for (int i = 0; i < tempSkinKindList.Count; i++)
                {
                    GameObject prepab = null;
                    if (tempSkinKindList[i] != SkinKind.body)
                    {
                        prepab = Instantiate(stage_01_btn_Prepab, context.position, Quaternion.identity, context);
                        SkinKind tempSkinkind = tempSkinKindList[i];
                        prepab.GetComponent<Button>().onClick.AddListener(() => { State01_Btn(tempSkinkind); });
                    }
                    switch (tempSkinKindList[i])
                    {
                        case SkinKind.acc:
                            prepab.transform.Find("Image").GetComponent<Image>().sprite = atlas.GetSprite("icon_dress_acce");
                            prepab.transform.Find("Text").GetComponent<Text>().text = "악세사리";
                            break;
                        case SkinKind.top:
                            prepab.transform.Find("Image").GetComponent<Image>().sprite = atlas.GetSprite("icon_dress_top");
                            prepab.transform.Find("Text").GetComponent<Text>().text = "상의";
                            break;
                        case SkinKind.pan:
                            prepab.transform.Find("Image").GetComponent<Image>().sprite = atlas.GetSprite("icon_dress_skirt");
                            prepab.transform.Find("Text").GetComponent<Text>().text = "하의";
                            break;
                        case SkinKind.eye:
                            prepab.transform.Find("Image").GetComponent<Image>().sprite = atlas.GetSprite("icon_dress_eye");
                            prepab.transform.Find("Text").GetComponent<Text>().text = "눈";
                            break;
                        case SkinKind.face:
                            prepab.transform.Find("Image").GetComponent<Image>().sprite = atlas.GetSprite("icon_dress_eye");
                            prepab.transform.Find("Text").GetComponent<Text>().text = "얼굴";
                            break;
                        case SkinKind.haF:
                            prepab.transform.Find("Image").GetComponent<Image>().sprite = atlas.GetSprite("icon_dress_hair");
                            prepab.transform.Find("Text").GetComponent<Text>().text = "앞머리";
                            break;
                        case SkinKind.haB:
                            prepab.transform.Find("Image").GetComponent<Image>().sprite = atlas.GetSprite("icon_dress_hair");
                            prepab.transform.Find("Text").GetComponent<Text>().text = "뒷머리";
                            break;
                        case SkinKind.outt:
                            prepab.transform.Find("Image").GetComponent<Image>().sprite = atlas.GetSprite("icon_dress_set");
                            prepab.transform.Find("Text").GetComponent<Text>().text = "외투";
                            break;
                        case SkinKind.sho:
                            prepab.transform.Find("Image").GetComponent<Image>().sprite = atlas.GetSprite("icon_dress_shoes");
                            prepab.transform.Find("Text").GetComponent<Text>().text = "신발";
                            break;
                        case SkinKind.cap:
                            prepab.transform.Find("Image").GetComponent<Image>().sprite = atlas.GetSprite("icon_dress_cap");
                            prepab.transform.Find("Text").GetComponent<Text>().text = "모자";
                            break;
                        case SkinKind.set:
                            prepab.transform.Find("Image").GetComponent<Image>().sprite = atlas.GetSprite("icon_dress_set");
                            prepab.transform.Find("Text").GetComponent<Text>().text = "세트";
                            break;
                    }
                    context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * 116f);
                }
            }));
        }
    }

    IEnumerator Open_01_Coroutine(Action callBack = null)
    {
        clickFlag = true; // 클릭 방지

        if (callBack != null)
        {
            callBack();
        }
        characterCamera.transform.DOMoveX(characterCamMoveX, cameraMoveSpeed);
        uiCamera.transform.DOMoveX(uiCamMoveX, cameraMoveSpeed);
        shadow.transform.DOMoveX(shadowMoveX, cameraMoveSpeed);
        yield return new WaitForSeconds(cameraMoveSpeed);

        clickFlag = false; // 클릭 방지
    }

    IEnumerator Open_02_Coroutine(Action callBack = null)
    {
        clickFlag = true;
        characterCamera.transform.DOMoveX(originChracterCamera.x, cameraMoveSpeed);
        uiCamera.transform.DOMoveX(originUiCamera.x, cameraMoveSpeed);
        shadow.transform.DOMoveX(originShadow.x, cameraMoveSpeed);
        yield return new WaitForSeconds(cameraMoveSpeed);
        if (callBack != null)
        {
            callBack();
        }
        characterCamera.transform.DOMoveX(characterCamMoveX, cameraMoveSpeed);
        uiCamera.transform.DOMoveX(uiCamMoveX, cameraMoveSpeed);
        shadow.transform.DOMoveX(shadowMoveX, cameraMoveSpeed);
        yield return new WaitForSeconds(cameraMoveSpeed);
        clickFlag = false;
    }

    public void State01_Btn(SkinKind skinKind)
    {
        currentState = 2;

        StartCoroutine(Open_02_Coroutine(() => {
            for (int i = 0; i < context.childCount; i++)
            {
                Destroy(context.GetChild(i));
            }
        }));

        context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, 0);
        List<SpineSkinInfo> tempSpineSkinInfo = GameManager.instance.spineSkinInfoManager.GetSpineSkinInfo(skinKind);
        for (int i = 0; i < tempSpineSkinInfo.Count; i++)
        {
            string tempName = tempSpineSkinInfo[i].skinName.Split('/')[1]; // 스킨종류를 제외한 스킨 이름
            //  GameObject tempIcon = GameManager.instance.iconManager.GetIcon(context.GetChild(i).name);
            // iConList_Stage02.Add(Instantiate(tempIcon, context.GetChild(i).position, quaternion.identity, context.GetChild(i)));
         //   context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * context_Height);
        }
    }
}
