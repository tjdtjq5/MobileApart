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
    public Camera characterCamera;
    public Camera uiCamera;
    float cameraMoveSpeed = 0.3f;
    [Header("그림자")]
    public Transform shadow;
    [Header("아틀라스")]
    public SpriteAtlas atlas;
    [Header("아이콘프리팹")]
    public GameObject stage_01_btn_Prepab;

    int currentState;

    int[] selectBtnNum = new int[5];
    string[] selectString = new string[5];

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
        if (currentState == 0)
        {
            clickFlag = true; // 클릭 방지
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
                    GameObject prepab = Instantiate(stage_01_btn_Prepab, context.position, Quaternion.identity, context);
                    prepab.name = tempSkinKindList[i].ToString();
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
                            break;
                        case SkinKind.face:
                            break;
                        case SkinKind.haF:
                            break;
                        case SkinKind.haB:
                            break;
                        case SkinKind.outt:
                            break;
                        case SkinKind.sho:
                            break;
                        case SkinKind.cap:
                            break;
                        case SkinKind.set:
                            break;
                        case SkinKind.body:
                            break;
                        default:
                            break;
                    }
                    context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * 116f);
                }
            }));
        }
    }

    IEnumerator Open_01_Coroutine(Action callBack = null)
    {
        if (callBack != null)
        {
            callBack();
        }
        characterCamera.transform.DOMoveX(characterCamMoveX, cameraMoveSpeed);
        uiCamera.transform.DOMoveX(uiCamMoveX, cameraMoveSpeed);
        shadow.transform.DOMoveX(shadowMoveX, cameraMoveSpeed);
        yield return new WaitForSeconds(cameraMoveSpeed);
    }
}
