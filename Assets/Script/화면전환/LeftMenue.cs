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

    private void Start()
    {
        originLeftPannel = leftPannel.position;
    }


    public void LeftMenueOpen()
    {
        leftPannel.DOMoveX(originLeftPannel.x + moveLeftPannelX, moveSpeed);
        backPannel.SetActive(true);
    }

    public void LeftMenueClose()
    {
        leftPannel.DOMoveX(originLeftPannel.x, moveSpeed);
        backPannel.SetActive(false);
    }
}
