using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.Mathematics;
using UnityEngine.UI;

public class SlideCustom : MonoBehaviour
{
    public Transform slidePannel;   Vector2 originSlidePannel;
    public Transform character;        Vector2 originCharacter;
    public Transform colorItem_Btn;     Vector2 originColorItemBtn;
    public Transform backPannel; 
    public Transform context;
    public TransformSkin transformSkin;

    //염색패널
    public GameObject colorItem_UnderPannel;
    public GameObject colorItem_pannel;
    public Transform colorItem_context;
    public GameObject slotSelect_01;
    public GameObject slotSelect_02;
    public GameObject checkPannel;

    Color tempColor;
    UserSkin userSkin;

    public float context_Height = 150;
    public float movePannelX = .8f;

    float slideMoveSpeed = 0.3f;
    int currentState;

    
    int[] selectBtnNum = new int[5];
    string[] selectString = new string[5];

    private void Start()
    {
        originSlidePannel = slidePannel.position;
        originCharacter = character.position;
        originColorItemBtn = colorItem_Btn.position;
    }

    List<GameObject> iConList_Stage01 = new List<GameObject>();
    List<GameObject> iConList_Stage02 = new List<GameObject>();
    List<GameObject> iConList_Stage03 = new List<GameObject>();
    List<GameObject> iConList_ColorItem = new List<GameObject>();
    List<GameObject> iConList_ColorItem_02 = new List<GameObject>();

    bool clickFlag;
    void ClickFlagFalse()
    {
        clickFlag = false;
    }

