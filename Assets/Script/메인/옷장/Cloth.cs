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
    public GameObject colorInventory;
    [Header("카메라")]
    public Camera characterCamera;         public Vector3 originChracterCamera; public Vector3 moveChracterCamera;
    public Camera uiCamera;                public Vector3 originUiCamera;
    float uiCamMoveX = 1.7f;
    float cameraMoveSpeed = 0.3f;
  
    [Header("아틀라스")]
    public SpriteAtlas atlas;
    [Header("스프라이트")]
    public Sprite randomColorIcon;
    public Sprite colorIcon;
    [Header("아이콘프리팹")]
    public GameObject skinKindIcon;
    public GameObject clothKindIcon;
    public GameObject colorBtn;
    public GameObject colorSlotBtn01;
    public GameObject colorSlotBtn02;
    public GameObject colorItemIcon;

    [Header("캐릭터 스파인")]
    public TransformSkin transformSkin;

    [Header("꺼지는 것들")]
    public GameObject[] setOff;

    public void ClothOpen()
    {
        clothBtn.SetActive(false);

        characterCamera.transform.DOMove(moveChracterCamera, cameraMoveSpeed);
        uiCamera.transform.DOMoveX(uiCamMoveX, cameraMoveSpeed);

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(false);
        }

        ContextInit(15, 25);
        float contextHeight = 30f;

        backBtn.SetActive(true);
        backBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        backBtn.GetComponent<Button>().onClick.AddListener(() => { ClothClose(); });

        int skinKindLength = System.Enum.GetNames(typeof(SkinKind)).Length;
        for (int i = 0; i < skinKindLength; i++)
        {
            int index = i;
            GameObject tempSkinkindIcon = null;
            switch ((SkinKind)i)
            {
                case SkinKind.cap:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "모자";
                    break;
                case SkinKind.haF:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "앞 머리";
                    break;
                case SkinKind.haB:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "뒷 머리";
                    break;
                case SkinKind.eye:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "눈동자";
                    break;
                case SkinKind.face:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "얼굴";
                    break;
                case SkinKind.set:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "세트";
                    break;
                case SkinKind.outt:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "외투";
                    break;
                case SkinKind.top:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "상의";
                    break;
                case SkinKind.pan:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "하의";
                    break;
                case SkinKind.unde:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "속옷";
                    break;
                case SkinKind.accface:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "얼굴 악세";
                    break;
                case SkinKind.accneck:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "목 악세";
                    break;
                case SkinKind.accbody:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "몸 악세";
                    break;
                case SkinKind.accarm:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "팔 악세";
                    break;
                case SkinKind.accleg:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "다리 악세";
                    break;
                case SkinKind.soc:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "양말";
                    break;
                case SkinKind.sho:
                    contextHeight += 90f;
                    tempSkinkindIcon = Instantiate(skinKindIcon, Vector2.zero, Quaternion.identity, context);
                    tempSkinkindIcon.GetComponent<Button>().onClick.AddListener(() => { SkinKindIconBtn((SkinKind)index); });
                    tempSkinkindIcon.transform.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite(((SkinKind)i).ToString());
                    tempSkinkindIcon.transform.GetChild(1).GetComponent<Text>().text = "신발";
                    break;
            }
        }

        context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, contextHeight);
        clothPannel.GetChild(0).GetChild(2).GetComponent<Scrollbar>().value = 1;
    }

    public void ClothClose()
    {
        clothBtn.SetActive(true);

        characterCamera.transform.DOMove(originChracterCamera, cameraMoveSpeed);
        uiCamera.transform.DOMoveX(originUiCamera.x, cameraMoveSpeed);

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }

        backBtn.SetActive(false);
    }

    void ContextInit(int top, int spacing)
    {
        context.GetComponent<VerticalLayoutGroup>().padding.top = top;
        context.GetComponent<VerticalLayoutGroup>().spacing = spacing;

        for (int i = 0; i < context.childCount; i++)
        {
            Destroy(context.GetChild(i).gameObject);
        }
    }

    void SkinKindIconBtn(SkinKind skinKind)
    {
        uiCamera.transform.DOMove(originUiCamera, cameraMoveSpeed).OnComplete(()=> {

            uiCamera.transform.DOMoveX(uiCamMoveX, cameraMoveSpeed);

            backBtn.SetActive(true);
            backBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            backBtn.GetComponent<Button>().onClick.AddListener(() => { BackSkinKindIconBtn(); });

            clothPannel.GetChild(0).GetChild(2).GetComponent<Scrollbar>().value = 1;

            SkinKindReload(skinKind);
        });
    }

    void SkinKindReload(SkinKind skinKind)
    {
        ContextInit(15, 25);
        ColorInit();

        float contextHeight = 160f;

        // 모두벗기 버튼 
        if (skinKind == SkinKind.cap || skinKind == SkinKind.set || skinKind == SkinKind.outt || skinKind == SkinKind.top || skinKind == SkinKind.pan || skinKind == SkinKind.accarm || skinKind == SkinKind.accbody || skinKind == SkinKind.accface || skinKind == SkinKind.accleg || skinKind == SkinKind.accneck || skinKind == SkinKind.soc || skinKind == SkinKind.sho)
        {
            GameObject noneSkinkindBtn = Instantiate(clothKindIcon, Vector2.zero, Quaternion.identity, context);
            noneSkinkindBtn.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "모두 벗기";
            noneSkinkindBtn.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.userInfoManager.PullUserEqip(skinKind); transformSkin.UserEqipInfoSetting(); GameManager.instance.userInfoManager.SaveUserEqip(GameManager.instance.userInfoManager.currentCharacter); SkinKindReload(skinKind); });

            contextHeight += 185f;
        }

        // 일반 skinKindBtn 세팅 
        List<int> userSkinIndexList = GameManager.instance.userInfoManager.GetSkinItemIndexList(skinKind);
        for (int i = 0; i < userSkinIndexList.Count; i++)
        {
            int itemIndex = userSkinIndexList[i];

            UserSkin tempUserSkin = GameManager.instance.userInfoManager.skinItem[userSkinIndexList[i]];

            GameObject nomalSkinkindBtn = Instantiate(clothKindIcon, Vector2.zero, Quaternion.identity, context);
            nomalSkinkindBtn.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.instance.itemManager.GetItemInfo(tempUserSkin.skinName).inGameName;
            GameObject icon = Instantiate(GameManager.instance.itemManager.GetItemInfo(tempUserSkin.skinName).iconObj, Vector2.zero, Quaternion.identity, nomalSkinkindBtn.transform.GetChild(0));
            IconColor(icon, tempUserSkin.color_01, tempUserSkin.color_02);

            //장착중인가 아닌가 
            if (tempUserSkin.isEqip)
            {
                nomalSkinkindBtn.GetComponent<Image>().color = new Color(0, 213 / 255f, 165 / 255f, 166 / 255f);
                nomalSkinkindBtn.GetComponent<Button>().onClick.AddListener(() => { ColorBtnSet(new Vector2(nomalSkinkindBtn.transform.position.x - 1.2f, nomalSkinkindBtn.transform.position.y), itemIndex); });
            }
            else
            {
                // 악세사리 종류면 중복 장착 가능
                if (skinKind == SkinKind.accarm || skinKind == SkinKind.accbody || skinKind == SkinKind.accface || skinKind == SkinKind.accleg || skinKind == SkinKind.accneck)
                {
                    nomalSkinkindBtn.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.userInfoManager.PushUserEqip(itemIndex); transformSkin.UserEqipInfoSetting(); GameManager.instance.userInfoManager.SaveUserEqip(GameManager.instance.userInfoManager.currentCharacter); SkinKindReload(skinKind); });
                }
                else
                {
                    nomalSkinkindBtn.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.userInfoManager.PullUserEqip(skinKind); GameManager.instance.userInfoManager.PushUserEqip(itemIndex); transformSkin.UserEqipInfoSetting(); GameManager.instance.userInfoManager.SaveUserEqip(GameManager.instance.userInfoManager.currentCharacter); SkinKindReload(skinKind); });
                }
            }

            contextHeight += 185f;
        }

        context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, contextHeight);
    }

    void BackSkinKindIconBtn()
    {
        uiCamera.transform.DOMove(originUiCamera, cameraMoveSpeed).OnComplete(() =>
        {
            ClothOpen();
        });
        ColorInit();
    }

    void IconColor(GameObject icon, Color color_01 , Color color_02)
    {
        for (int i = 0; i < icon.transform.childCount; i++)
        {
            if (icon.transform.GetChild(i).name == "color_01")
            {
                icon.transform.GetChild(i).GetComponent<Image>().color = color_01;
            }
            if (icon.transform.GetChild(i).name == "color_02")
            {
                icon.transform.GetChild(i).GetComponent<Image>().color = color_02;
            }
        }
    }

    // 염색 
    Color slotColor_01;
    Color slotColor_02;

    void ColorInit()
    {
        slotColor_01 = Color.clear;
        slotColor_02 = Color.clear;

        colorBtn.transform.position = new Vector2(2000,2000);
        colorBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        colorSlotBtn01.transform.position = new Vector2(2000, 2000);
        colorSlotBtn01.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        colorSlotBtn01.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        colorSlotBtn02.transform.position = new Vector2(2000, 2000);
        colorSlotBtn02.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        colorSlotBtn02.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
    }
    void ColorBtnSet(Vector2 pos, int itemIndex)
    {
        ColorInit();

        colorBtn.transform.position = pos;
        colorBtn.GetComponent<Button>().onClick.AddListener(() => { ColorSlotBtnSet(pos, itemIndex); });
    }
    void ColorSlotBtnSet(Vector2 pos ,int itemIndex)
    {
        ColorInit();
        SlotColor();

        List<int> slotIndexList = transformSkin.CheckColorSlot(GameManager.instance.userInfoManager.skinItem[itemIndex].skinName);
        for (int i = 0; i < slotIndexList.Count; i++)
        {
            if (slotIndexList[i] == 1)
            {
                colorSlotBtn01.transform.position = new Vector2(pos.x - 1.5f, pos.y + 0.45f);
                colorSlotBtn01.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { ChangeColor(itemIndex, 1); });

                colorSlotBtn01.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() => { ColorInventory(1); });

            }
            if (slotIndexList[i] == 2)
            {
                colorSlotBtn02.transform.position = new Vector2(pos.x - 1.5f, pos.y - 0.3f);
                colorSlotBtn02.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { ChangeColor(itemIndex, 2); });

                colorSlotBtn02.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() => { ColorInventory(2); });
            }
        }
    }
    void SlotColor()
    {
        if (GameManager.instance.userInfoManager.GetIndexColorItem(slotColor_01) == -1)
        {
            slotColor_01 = Color.clear;
        }
        if (GameManager.instance.userInfoManager.GetIndexColorItem(slotColor_02) == -1)
        {
            slotColor_02 = Color.clear;
        }

        if (slotColor_01 == Color.clear)
        {
            colorSlotBtn01.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            colorSlotBtn01.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            colorSlotBtn01.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = slotColor_01;
        }

        colorSlotBtn01.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[GameManager.instance.userInfoManager.GetIndexColorItem(slotColor_01)].num;

        if (slotColor_02 == Color.clear)
        {
            colorSlotBtn02.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            colorSlotBtn02.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            colorSlotBtn02.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = slotColor_02;
        }
        colorSlotBtn02.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[GameManager.instance.userInfoManager.GetIndexColorItem(slotColor_02)].num;
    }
    void ChangeColor(int skinItemIndex, int slot)
    {
        switch (slot)
        {
            case 1:
                if (GameManager.instance.userInfoManager.colorItem[GameManager.instance.userInfoManager.GetIndexColorItem(slotColor_01)].num < 1)
                {
                    OverrideCanvas.instance.RedAlram("염색아이템 수량이 부족합니다.");
                    return;
                }
                GameManager.instance.userInfoManager.DeleteColorItem(slotColor_01);

                Color tempColor = slotColor_01;
                if (tempColor == Color.clear)
                {
                    float randR = UnityEngine.Random.Range(0, 255) / (float)255;
                    float randG = UnityEngine.Random.Range(0, 255) / (float)255;
                    float randB = UnityEngine.Random.Range(0, 255) / (float)255;

                    tempColor = new Color(randR, randG, randB, 1);
                }

                GameManager.instance.userInfoManager.skinItem[skinItemIndex].color_01 = tempColor;

                GameManager.instance.userInfoManager.SaveSkinItem();
                transformSkin.UserEqipInfoSetting();
                break;
            case 2:
                if (GameManager.instance.userInfoManager.colorItem[GameManager.instance.userInfoManager.GetIndexColorItem(slotColor_02)].num < 1)
                {
                    OverrideCanvas.instance.RedAlram("염색아이템 수량이 부족합니다.");
                    return;
                }
                GameManager.instance.userInfoManager.DeleteColorItem(slotColor_02);

                Color tempColor2 = slotColor_02;
                if (tempColor2 == Color.clear)
                {
                    float randR = UnityEngine.Random.Range(0, 255) / (float)255;
                    float randG = UnityEngine.Random.Range(0, 255) / (float)255;
                    float randB = UnityEngine.Random.Range(0, 255) / (float)255;

                    tempColor2 = new Color(randR, randG, randB, 1);
                }

                GameManager.instance.userInfoManager.skinItem[skinItemIndex].color_02 = tempColor2;

                GameManager.instance.userInfoManager.SaveSkinItem();
                transformSkin.UserEqipInfoSetting();
                break;
        }

        SlotColor();

        SkinKind userEqipskinKind = (SkinKind)System.Enum.Parse(typeof(SkinKind), GameManager.instance.userInfoManager.skinItem[skinItemIndex].skinName.Split('/')[0]);
        SkinKindReload(userEqipskinKind);
    }
    void ColorInventory(int slot)
    {
        colorInventory.SetActive(true);

        for (int i = 0; i < colorInventory.transform.Find("패널").childCount; i++)
        {
            Destroy(colorInventory.transform.Find("패널").GetChild(i).gameObject);
        }
        colorInventory.transform.Find("확인버튼").GetComponent<Button>().onClick.RemoveAllListeners();
        colorInventory.transform.Find("확인버튼").GetComponent<Button>().onClick.AddListener(() => { colorInventory.SetActive(false); });
        colorInventory.transform.Find("버리기버튼").GetComponent<Button>().onClick.RemoveAllListeners();
        colorInventory.transform.Find("버리기버튼").GetComponent<Button>().onClick.AddListener(() => {
            OverrideCanvas.instance.Caution("모두 버리시겠습니까?", () => {
                switch (slot)
                {
                    case 1:
                        while (true)
                        {
                            if (GameManager.instance.userInfoManager.GetIndexColorItem(slotColor_01) == -1)
                            {
                                break;
                            }
                            if (GameManager.instance.userInfoManager.colorItem[GameManager.instance.userInfoManager.GetIndexColorItem(slotColor_01)].num <= 0)
                            {
                                break;
                            }
                            GameManager.instance.userInfoManager.DeleteColorItem(slotColor_01);
                        }
                        slotColor_01 = Color.clear;
                        break;
                    case 2:
                        while (true)
                        {
                            if (GameManager.instance.userInfoManager.GetIndexColorItem(slotColor_02) == -1)
                            {
                                break;
                            }
                            if (GameManager.instance.userInfoManager.colorItem[GameManager.instance.userInfoManager.GetIndexColorItem(slotColor_02)].num <= 0)
                            {
                                break;
                            }
                            GameManager.instance.userInfoManager.DeleteColorItem(slotColor_02);
                        }
                        slotColor_02 = Color.clear;
                        break;
                }
                ColorInventory(slot);
                SlotColor();
            });
        });

        for (int i = 0; i < GameManager.instance.userInfoManager.colorItem.Count; i++)
        {
            GameObject tempColorItemIcon = Instantiate(colorItemIcon, Vector2.zero, Quaternion.identity, colorInventory.transform.Find("패널"));
            tempColorItemIcon.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "x" + GameManager.instance.userInfoManager.colorItem[i].num;

            int index = i;

            switch (slot)
            {
                case 1:
                    if (GameManager.instance.userInfoManager.colorItem[i].color == slotColor_01)
                    {
                        tempColorItemIcon.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 213 / 255f, 165 / 255f, 1);
                    }
                    tempColorItemIcon.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { slotColor_01 = GameManager.instance.userInfoManager.colorItem[index].color; SlotColor(); ColorInventory(slot); });
                    break;
                case 2:
                    if (GameManager.instance.userInfoManager.colorItem[i].color == slotColor_02)
                    {
                        tempColorItemIcon.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 213 / 255f, 165 / 255f, 1);
                    }
                    tempColorItemIcon.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { slotColor_02 = GameManager.instance.userInfoManager.colorItem[index].color; SlotColor(); ColorInventory(slot); });
                    break;
            }

            if (GameManager.instance.userInfoManager.colorItem[i].color != Color.clear)
            {
                tempColorItemIcon.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                tempColorItemIcon.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = GameManager.instance.userInfoManager.colorItem[i].color;
            }
          
        }
    }
}
