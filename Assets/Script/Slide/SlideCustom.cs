using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine;
using System;
using Unity.Mathematics;
using UnityEngine.UI;

public class SlideCustom : MonoBehaviour
{
    public Transform slidePannel;   Vector2 originSlidePannel;
    public Transform theCam;        Vector2 originTheCam;
    public Transform colorItem_Btn;     Vector2 originColorItemBtn;
    public Transform backPannel; 
    public Transform context;
    public TransformSkin transformSkin;

    //염색패널
    public GameObject colorItem_UnderPannel;
    public GameObject colorItem_pannel;
    public Transform colorItem_context;

    List<GameObject> colorItemList = new List<GameObject>();
    Color tempColor;
    UserSkin userSkin;

    public float context_Height = 150;

    float slideMoveSpeed = 0.3f;
    int currentState;

    List<GameObject> iConList = new List<GameObject>();

    private void Start()
    {
        originSlidePannel = slidePannel.position;
        originTheCam = theCam.position;
        originColorItemBtn = colorItem_Btn.position;
        State0();
    }

    public void OpenkSlide()
    {
        if (currentState == 0)
        {
            State1();
        }
    }

    int[] backNum = new int[5];
    string[] selectString = new string[5];

    public void BackPannelBtn()
    {
        if (currentState == 4)
        {
            currentState = 3;
        }

        ColorItemUnderPannelClose();

        currentState--;

        switch (currentState)
        {
            case 1:
                State1();
                break;
            case 2:
                State2(backNum[1]);
                break;
            case 3:
                State3(backNum[2]);
                break;
            default:
                State0();
                break;
        }
    }

    IEnumerator SlideMoveCoroutine(Action callBack = null)
    {
        colorItem_Btn.position = originColorItemBtn;

        if (slidePannel.transform.position.x == originSlidePannel.x)
        {
            if (callBack != null)
            {
                callBack();
            }
            slidePannel.transform.DOMoveX(originSlidePannel.x - 200f, slideMoveSpeed);
        }

        if (slidePannel.transform.position.x == originSlidePannel.x - 200f)
        {
            slidePannel.transform.DOMoveX(originSlidePannel.x, slideMoveSpeed);
            yield return new WaitForSeconds(slideMoveSpeed);
            if (callBack != null)
            {
                callBack();
            }
            slidePannel.transform.DOMoveX(originSlidePannel.x - 200f, slideMoveSpeed);
        }
    }

    public void SelectBtn(int num)
    {
        if (currentState < 4)
        {
            currentState++;
        }

        switch (currentState)
        {
            case 1:
                backNum[currentState] = num;
                selectString[currentState] = context.GetChild(num).name;
                State1();
                break;
            case 2:
                backNum[currentState] = num;
                selectString[currentState] = context.GetChild(num).name;
                State2(num);
                break;
            case 3:
                backNum[currentState] = num;
                selectString[currentState] = context.GetChild(num).name;
                State3(num);
                break;
            case 4:
                backNum[currentState] = num;
                selectString[currentState] = context.GetChild(num).name;
                // 선택 완료 
                string skinName = selectString[2] + "/" + selectString[3];
                //스킨 장착 
                transformSkin.SkinChange((SkinKind)Enum.Parse(typeof(SkinKind), selectString[2]) , skinName);
                //색 버튼 나오게하기 
                colorItem_Btn.position = context.GetChild(num).position;
                colorItem_Btn.DOMoveX(context.GetChild(num).position.x - 162f, slideMoveSpeed);
                //입은 옷 전역변수 저장 
                List<UserSkin> userSkinList = GameManager.instance.userInfoManager.GetSkinItemList(selectString[3]);
                userSkin = userSkinList[num];
                break;
            default:
                State0();
                break;
        }
    }