    public void OpenkSlide()
    {
        if (currentState == 0)
        {
            clickFlag = true; // 클릭 방지

            backPannel.gameObject.SetActive(true);
            currentState = 1;
            StartCoroutine(Open_01_Coroutine(() =>
            {
                for (int i = 0; i < context.childCount; i++)
                {
                    context.GetChild(i).gameObject.SetActive(false);
                }
                context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, 0);
                // 스킨 종류가 리스트에 담긴다
                List<SkinKind> tempSkinKindList = GameManager.instance.spineSkinInfoManager.GetSkinKindList();
                for (int i = 0; i < tempSkinKindList.Count; i++)
                {
                    context.GetChild(i).gameObject.SetActive(true);
                    context.GetChild(i).name = tempSkinKindList[i].ToString();
                  //  GameObject tempIcon = GameManager.instance.iconManager.GetIcon(context.GetChild(i).name);
                //    iConList_Stage01.Add(Instantiate(tempIcon, context.GetChild(i).position, quaternion.identity, context.GetChild(i)));
                    context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * context_Height);
                }
                character.DOMoveX(originCharacter.x - .6f, slideMoveSpeed);
            }));
            Invoke("ClickFlagFalse", slideMoveSpeed);
        }
    }

    public void SelectBtn(int num)
    {
        if (clickFlag)
        {
            return;
        }

        if (currentState < 4)
        {
            currentState++;
        }

        switch (currentState)
        {
            case 1:
                selectBtnNum[currentState] = num;
                selectString[currentState] = context.GetChild(num).name;
                State1();
                break;
            case 2:
                // 스킨 종류를 담는다 
                selectBtnNum[currentState] = num;
                selectString[currentState] = context.GetChild(num).name;
                State2(num);
                break;
            case 3:
                selectBtnNum[currentState] = num;
                selectString[currentState] = context.GetChild(num).name;
                State3(num);
                break;
            case 4:
                selectBtnNum[currentState] = num;
                selectString[currentState] = context.GetChild(num).name;
                State4(num);
                break;
            default:
                State0();
                break;
        }
    }

    IEnumerator Open_01_Coroutine(Action callBack = null)
    {
        if (callBack != null)
        {
            callBack();
        }
        slidePannel.transform.DOMoveX(originSlidePannel.x - movePannelX, slideMoveSpeed);
        yield return new WaitForSeconds(slideMoveSpeed);
    }
    IEnumerator Open_02_Coroutine(Action callBack = null)
    {
        clickFlag = true;
        slidePannel.transform.DOMoveX(originSlidePannel.x, slideMoveSpeed);
        yield return new WaitForSeconds(slideMoveSpeed);
        if (callBack != null)
        {
            callBack();
        }
        slidePannel.transform.DOMoveX(originSlidePannel.x - movePannelX, slideMoveSpeed);
        yield return new WaitForSeconds(slideMoveSpeed);
        clickFlag = false;
    }

    IEnumerator Close_Coroutine(Action callBack = null)
    {
        clickFlag = true;
        slidePannel.transform.DOMoveX(originSlidePannel.x, slideMoveSpeed);
        yield return new WaitForSeconds(slideMoveSpeed);
        if (callBack != null)
        {
            callBack();
        }
        clickFlag = false;
    }

    public void BackPannelBtn()
    {
        if (clickFlag)
        {
            return;
        }

        if (currentState > 3)
        {
            currentState = 3;
        }
        currentState--;

        ColorItemUnderPannelClose();
        ColorChangeFail();

        switch (currentState)
        {
            case 1:
                State1();
                break;
            case 2:
                State2(selectBtnNum[1]);
                break;
            case 3:
                State3(selectBtnNum[2]);
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

        StartCoroutine(Close_Coroutine(() =>
        {
            for (int i = 0; i < context.childCount; i++)
            {
                context.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < iConList_Stage01.Count; i++)
            {
                Destroy(iConList_Stage01[i]);
            }
            iConList_Stage01.Clear();

        }));

        character.transform.DOMoveX(originCharacter.x, slideMoveSpeed);
        backPannel.gameObject.SetActive(false);
    }
    // 스킨 대분류
    public void State1()
    {
        StartCoroutine(Open_02_Coroutine(()=> {
            for (int i = 0; i < iConList_Stage02.Count; i++)
            {
                Destroy(iConList_Stage02[i]);
            }
            iConList_Stage02.Clear();

            context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, 0);
            List<SkinKind> tempSkinKindList = GameManager.instance.spineSkinInfoManager.GetSkinKindList();
            for (int i = 0; i < tempSkinKindList.Count; i++)
            {
                context.GetChild(i).gameObject.SetActive(true);
                context.GetChild(i).name = tempSkinKindList[i].ToString();
              //  GameObject tempIcon = GameManager.instance.iconManager.GetIcon(context.GetChild(i).name);
               // iConList_Stage01.Add(Instantiate(tempIcon, context.GetChild(i).position, quaternion.identity, context.GetChild(i)));
                context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * context_Height);
            }
        }));
    }
    // 스킨 종류
    public void State2(int num)
    {
        StartCoroutine(Open_02_Coroutine(()=> {
            if (num != -1)
            {
                for (int i = 0; i < iConList_Stage01.Count; i++)
                {
                    Destroy(iConList_Stage01[i]);
                }
                iConList_Stage01.Clear();

                for (int i = 0; i < iConList_Stage03.Count; i++)
                {
                    Destroy(iConList_Stage03[i]);
                }
                iConList_Stage03.Clear();

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
                        string tempName = tempSpineSkinInfo[i].skinName.Split('/')[1]; // 스킨종류를 제외한 스킨 이름
                        context.GetChild(i).name = tempName;
                      //  GameObject tempIcon = GameManager.instance.iconManager.GetIcon(context.GetChild(i).name);
                       // iConList_Stage02.Add(Instantiate(tempIcon, context.GetChild(i).position, quaternion.identity, context.GetChild(i)));
                        context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * context_Height);
                    }
                }
            }
        }));
    }
    // 유저 스킨 아이템 종류 
    public void State3(int num)
    {
        StartCoroutine(Open_02_Coroutine(()=> {
            if (num != -1)
            {
                for (int i = 0; i < iConList_Stage02.Count; i++)
                {
                    Destroy(iConList_Stage02[i]);
                }
                iConList_Stage02.Clear();

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
               //     GameObject tempIcon = GameManager.instance.iconManager.GetIcon(context.GetChild(i).name);
                    // 임시적으로 유저 아이콘 색갈 변경 
               //     tempIcon.GetComponent<Image>().color = userSkinList[i].color_01;

               //     iConList_Stage03.Add(Instantiate(tempIcon, context.GetChild(i).position, quaternion.identity, context.GetChild(i)));
                    context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * context_Height);
                }
            }
        }));
    }

    public void State4(int num)
    {
        // 선택 완료 
        //입은 옷 전역변수 저장 
        List<UserSkin> userSkinList = GameManager.instance.userInfoManager.GetSkinItemList(selectString[3]);
        userSkin = userSkinList[num];
        //스킨 장착 , 유저정보에 저장 
        string skinName = userSkin.skinName;
        GameManager.instance.userInfoManager.PushUserEqip(GameManager.instance.userInfoManager.skinItem[GameManager.instance.userInfoManager.GetSkinItemIndex(userSkin)]);
        transformSkin.UserEqipInfoSetting();
        //색 버튼 나오게하기 
        colorItem_Btn.position = context.GetChild(num).position;
        colorItem_Btn.DOMoveX(context.GetChild(num).position.x - movePannelX, slideMoveSpeed);

        slotSelect_01.GetComponent<Image>().color = Color.white;
        slotSelect_02.GetComponent<Image>().color = Color.white;
        if (transformSkin.GetColor(skinName, 1) == Color.clear)
        {
            slotSelect_01.GetComponent<Image>().color = Color.red;
        }
        if (transformSkin.GetColor(skinName, 2) == Color.clear)
        {
            slotSelect_02.GetComponent<Image>().color = Color.red;
        }
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
         //   GameObject tempColorItemIcon = Instantiate(GameManager.instance.iconManager.GetIcon("고정염색약"), colorItem_context.GetChild(i).position, quaternion.identity, colorItem_context.GetChild(i));
         //   tempColorItemIcon.transform.GetChild(0).GetComponent<Image>().color = GameManager.instance.userInfoManager.colorItem[i].color;
          //  iConList_ColorItem.Add(tempColorItemIcon); 
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
        for (int i = 0; i < iConList_ColorItem.Count; i++)
        {
            Destroy(iConList_ColorItem[i]);
        }
        iConList_ColorItem.Clear();
        for (int i = 0; i < iConList_ColorItem_02.Count; i++)
        {
            Destroy(iConList_ColorItem_02[i]);
        }
        iConList_ColorItem_02.Clear();
    }

    public void ColorItemSelect(int num)
    {
      //  GameObject tempColorItemIcon = Instantiate(GameManager.instance.iconManager.GetIcon("고정염색약"), colorItem_UnderPannel.transform.Find("염색아이템창").position, quaternion.identity, colorItem_UnderPannel.transform);
      //  tempColorItemIcon.transform.GetChild(0).GetComponent<Image>().color = GameManager.instance.userInfoManager.colorItem[num].color;
        tempColor = GameManager.instance.userInfoManager.colorItem[num].color;
   //     iConList_ColorItem_02.Add(tempColorItemIcon);
        SlotSelect_01();
    }

    bool select_01 = false;
    bool select_02 = false;

    public void SlotSelect_01()
    {
        if (slotSelect_01.GetComponent<Image>().color == Color.red)
        {
            return;
        }

        select_01 = true;
        select_02 = false;

        transformSkin.UserEqipInfoSetting();
        transformSkin.SetColor(userSkin.skinName, tempColor, 1);
    }

    public void SlotSelect_02()
    {
        if (slotSelect_02.GetComponent<Image>().color == Color.red)
        {
            return;
        }

        select_01 = false;
        select_02 = true;

        transformSkin.UserEqipInfoSetting();
        transformSkin.SetColor(userSkin.skinName, tempColor, 2);
    }

    public void ColorChangeComplete()
    {
        if (select_01)
        {
            // 아이템 색 바꾸기 
            GameManager.instance.userInfoManager.ChangeColorSkinItem(GameManager.instance.userInfoManager.GetSkinItemIndex(userSkin), tempColor, 1);
            // 아이템 장착
            GameManager.instance.userInfoManager.PushUserEqip(GameManager.instance.userInfoManager.skinItem[GameManager.instance.userInfoManager.GetSkinItemIndex(userSkin)]);
        }

        if (select_02)
        {
            GameManager.instance.userInfoManager.ChangeColorSkinItem(GameManager.instance.userInfoManager.GetSkinItemIndex(userSkin), tempColor, 2);
            GameManager.instance.userInfoManager.PushUserEqip(GameManager.instance.userInfoManager.skinItem[GameManager.instance.userInfoManager.GetSkinItemIndex(userSkin)]);
        }

        select_01 = false;
        select_02 = false;

     
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
      //      GameObject tempIcon = GameManager.instance.iconManager.GetIcon(context.GetChild(i).name);
            // 임시적으로 유저 아이콘 색갈 변경 
      //      tempIcon.GetComponent<Image>().color = userSkinList[i].color_01;

      //      iConList_ColorItem_02.Add(Instantiate(tempIcon, context.GetChild(i).position, quaternion.identity, context.GetChild(i)));
            context.GetComponent<RectTransform>().sizeDelta = new Vector2(context.GetComponent<RectTransform>().sizeDelta.x, i * context_Height);
        }

        CheckPannelClose();
    }

    public void ColorChangeFail()
    {
        select_01 = false;
        select_02 = false;
        transformSkin.UserEqipInfoSetting();
    }

    // 컬러 변경 확인창 

    public void CheckPannelOpen()
    {
        checkPannel.SetActive(true);
    }

    public void CheckPannelClose()
    {
        checkPannel.SetActive(false);
    }

}