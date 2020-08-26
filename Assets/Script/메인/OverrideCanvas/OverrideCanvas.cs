using DG.Tweening;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverrideCanvas : MonoBehaviour
{
    public static OverrideCanvas instance;

    public Transform theCam; Vector2 originPos;

    [SerializeField] GameObject donTouchPannel;
    [SerializeField] Transform alram;
    [SerializeField] Transform redAlram;
    [SerializeField] Transform caution;
    [SerializeField] Transform purchaseAlram;
    [SerializeField] Transform polaroid;
    [SerializeField] Transform screenPhoto;
    [SerializeField] Transform wating;
    [SerializeField] Transform itemOpen;

    void Start()
    {
        instance = this;
        originPos = theCam.position;
    }

    void InitPos()
    {
        this.transform.position = originPos;
        this.transform.position = new Vector2(this.transform.position.x + theCam.position.x - originPos.x, this.transform.position.y + theCam.position.y - originPos.y);
    }

    public void Alram(string text)
    {
        InitPos();
        donTouchPannel.SetActive(true);

        alram.gameObject.SetActive(true);
        alram.GetComponent<Image>().DOFade(1, 0);
        alram.transform.GetChild(0).GetComponent<Text>().DOFade(1, 0);
        alram.transform.GetChild(0).GetComponent<Text>().text = text;
        alram.GetComponent<Image>().DOFade(0, 1.3f).OnComplete(() => { alram.gameObject.SetActive(false); donTouchPannel.SetActive(false); });
        alram.transform.GetChild(0).GetComponent<Text>().DOFade(0, 1.3f);
    }

    public void RedAlram(string text)
    {
        InitPos();
        donTouchPannel.SetActive(true);

        redAlram.gameObject.SetActive(true);
        redAlram.GetComponent<Image>().DOFade(1, 0);
        redAlram.transform.GetChild(0).GetComponent<Text>().DOFade(1, 0);
        redAlram.transform.GetChild(0).GetComponent<Text>().text = text;
        redAlram.GetComponent<Image>().DOFade(0, 1.3f).OnComplete(() => { redAlram.gameObject.SetActive(false); donTouchPannel.SetActive(false); });
        redAlram.transform.GetChild(0).GetComponent<Text>().DOFade(0, 1.3f);
    }

    public void Caution(string text, System.Action function)
    {
        InitPos();
        donTouchPannel.SetActive(true);

        caution.gameObject.SetActive(true);
        caution.Find("Text").GetComponent<Text>().text = text;
        caution.Find("확인").GetComponent<Button>().onClick.RemoveAllListeners();
        caution.Find("확인").GetComponent<Button>().onClick.AddListener(() => { function(); donTouchPannel.SetActive(false); caution.gameObject.SetActive(false); });
        caution.Find("취소").GetComponent<Button>().onClick.RemoveAllListeners();
        caution.Find("취소").GetComponent<Button>().onClick.AddListener(() => { donTouchPannel.SetActive(false); caution.gameObject.SetActive(false); });
    }

    public void PurchaseAlram(System.Action function)
    {
        InitPos();
        donTouchPannel.SetActive(true);

        purchaseAlram.gameObject.SetActive(true);
        purchaseAlram.Find("확인").GetComponent<Button>().onClick.RemoveAllListeners();
        purchaseAlram.Find("확인").GetComponent<Button>().onClick.AddListener(() => { function(); donTouchPannel.SetActive(false); purchaseAlram.gameObject.SetActive(false); });
        caution.Find("취소").GetComponent<Button>().onClick.RemoveAllListeners();
        caution.Find("취소").GetComponent<Button>().onClick.AddListener(() => { donTouchPannel.SetActive(false); purchaseAlram.gameObject.SetActive(false); });
    }

    public void PolaroidPhoto(Sprite screenShot, int month, int day)
    {
        InitPos();
        if (screenShot == null)
        {
            Debug.Log("aa");
        }
        Debug.Log("bb");
        polaroid.gameObject.SetActive(true);
        polaroid.Find("ScreenShot").GetComponent<Image>().sprite = screenShot;
        polaroid.Find("ScreenShot").GetComponent<Button>().onClick.RemoveAllListeners();
        polaroid.Find("ScreenShot").GetComponent<Button>().onClick.AddListener(() => { ScreenPhoto(screenShot); });
        polaroid.Find("날짜").GetComponent<Text>().text = month + "월" + day + "일";
        polaroid.Find("Skip").GetComponent<Button>().onClick.RemoveAllListeners();
        polaroid.Find("Skip").GetComponent<Button>().onClick.AddListener(() => {
            polaroid.gameObject.SetActive(false);
        });
    }

    public void ScreenPhoto(Sprite screenShot)
    {
        InitPos();
        screenPhoto.gameObject.SetActive(true);

        screenPhoto.GetChild(1).GetComponent<Image>().sprite = screenShot;
        screenPhoto.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        screenPhoto.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { screenPhoto.gameObject.SetActive(false); });
    }

    public void Wating(string text, bool flag)
    {
        InitPos();
        wating.gameObject.SetActive(flag);
        wating.GetChild(0).GetChild(0).GetComponent<Text>().text = text;
    }

    List<UserSkin> itemNameList;
    List<int> numList;

    public void GetItem(List<UserSkin> itemNameList, List<int> numList, System.Action callback = null)
    {
        if (itemNameList.Count != numList.Count)
        {
            Debug.Log("아이템 이름과 수량이 맞지 않습니다");
            return;
        }

        this.itemNameList = itemNameList;
        this.numList = numList;

        for (int i = 0; i < itemNameList.Count; i++)
        {
            switch (GameManager.instance.itemManager.GetItemInfo(itemNameList[i].skinName).itemKind)
            {
                case ItemKind.골드:
                    GameManager.instance.userInfoManager.SetUserMoney(MoneyKind.Gold, GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Gold) + numList[i]);
                    break;
                case ItemKind.크리스탈:
                    GameManager.instance.userInfoManager.SetUserMoney(MoneyKind.Crystal, GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Crystal) + numList[i]);
                    break;
                case ItemKind.랜덤염색약:
                    break;
                case ItemKind.염색약:
                    break;
                case ItemKind.스킨:
                    GameManager.instance.userInfoManager.PushSkinItem(itemNameList[i]);
                    break;
            }
        }

        GameManager.instance.userInfoManager.SaveSkinItem();
        GameManager.instance.userInfoManager.SaveUserMoney();

        itemOpen.Find("touchPannel").GetComponent<Button>().onClick.RemoveAllListeners();
        StartCoroutine(ItemOpen(itemNameList[0], numList[0], 0, itemNameList.Count, callback));
    }

    IEnumerator ItemOpen(UserSkin item, int num, int count, int totalCount, System.Action callback = null)
    {
        itemOpen.Find("touchPannel").GetComponent<Button>().onClick.RemoveAllListeners();

        itemOpen.gameObject.SetActive(false);
        itemOpen.gameObject.SetActive(true);

        for (int i = 0; i < itemOpen.Find("배경노란줄").Find("네모박스").childCount; i++)
        {
            Destroy(itemOpen.Find("배경노란줄").Find("네모박스").GetChild(i).gameObject);
        }

        string numString = " " + num;
        if (num == 1)
        {
            numString = "";
        }

        itemOpen.Find("배경노란줄").Find("뽑기개수").GetComponent<Text>().text = (count + 1) + " / " + totalCount;
        itemOpen.Find("배경노란줄").Find("아이템이름").GetComponent<Text>().text = GameManager.instance.itemManager.GetItemInfo(item.skinName).inGameName + numString;

        GameObject iconObj = Instantiate(GameManager.instance.itemManager.GetItemInfo(item.skinName).iconObj, Vector2.zero, Quaternion.identity, itemOpen.Find("배경노란줄").Find("네모박스"));
        for (int i = 0; i < iconObj.transform.childCount; i++)
        {
            iconObj.transform.GetChild(i).transform.localScale = new Vector3(.7f, .7f, .7f);
            if (iconObj.transform.GetChild(i).name == "color_01")
            {
                iconObj.transform.GetChild(i).GetComponent<Image>().color = item.color_01;
            }
            if (iconObj.transform.GetChild(i).name == "color_02")
            {
                iconObj.transform.GetChild(i).GetComponent<Image>().color = item.color_02;
            }
        }

        int indexCount = count + 1;
        yield return new WaitForSeconds(1.1f);
        if (indexCount < itemNameList.Count)
        {
            itemOpen.Find("touchPannel").GetComponent<Button>().onClick.AddListener(() => {
       
                StartCoroutine(ItemOpen(itemNameList[indexCount], numList[indexCount], indexCount, itemNameList.Count));
            });
        }
        else
        {
            itemOpen.Find("touchPannel").GetComponent<Button>().onClick.AddListener(() => {
                itemOpen.gameObject.SetActive(false);
                if (callback != null)
                {
                    callback();
                }
            });
        }
    }
}
