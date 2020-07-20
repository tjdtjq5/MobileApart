using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.U2D;

public class Cloth : MonoBehaviour
{
    [Header("옷장")]
    public Transform clothPannel;
    public GameObject clothBtn;
    public GameObject backBtn;
    public Transform context;
    public GameObject colorItemPannel;
    [Header("카메라")]
    public Camera characterCamera;         public Vector3 originChracterCamera; public Vector3 moveChracterCamera;
    public Camera uiCamera;                public Vector3 originUiCamera;      
    float cameraMoveSpeed = 0.3f;
  
    [Header("아틀라스")]
    public SpriteAtlas atlas;
    [Header("스프라이트")]
    public Sprite randomColorIcon;
    public Sprite colorIcon;
    [Header("아이콘프리팹")]
    public GameObject stage_01_btn_Prepab;
    public GameObject stage_02_btn_Prepab;
    public GameObject colorBtn;
    public GameObject colorSlotBtn01;
    public GameObject colorSlotBtn02;
    public GameObject colorItem;

    [Header("캐릭터 스파인")]
    public TransformSkin transformSkin;

    [Header("꺼지는 것들")]
    public GameObject[] setOff;

    int currentState;

    bool clickFlag;
    void ClickFlagFalse()
    {
        clickFlag = false;
    }

    float uiCamMoveX = 1.7f;

    private void Start()
    {
        colorSlotBtn01.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = randomColorIcon;
        colorSlotBtn01.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        colorSlotBtn01.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + 0;
        colorSlotBtn02.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = randomColorIcon;
        colorSlotBtn02.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        colorSlotBtn02.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + 0;
    }

    public void ClothOpen()
    {
        if (currentState == 0 && !clickFlag)
        {
            currentState = 1;

            selectColorItemIndexNum = -1;

            for (int i = 0; i < setOff.Length; i++)
            {
                setOff[i].gameObject.SetActive(false);
            }

            int num = GameManager.instance.userInfoManager.GetIndexColorItem(Color.clear);
            colorSlotBtn01.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = randomColorIcon;
            colorSlotBtn01.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
            colorSlotBtn01.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[num].num;
            colorSlotBtn02.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = randomColorIcon;
            colorSlotBtn02.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
            colorSlotBtn02.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[num].num;

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
        if (currentState == 2 || currentState == 3 && !clickFlag)
        {
            //컬러버튼 슬롯버튼 위치 리셋
            colorBtn.transform.position = new Vector2(colorBtn.transform.position.x, 2000);
            colorSlotBtn01.transform.position = new Vector2(colorSlotBtn01.transform.position.x, 2000);
            colorSlotBtn02.transform.position = new Vector2(colorSlotBtn02.transform.position.x, 2000);

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

            for (int i = 0; i < setOff.Length; i++)
            {
                setOff[i].gameObject.SetActive(true);
            }

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
        characterCamera.transform.DOMove(new Vector3(moveChracterCamera.x , moveChracterCamera.y, characterCamera.transform.position.z), cameraMoveSpeed);
        characterCamera.GetComponent<Camera>().DOOrthoSize(6.5f, cameraMoveSpeed);
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
        characterCamera.GetComponent<Camera>().DOOrthoSize(5f, cameraMoveSpeed);
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
        {
            return;
        }
        stage01_data = skinKind;
        stage02_data = null;
        currentState = 2;
        backBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        backBtn.GetComponent<Button>().onClick.AddListener(() => { ClothOpen(); });

        int contextSize = 0;

        StartCoroutine(Open_02_Coroutine(() =>
        {
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
                List<UserSkin> userSkinList = GameManager.instance.userInfoManager.GetSkinItemList(tempSpineSkinInfo[i].skinName);
                for (int j = 0; j < userSkinList.Count; j++)
                {
                    GameObject prepab = Instantiate(stage_02_btn_Prepab, context.transform.position, Quaternion.identity, context.transform);
                    prepab.transform.Find("이름").GetChild(0).GetComponent<Text>().text = GameManager.instance.spineSkinInfoManager.GetSpineSkinInfo(userSkinList[j].skinName).inGameName;
                    GameObject iconObj = Instantiate(GameManager.instance.itemManager.GetItemInfo(userSkinList[j].skinName).iconObj, prepab.transform.Find("ImgPos").position, Quaternion.identity, prepab.transform.Find("ImgPos"));
                    for (int k = 0; k < iconObj.transform.childCount; k++)
                    {
                        if (iconObj.transform.GetChild(k).name == "color_01")
                        {
                            iconObj.transform.GetChild(k).GetComponent<Image>().color = userSkinList[j].color_01;
                        }
                        if (iconObj.transform.GetChild(k).name == "color_02")
                        {
                            iconObj.transform.GetChild(k).GetComponent<Image>().color = userSkinList[j].color_02;
                        }
                    }

                    contextSize += 210;
                    UserSkin tempUserSkin = userSkinList[j];
                    prepab.GetComponent<Button>().onClick.AddListener(() => { State02_Btn(tempUserSkin, prepab.transform); });
                }
            }
            context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, contextSize);
        }));
    }




   