    // 원래 상태
    public void State0()
    {
        colorItem_Btn.position = originColorItemBtn;

        for (int i = 0; i < context.childCount; i++)
        {
            context.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < iConList.Count; i++)
        {
            Destroy(iConList[i]);
        }
        iConList.Clear();

      

        currentState = 0;
        theCam.transform.DOMoveX(originTheCam.x, slideMoveSpeed);
        slidePannel.transform.DOMoveX(originSlidePannel.x, slideMoveSpeed);
        backPannel.gameObject.SetActive(false);
    }
    // 스킨 대분류
    public void State1()
    {
        backPannel.gameObject.SetActive(true);
        currentState = 1;
        theCam.transform.DOMoveX(originTheCam.x + 1f, slideMoveSpeed);
        StartCoroutine(SlideMoveCoroutine(()=> {
            context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, 0);
            List<SkinKind> tempSkinKindList = GameManager.instance.spineSkinInfoManager.GetSkinKindList();
            for (int i = 0; i < tempSkinKindList.Count; i++)
            {
                context.GetChild(i).gameObject.SetActive(true);
                context.GetChild(i).name = tempSkinKindList[i].ToString();
                GameObject tempIcon = GameManager.instance.iconManager.GetIcon(context.GetChild(i).name);
                iConList.Add(Instantiate(tempIcon, context.GetChild(i).position, quaternion.identity, context.GetChild(i)));
                context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * context_Height);
            }
        }));
    }
    // 스킨 종류
    public void State2(int num)
    {
        currentState = 2;
        StartCoroutine(SlideMoveCoroutine(()=> {
            if (num != -1)
            {
                for (int i = 0; i < context.childCount; i++)
                {
                    context.GetChild(i).gameObject.SetActive(false);
                }
                context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, 0);
                string tempSelectString = selectString[currentState];
                if (tempSelectString != "")
                {
                    List<SpineSkinInfo> tempSpineSkinInfo = GameManager.instance.spineSkinInfoManager.GetSpineSkinInfo((SkinKind)Enum.Parse(typeof(SkinKind), tempSelectString));
                    for (int i = 0; i < tempSpineSkinInfo.Count; i++)
                    {
                        context.GetChild(i).gameObject.SetActive(true);
                        string tempName = tempSpineSkinInfo[i].skinName.Split('/')[1];
                        context.GetChild(i).name = tempName;
                        GameObject tempIcon = GameManager.instance.iconManager.GetIcon(context.GetChild(i).name);
                        iConList.Add(Instantiate(tempIcon, context.GetChild(i).position, quaternion.identity, context.GetChild(i)));
                        context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * context_Height);
                    }
                }
            }
        }));
    }
    // 유저 스킨 아이템 종류 
    public void State3(int num)
    {
        currentState = 3;
        StartCoroutine(SlideMoveCoroutine(()=> {
            if (num != -1)
            {
                for (int i = 0; i < context.childCount; i++)
                {
                    context.GetChild(i).gameObject.SetActive(false);
                }
                context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, 0);
                string tempSelectString = selectString[currentState];
                
                List<UserSkin> userSkinList = GameManager.instance.userInfoManager.GetSkinItemList(tempSelectString);
                for (int i = 0; i < userSkinList.Count; i++)
                {
                    context.GetChild(i).gameObject.SetActive(true);
                    string tempName = userSkinList[i].skinName.Split('/')[1];
                    context.GetChild(i).name = tempName;
                    GameObject tempIcon = GameManager.instance.iconManager.GetIcon(context.GetChild(i).name);
                    iConList.Add(Instantiate(tempIcon, context.GetChild(i).position, quaternion.identity, context.GetChild(i)));
                    context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * context_Height);
                }
            }
        }));
    }

    // 컬러아이템 패널창이 뜨도록 하는 함수
    public void ColorItemPannelOpen()
    {
        colorItem_pannel.SetActive(true);

        for (int i = 0; i < colorItem_context.childCount; i++)
        {
            colorItem_context.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < GameManager.instance.userInfoManager.colorItem.Count; i++)
        {
            colorItem_context.GetChild(i).gameObject.SetActive(true);
            GameObject tempColorItemIcon = Instantiate(GameManager.instance.iconManager.GetIcon("고정염색약"), colorItem_context.GetChild(i).position, quaternion.identity, colorItem_context.GetChild(i));
            tempColorItemIcon.transform.GetChild(0).GetComponent<Image>().color = GameManager.instance.userInfoManager.colorItem[i].color;
            iConList.Add(tempColorItemIcon); 
        }
    }

    public void ColorItemPannelClose()
    {
        colorItem_pannel.SetActive(false);
    }

    public void ColorItemUnderPannel()
    {
        colorItem_UnderPannel.SetActive(true);
    }

    public void ColorItemUnderPannelClose()
    {
        colorItem_UnderPannel.SetActive(false);
        for (int i = 0; i < colorItemList.Count; i++)
        {
            Destroy(colorItemList[i]);
        }
        colorItemList.Clear();
    }



    public void ColorItemSelect(int num)
    {
        GameObject tempColorItemIcon = Instantiate(GameManager.instance.iconManager.GetIcon("고정염색약"), colorItem_UnderPannel.transform.Find("염색아이템창").position, quaternion.identity, colorItem_UnderPannel.transform);
        tempColorItemIcon.transform.GetChild(0).GetComponent<Image>().color = GameManager.instance.userInfoManager.colorItem[num].color;
        tempColor = GameManager.instance.userInfoManager.colorItem[num].color;
        colorItemList.Add(tempColorItemIcon);
    }

    public void SlotSelect(int num)
    {
        Color originColor_01 = transformSkin.GetColor(userSkin.skinName, 1);
        Color originColor_02 = transformSkin.GetColor(userSkin.skinName, 2);
        switch (num)
        {
            case 2:
                transformSkin.SetColor(userSkin.skinName, tempColor, 2);
                if (userSkin.color_01 == Color.clear)
                {
                    transformSkin.SetColor(userSkin.skinName, originColor_01, 1);
                }
                else
                {
                    transformSkin.SetColor(userSkin.skinName, userSkin.color_01, 1);
                }
                break;
            default:
                transformSkin.SetColor(userSkin.skinName, tempColor, 1);
                if (userSkin.color_02 == Color.clear)
                {
                    transformSkin.SetColor(userSkin.skinName, originColor_02, 2);
                }
                else
                {
                    transformSkin.SetColor(userSkin.skinName, userSkin.color_02, 2);
                }
                break;
        }
    }
}