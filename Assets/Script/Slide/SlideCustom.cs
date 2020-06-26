using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlideCustom : MonoBehaviour
{
    public Transform slideTransform_01; // 스킨 종류 선택
    public Transform slideTransform_02; // 옷 종류 선택
    public Transform slideTransform_03; // 색상 선택 
    public Transform slideBtnTransform;
    Vector2 originSlidePos_01;
    Vector2 originSlidePos_02;
    Vector2 originSlidePos_03;
    Vector2 originSlideBtnPos;

    public Transform theCam;
    Vector2 originCamPos;

    public GameObject backBtnPannel;

    int currentState;

    SkinKind slectType;

    List<GameObject> IconObjList = new List<GameObject>();

    public void Start()
    {
        originSlidePos_01 = slideTransform_01.position;
        originSlidePos_02 = slideTransform_02.position;
        originSlidePos_03 = slideTransform_03.position;
        originSlideBtnPos = slideBtnTransform.position;
        originCamPos = theCam.position;
    }

    public void SlideOpen()
    {
        currentState = 1;
        SlideState(1);

    }

    public void NextBtn()
    {
        currentState++;
        SlideState(currentState);
    }

    public void BackBtn()
    {
        currentState--;
        SlideState(currentState);
    }

    public void SlideState(int currentState)
    {
        this.currentState = currentState;
        StartCoroutine(SlideCoroutine(currentState));
    }

    float moveSpeed = 0.3f;
    IEnumerator SlideCoroutine(int currentState)
    {
        switch (currentState)
        {
            case 1:
                DestroyIconObjList();
                Transform Temptransform = slideTransform_01.GetChild(0).GetChild(0).GetChild(0);
                for (int i = 0; i < Temptransform.childCount; i++)
                {
                    GameObject tempIconObj = GameManager.instance.iconManager.GetIcon(Temptransform.GetChild(i).name);
                    IconObjList.Add(Instantiate(tempIconObj, Temptransform.GetChild(i).position, Quaternion.identity, Temptransform.GetChild(i)));
                }
                slideBtnTransform.DOMoveX(originSlideBtnPos.x - 200F, moveSpeed);
                slideTransform_01.DOMoveX(originSlidePos_01.x - 200F, moveSpeed);
                slideTransform_02.DOMoveX(originSlidePos_02.x, moveSpeed);
                slideTransform_03.DOMoveX(originSlidePos_03.x, moveSpeed);
                yield return new WaitForSeconds(moveSpeed);
                theCam.DOMoveX(originCamPos.x + 1f, moveSpeed).OnComplete(() => { backBtnPannel.SetActive(true); });
                break;
            case 2:
                slideBtnTransform.DOMoveX(originSlideBtnPos.x, moveSpeed);
                slideTransform_01.DOMoveX(originSlidePos_01.x, moveSpeed);
                slideTransform_03.DOMoveX(originSlidePos_03.x, moveSpeed);
                yield return new WaitForSeconds(moveSpeed);
               
                slideBtnTransform.DOMoveX(originSlideBtnPos.x - 200F, moveSpeed);
                slideTransform_02.DOMoveX(originSlidePos_02.x - 200F, moveSpeed);
                break;
            case 3:
                slideBtnTransform.DOMoveX(originSlideBtnPos.x, moveSpeed);
                slideTransform_01.DOMoveX(originSlidePos_01.x, moveSpeed);
                slideTransform_02.DOMoveX(originSlidePos_02.x, moveSpeed);
                yield return new WaitForSeconds(moveSpeed);
                DestroyIconObjList();
                slideBtnTransform.DOMoveX(originSlideBtnPos.x - 200F, moveSpeed);
                slideTransform_03.DOMoveX(originSlidePos_03.x - 200F, moveSpeed);
                break;
            default:
                DestroyIconObjList();
                slideTransform_01.DOMoveX(originSlidePos_01.x, moveSpeed);
                slideTransform_02.DOMoveX(originSlidePos_02.x, moveSpeed);
                slideTransform_03.DOMoveX(originSlidePos_03.x, moveSpeed);
                slideBtnTransform.DOMoveX(originSlideBtnPos.x, moveSpeed);
                theCam.DOMoveX(originCamPos.x, moveSpeed).OnComplete(() => { backBtnPannel.SetActive(false); DestroyIconObjList(); });
                break;
        }
    }

    // pannel_02 아래 스킨종류로 이름을 바꾼다,아이콘도 생성 ,해당 종류의 스킨 수 만큼 setActive  
    public void SelectBtn_01(string skinKindString)
    {
        Transform Temptransform = slideTransform_02.GetChild(0).GetChild(0).GetChild(0);
        for (int i = 0; i < Temptransform.childCount; i++)
        {
            Temptransform.GetChild(i).gameObject.SetActive(false);
        }

        SkinKind skinKine = (SkinKind)System.Enum.Parse(typeof(SkinKind), skinKindString);
        slectType = skinKine;
        List<SpineSkinInfo> spineInfo = GameManager.instance.spineSkinInfoManager.GetSpineSkinInfo(skinKine);

        for (int i = 0; i < spineInfo.Count; i++)
        {
            Temptransform.GetChild(i).gameObject.SetActive(true);
            Temptransform.GetChild(i).name = spineInfo[i].skinName;
            Debug.Log(spineInfo[i].skinName);
            GameObject tempIconObj = GameManager.instance.iconManager.GetIcon(spineInfo[i].skinName);
            IconObjList.Add(Instantiate(tempIconObj, Temptransform.GetChild(i).position, Quaternion.identity, Temptransform.GetChild(i)));
        }
    }

    void DestroyIconObjList()
    {
        for (int i = IconObjList.Count -1; i >= 0; i--)
        {
            Destroy(IconObjList[i]);
            IconObjList.RemoveAt(i);
        }
    }
}