    UserSkin stage02_data;
    public void State02_Btn(UserSkin userSkin, Transform pos)
    {
        if (clickFlag)
            return;

        currentState = 3;
        stage02_data = userSkin;

        // 스킨 장착
        GameManager.instance.userInfoManager.PushUserEqip(GameManager.instance.userInfoManager.skinItem[GameManager.instance.userInfoManager.GetSkinItemIndex(userSkin)]);
        transformSkin.UserEqipInfoSetting();
        GameManager.instance.userInfoManager.SaveUserEqip(GameManager.instance.userInfoManager.currentCharacter);

        //컬러버튼 
        colorBtn.transform.position = new Vector3(colorBtn.transform.position.x, pos.position.y);
        colorBtn.transform.localPosition = new Vector3(colorBtn.transform.localPosition.x, colorBtn.transform.localPosition.y, 0);
        colorBtn.transform.localScale = new Vector2(0.5f, 0.5f);
        colorBtn.transform.DOScale(new Vector2(1, 1f), cameraMoveSpeed);
        colorBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        colorBtn.GetComponent<Button>().onClick.AddListener(() => ColorBtn());

        //슬롯버튼 위치 리셋
        colorSlotBtn01.transform.position = new Vector2(colorSlotBtn01.transform.position.x, 2000);
        colorSlotBtn02.transform.position = new Vector2(colorSlotBtn02.transform.position.x, 2000);

    }

    public void ColorBtn()
    {
        if (transformSkin.GetColor(stage02_data.skinName, 1) != Color.clear)
        {
            colorSlotBtn01.transform.position = new Vector2(colorBtn.transform.position.x - 1f, colorBtn.transform.position.y + 0.3f);
            colorSlotBtn01.transform.localScale = new Vector2(0, 1f);
            colorSlotBtn01.transform.DOScale(new Vector2(1, 1f), cameraMoveSpeed);
        }
        if (transformSkin.GetColor(stage02_data.skinName, 2) != Color.clear)
        {
            colorSlotBtn02.transform.position = new Vector2(colorBtn.transform.position.x - 1f, colorBtn.transform.position.y - 0.3f);
            colorSlotBtn02.transform.localScale = new Vector2(0, 1f);
            colorSlotBtn02.transform.DOScale(new Vector2(1, 1f), cameraMoveSpeed);
        }

        //컬러버튼 위치 리셋
        colorBtn.transform.position = new Vector2(colorBtn.transform.position.x, 2000);
    }

    int slotNum;
    public void ColorItemPannelOpen(int slotNum)
    {
        if (clickFlag)
        {
            return;
        }
        clickFlag = true;

        this.slotNum = slotNum;
        selectColorItemIndexNum = -1;

        colorItemPannel.SetActive(true);
        colorItemPannel.transform.localScale = new Vector2(0f, 0f);
        colorItemPannel.transform.DOScale(new Vector2(1.2f, 1.2f), cameraMoveSpeed).OnComplete(() => {
            colorItemPannel.transform.DOScale(new Vector2(1f, 1f), cameraMoveSpeed).OnComplete(() => {
                clickFlag = false;
            });

        });
        ColorItemReset();
    }

