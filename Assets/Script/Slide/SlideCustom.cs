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
    int currentClickState;

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
        currentClickState = 0;
        currentState = 1;
        SlideState(1);
    }

    public void SelectBtn(int currentClickState)
    {
        this.currentClickState = currentClickState;
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
                slideBtnTransform.DOMoveX(originSlideBtnPos.x - 200F, moveSpeed);
                slideTransform_01.DOMoveX(originSlidePos_01.x - 200F, moveSpeed);
                slideTransform_02.DOMoveX(originSlidePos_02.x, moveSpeed);
                slideTransform_03.DOMoveX(originSlidePos_03.x, moveSpeed);
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
                slideBtnTransform.DOMoveX(originSlideBtnPos.x - 200F, moveSpeed);
                slideTransform_03.DOMoveX(originSlidePos_03.x - 200F, moveSpeed);
                break;
            default:
                slideTransform_01.DOMoveX(originSlidePos_01.x, moveSpeed);
                slideTransform_02.DOMoveX(originSlidePos_02.x, moveSpeed);
                slideTransform_03.DOMoveX(originSlidePos_03.x, moveSpeed);
                slideBtnTransform.DOMoveX(originSlideBtnPos.x, moveSpeed);
                theCam.DOMoveX(originCamPos.x, moveSpeed).OnComplete(() => { backBtnPannel.SetActive(false); });
                break;
        }
    }

}
