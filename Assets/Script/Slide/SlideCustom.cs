using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine;
using System;
using Unity.Mathematics;

public class SlideCustom : MonoBehaviour
{
    public Transform slidePannel;   Vector2 originSlidePannel;
    public Transform theCam;        Vector2 originTheCam;
    public Transform backPannel; 
    public Transform context;
    public TransformSkin transformSkin;

    public float context_Height = 150;

    float slideMoveSpeed = 0.3f;
    int currentState;

    List<GameObject> iConList = new List<GameObject>();

    private void Start()
    {
        originSlidePannel = slidePannel.position;
        originTheCam = theCam.position;
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
        currentState--;
        switch (currentState)
        {
            case 1:
                State1();
                break;
            case 2:
                State2(backNum[currentState]);
                break;
            case 3:
                State3(backNum[currentState]);
                break;
            default:
                State0();
                break;
        }
    }

    IEnumerator SlideMoveCoroutine(Action callBack = null)
    {
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
      
        currentState++;
    
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
                transformSkin.SkinChange((SkinKind)Enum.Parse(typeof(SkinKind), selectString[2]) , skinName);
                State0();
                break;
            default:
                State0();
                break;
        }
    }

    // 원래 상태
    public void State0()
    {
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
}