    public void ColorItemReset()
    {
        for (int i = 0; i < colorItemPannel.transform.Find("패널").childCount; i++)
        {
            Destroy(colorItemPannel.transform.Find("패널").GetChild(i).gameObject);
        }
        for (int i = 0; i < GameManager.instance.userInfoManager.colorItem.Count; i++)
        {
            if (GameManager.instance.userInfoManager.colorItem[i].color == Color.clear)
            {
                GameObject colorIcon = Instantiate(colorItem, colorItemPannel.transform.position, Quaternion.identity, colorItemPannel.transform.Find("패널"));
                colorIcon.transform.GetChild(0).Find("아이콘이미지").GetComponent<Image>().sprite = randomColorIcon;
                colorIcon.transform.GetChild(0).Find("아이콘이미지").GetChild(0).gameObject.SetActive(false);
                colorIcon.transform.GetChild(0).Find("갯수").GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[i].num;
              
                int tempIndex = i;
                GameObject tempObj = colorIcon;
                colorIcon.transform.Find("circle").GetComponent<Button>().onClick.AddListener(()=>ColorItemSelect(tempIndex, tempObj));
            }
        }
        for (int i = 0; i < GameManager.instance.userInfoManager.colorItem.Count; i++)
        {
            if (GameManager.instance.userInfoManager.colorItem[i].color != Color.clear)
            {
                GameObject colorIcon = Instantiate(colorItem, colorItemPannel.transform.position, Quaternion.identity, colorItemPannel.transform.Find("패널"));
                colorIcon.transform.GetChild(0).Find("아이콘이미지").GetComponent<Image>().sprite = this.colorIcon;
                colorIcon.transform.GetChild(0).Find("아이콘이미지").GetChild(0).gameObject.SetActive(true);
                colorIcon.transform.GetChild(0).Find("아이콘이미지").GetChild(0).GetComponent<Image>().color = GameManager.instance.userInfoManager.colorItem[i].color;
                colorIcon.transform.GetChild(0).Find("갯수").GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[i].num;


                int tempIndex = i;
                GameObject tempObj = colorIcon;
                colorIcon.transform.Find("circle").GetComponent<Button>().onClick.AddListener(() => ColorItemSelect(tempIndex, tempObj));
            }
        }
    }
    public void ColorItemPannelClose()
    {
        if (clickFlag)
        {
            return;
        }
        clickFlag = true;

       
        colorItemPannel.transform.localScale = new Vector2(1f, 1f);
        colorItemPannel.transform.DOScale(new Vector2(1.2f, 1.2f), cameraMoveSpeed).OnComplete(() => {
            colorItemPannel.transform.DOScale(new Vector2(0f, 0f), cameraMoveSpeed).OnComplete(()=> { 
                colorItemPannel.SetActive(false);
                clickFlag = false;
            });
        });
    }

    int selectColorItemIndexNum;
    public void ColorItemSelect(int colorItemIndex, GameObject selectIcon)
    {
        selectColorItemIndexNum = colorItemIndex;

        Transform tempPannel = colorItemPannel.transform.Find("패널");
        for (int i = 0; i < tempPannel.childCount; i++)
        {
            tempPannel.GetChild(i).Find("circle").GetComponent<Image>().color = Color.white;
        }

        selectIcon.transform.Find("circle").GetComponent<Image>().color = new Color(0, (float)213 / 255, (float)165 / 255, 1);
    }


    //확인버튼 
    public void ColorItemSlotSet()
    {
        if (selectColorItemIndexNum == -1)
        {
            colorSlotBtn01.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = randomColorIcon;
            colorSlotBtn01.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
            colorSlotBtn01.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[GameManager.instance.userInfoManager.GetIndexColorItem(Color.clear)].num;
            colorSlotBtn02.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = randomColorIcon;
            colorSlotBtn02.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
            colorSlotBtn02.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[GameManager.instance.userInfoManager.GetIndexColorItem(Color.clear)].num;

            ColorItemPannelClose();
            return;
        }

        if (slotNum == 1)
        {
            if (GameManager.instance.userInfoManager.colorItem[selectColorItemIndexNum].color == Color.clear)
            {
                colorSlotBtn01.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = randomColorIcon;
                colorSlotBtn01.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
                colorSlotBtn01.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[selectColorItemIndexNum].num;
            }
            else
            {
                colorSlotBtn01.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = colorIcon;
                colorSlotBtn01.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                colorSlotBtn01.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = GameManager.instance.userInfoManager.colorItem[selectColorItemIndexNum].color;
                colorSlotBtn01.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[selectColorItemIndexNum].num;
            }
            
        }

        if (slotNum == 2)
        {
            if (GameManager.instance.userInfoManager.colorItem[selectColorItemIndexNum].color == Color.clear)
            {
                colorSlotBtn02.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = randomColorIcon;
                colorSlotBtn02.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
                colorSlotBtn02.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[selectColorItemIndexNum].num;
            }
            else
            {
                colorSlotBtn02.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = colorIcon;
                colorSlotBtn02.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                colorSlotBtn02.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = GameManager.instance.userInfoManager.colorItem[selectColorItemIndexNum].color;
                colorSlotBtn02.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[selectColorItemIndexNum].num;
            }
        }
        ColorItemPannelClose();

    }

