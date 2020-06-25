using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour
{
    public GameObject pannel;
    public GameObject exitBtn;

    string[] bagItem;

    private void Start()
    {
        bagItem = new string[pannel.transform.childCount];
    }

    public void OpenBag()
    {
        pannel.SetActive(true);
        exitBtn.SetActive(true);
        int count = pannel.transform.childCount;
        int iCount = 0;
    
        //컬러 아이템 정렬
        for (int i = 0; i < GameManager.instance.itemManager.colorItemList.Length; i++)
        {
            if (count == iCount)
                return;

            string tempItemName = GameManager.instance.itemManager.colorItemList[i].name;
            if (GameManager.instance.userInfoManager.ExistItem(tempItemName))
            {
                bagItem[i] = tempItemName;
                pannel.transform.GetChild(iCount).GetComponent<Image>().sprite = GameManager.instance.itemManager.GetColorItem(tempItemName).sprite;
                pannel.transform.GetChild(iCount).GetChild(0).GetComponent<Text>().text = GameManager.instance.userInfoManager.GetUserItemNum(tempItemName).ToString();
                iCount++;
            }
        }
    }

    public void CloseBag()
    {
        for (int i = 0; i < pannel.transform.childCount; i++)
        {
            pannel.transform.GetChild(i).GetComponent<Image>().sprite = null;
            pannel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
        }

        pannel.SetActive(false);
        exitBtn.SetActive(false);
    }

    public void UseItem(int num)
    {

    }
}
