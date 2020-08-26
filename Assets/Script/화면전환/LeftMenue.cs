using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    [Header("Mobile Blur")]
    public MobileBlur mobileBlur;

    [Header("드래그 패널")]
    public GameObject dragPannel;


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

        mobileBlur.enabled = true;
    }

    public void LeftMenueClose()
    {
        leftPannel.DOMoveX(originLeftPannel.x, moveSpeed);
        backPannel.SetActive(false);

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }

        mobileBlur.enabled = false;
    }

    public void Gift()
    {
        leftPannel.DOMoveX(movePosX, moveSpeed);

        circle.SetActive(true);
        circle.transform.localPosition = leftPannel.Find("상자").localPosition;
        leftPannel.Find("상자").Find("상자").gameObject.SetActive(true);
        leftPannel.Find("상자").Find("재화UI ").gameObject.SetActive(true);

        dragPannel.SetActive(true);

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { GiftClose((PointerEventData)data); });
        dragPannel.GetComponent<EventTrigger>().triggers.Add(entry);
    }

    void GiftClose(PointerEventData data)
    {
        leftPannel.Find("상자").Find("상자").gameObject.SetActive(false);
        leftPannel.Find("상자").Find("재화UI ").gameObject.SetActive(false);
        LeftMenueOpen();

        dragPannel.SetActive(false);
    }

    public void AlbumOpen()
    {
        leftPannel.DOMoveX(movePosX, moveSpeed);

        circle.SetActive(true);
        circle.transform.localPosition = leftPannel.Find("앨범").localPosition;

        leftPannel.Find("앨범").Find("앨범").gameObject.SetActive(true);
        leftPannel.Find("앨범").Find("휴지통").gameObject.SetActive(true);
        leftPannel.Find("앨범").GetComponent<Album>().AlbumOpen();
       
        dragPannel.SetActive(true);

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { AlbumClose((PointerEventData)data); });
        dragPannel.GetComponent<EventTrigger>().triggers.Add(entry);
    }

    public void AlbumClose(PointerEventData data)
    {
        leftPannel.Find("앨범").Find("앨범").gameObject.SetActive(false);
        leftPannel.Find("앨범").Find("휴지통").gameObject.SetActive(false);
        leftPannel.Find("앨범").GetComponent<Album>().AlbumClose();
        LeftMenueOpen();

        dragPannel.SetActive(false);
    }

    public void MailOpen()
    {
        leftPannel.Find("메일").GetComponent<Mail>().MailOpen();
    }
}