    //염색하기 버튼
    [Header("염색완료 알림")]
    public GameObject complete;
    public void ColorChange(int slotNum)
    {
        if (complete.activeSelf)
        {
            return;
        }

        if (slotNum == 1)
        {
            //랜덤 염색 사용하기 
            if (colorSlotBtn01.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite == randomColorIcon)
            {
                int num = int.Parse(colorSlotBtn01.transform.GetChild(0).Find("수량").GetComponent<Text>().text.Split('x')[1]);
                if (num > 0)
                {
                    num--;
                    GameManager.instance.userInfoManager.DeleteColorItem(Color.clear);
                    colorSlotBtn01.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + num;

                    // 아이템 색 바꾸기 
                    GameManager.instance.userInfoManager.ChangeColorSkinItem(GameManager.instance.userInfoManager.GetSkinItemIndex(stage02_data), Color.clear, 1);
                    // 아이템 장착
                    GameManager.instance.userInfoManager.PushUserEqip(GameManager.instance.userInfoManager.skinItem[GameManager.instance.userInfoManager.GetSkinItemIndex(stage02_data)]);

                    complete.SetActive(true);
                    complete.GetComponent<Image>().DOFade(1, 0);
                    complete.transform.GetChild(0).GetComponent<Text>().DOFade(1, 0);
                    complete.GetComponent<Image>().DOFade(0, 1.3f).OnComplete(()=> { complete.SetActive(false); });
                    complete.transform.GetChild(0).GetComponent<Text>().DOFade(0, 1.3f);

                    transformSkin.UserEqipInfoSetting();

                }
            }
            else
            {
                int num = int.Parse(colorSlotBtn01.transform.GetChild(0).Find("수량").GetComponent<Text>().text.Split('x')[1]);
                if (num > 0)
                {
                    num--;
                    Color tempColor = colorSlotBtn01.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color;
                    GameManager.instance.userInfoManager.DeleteColorItem(tempColor);
                    transformSkin.SetColor(stage02_data.skinName, tempColor, 1);
                    colorSlotBtn01.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + num;

                    // 아이템 색 바꾸기 
                    GameManager.instance.userInfoManager.ChangeColorSkinItem(GameManager.instance.userInfoManager.GetSkinItemIndex(stage02_data), tempColor, 1);
                    // 아이템 장착
                    GameManager.instance.userInfoManager.PushUserEqip(GameManager.instance.userInfoManager.skinItem[GameManager.instance.userInfoManager.GetSkinItemIndex(stage02_data)]);

                    complete.SetActive(true);
                    complete.GetComponent<Image>().DOFade(1, 0);
                    complete.transform.GetChild(0).GetComponent<Text>().DOFade(1, 0);
                    complete.GetComponent<Image>().DOFade(0, 1.3f).OnComplete(() => { complete.SetActive(false); });
                    complete.transform.GetChild(0).GetComponent<Text>().DOFade(0, 1.3f);

                    transformSkin.UserEqipInfoSetting();
                }
            }
        }
        if (slotNum == 2)
        {
            //랜덤 염색 사용하기 
            if (colorSlotBtn02.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite == randomColorIcon)
            {
                int num = int.Parse(colorSlotBtn02.transform.GetChild(0).Find("수량").GetComponent<Text>().text.Split('x')[1]);
                if (num > 0)
                {
                    num--;
                    GameManager.instance.userInfoManager.DeleteColorItem(Color.clear);
                    transformSkin.SetColor(stage02_data.skinName, Color.clear, 2);
                    colorSlotBtn02.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + num;

                    // 아이템 색 바꾸기 
                    GameManager.instance.userInfoManager.ChangeColorSkinItem(GameManager.instance.userInfoManager.GetSkinItemIndex(stage02_data), Color.clear, 2);
                    // 아이템 장착
                    GameManager.instance.userInfoManager.PushUserEqip(GameManager.instance.userInfoManager.skinItem[GameManager.instance.userInfoManager.GetSkinItemIndex(stage02_data)]);

                    complete.SetActive(true);
                    complete.GetComponent<Image>().DOFade(1, 0);
                    complete.transform.GetChild(0).GetComponent<Text>().DOFade(1, 0);
                    complete.GetComponent<Image>().DOFade(0, 1.3f).OnComplete(() => { complete.SetActive(false); });
                    complete.transform.GetChild(0).GetComponent<Text>().DOFade(0, 1.3f);

                    transformSkin.UserEqipInfoSetting();
                }
            }
            else
            {
                int num = int.Parse(colorSlotBtn02.transform.GetChild(0).Find("수량").GetComponent<Text>().text.Split('x')[1]);
                if (num > 0)
                {
                    num--;
                    Color tempColor = colorSlotBtn02.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color;
                    GameManager.instance.userInfoManager.DeleteColorItem(tempColor);
                    transformSkin.SetColor(stage02_data.skinName, tempColor, 2);
                    colorSlotBtn02.transform.GetChild(0).Find("수량").GetComponent<Text>().text = "x" + num;

                    // 아이템 색 바꾸기 
                    GameManager.instance.userInfoManager.ChangeColorSkinItem(GameManager.instance.userInfoManager.GetSkinItemIndex(stage02_data), tempColor, 2);
                    // 아이템 장착
                    GameManager.instance.userInfoManager.PushUserEqip(GameManager.instance.userInfoManager.skinItem[GameManager.instance.userInfoManager.GetSkinItemIndex(stage02_data)]);

                    complete.SetActive(true);
                    complete.GetComponent<Image>().DOFade(1, 0);
                    complete.transform.GetChild(0).GetComponent<Text>().DOFade(1, 0);
                    complete.GetComponent<Image>().DOFade(0, 1.3f).OnComplete(() => { complete.SetActive(false); });
                    complete.transform.GetChild(0).GetComponent<Text>().DOFade(0, 1.3f);

                    transformSkin.UserEqipInfoSetting();
                }
            }
        }

        // 저장
        GameManager.instance.userInfoManager.SaveSkinItem();
        Debug.Log(GameManager.instance.userInfoManager.currentCharacter);
        GameManager.instance.userInfoManager.SaveUserEqip(GameManager.instance.userInfoManager.currentCharacter);
    }

