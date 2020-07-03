using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEditor.U2D;

public class Cloth : MonoBehaviour
{
    [Header("옷장")]
    public Transform clothPannel;
    public GameObject clothBtn;
    public GameObject backBtn;
    public Transform context;     
    [Header("카메라")]
    public Camera characterCamera;         public Vector3 originChracterCamera; public Vector3 moveChracterCamera;
    public Camera uiCamera;                public Vector3 originUiCamera;      
    float cameraMoveSpeed = 0.3f;
  
    [Header("아틀라스")]
    public SpriteAtlas atlas;
    [Header("아이콘프리팹")]
    public GameObject stage_01_btn_Prepab;
    public GameObject stage_02_btn_Prepab;

    [Header("캐릭터 스파인")]
    public TransformSkin transformSkin;

    int currentState;

    private void Start()
    {
      
     

    }

    bool clickFlag;
    void ClickFlagFalse()
    {
        clickFlag = false;
    }

    float uiCamMoveX = 17f;

    public void ClothOpen()
    {
        if (currentState == 0 && !clickFlag)
        {
            currentState = 1;

            backBtn.SetActive(true);
            clothBtn.SetActive(false);
            backBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            backBtn.GetComponent<Button>().onClick.AddListener(() => { ClothClose(); });
            StartCoroutine(Open_01_Coroutine(() =>
            {
                context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, 0);
                context.GetComponent<VerticalLayoutGroup>().padding.top = 30;
                context.GetComponent<VerticalLayoutGroup>().spacing = 30;
               // 스킨 종류가 리스트에 담긴다
               List <SkinKind> tempSkinKindList = GameManager.instance.spineSkinInfoManager.GetSkinKindList();
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
        if (currentState == 2 && !clickFlag)
        {
            currentState = 1;
            backBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            backBtn.GetComponent<Button>().onClick.AddListener(() => { ClothClose(); });
            StartCoroutine(Open_02_Coroutine(() => {
                for (int i = 0; i < context.childCount; i++)
                {
                    Destroy(context.GetChild(i).gameObject);
                }
                context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, 0);
                context.GetComponent<VerticalLayoutGroup>().padding.top = 30;
                context.GetComponent<VerticalLayoutGroup>().spacing = 30;
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

    public void ClothClose()
    {
        StartCoroutine(Close_Coroutine(() =>
        {
            currentState = 0;
            clothBtn.SetActive(true);
            backBtn.SetActive(false);

            for (int i = 0; i < context.childCount; i++)
            {
                Destroy(context.GetChild(i).gameObject);
            }
        }));
    }

    IEnumerator Open_01_Coroutine(Action callBack = null)
    {
        clickFlag = true; // 클릭 방지

        if (callBack != null)
        {
            callBack();
        }
        characterCamera.transform.DOMove(moveChracterCamera, cameraMoveSpeed);
        uiCamera.transform.DOMoveX(uiCamMoveX, cameraMoveSpeed);
        yield return new WaitForSeconds(cameraMoveSpeed);

        clickFlag = false; // 클릭 방지
    }

    IEnumerator Open_02_Coroutine(Action callBack = null)
    {
        clickFlag = true;
        uiCamera.transform.DOMoveX(originUiCamera.x, cameraMoveSpeed);
        yield return new WaitForSeconds(cameraMoveSpeed);
        if (callBack != null)
        {
            callBack();
        }
        uiCamera.transform.DOMoveX(uiCamMoveX, cameraMoveSpeed);
        yield return new WaitForSeconds(cameraMoveSpeed);
        clickFlag = false;
    }

    IEnumerator Close_Coroutine(System.Action callback)
    {
        clickFlag = true;
        characterCamera.transform.DOMove(originChracterCamera, cameraMoveSpeed);
        uiCamera.transform.DOMoveX(originUiCamera.x, cameraMoveSpeed);
        yield return new WaitForSeconds(cameraMoveSpeed);
        if (callback != null)
        {
            callback();
        }
        clickFlag = false;
    }

    SkinKind stage01_data;
    public void State01_Btn(SkinKind skinKind)
    {
        if (clickFlag)
            return;

        stage01_data = skinKind;
        currentState = 2;
        backBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        backBtn.GetComponent<Button>().onClick.AddListener(() => { ClothOpen(); });
        StartCoroutine(Open_02_Coroutine(() => {
            for (int i = 0; i < context.childCount; i++)
            {
                Destroy(context.GetChild(i).gameObject);
            }
            context.GetComponent<VerticalLayoutGroup>().padding.top = 15;
            context.GetComponent<VerticalLayoutGroup>().spacing = 25;
            context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, 0);
            List<SpineSkinInfo> tempSpineSkinInfo = GameManager.instance.spineSkinInfoManager.GetSpineSkinInfo(skinKind);
            for (int i = 0; i < tempSpineSkinInfo.Count; i++)
            {
                GameObject prepab = Instantiate(stage_02_btn_Prepab, context.transform.position, Quaternion.identity, context.transform);
                prepab.transform.Find("이름").GetChild(0).GetComponent<Text>().text = tempSpineSkinInfo[i].inGameName;
                string skinName = tempSpineSkinInfo[i].skinName;
                prepab.GetComponent<Button>().onClick.AddListener(() => { State02_Btn(skinName); });
                GameObject iconObj = Instantiate(tempSpineSkinInfo[i].iconObj, prepab.transform.Find("ImgPos").position, Quaternion.identity, prepab.transform.Find("ImgPos"));
                context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * 200f);
            }
        }));
    }

    string stage02_data;
    public void State02_Btn(string skinName)
    {
        if (clickFlag)
            return;

        stage02_data = skinName;
        currentState = 3;
        backBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        backBtn.GetComponent<Button>().onClick.AddListener(() => { State01_Btn(stage01_data); });
        StartCoroutine(Open_02_Coroutine(() => {
            for (int i = 0; i < context.childCount; i++)
            {
                Destroy(context.GetChild(i).gameObject);
            }
            context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, 0);
            List<UserSkin> userSkinList = GameManager.instance.userInfoManager.GetSkinItemList(skinName);
            for (int i = 0; i < userSkinList.Count; i++)
            {
                GameObject prepab = Instantiate(stage_02_btn_Prepab, context.transform.position, Quaternion.identity, context.transform);
                prepab.transform.Find("이름").GetChild(0).GetComponent<Text>().text = GameManager.instance.spineSkinInfoManager.GetSpineSkinInfo(skinName).inGameName;
                GameObject iconObj = Instantiate(GameManager.instance.spineSkinInfoManager.GetSpineSkinInfo(skinName).iconObj, prepab.transform.Find("ImgPos").position, Quaternion.identity, prepab.transform.Find("ImgPos"));
                for (int j = 0; j < iconObj.transform.childCount; j++)
                {
                    if (iconObj.transform.GetChild(j).gameObject.activeSelf)
                    {
                        if (iconObj.transform.GetChild(j).name.Contains("color_01"))
                        {
                            iconObj.transform.GetChild(j).GetComponent<Image>().color = userSkinList[i].color_01;
                        }
                        if (iconObj.transform.GetChild(j).name.Contains("color_02"))
                        {
                            iconObj.transform.GetChild(j).GetComponent<Image>().color = userSkinList[i].color_02;
                        }
                    }
                }
            }
        }));
    }

    UserSkin stage03_data;
    public void State03_Btn(UserSkin userSkin)
    {
        if (clickFlag)
            return;

        stage03_data = userSkin;
        // 스킨 장착
        GameManager.instance.userInfoManager.PushUserEqip(GameManager.instance.userInfoManager.skinItem[GameManager.instance.userInfoManager.GetSkinItemIndex(userSkin)]);
        transformSkin.UserEqipInfoSetting();


    }
}
