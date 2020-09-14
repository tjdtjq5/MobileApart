using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [Header("욕구 상태 아이콘")]
    public Sprite best_icon;
    public Sprite good_icon;
    public Sprite soso_icon;
    public Sprite notBad_icon;
    public Sprite bad_icon;

    private void Start()
    {
        SetCurrentNeedInfo();
    }

    public void SetCurrentNeedInfo()
    {
        int needPercentage = GameManager.instance.userInfoManager.GetUserNeed(theWorstNeed());

        this.transform.GetChild(0).Find("욕구이미지").GetComponent<Image>().sprite = GetNeedIcon(needPercentage);
    }
    public NeedKind theWorstNeed()
    {
        List<int> needList = new List<int>();
        needList.Add(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력));
        needList.Add(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감));
        needList.Add(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함));
        needList.Add(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움));
        needList.Sort();

        if (needList[0] == GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력))
            return NeedKind.활력;
        if (needList[0] == GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감))
            return NeedKind.포만감;
        if (needList[0] == GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함))
            return NeedKind.청결함;
        if (needList[0] == GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움))
            return NeedKind.즐거움;

        return NeedKind.Null;
    }
    public Sprite GetNeedIcon(int needPercent)
    {
        if (needPercent >= 80)
        {
            return best_icon;
        }
        else if (needPercent >= 60)
        {
            return good_icon;
        }
        else if (needPercent >= 40)
        {
            return soso_icon;
        }
        else if (needPercent >= 20)
        {
            return notBad_icon;
        }
        else
        {
            return bad_icon;
        }
    }
    
    public void NeedOnClick()
    {
        GameObject needUI = this.transform.GetChild(0).Find("욕구UI").gameObject;
        if (needUI.activeSelf)
        {
            NeedClose();
        }
        else
        {
            NeedOpen();
        }
    }

    public void NeedOpen()
    {
        GameObject needUI = this.transform.GetChild(0).Find("욕구UI").gameObject;
        needUI.SetActive(true);

        needUI.transform.Find("즐거움").Find("흑").Find("수치").GetComponent<Text>().text = GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움).ToString();
        needUI.transform.Find("즐거움").Find("흑").Find("이미지").GetComponent<Image>().sprite = GetNeedIcon(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움));
        needUI.transform.Find("포만감").Find("흑").Find("수치").GetComponent<Text>().text = GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감).ToString();
        needUI.transform.Find("포만감").Find("흑").Find("이미지").GetComponent<Image>().sprite = GetNeedIcon(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감));
        needUI.transform.Find("청결함").Find("흑").Find("수치").GetComponent<Text>().text = GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함).ToString();
        needUI.transform.Find("청결함").Find("흑").Find("이미지").GetComponent<Image>().sprite = GetNeedIcon(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함));
        needUI.transform.Find("활력").Find("흑").Find("수치").GetComponent<Text>().text = GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력).ToString();
        needUI.transform.Find("활력").Find("흑").Find("이미지").GetComponent<Image>().sprite = GetNeedIcon(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력));
    }

    public void NeedClose()
    {
        GameObject needUI = this.transform.GetChild(0).Find("욕구UI").gameObject;
        needUI.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject needUI = this.transform.GetChild(0).Find("욕구UI").gameObject;
            if (needUI.activeSelf)
            {
                NeedClose();
            }
        }
    }
}

public enum NeedKind
{
    // 즐거움, 포만감, 청결함, 활력
    Null,
    즐거움,
    포만감,
    청결함,
    활력
}