    [Header("경고창")]
    public GameObject redAlam;
    //버리기버튼 
    public void RemoveColorItemBtn()
    {
        if (selectColorItemIndexNum == -1)
        {
            return;
        }

        redAlam.SetActive(true);
        redAlam.GetComponent<Image>().DOFade(0, 0);
        redAlam.GetComponent<Image>().DOFade(1, cameraMoveSpeed);
        redAlam.transform.Find("Text").GetComponent<Text>().DOFade(0, 0);
        redAlam.transform.Find("Text").GetComponent<Text>().DOFade(1, cameraMoveSpeed);
        redAlam.transform.Find("확인").GetComponent<Image>().DOFade(0, 0);
        redAlam.transform.Find("확인").GetComponent<Image>().DOFade(1, cameraMoveSpeed);
        redAlam.transform.Find("확인").GetChild(0).GetComponent<Text>().DOFade(0, 0);
        redAlam.transform.Find("확인").GetChild(0).GetComponent<Text>().DOFade(1, cameraMoveSpeed);
        redAlam.transform.Find("취소").GetComponent<Image>().DOFade(0, 0);
        redAlam.transform.Find("취소").GetComponent<Image>().DOFade(1, cameraMoveSpeed);
        redAlam.transform.Find("취소").GetChild(0).GetComponent<Text>().DOFade(0, 0);
        redAlam.transform.Find("취소").GetChild(0).GetComponent<Text>().DOFade(1, cameraMoveSpeed);
    }
    // 경고 확인 
    public void RemoveColorItem()
    {
        int count = GameManager.instance.userInfoManager.colorItem[selectColorItemIndexNum].num;
        for (int i = 0; i < count; i++)
        {
            GameManager.instance.userInfoManager.DeleteColorItem(selectColorItemIndexNum);
        }
        ColorItemReset();
        selectColorItemIndexNum = -1;
        redAlam.SetActive(false);
    }

    public void AlramExit()
    {
        redAlam.SetActive(false);
    }

    
}
