using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftMenue : MonoBehaviour
{
    [Header("왼쪽메뉴패널")]
    public Transform leftPannel;  Vector2 originLeftPannel;
    public float moveLeftPannelX;
    float moveSpeed = 0.3f;
    [Header("뒤로가기")]
    public GameObject backPannel;
    [Header("MovePosX")]
    public float movePosX;        float originMovePosX;
    [Header("circle")]
    public GameObject circle;

    [Header("SetOff")]
    public GameObject[] setOff;

    bool openFlag = false;

    private void Start()
    {
        originLeftPannel = leftPannel.position;
        originMovePosX = leftPannel.position.x;
        
    }


    public void LeftMenueOpen()
    {
        leftPannel.DOMoveX(originLeftPannel.x + moveLeftPannelX, moveSpeed);
        backPannel.SetActive(true);
        circle.SetActive(false);

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(false);
        }
    }

    public void LeftMenueClose()
    {
        leftPannel.DOMoveX(originLeftPannel.x, moveSpeed);
        backPannel.SetActive(false);

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }
    }

    public void Gift()
    {
        if (!openFlag)
        {
            openFlag = true;
            leftPannel.DOMoveX(movePosX, moveSpeed);

            circle.SetActive(true);
            circle.transform.localPosition = leftPannel.Find("상자").localPosition;
            leftPannel.Find("상자").Find("상자").gameObject.SetActive(true);
            leftPannel.Find("상자").Find("재화UI ").gameObject.SetActive(true);
        }
        else
        {
            openFlag = false;
            leftPannel.Find("상자").Find("상자").gameObject.SetActive(false);
            leftPannel.Find("상자").Find("재화UI ").gameObject.SetActive(false);
            LeftMenueOpen();
        }
    }


}
